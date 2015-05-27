SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


ALTER  PROCEDURE SALES3_HTMLResellerSummary
@userid uniqueidentifier,
@resellercode varchar(8) = null,
@starting varchar(30) = null,
@ending varchar(30) = null

AS

SET NOCOUNT ON

/*---------------------------------------
Determine the date range
---------------------------------------*/
declare @startdate as datetime
declare @enddate as datetime

declare @start7 datetime
set @start7 = left(cast(dateadd(day, -7, getdate())as varchar),12)  -- 7 days ago

declare @endofday datetime
set @endofday = left(cast(dateadd(day, 1, getdate())as varchar),12)  -- midnight tonight

declare @timezero datetime
set @timezero = 'Dec 1, 2001'

if (@starting is not null and @starting <> '' and  isdate(@starting) = 0)
 or (@ending is not null and @ending <> '' and isdate(@ending) = 0)
begin
	select 'Start date and/or end date not correct - try YYYY-MM-DD'
	return
end

---------------------------------------
if @starting is not null and @starting<>''
	set @startdate = @starting
else
	set @startdate = @start7
---------------------------------------
if @ending is not null and @ending<>''
	set @enddate = @ending
else
	set @enddate = @endofday


if isdate(@startdate) = 0 or isdate(@enddate) = 0
begin
	select 'Start date and/or end date not correct - try YYYY-MM-DD'
	return
end

/*--------------------------------
  identify all the salesperps
---------------------------------*/
create table #p (userid uniqueidentifier, login varchar(96), name varchar(161))
insert into #p (userid, login, name)
select distinct l.userid, l.login, isnull(p.firstname+' '+p.lastname,'')
from libuser l with(nolock) 
join permissions pm with(nolock) on pm.userid=l.userid
join person p with(nolock) on p.personid=l.personid
where l.userid in
	(select sgm.userid
	from salesgroupmember sgm with(nolock), salesgroup sg with(nolock)
	where sg.salesgroupid=sgm.salesgroupid and sg.resellercode = @resellercode)
 and pm.reseller=1 
 and (pm.salesmanager=0 and pm.generaladmin=0 and pm.superuser=0)

/*--------------------------------
  add self
---------------------------------*/
insert into #p (userid, login, name)
select distinct l.userid, l.login, isnull(p.firstname+' '+p.lastname,'')
from libuser l with(nolock) 
join person p with(nolock) on p.personid=l.personid
where l.userid = @userid

/*------------------------------------------
Filter the resellercode through the list
that user belongs to
---------------------------------------*/
declare @hasaccess int
select @hasaccess = count(sg.resellercode)
from salesgroup sg with(nolock), salesgroupmember sm with(nolock), libuser u with(nolock)
where sg.salesgroupid=sm.salesgroupid and sm.userid=u.userid
and u.userid = @userid
and sg.resellercode = @resellercode
/*------------------------------------------
See others if permisssion sufficient
---------------------------------------*/
if(@hasaccess=0)
	select @hasaccess= count(superuser)
	from libuser u with(nolock), permissions pm with(nolock)
	where u.userid=@userid
	and u.userid = pm.userid
	and pm.superuser=1
if(@hasaccess=0)
 set @resellercode = null

/*------------------------------------------
Title
------------------------------------------*/
declare @resellername varchar(80)
select @resellername=name from Salesgroup where resellercode=@resellercode
select 'Reseller Summary: '+@resellername

select 'Starting Date'=left(@startdate,12), 'Ending Date'=left(@enddate,12)
select 'Sales Persons Included:'=''
select distinct 'Name'=#p.name, 'Login'=#p.login from #p

/*------------------------------------------
Get a list of all the relevant subscription
------------------------------------------*/
create table #s (subscriptionid uniqueidentifier, created datetime)
-- those owned by the user ---
/*
insert into #s select distinct s.subscriptionid
from subscription s with(nolock)
where s.salespersonid=@userid
and s.created>@startdate
*/
-- those with the resellercode ---
insert into #s select distinct s.subscriptionid, s.created
from subscription s with(nolock)
where s.resellercode=@resellercode
--and s.created>@startdate and s.created<@enddate
and s.expires>@startdate and s.starts<@enddate

-- those owned by other resellers in the same salesgroup ---
insert into #s select distinct s.subscriptionid, s.created
from subscription s with(nolock), #p
where s.salespersonid=#p.userid
--and s.created>@startdate and s.created<@enddate
and s.expires>@startdate and s.starts<@enddate

/*--------------------------------
  summary of subscriptions by type
  and status
---------------------------------*/
select 'Reseller Subscription Summary' = ''
select 'type'=isnull(st.description,''),
       'status'=isnull(ss.description,''),
       'count'= count(distinct s.passwordroot),
--       'seats'=sum(distinct s.seats),
       'downloads'=isnull(sum(u.downloadcount),-1),
       'chunks'=isnull(sum(u.pagecount),-1)
from #p, subscription s with(nolock),
domaintable st with(nolock), domaintable ss with(nolock), libuser u with(nolock)
where s.type = st.number and st.tablename='Subscription' and st.columnname='Type'
  and s.status = ss.number and ss.tablename='Subscription' and ss.columnname='Status'
  and u.subscriptionid = s.subscriptionid
  and s.salespersonid=#p.userid
--and s.created>@start and s.created<@end
  and s.expires>@startdate and s.starts<@enddate
  and s.resellercode=@resellercode
group by st.description, ss.description --with rollup
order by s.type desc, s.status

/*------------------------------------------
trial subscriptions
------------------------------------------*/
select 'Trial Subscriptions'=''
select '$help' = 'These trial subscriptions were active between '+left(@startdate,12)+' and '+left(@enddate,12)+'. Note activity numbers indicate total over duration of subscription.'
select
'Code' = s.passwordroot,
'Status'=ss.description,
'Expiration'= left(s.expires,11),
'Company'=c.name+'/'+s.companyordepartment,
'Reseller'= isnull(s.resellercode,''),
'Users' = count(cast(u.userid as varchar(36))),
'Sessions'=sum(isnull(u.sessioncount,0)), 
'Sections'=sum(isnull(u.pagecount,0)), 
'DL'=sum(isnull(u.downloadcount,0)),
'@subscriptionid' = s.subscriptionid
from libuser u with(nolock), subscription s with(nolock), company c with(nolock), domaintable ss with(nolock)
where u.subscriptionid = s.subscriptionid and c.companyid = s.companyid
and s.status = ss.number and ss.tablename='Subscription' and ss.columnname='Status'
and s.type=0
and s.expires>@startdate and s.starts<@enddate
and s.subscriptionid in (select distinct subscriptionid from #s)
and s.resellercode=@resellercode
--and u.sessioncount>0
group by s.passwordroot, ss.description,  c.name+'/'+s.companyordepartment, s.subscriptionid, s.resellercode, s.expires
order by s.status, 'DL' desc, 'Sections' desc


/*------------------------------------------
paid subscriptions
------------------------------------------*/
select 'Active Paid Subscriptions'=''
--select '$help' = 'The following paid subscriptions have had users log-in between '+left(@startdate,12)+' and '+left(@enddate,12)+'. Note: other paid subscriptions are not shown.'
select '$help' = 'The following paid subscriptions were/are active between '+left(@startdate,12)+' and '+left(@enddate,12)+'. Note activity numbers indicate total over duration of subscription.'
select
'Code' = s.passwordroot,
'Status'=ss.description,
'Expiration'= left(s.expires,11),
'Company'=c.name+'/'+s.companyordepartment,
'Reseller'= isnull(s.resellercode,''),
'Users' = count(cast(u.userid as varchar(36))),
'Sessions'=sum(isnull(u.sessioncount,0)),
'Sections'=sum(isnull(u.pagecount,0)), 
'DL'=sum(isnull(u.downloadcount,0)),
'@subscriptionid' = s.subscriptionid
from libuser u with(nolock), subscription s with(nolock), company c with(nolock), domaintable ss with(nolock)
where c.companyid = s.companyid 
and u.subscriptionid = s.subscriptionid 
and s.status = ss.number and ss.tablename='Subscription' and ss.columnname='Status'
and s.type=1
and s.expires>@startdate and s.starts<@enddate
--and s.expires > @start7 				-- expires no earlier than 7 days ago
--and u.sessioncount>0 --and max(u.lastlogin)>@start7  -- with session in last 7 days
and s.subscriptionid in (select distinct subscriptionid from #s)
and s.resellercode=@resellercode
group by s.passwordroot,ss.description,  c.name+'/'+s.companyordepartment, s.subscriptionid, s.resellercode, s.expires
order by s.status, 'DL' desc, 'Sections' desc


/*------------------------------------------
probation users
------------------------------------------*/
/*
select 'Scrambled Users Summary'=''
select
'code' = s.passwordroot,
'status'=ss.description,
'company'=c.name+'/'+s.companyordepartment,
'reseller'= isnull(s.resellercode,''),
'users' = count(cast(u.userid as varchar)),
'sessions'=sum(isnull(u.sessioncount,0)), 'pages'=sum(isnull(u.pagecount,0)), 'dl'=sum(isnull(u.downloadcount,0))
,'@subscriptionid' = s.subscriptionid
from libuser u with(nolock), subscription s with(nolock), company c with(nolock), domaintable ss with(nolock), #s
where u.subscriptionid = s.subscriptionid and c.companyid = s.companyid
and s.status = ss.number and ss.tablename='Subscription' and ss.columnname='Status'
and s.subscriptionid = #s.subscriptionid
and s.resellercode=@resellercode
and (u.probationlevel=1 or u.probationlevel=2)
group by s.passwordroot, ss.description,  c.name+'/'+s.companyordepartment, s.subscriptionid, s.resellercode
order by s.status, 'dl' desc, 'pages' desc
*/

/*------------------------------------------
users in complimentary subs
------------------------------------------*/
select 'Complimentary Subscriptions'=''
select '$help' = 'These are all the complimentary subscriptions assigned to this reseller.'
select
'Code' = s.passwordroot,
'status'=ss.description,
'expiration'= left(s.expires,11),
'company'=c.name+'/'+s.companyordepartment,
'reseller'= isnull(s.resellercode,''),
'pages'=sum(isnull(u.pagecount,0)), 'dl'=sum(isnull(u.downloadcount,0)), 'sessions'=sum(isnull(u.sessioncount,0))
,'@subscriptionid' = s.subscriptionid
from libuser u with(nolock), subscription s with(nolock), person p with(nolock), company c with(nolock), domaintable ss with(nolock)
where u.subscriptionid = s.subscriptionid and c.companyid = s.companyid
and s.status = ss.number and ss.tablename='Subscription' and ss.columnname='Status'
and s.subscriptionid in (select distinct subscriptionid from #s)
and s.resellercode=@resellercode
and s.type=2
group by s.passwordroot,ss.description,  c.name+'/'+s.companyordepartment, s.subscriptionid, s.resellercode, s.expires
order by s.status, 'company', 'dl', 'pages'

/*--------------------------------
  summary of geocodes
---------------------------------*/
select 'Usage Geographic Summary' = ''
select 
'Country' = substring(s2.akstring,charindex('country_code=',s2.akstring)+len('country+code='),2),
'Region' = substring(s2.akstring,charindex('region_code=',s2.akstring)+len('region_code='),
		case when charindex(',',s2.akstring,charindex('region_code=',s2.akstring))>0
			then charindex(',',s2.akstring,charindex('region_code=',s2.akstring))-
				charindex('region_code=',s2.akstring)-len('region_code=')
		else 0 end),
'City' = substring(s2.akstring,charindex('city=',s2.akstring)+len('city='),
		case when charindex(',',s2.akstring,charindex('city=',s2.akstring))>0
			then charindex(',',s2.akstring,charindex('city=',s2.akstring))-
				charindex('city=',s2.akstring)-len('city=') else 0 end),
'Sessions' = count(*)
from #s, session s with(nolock)
left outer join sessionextensiondata s2 with(nolock) on s.sessionid=s2.sessionid
where s.subscriptionid =#s.subscriptionid
and s.starttime>@startdate and s.starttime<@enddate
and s2.akstring is not null
group by 
substring(s2.akstring,charindex('country_code=',s2.akstring)+len('country+code='),2),
substring(s2.akstring,charindex('region_code=',s2.akstring)+len('region_code='),
case when charindex(',',s2.akstring,charindex('region_code=',s2.akstring))>0
	then charindex(',',s2.akstring,charindex('region_code=',s2.akstring))-
		charindex('region_code=',s2.akstring)-len('region_code=') else 0 end),
substring(s2.akstring,charindex('city=',s2.akstring)+len('city='),
case when charindex(',',s2.akstring,charindex('city=',s2.akstring))>0
	then charindex(',',s2.akstring,charindex('city=',s2.akstring))-
		charindex('city=',s2.akstring)-len('city=') else 0 end)
order by count(*) desc


/*--------------------------------
  conversions
---------------------------------*/
declare @trialcount as float
declare @trialseatcount as float
declare @paidcount as float
declare @paidseatcount as float

select @trialcount=isnull(count(distinct s.passwordroot),0),
       @trialseatcount=isnull(sum(s.seats),0)
from #p, subscription s with(nolock)
where s.salespersonid=#p.userid and s.type=0
and s.created>@startdate and s.created<@enddate

--and s.created<@end

select @paidcount= isnull(count(distinct s.passwordroot),0),
       @paidseatcount=isnull(sum(s.seats),0)
from #p, subscription s with(nolock)
where s.salespersonid=#p.userid and s.type=1
and s.created>@startdate and s.created<@enddate
-- and s.created<@end

select 'Conversion Rate' = ''
select 'Unit'='Subscription',
       'Trial'=@trialcount,
       'Paid'=@paidcount,
       'Conversion' = cast(case when @trialcount=0 then 0 else 100*@paidcount/@trialcount end as varchar)+' %'
union
select 'Unit'='Seats',
       'Trial'=@trialseatcount,
       'Paid'=@paidseatcount,
       'Conversion' = cast(case when @trialseatcount=0 then 0 else 100*@paidseatcount/@trialseatcount end as varchar)+' %'



drop table #s
drop table #p


-- exec SALES3_HTMLMenu @userid

---------------------
/*
GRANT  EXECUTE  ON [dbo].[SALES3_HTMLResellerSummary]  TO [PolecatProduction]

insert into PolecatReports
(ProcName,Category,ReportCategoryID, Description,InternalOnly, IsSales, HelpText,IsProduction, ReportLevel, IsUserReport, IsPolecatReport, TimeoutMinutes)
values
('SALES3_HTMLResellerSummary','Sales3', -1,'Reseller Activity Summary',0,0,NULL,0,10,0,0,0)

insert into PolecatReportsHost (HostApplication, ProcName, isdefault, sequence)
values ('Sales3','SALES3_HTMLResellerSummary', 0, null)
*/






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[SALES3_HTMLResellerSummary]  TO [PolecatProduction]
GO

