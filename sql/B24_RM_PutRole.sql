SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[B24_RM_PutRole]
@AppId int, 
@RoleId int,
@Description varchar(50),
@RqstUserID uniqueidentifier
AS

set nocount on
/*=====================================================================================
Update or insert role details. 
=======================================================================================*/
Declare @Count Int 
Set @Count=0

SELECT 
	@Count =Count(*)
FROM
	[Role] with (NOLOCK) 
WHERE
	RoleId = @RoleId
	
if(@Count = 0)
	Begin
		Select @RoleId = Max(RoleId) + 1 From [Role]
		INSERT INTO [Role]
			   (ApplicationId, RoleId, Description, Created, CreatorId, LastUpdated, LastUpdaterId)
		 VALUES
			   (@AppId, @RoleId, @Description, GETDATE(), @RqstUserID, GETDATE(), @RqstUserID)
	End
Else
	Begin
		UPDATE 
			[Role] 
		SET 
			ApplicationId  = @AppId, RoleId = @RoleId, Description = @Description, 
			LastUpdated=GETDATE(),  LastUpdaterId = @RqstUserID 
		WHERE
			RoleId = @RoleId
	End
return @RoleId
GO


