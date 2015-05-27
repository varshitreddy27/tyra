
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[B24_RM_PutRoleManagement]
@ManageId uniqueidentifier = Null,
@AppId int, 
@RoleId int,
@ModuleId int,
@SubModuleId int = Null,
@FeatureId int = Null,
@Action varchar(3) = Null,
@RqstUserID uniqueidentifier
AS

set nocount on
/*=====================================================================================
Update or insert in RoleManagement table. 
=======================================================================================*/
Declare @Count Int 
Set @Count=0

SELECT 
	@Count =Count(*)
FROM
	RoleManagement with (NOLOCK) 
WHERE
	ManageId = @ManageId
	
if(@Count = 0)
	Begin
		INSERT INTO RoleManagement
			   (ManageId, ApplicationId, RoleId, ModuleId, SubModuleId, FeatureId, Action, Created, CreatorId, LastUpdated, LastUpdaterId)
		 VALUES
			   (NEWID(), @AppId, @RoleId, @ModuleId, @SubModuleId, @FeatureId, @Action, GETDATE(), @RqstUserID, GETDATE(), @RqstUserID)
	End
Else
	Begin
		UPDATE 
			RoleManagement 
		SET 
			ApplicationId = @AppId, RoleId = @RoleId, ModuleId = @ModuleId, SubModuleId = @SubModuleId, 
			FeatureId = @FeatureId, Action = @Action, 
			LastUpdated=GETDATE(),  LastUpdaterId = @RqstUserID 
		WHERE
			ManageId = @ManageId
	End
GO


