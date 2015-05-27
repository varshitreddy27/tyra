USE [bookdb]
GO

/****** Object:  StoredProcedure [dbo].[SALES3_HTMLWelcome]    Script Date: 10/06/2010 22:51:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





ALTER PROCEDURE [dbo].[SALES3_HTMLWelcome]
@userid uniqueidentifier = null
AS

set nocount on

declare @nullguid uniqueidentifier
set @nullguid = 0x0

declare @name varchar(64)
declare @login varchar(96)
select @name = isnull(p.firstname,'')+' '+isnull(p.lastname, ''), @login=l.login
from libuser l with(nolock), person p with(nolock)
where l.personid = p.personid and l.userid=@userid

--select @login = isnull(l.login, '') from libuser l with(nolock) where l.userid=@userid

/*---------------------------------------
 Precalculate some date ranges for
 reporting on recent activity
---------------------------------------*/

declare @end datetime
declare @start datetime
declare @start4 datetime
declare @start7 datetime
declare @start14 datetime

set @end = getdate()
set @start = dateadd(day, -30, @end)
set @start4 = dateadd(day, -4, @end)
set @start7 = dateadd(day, -7, @end)
set @start14 = dateadd(day, -14, @end)

/*---------------------------------------
 Display a welcome message identifying
 the user
---------------------------------------*/
select 'Welcome to B24 Sales, '+@name

/*---------------------------------------
 Display a list of recent activity that
 the user is entitled to.
---------------------------------------*/
/*
select 'My Sales Activity (last 7 days)'=''
select 'Start'=@start7, 'End'=@end
select	'Salesperson'=sl.login,
	'Type'=st.description,
	'Active seats'=cast(sum(s.seats) as varchar),
	'Active subs'=cast(count(*) as varchar),
	'Expired seats'='',
	'Expired subs'=''
from libuser sl with(nolock), subscription s with(nolock), domaintable st with(nolock)
where s.salespersonid=@userid and sl.userid=@userid and s.type in (0,1,5) and s.expires>=getdate()
  and s.starts >= @start7 and s.starts < @end and s.type=st.number and st.tablename='Subscription' and st.columnname='Type'
group by login,st.description
UNION
select	'Salesperson'=login,
	'Type'=st.description,
	'Active seats'='',
	'Active subs'='',
	'Expired seats'=cast(sum(s.seats) as varchar),
	'Expired subs'=cast(count(*) as varchar)
from libuser sl with(nolock), subscription s with(nolock), domaintable st with(nolock)
where s.salespersonid=@userid and sl.userid=@userid and s.type in (0,1,5) and s.expires<getdate()
    and s.starts >= @start7 and s.starts < @end and s.type=st.number and st.tablename='Subscription' and st.columnname='Type'
group by login,st.description
order by salesperson,s.type
*/
/*---------------------------------------
 Display a list of recent activity summary
 for the user's **SALES TEAM**
---------------------------------------*/
/*
select 'My Sales Team Activity (last 7 days)'=''
select 'Start'=@start7, 'End'=@end
select	'Salesperson'=login,
	'Type'=st.description,
	'Active seats'=cast(sum(s.seats) as varchar),
	'Active subs'=cast(count(*) as varchar),
	'Expired seats'='',
	'Expired subs'=''
from libuser sl with(nolock), subscription s with(nolock), domaintable st with(nolock), salesgroupmember sgm1 with(nolock), salesgroupmember sgm2 with(nolock)
where s.salespersonid=sl.userid and s.type in (0,1,5) and s.expires>=getdate()
  and s.starts >= @start7 and s.starts < @end and s.type=st.number and st.tablename='Subscription' and st.columnname='Type'
	and sgm1.userid=sl.userid and sgm2.userid=@userid and sgm2.salesgroupid=sgm1.salesgroupid
group by login,st.description
UNION
select	'Salesperson'=login,
	'Type'=st.description,
	'Active seats'='',
	'Active subs'='',
	'Expired seats'=cast(sum(s.seats) as varchar),
	'Expired subs'=cast(count(*) as varchar)
from libuser sl with(nolock), subscription s with(nolock), domaintable st with(nolock), salesgroupmember sgm1 with(nolock), salesgroupmember sgm2 with(nolock)
where s.salespersonid=sl.userid and s.type in (0,1,5) and s.expires<getdate()
  and s.starts >= @start7 and s.starts < @end and s.type=st.number and st.tablename='Subscription' and st.columnname='Type'
	and sgm1.userid=sl.userid and sgm2.userid=@userid and sgm2.salesgroupid=sgm1.salesgroupid
group by login,st.description
order by salesperson,s.type
*/

/*---------------------------------------
 Display a list of the last subscriptions
 **CREATED** by this user.
---------------------------------------*/
/*
select 'My Recently Created accounts (7 days)'=''
select 	'Created'=left(a.time,20),
	'Name' = c.name+'/'+s.CompanyOrDepartment,
	'Type'=st.description,
	'Status'=ss.description,
	'Application' = s.applicationname,
	s.Seats,
	'@subscriptionid' = a.userid
from bookdbarc.dbo.adminlogarc a with(nolock), subscription s with(nolock), company c with(nolock), domaintable st with(nolock), domaintable ss with(nolock)
where 	a.type like 'subnew%'
  and patindex('by '+@login+':%',a.description)>0
  and a.time >= @start7 and a.time < @end
	and c.companyid = s.companyid
	and s.subscriptionid = a.userid
  and st.number= right(a.type,1) and st.tablename='Subscription' and st.columnname='Type'
	and ss.number= left(right(a.type,2),1) and ss.tablename='Subscription' and ss.columnname='Status'
order by a.time desc
*/

/*---------------------------------------
 Display a list of the most recently
 modifed subscriptions owned by this user
 plus
 others on the same resellercode(s) but
 if they have sales manager privs or more
---------------------------------------*/
select 'My 10 most recently updated subscriptions'=''
select  distinct top 10
	Convert ( char(15), (case when s.lastupdate>s.created then s.lastupdate  else s.created end),107) as 'Last Updated',
	'HiddenColumn' = case when s.lastupdate>s.created then s.lastupdate  else s.created end, --use all mighty hiddencolumn to sort by date
	'B24 SubID'= s.passwordroot,
	'Company' = c.name+'/'+s.CompanyOrDepartment,
	'Type'=	case when s.type in (0,1,2,3) then st.description
		else 'Other ('+cast(s.type as varchar)+')' end,
	'Status'=ss.description,
	'Application' = case when s.applicationname = 'B24Library' then 'B24Library' else s.applicationname end,
	 'Salesgroup' = s.resellercode,
--	 sp.firstname+' '+sp.lastname,
	'Seats' = s.Seats,
	'@subscriptionid' = s.subscriptionid
from  	subscription s with(nolock)
	join company c with(nolock) on c.companyid = s.companyid
	join domaintable st with(nolock) on st.number= s.type and st.tablename='Subscription' and st.columnname='Type'
	join domaintable ss with(nolock) on ss.number= s.status and ss.tablename='Subscription' and ss.columnname='Status'
	join libuser su with(nolock)on su.userid=s.salespersonid
	join person sp with(nolock)on sp.personid=su.personid
where 	s.salespersonid = @userid
--   	and s.lastupdate >= @start7 and s.lastupdate < @end
--order by case when s.lastupdate>s.created then s.lastupdate else s.created end desc
order by 'HiddenColumn' desc
--union
/*
if exists (select pm.userid from permissions pm with(nolock) where pm.userid=@userid and (pm.salesmanager>0 or pm.generaladmin>0 or pm.superuser>0))
begin
select 'Recently Updated Colleague Accounts'=''
select  distinct top 25
	'Last Updated'= case when s.lastupdate>s.created then s.lastupdate else s.created end,
        'ID'= s.passwordroot,
	'Name' = c.name+'/'+s.CompanyOrDepartment,
	'Type'=	case when s.type in (0,1,2,3) then st.description
		else 'Other ('+cast(s.type as varchar)+')' end,
	'Status'=ss.description,
	'Application' = s.applicationname,
	'Sales Group' = s.resellercode,
	'Sales Person' =  sp.firstname+' '+sp.lastname,
	'Seats' = s.Seats,
	'@subscriptionid' = s.subscriptionid
from  	subscription s with(nolock)
	join libuser su with(nolock)on su.userid=s.salespersonid
	join company c with(nolock) on c.companyid = s.companyid
	join domaintable st with(nolock) on st.number= s.type and st.tablename='Subscription' and st.columnname='Type'
	join domaintable ss with(nolock) on ss.number= s.status and ss.tablename='Subscription' and ss.columnname='Status'
	join salesgroup sg with(nolock)on sg.resellercode = s.resellercode
	join salesgroupmember sgm with(nolock) on sgm.salesgroupid=sg.salesgroupid
	join person sp with(nolock) on sp.personid=su.personid
	join permissions pm with(nolock) on pm.userid=su.userid
where 	sgm.userid=@userid
	and s.salespersonid<>@userid
	and (pm.salesmanager>0 or pm.generaladmin>0 or pm.superuser>0)
--   	and s.lastupdate >= @start7 and s.lastupdate < @end
order by case when s.lastupdate>s.created then s.lastupdate else s.created end desc

end
*/


/*---------------------------------------
 Display a list of the last subscriptions
 **MANAGED** by this user.
---------------------------------------*/
/*
select 'My Recently Managed accounts (4 days)'=''
select 	'Last Managed'=left(a.time,20),
	'Name' = c.name+'/'+s.CompanyOrDepartment,
	'Type'=	case when s.type in (0,1,2,3) then st.description
		else 'Other ('+cast(s.type as varchar)+')' end,
	'Status'=ss.description,
	'Application' = s.applicationname, s.Seats,
	'@subscriptionid' = a.userid
from 	bookdbarc.dbo.adminlogarc a with(nolock)
	join subscription s with(nolock) on s.subscriptionid = a.userid
	join company c with(nolock) on c.companyid = s.companyid
	join domaintable st with(nolock) on st.number= right(a.type,1) and st.tablename='Subscription' and st.columnname='Type'
	join domaintable ss with(nolock) on ss.number= left(right(a.type,2),1) and ss.tablename='Subscription' and ss.columnname='Status'
where 	a.type like 'subup%'
        and patindex('by '+@login+':%',a.description)>0
	and a.time >= @start4 and a.time < @end
order by a.time desc
*/

/*Remove the salesgroups section from the bottom of Home page */
/*
exec SALES3_HTMLMenu @userid
*/

select '<ulink url="lookup.asp">Find an existing account or user</ulink> within the system.' = ''





GO


