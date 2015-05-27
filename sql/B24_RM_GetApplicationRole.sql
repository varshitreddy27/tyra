USE [bookdb]
GO

/****** Object:  StoredProcedure [dbo].[B24_RM_GetApplicationRole]    Script Date: 07/19/2011 11:12:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[B24_RM_GetApplicationRole]
@AppId int
AS

set nocount on
/*=======================================
Get all role based on the application
========================================*/
SELECT 
	RoleId, Description As Role
FROM
	Role WITH (NOLOCK)
WHERE
	ApplicationId = @AppId
ORDER BY
	Description
GO


