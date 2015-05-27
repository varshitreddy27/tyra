
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[B24_RM_PutModule]
@AppId int, 
@ModuleId int,
@LabelText varchar(50),
@OrderIndex int = 0,
@Remarks varchar(300) = null,
@RqstUserID uniqueidentifier
AS

set nocount on
/*=======================================
Update or insert module 
========================================*/
Declare @Count Int 
Set @Count=0

SELECT 
	@Count =Count(*)
FROM
	Module with (NOLOCK) 
WHERE
	ModuleId = @ModuleId
	
if(@Count = 0)
	Begin
		Select @ModuleId = Max(ModuleId) + 1 From Module
		INSERT INTO Module
			   (ApplicationId, ModuleId, LabelText, OrderIndex, Remarks, Created, CreatorId, LastUpdated, LastUpdaterId)
		 VALUES
			   (@AppId, @ModuleId, @LabelText, @OrderIndex, @Remarks, GETDATE(), @RqstUserID, GETDATE(), @RqstUserID)
	End
Else
	Begin
		UPDATE 
			Module 
		SET 
			ApplicationId  = @AppId, LabelText = @LabelText, OrderIndex = @OrderIndex, Remarks = @Remarks, 
			LastUpdated=GETDATE(),  LastUpdaterId = @RqstUserID 
		WHERE
			ModuleId = @ModuleId
	End

return @ModuleId
GO


