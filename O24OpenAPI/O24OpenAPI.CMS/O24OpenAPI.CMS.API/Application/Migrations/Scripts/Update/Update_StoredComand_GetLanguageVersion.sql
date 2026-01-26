BEGIN TRANSACTION

USE o24cms
go

IF NOT EXISTS (
    SELECT 1
    FROM [dbo].[StoredCommand]
    WHERE [Name] = N'GetLanguagesVersion'
)
BEGIN
INSERT INTO [dbo].[StoredCommand] ([Name], [Query], [Type], [Description], [CreatedOnUtc], [UpdatedOnUtc]) 
VALUES (N'GetLanguagesVersion', N'SELECT Language, Version FROM o24cms.dbo.TranslationLanguages', N'SELECT', NULL, '2025-07-03 10:37:22.5600000', '2025-05-23 10:37:22.5600000');

END

COMMIT