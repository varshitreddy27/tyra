USE [bookdb]
GO

/****** Object:  StoredProcedure [dbo].[B24_RM_GetUserModule]    Script Date: 07/19/2011 11:12:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[B24_RM_GetUserModule]
@UserId uniqueidentifier,
@AppId int = 0
AS

set nocount on
/*=======================================
Get user specific Module
========================================*/
SELECT
  Distinct(M.ModuleId) AS ModuleId, M.OrderIndex
FROM
  Module M with (NOLOCK) JOIN
    RoleManagement RM with (NOLOCK) JOIN
      Role R with (NOLOCK) JOIN
        UserRole UR with (NOLOCK)
      ON UR.RoleId = R.RoleId AND UR.ApplicationId = R.ApplicationId
    ON R.RoleId = RM.RoleId AND R.ApplicationId = RM.ApplicationId
  ON RM.ModuleId = M.ModuleId AND RM.ApplicationId = M.ApplicationId
WHERE
  UR.UserId = @UserId
  AND UR.ApplicationId = @AppId
ORDER BY
	M.OrderIndex ASC

GO


