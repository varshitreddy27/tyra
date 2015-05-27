
/****** Object:  StoredProcedure [dbo].[B24_RM_GetApplicationId]    Script Date: 07/26/2011 21:01:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[B24_RM_GetApplicationId]
@ApplicationName varchar(50)
AS
DECLARE @err_message nvarchar(255)

set nocount on
/*=======================================
Get ApplicationID based Applicaiton name
========================================*/
IF EXISTS(SELECT ApplicationId FROM SystemApplication with(nolock)WHERE ApplicationName = @ApplicationName)
BEGIN
	SELECT 
		ApplicationId
	FROM 
		SystemApplication with(nolock)
	WHERE 
		ApplicationName = @ApplicationName
	return @@rowcount
END
ELSE
BEGIN
	SET @err_message = 'Invalid Application Name'
	raiserror (@err_message,15, 1)
END
