USE [bookdb]
GO

/****** Object:  StoredProcedure [dbo].[B24_RM_PutUserRole]    Script Date: 07/19/2011 11:14:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[B24_RM_PutUserRole]
@UserId uniqueidentifier,
@RqstUserID uniqueidentifier,
@AppId int, 
@RoleId int
AS

set nocount on
/*=======================================
Update or insert role for the user
========================================*/
Declare @Count Int 
Set @Count=0

SELECT 
	@Count =Count(*)
FROM
	UserRole UR with (NOLOCK) 
WHERE
	UR.ApplicationId = @AppId
	AND UR.UserId = @UserId

if(@Count = 0)
	Begin
		INSERT INTO UserRole
			   (ApplicationId, RoleId, UserId, Created, CreatorId, LastUpdated, LastUpdaterId)
		 VALUES
			   (@AppId, @RoleId, @UserId, GETDATE(), @RqstUserID, GETDATE(), @RqstUserID)
	End
Else
	Begin
		UPDATE 
			UserRole 
		SET 
			RoleId  = @RoleId,  LastUpdated=GETDATE(),  LastUpdaterId = @RqstUserID 
		WHERE
			ApplicationId = @AppId
			AND	UserId = @UserId
	End


GO


