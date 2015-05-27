SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[B24_RM_GetUserSubModule]
@UserId uniqueidentifier,
@AppId int = 0,
@ModuleId int = 0
AS

set nocount on
/*=======================================
Get user specific Submodule
========================================*/
SELECT
  Distinct(SM.SubModuleId) AS SubModuleId, RM.ModuleId, MSM.OrderIndex
FROM
ModuleSubModule MSM with (NOLOCK) JOIN
  SubModule SM with (NOLOCK) JOIN
    RoleManagement RM with (NOLOCK) JOIN
      Role R with (NOLOCK) JOIN
        UserRole UR with (NOLOCK)
      ON UR.RoleId = R.RoleId  AND UR.ApplicationId = R.ApplicationId
    ON R.RoleId = RM.RoleId  AND R.ApplicationId = RM.ApplicationId
  ON RM.SubModuleId = SM.SubModuleId AND RM.ApplicationId = SM.ApplicationId
ON SM.SubModuleId = MSM.SubModuleId AND SM.ApplicationId = MSM.ApplicationId
WHERE
  UR.UserId = @UserId
  AND UR.ApplicationId = @AppId
  AND RM.ModuleId = @ModuleId
ORDER BY
	MSM.OrderIndex ASC
