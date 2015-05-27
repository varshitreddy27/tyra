USE [bookdb]
GO
/****** Object:  StoredProcedure [dbo].[SALES3_RecentlyUpdatedAccounts]    Script Date: 06/28/2011 19:51:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SALES3_RecentlyUpdatedAccounts]
@userid uniqueidentifier = null
AS

set nocount on

declare @nullguid uniqueidentifier
set @nullguid = 0x0

declare @name varchar(64)
declare @login varchar(96)

/*---------------------------------------
 Display a list of the most recently
 modifed subscriptions owned by this user
 plus
 others on the same resellercode(s) but
 if they have sales manager privs or more
---------------------------------------*/
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
	'Seats' = s.Seats,
	'subscriptionid' = s.SubscriptionID
from  	subscription s with(nolock)
	join company c with(nolock) on c.companyid = s.companyid
	join domaintable st with(nolock) on st.number= s.type and st.tablename='Subscription' and st.columnname='Type'
	join domaintable ss with(nolock) on ss.number= s.status and ss.tablename='Subscription' and ss.columnname='Status'
	join libuser su with(nolock)on su.userid=s.salespersonid
	join person sp with(nolock)on sp.personid=su.personid
where 	s.salespersonid = @userid

order by 'HiddenColumn' desc
