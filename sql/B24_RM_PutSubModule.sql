SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[B24_RM_PutSubModule]
@AppId int, 
@SubModuleId int,
@LabelText varchar(50),
@OrderIndex int = 0,
@Remarks varchar(300) = null,
@ModuleId int = null,
@RqstUserID uniqueidentifier
AS

set nocount on
/*================================================================================
Update or insert sub module details. 
If Module id passed then map the moduleid and submoduleid in ModuleSubModule table
=================================================================================*/
Declare @Count Int 
Set @Count=0

SELECT 
	@Count =Count(*)
FROM
	SubModule with (NOLOCK) 
WHERE
	SubModuleId = @SubModuleId
	
if(@Count = 0)
	Begin
		Select @SubModuleId = Max(SubModuleId) + 1 From SubModule
		INSERT INTO SubModule
			   (ApplicationId, SubModuleId, LabelText, Remarks, Created, CreatorId, LastUpdated, LastUpdaterId)
		 VALUES
			   (@AppId, @SubModuleId, @LabelText, @Remarks, GETDATE(), @RqstUserID, GETDATE(), @RqstUserID)
	End
Else
	Begin
		UPDATE 
			SubModule 
		SET 
			ApplicationId  = @AppId, LabelText = @LabelText, Remarks = @Remarks, 
			LastUpdated=GETDATE(),  LastUpdaterId = @RqstUserID 
		WHERE
			SubModuleId = @SubModuleId
	End

-- Map sub module to module if module id is not null
if(@ModuleId is not null)
Begin
	SELECT 
		@Count =Count(*)
	FROM
		ModuleSubModule with (NOLOCK) 
	WHERE
		ModuleId = @ModuleId And SubModuleId = @SubModuleId
		
	if(@Count = 0)
	Begin
		INSERT INTO ModuleSubModule
			   (ApplicationId, ModuleId, SubModuleId, OrderIndex, Created, CreatorId, LastUpdated, LastUpdaterId)
		 VALUES
			   (@AppId, @ModuleId, @SubModuleId, @OrderIndex, GETDATE(), @RqstUserID, GETDATE(), @RqstUserID)
	End
End

return @SubModuleId