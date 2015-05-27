SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

ALTER  PROCEDURE SALES3_HTMLAccountUsage
@userid uniqueidentifier = null,  	-- the person making the request
@subscriptionid varchar(38) = null	-- subscription id
--@login varchar(20) = null		-- or login of someone in sub
AS

SET NOCOUNT ON

declare @startdate as datetime
set @startdate = 'Dec 1, 2001'

declare @end datetime
set @end = getdate()

declare @start7 datetime
set @start7 = dateadd(day, -7, @end)

declare @UnassignedID uniqueidentifier
select @UnassignedID=userid from libuser with(nolock) where login='unassignedsales'

/*--------------------------------
  identify all the salesperps
---------------------------------*/
create table #p (userid uniqueidentifier, login varchar(96), name varchar(161))
insert into #p
select distinct l.userid, l.login, isnull(p.firstname+' '+p.lastname,'')
from libuser l with(nolock), person p with(nolock),
salesgroup sg with(nolock), salesgroupmember sgm with(nolock)
where sg.salesgroupid=sgm.salesgroupid
and sgm.userid=l.userid and p.personid=l.personid
and sg.salesgroupid in
(select sgm.salesgroupid from salesgroupmember sgm with(nolock) where sgm.userid=@userid)
union
select l.userid, l.login, isnull(p.firstname+' '+p.lastname,'')
from libuser l with(nolock), person p with(nolock)
where p.personid=l.personid and login='unassignedsales'

select 'Sales Persons Included:'=''
select 'Name'=#p.name, 'Login'=#p.login from #p

/*-------------------------------------
  Subscription Table
-------------------------------------*/
create table #s (subscriptionid uniqueidentifier, subnum int identity(1,1), used int, collectionstr varchar(1024))

--- collect list of subscriptions --
insert into #s (subscriptionid)
select s.subscriptionid
from  company c with(nolock), subscription s with(nolock), libuser sl with(nolock)
where s.salespersonid=sl.userid
--and c.name like @CompNameMask
and s.companyid=c.companyid
--and s.applicationname like @AppNameMask
and s.subscriptionid = @subscriptionid
and s.salespersonid in (select userid from #p)

--- add usage status ---
update #s
set used = (select count(*) from #s, libuser l with(nolock) where l.subscriptionid=#s.subscriptionid group by #s.subscriptionid)
select * from #s

/*-------------------------------------
  User Table
-------------------------------------*/
create table #u (userid uniqueidentifier, subscriptionid uniqueidentifier, sessions int, activemins int, pages int)

--- get user subset ---
insert into #u (userid, subscriptionid)
select l.userid, #s.subscriptionid
from libuser l with(nolock), #s  where #s.subscriptionid=l.subscriptionid

--- calculate usage ---
create table #n (userid uniqueidentifier, sessions int, activemins int, pages int)
insert into #n (userid, sessions, activemins, pages)
select #u.userid, count(*), sum(datediff(n,starttime,lasttime)), sum(s.hitcnt)
from session s with(nolock), libuser l with(nolock), #u
where s.userid=l.userid and l.userid=#u.userid and s.starttime>=@start7 and s.lasttime<=@end
group by #u.userid, l.userid, l.subscriptionid

--- backfill subscription activity ---
update #s
set used = (select count(*) from #u, libuser l where #u.subscriptionid=#s.subscriptionid group by l.subscriptionid)

select * from #s
select * from #u

/*
select 'Overall Subscription(s) Summary'=''
select	'Company' = left(c.name,40),
	'Department' = left(s.CompanyOrDepartment,18),
	'Status' = left(ss.description,10),
	'Type' = left(st.description,10),
	'Seats' = s.seats,
	'Used' = isnull(u.used,0),
	'Active' = isnull(sc.active,0),
	'% Active' = case when u.used=0 then 0 else isnull(cast((sc.active*100.00)
		   / case when u.used=0 then 1 else u.used end as decimal(18,2)),0.00) end,
	'Session' = isnull(sum(lb.sessions),0),
	'Pages' = isnull(sum(lb.pages),0),
	'Total Days' = datediff(day,s.created,s.expires),
	'Total Active mins' = isnull(sum(lb.activemins),0),
	'Created' = SUBSTRING(convert(varchar,s.created,120),1,10),
	'Starts' = SUBSTRING(convert(varchar,s.starts,120),1,10),
	'Expires' = SUBSTRING(convert(varchar,s.expires,120),1,10),
	'Application' = s.applicationname,
	'SalesPerson' = sp.login,
	'Collections' = sc.collectionstr
from #s	left outer join #u u on u.subscriptionid=@subscriptionid
	left outer join #lb lb on lb.subscriptionid=@subscriptionid
	left outer join #s sc on sc.subscriptionid=@subscriptionid,
	domaintable ss with(nolock), domaintable st with(nolock),
        company c with(nolock), subscription s with(nolock)
	left outer join libuser sp with(nolock) on s.salespersonid=sp.userid
where s.companyid=c.companyid
  and s.subscriptionid=@subscriptionid
  and s.status=ss.number and ss.tablename='Subscription' and ss.columnname='Status'
  and s.type=st.number and st.tablename='Subscription' and st.columnname='Type'
group by s.created,s.expires,s.starts,ss.description,st.description,
	 c.name,s.seats,sp.login,s.companyordepartment, s.applicationname,sc.collectionstr,
	 sc.active, u.used
order by 'SubNum', 'Company', 'Department'
*/

drop table #n
drop table #u
drop table #s
drop table #p

---------------------
/*
GRANT  EXECUTE  ON [dbo].[SALES3_HTMLAccountUsage]  TO [PolecatProduction]

insert into PolecatReports
(ProcName,Category,ReportCategoryID, Description,InternalOnly, IsSales, HelpText,IsProduction, ReportLevel, IsUserReport, IsPolecatReport, TimeoutMinutes)
values
('SALES3_HTMLAccountUsage','Sales3', -1,'Reseller Activity Summary',0,0,NULL,0,10,0,0,0)

insert into PolecatReportsHost (HostApplication, ProcName, isdefault, sequence)
values ('Sales3','SALES3_HTMLAccountUsage', 0, null)
*/



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

GRANT  EXECUTE  ON [dbo].[SALES3_HTMLResellerSummary]  TO [PolecatProduction]
GO

