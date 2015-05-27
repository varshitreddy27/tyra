SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


ALTER  PROCEDURE SALES3_HTMLSalesGroupActivity
@userid uniqueidentifier,
@resellercode varchar (8) = null,
@startdate varchar (36) = null,
@enddate varchar (36) = null

AS

set nocount on


if (@startdate is not null and isdate(@startdate) = 0) or (@enddate is not null and isdate(@enddate) = 0)
begin
	select 'Start date and/or end date not correct - try YYYY-MM-DD'
	return
end

if @startdate is null set @startdate = dateadd (day, -30, getdate())
if @enddate is not null set @enddate = dateadd (day, 1, @enddate)
if @enddate is null set @enddate = getdate()


/*-----------debug
declare @startdate datetime, @enddate datetime
set @startdate = dateadd (day, -30, getdate())
set @enddate = getdate()
drop table #subs

end debug--------*/


/* -----------------------------------------------
   Obtain a subset of the subscriptions involved 
   ----------------------------------------------- */
create table #subs (subscriptionid uniqueidentifier, resellercode varchar (28), passwordroot varchar(8), collectionstr varchar (1024))
begin
	insert into #subs (subscriptionid, resellercode, passwordroot)
	select subscriptionid, resellercode, passwordroot from subscription s with (Nolock)
	where s.type = 1
	and s.status  in (1,2,3,4)
	and s.resellercode =@resellercode
end


/* -----------------------------------------------
   Backfill the collection info into #subs
   ----------------------------------------------- */
exec mabi_getcollectionsforsubs

/* -----------------------------------------------
   Report on dates involved 
   ----------------------------------------------- */
select 	'Group' = @resellercode,
	'From'=convert (char (12), @startdate, 107),
	'To' = convert (char (12), @enddate, 107)


/* -----------------------------------------------
   Report on sales involved
   ----------------------------------------------- */

/* find all the subscriptions which have been "sold" within the date range */

create table #s (pwroot char (5))
	insert into #s
	select #subs.passwordroot
	from subscriptionchangehistory sch with (nolock)
	join #subs on sch.subscriptionid = #subs.subscriptionid
	where sch.time between @startdate and @enddate
	and sch.type = 'sale'


/* report all sales and renewal transactions */
select
'ID' = s.passwordroot,
--'Name' = cast (left (case when (c.name=s.companyordepartment or s.companyordepartment='-') then c.name else c.name+' ('+s.companyordepartment+')' end, 30) as varchar(30)),
'Name' = case when (c.name=s.companyordepartment or s.companyordepartment='-') then c.name else c.name+' ('+s.companyordepartment+')' end,
--'Salesperson' =cast (pe.firstname + ' ' + pe.lastname + ' (' + lu.login + ')' as varchar(30)),
'Salesperson' =pe.firstname + ' ' + pe.lastname + ' (' + lu.login + ')',
'Transaction*' = sch.type,
'Date Changed to Paid' = convert (char (12), sch.time, 107),
'Starts' = convert (char (12), s.starts, 107),
'Ends' = convert (char (12), s.expires, 107),
'Seats (before)' = cast(sch.intvalue1 as char (5)),
'Seats (after)' = cast(sch.intvalue2 as char(6)),
'Collections' = replace (replace (#subs.collectionstr,'default','ITPro (domestic) '), '(all)','')
from subscriptionchangehistory sch with (nolock)
join subscription s with (nolock) on s.subscriptionid = sch.subscriptionid
join #subs on s.subscriptionid = #subs.subscriptionid
join company c with (nolock) on c.companyid = s.companyid
join libuser lu with (nolock) on lu.userid = s.salespersonid
join person pe with (nolock) on lu.personid = pe.personid
where sch.time between @startdate and @enddate
and sch.type in ('sale','renew','renewadd','renewsub')

union

/* merge in all transactions of 'New' subscriptions, which are newly created, w/o going through a trial */

select
s.passwordroot,
--cast (left (case when (c.name=s.companyordepartment or s.companyordepartment='-') then c.name else c.name+' ('+s.companyordepartment+')' end, 30) as varchar(30)) ,
cast (left (case when (c.name=s.companyordepartment or s.companyordepartment='-') then c.name else c.name+' ('+s.companyordepartment+')' end, 30) as varchar(30)) ,
--cast (pe.firstname+' '+pe.lastname+' ('+lu.login+')' as varchar(30)),
pe.firstname+' '+pe.lastname+' ('+lu.login+')',
'New',
convert (char (12), 'N/A', 107),
convert (char (12), s.starts, 107),
convert (char (12), s.expires, 107),
cast('N/A' as char (5)),
cast(s.seats as char(6)),
'Collections' = replace (replace (#subs.collectionstr,'default','ITPro (domestic) '), '(all)','')
from
subscription s with (nolock)
join #subs on s.subscriptionid = #subs.subscriptionid
join company c with (nolock) on c.companyid = s.companyid
join libuser lu with (nolock) on lu.userid = s.salespersonid
join person pe with (nolock) on lu.personid = pe.personid
where s.type = 1
and s.created between @startdate and @enddate
and s.applicationname in ('b24library','businesspro','officepro','bestbuycorp','skillport','smartforce')
and s.passwordroot not in (select pwroot from #s)

order by 'Date Changed to Paid' 


/* legend */

create table #l (ordernum integer, type varchar(32), description text)
insert into #l (ordernum,type,description) values (1, 'Sale', 'Trial converted to Paid account')
insert into #l (ordernum,type,description) values (2, 'Renew', 'Paid account renewed')
insert into #l (ordernum,type,description) values (3, 'RenewAdd', 'Paid account renewed with increased seat count')
insert into #l (ordernum,type,description) values (4, 'RenewSub', 'Paid account renewed with reduced seat count')
insert into #l (ordernum,type,description) values (5, 'New', 'New Paid account w/o Trial')
select '*Transaction '=type, ' '=description from #l order by ordernum 
drop table #l



/*
delete from polecatreports where procname='SALES3_HTMLSalesGroupActivity'
insert into polecatreports
(procname, category, reportcategoryid, description, helptext, reportlevel,
internalonly, issales, isproduction, ispolecatreport, isuserreport,isofflinereport)
values
('SALES3_HTMLSalesGroupActivity', 'Sales3', -1, 'Activations/Renewals', '', 10,
0,0,0,0,0,0)
delete from polecatreportsHost where procname='SALES3_HTMLSalesGroupActivity'
insert into polecatreportsHost
(procname,hostapplication,isdefault)
values
('SALES3_HTMLSalesGroupActivity', 'Sales3', 0)

*/






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[SALES3_HTMLSalesGroupActivity]  TO [PolecatProduction]
GO

GRANT  EXECUTE  ON [dbo].[SALES3_HTMLSalesGroupActivity]  TO [DBReader]
GO

