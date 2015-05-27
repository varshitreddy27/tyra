
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[B24_RM_PutApplication]
@AppId int, 
@AppName varchar(50),
@RqstUserID uniqueidentifier
AS

set nocount on
/*=======================================
Update or insert application info
========================================*/
Declare @Count Int 
Set @Count=0

SELECT 
	@Count =Count(*)
FROM
	SystemApplication with (NOLOCK) 
WHERE
	ApplicationId = @AppId
	
if(@Count = 0)
	Begin
		Select @AppId = Max(ApplicationId) + 1 From SystemApplication

		INSERT INTO SystemApplication
			   (ApplicationId, ApplicationName, Created, CreatorId, LastUpdated, LastUpdaterId)
		 VALUES
			   (@AppId, @AppName, GETDATE(), @RqstUserID, GETDATE(), @RqstUserID)
	End
Else
	Begin
		UPDATE 
			SystemApplication 
		SET 
			ApplicationName  = @AppName,  LastUpdated=GETDATE(),  LastUpdaterId = @RqstUserID 
		WHERE
			ApplicationId = @AppId
	End

return @AppId
GO


