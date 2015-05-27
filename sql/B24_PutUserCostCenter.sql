
/*** B24_PutUserCostCenter	****************************************/
/***Created on 14th July 2010 to update cost center for the user ***/

CREATE PROCEDURE dbo.B24_PutUserCostCenter
@UserID uniqueidentifier,  
@CostCenter varchar(32)
AS  
BEGIN
set nocount on

Update LibUser 
Set 
	CostCenter=@CostCenter 
Where 
	UserID=@UserID	
	
return 0  

END

-- Exec B24_PutUserCostCenter '79A40ED2-48FB-4419-84CB-B8320DFC8A0C','second'
