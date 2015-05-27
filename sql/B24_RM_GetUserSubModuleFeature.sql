USE [bookdb]
GO

/****** Object:  StoredProcedure [dbo].[B24_RM_GetUserSubModuleFeature]    Script Date: 07/19/2011 11:13:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[B24_RM_GetUserSubModuleFeature]
@UserId uniqueidentifier,
@AppId int = 0,
@ModuleId int = 0,
@SubModuleId int = 0
AS

set nocount on
/*=======================================
Get user specific Feature
========================================*/
SELECT
  Distinct(RM.FeatureId) As FeatureId, RM.SubModuleId, RM.ModuleId, RM.Action
FROM
  RoleManagement RM with (NOLOCK) JOIN
    Role R with (NOLOCK) JOIN
      UserRole UR with (NOLOCK) 
    ON UR.RoleId = R.RoleId AND UR.ApplicationId = R.ApplicationId
  ON R.RoleId = RM.RoleId AND R.ApplicationId = RM.ApplicationId
WHERE
  UR.UserId = @UserId
  AND RM.ApplicationId = @AppId
  AND RM.ModuleId = @ModuleId
  AND RM.SubModuleId = @SubModuleId

GO


