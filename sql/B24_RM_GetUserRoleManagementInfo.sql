SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[B24_RM_GetUserRoleManagementInfo]
@UserId uniqueidentifier,
@AppId int
AS

set nocount on
/*===================================================================
Get the full list of roll mapping for the given user and application.
=====================================================================*/
SELECT
  RM.ManageId, RM.ApplicationId, RM.RoleId, RM.ModuleId, M.LabelText AS Module, RM.SubModuleId, SM.LabelText AS SubModule,
  RM.FeatureId, F.LabelText AS Feature, RM.Action
FROM
Feature F with (NOLOCK) Right JOIN
  ModuleSubModule MSM with (NOLOCK) Right JOIN
    SubModule SM with (NOLOCK) Right JOIN
      Module M with (NOLOCK)  JOIN
        RoleManagement RM with (NOLOCK) JOIN
          Role R with (NOLOCK) JOIN
		    UserRole UR with (NOLOCK)
          ON UR.RoleId = R.RoleId AND UR.ApplicationId = R.ApplicationId
        ON R.RoleId = RM.RoleId AND R.ApplicationId = RM.ApplicationId
      ON RM.ModuleId = M.ModuleId AND RM.ApplicationId = M.ApplicationId
    ON RM.SubModuleId = SM.SubModuleId AND RM.ApplicationId = SM.ApplicationId
  ON SM.SubModuleId = MSM.SubModuleId AND SM.ApplicationId = MSM.ApplicationId And MSM.ModuleId = M.ModuleId
ON RM.FeatureId = F.FeatureId AND RM.ApplicationId = F.ApplicationId
WHERE
  UR.UserId = @UserId
  AND UR.ApplicationId = @AppId 
ORDER BY
	M.OrderIndex, MSM.OrderIndex
