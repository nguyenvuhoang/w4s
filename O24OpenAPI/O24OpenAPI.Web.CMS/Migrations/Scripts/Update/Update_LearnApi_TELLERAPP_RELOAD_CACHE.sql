BEGIN TRANSACTION

USE o24cms
go

UPDATE [dbo].[LearnApi] SET [URI] = N'$CMSSetting.STLUri$/api/caching/reloadcache' WHERE [LearnApiId] = N'TELLERAPP_RELOAD_CACHE';

COMMIT