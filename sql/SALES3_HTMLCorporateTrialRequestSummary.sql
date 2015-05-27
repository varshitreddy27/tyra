SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

ALTER         PROCEDURE SALES3_HTMLCorporateTrialRequestSummary
@userid uniqueidentifier,
@resellercode varchar(8) = null 	-- dummy parameter
AS

SET NOCOUNT ON

select 'Trial Request Summary'

declare @savedresellercode varchar(8)
set @savedresellercode = @resellercode

-- block resellercodes not allowed by user ---
select @resellercode=sg.resellercode
from salesgroup sg with(nolock) , salesgroupmember sgm with(nolock)
where sg.resellercode=@resellercode
and sgm.salesgroupid=sg.salesgroupid
and sgm.userid = @userid

-- filter these for the last 3 weeks only ---
declare @start datetime
set @start = dateadd(day, -21, getdate())

-- display all open Corporate Trial Requests for the resellercode
select 'Unprocessed Requests:'=''
select '$help'='The following are user-generated, reseller-coded requests for upgraded corporate trials with unscrambled content. At the present time the requestor is receiving scrambled content.'
select top 100
'Date' = tr.created,
'Name'= tr.firstname+' '+tr.lastname,
'Email' = tr.email,
'Phone' = tr.phone,
'Company' = tr.companyname+'/'+tr.departmentname,
'Country' = tr.countrycode,
'ip' = tr.ipaddr,
'reseller' = tr.resellercode,
'status' = case when upgraderqstid=1 then 'new' when upgraderqstid=4 then 'pending' when upgraderqstid=5 then 'approved' else '' end,
'@trialrequestid' = tr.trialrequestid
from trialrequest tr with(nolock)
where trialtypeid =1 and upgraderqstid in(1,4,5)
and tr.resellercode = @resellercode
--and tr.created > @start
order by tr.upgraderqstid desc, tr.created desc

-- display all denied Trial Requests for the resellercode
select 'Denied (Duplicate) Requests:'=''
select '$help'='The trial requests below have not been granted, not even in scrambled mode. These are considered duplicates, possibly because the requestor has already received a trial.'
select top 100
'Date' = tr.created,
'Name'= tr.firstname+' '+tr.lastname,
'Email' = tr.email,
'Phone' = tr.phone,
'Company' = tr.companyname+'/'+tr.departmentname,
'Country' = tr.countrycode,
'ip' = tr.ipaddr,
'reseller' = tr.resellercode
--,'@trialrequestid' = tr.trialrequestid
from trialrequest tr with(nolock)
where trialtypeid =1 and upgraderqstid=3
and tr.resellercode = @resellercode
and tr.subscriptionid is null
--and tr.created > @start
order by tr.upgraderqstid desc, tr.created desc

--- exec SALES3_HTMLMenu @userid



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[SALES3_HTMLCorporateTrialRequestSummary]  TO [PolecatCustomers]
GO

