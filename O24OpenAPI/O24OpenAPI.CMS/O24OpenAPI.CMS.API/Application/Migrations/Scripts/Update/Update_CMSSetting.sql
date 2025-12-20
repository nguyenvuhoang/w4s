BEGIN TRANSACTION

USE o24cms
go

IF NOT EXISTS (
    SELECT 1
    FROM [dbo].[Setting]
    WHERE [Name] = N'CMSSetting.STLUri'
)
BEGIN
    INSERT INTO [dbo].[Setting] ([Name], [Value], [OrganizationId]) 
	VALUES (N'CMSSetting.STLUri', N'https://192.168.1.138:5050', 0);
END

COMMIT