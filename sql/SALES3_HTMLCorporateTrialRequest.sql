SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO




ALTER      PROCEDURE SALES3_HTMLCorporateTrialRequest
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


/*---------------------------------------
Display a title
---------------------------------------*/
select 'Trial Request Details'

--- List the details of the trial request ---
select
'type' = 'Add-on',
'Name'= firstname+' '+lastname,
'Email' = email,
'Phone' = '',
'Company' = '',
'Country' = '',
'ip' = '',
'collections' = ''
from trialrequestuser with(nolock)
where trialrequestid=@trialrequestid
union
select
'type' = 'Requestor',
'Name'= firstname+' '+lastname,
'Email' = email,
'Phone' = phone,
'Company' = companyname+'/'+departmentname,
'Country' = countrycode,
'ip' = ipaddr,
'collections' = isnull([dbo].[B24_CSListHide](tr.collectionstr,Null,7),'')
from trialrequest tr with(nolock)
     left outer join libuser l with(nolock) on l.subscriptionid=tr.subscriptionid
where trialrequestid=@trialrequestid
order by type desc

/*---------------------------------------
Generate an upgrade form
---------------------------------------*/
if @trialrequestid is not null
  exec SALES3_HTMLCorporateTrialUpgradeForm @userid, @trialrequestid

/*---------------------------------------
Run report on requestor
---------------------------------------*/
declare @requestor as varchar(96)
declare @requestorid as uniqueidentifier
select @requestor=l.login,@requestorid=l.userid from libuser l with(nolock), subscription s with(nolock), trialrequest tr with(nolock)
where l.userid=s.customeradminid
and s.subscriptionid=tr.subscriptionid
and tr.trialrequestid=@trialrequestid

--select @requestor, @requestorid

exec SALES3_HTMLUserInfo @userid, @requestor
--exec SALES3_HTMLPersonInfo @userid, @requestorid
--exec SALES3_HTMLMenu @userid


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

