SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

ALTER  PROCEDURE SALES3_HTMLCorporateTrialUpgradeForm
@userid uniqueidentifier,
@trialrequestid uniqueidentifier = null
AS

SET NOCOUNT ON

-- Identifiy the resellercode associated with the userid
declare @resellercode varchar(8)
select @resellercode = sg.resellercode
from salesgroup sg with(nolock) , salesgroupmember sm with(nolock) , libuser u with(nolock)
where sg.salesgroupid=sm.salesgroupid and sm.userid=u.userid
and u.userid = @userid

--- Clear the trialrequestid through the resellercode ---
select @trialrequestid = trialrequestid
from trialrequest with(nolock)
where trialtypeid =1 and upgraderqstid=1
and resellercode=@resellercode and trialrequestid=@trialrequestid

-- determine the current request status ---
declare @requeststatus int
select @requeststatus=tr.upgraderqstid
from trialrequest tr with(nolock)
where tr.trialrequestid=@trialrequestid

-- determine the current subscription type ---
declare @subtype int, @subid uniqueidentifier
select @subtype=s.type , @subid=s.subscriptionid
from subscription s, trialrequest tr with(nolock)
where s.subscriptionid=tr.subscriptionid and  tr.trialrequestid=@trialrequestid

/*---------------------------------------
identify the sales user
---------------------------------------*/
declare @GeneralAdmin int, @SalesManager int
select @GeneralAdmin=count(*) from permissions with(nolock) where userid=@userid and GeneralAdmin=1
select @SalesManager=count(*) from permissions with(nolock) where userid=@userid and SalesManager=1

/*---------------------------------------
put down some action buttons
   requeststatus=0    no request for upgrade
   requeststatus=1    new request for upgrade
   requeststatus=2    processed upgrade
   requeststatus=3    rejected upgrade
   requeststatus=4    management approval requested
   requeststatus=5    management approval granted
---------------------------------------*/

if (@subtype=0 or @subtype is null) and @requeststatus<>2
select
  -- everyone gets a reject button if applicable
  '$help' = case when @requeststatus=12 then 'In process'
		 when @requeststatus=5 then 'This upgrade has been approved'
	    	 when @requeststatus=4 then 'An approval request has already been submitted for this upgrade'
	    	 when @requeststatus=3 then 'This upgrade has already been rejected'
 	         when @requeststatus=2 then 'This upgrade has already been processed'
	    	 when @requeststatus=1 then 'This trial has requested an upgrade'
	    	 when @requeststatus=0 then 'No request for upgrade has been submitted'
	    	 else null end,
  '!rejectctr' 	= case 	when (@requeststatus=1 or @requeststatus=4 or @requeststatus=5) then 'Reject'
			else null end,
/*
  '!approvalctr'= case 	when (@requeststatus=1 and @requeststatus<>4) then  'Request Approval' -- everyone gets a request for approval if applicable
			else null end ,
  '!approvectr' = case 	when(@GeneralAdmin>0 or @SalesManager>0) and (@requeststatus=4 or @requeststatus=1) then 'Approve Upgrade'
			when(@GeneralAdmin>0 or @SalesManager>0) and (@requeststatus=0) then 'Pre-Approve Upgrade'
			else null end,
  '!processctr' = case  when (@requeststatus<>2 and (@GeneralAdmin>0 or @SalesManager>0 or @requeststatus=5)) then 'Process Upgrade'
		 	else null end,
*/
  -- everyone gets a request for management approval, if applicable
  '!approvalctr'= null,
  --- grant management approval
  '!approvectr' = null,
  --- process the upgrade
  '!processctr' = case  when (@requeststatus=1 or @requeststatus=5) then 'Process Upgrade'
		 	else null end,
  'trialrequestid' = @trialrequestid,
  'subid' = @subid


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

GRANT  EXECUTE  ON [dbo].[SALES3_HTMLCorporateTrialUpgradeForm]  TO [PolecatCustomers]
GO

