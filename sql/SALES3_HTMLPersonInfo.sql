SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO


ALTER      PROCEDURE SALES3_HTMLPersonInfo
@userid uniqueidentifier = null,  -- the person making the request
@personid varchar(96) = null
AS
set nocount on

/*---------------------------------------
Display a title
---------------------------------------*/
select 'Contact Information'

select
	'Name' = isnull(p.firstname+' '+p.lastname,''),
	'Title' = isnull(p.title,''),
	'Address' = isnull(p.address1,''),
	' ' = isnull(p.address2,''),
	'City' = isnull(p.city,''),
	'State' = isnull(p.stateorprovince,''),
	'Country'= isnull(p.country,''),
	'PostalCode'=isnull(p.postalcode,''),
	'Email'=isnull(p.email,''),
	'Phone'=isnull(p.workphone,''),
	'Ext.'=isnull(p.workextension,''),
	'Fax'=isnull(p.faxnumber,''),
  'Comment' = isnull(p.Notes,'')
  --'@subscriptionid' = isnull(p.DefaultSubscriptionid,'')
from person p with(nolock)
where p.personid=@personid

-- exec SALES3_HTMLMenu @userid


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

