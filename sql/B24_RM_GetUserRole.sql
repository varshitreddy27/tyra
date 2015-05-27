USE [bookdb]
GO

/****** Object:  StoredProcedure [dbo].[B24_RM_GetUserRole]    Script Date: 07/19/2011 11:12:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[B24_RM_GetUserRole] 
@UserId uniqueidentifier,
@AppId int
AS

set nocount on
/*=======================================
Get role for the user
========================================*/
SELECT
	R.RoleId, Description As Role
FROM
	Role R with (NOLOCK) JOIN
		UserRole UR with (NOLOCK)
	ON  UR.RoleId = R.RoleId AND UR.ApplicationId = R.ApplicationId
WHERE
	UR.UserId = @UserId
	AND UR.ApplicationId = @AppId
GO


