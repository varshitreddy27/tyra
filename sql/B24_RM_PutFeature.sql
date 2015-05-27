
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[B24_RM_PutFeature]
@AppId int, 
@FeatureId int,
@LabelText varchar(50),
@Remarks varchar(300) = null,
@SubModuleId int = null,
@RqstUserID uniqueidentifier
AS

set nocount on
/*=====================================================================================
Update or insert sub feature details. 
If SubModule id passed then map the Submoduleid and Featureid in SubModuleFeature table
=======================================================================================*/
Declare @Count Int 
Set @Count=0

SELECT 
	@Count =Count(*)
FROM
	Feature with (NOLOCK) 
WHERE
	FeatureId = @FeatureId
	
if(@Count = 0)
	Begin
		Select @FeatureId = Max(FeatureId) + 1 From Feature
		INSERT INTO Feature
			   (ApplicationId, FeatureId, LabelText, Remark, Created, CreatorId, LastUpdated, LastUpdaterId)
		 VALUES
			   (@AppId, @FeatureId, @LabelText, @Remarks, GETDATE(), @RqstUserID, GETDATE(), @RqstUserID)
	End
Else
	Begin
		UPDATE 
			Feature 
		SET 
			ApplicationId  = @AppId, LabelText = @LabelText, Remark = @Remarks, 
			LastUpdated=GETDATE(),  LastUpdaterId = @RqstUserID 
		WHERE
			FeatureId = @FeatureId
	End

-- Map feature to submodule if submodule id is not null
if(@SubModuleId is not null)
Begin
	SELECT 
		@Count =Count(*)
	FROM
		SubModuleFeature with (NOLOCK) 
	WHERE
		FeatureId = @FeatureId And SubModuleId = @SubModuleId
		
	if(@Count = 0)
	Begin
		INSERT INTO SubModuleFeature
			   (ApplicationId, FeatureId, SubModuleId, Created, CreatorId, LastUpdated, LastUpdaterId)
		 VALUES
			   (@AppId, @FeatureId, @SubModuleId, GETDATE(), @RqstUserID, GETDATE(), @RqstUserID)
	End
End
return @FeatureId
GO


