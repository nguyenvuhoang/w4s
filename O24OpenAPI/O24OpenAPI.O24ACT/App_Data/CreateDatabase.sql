-- o24act
USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'o24act')
BEGIN
    CREATE DATABASE [o24act] CONTAINMENT = NONE;
END
GO

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.sql_logins WHERE name = 'o24act')
BEGIN
    CREATE LOGIN [o24act] WITH PASSWORD = N'o24act', 
        DEFAULT_DATABASE = [o24act], 
        DEFAULT_LANGUAGE = [us_english], 
        CHECK_EXPIRATION = OFF, 
        CHECK_POLICY = OFF;
END
GO

USE o24act;
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'o24act')
BEGIN
    CREATE USER [o24act] FOR LOGIN [o24act];
END
GO

USE master;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.server_permissions p
    JOIN sys.server_principals sp ON p.grantee_principal_id = sp.principal_id
    WHERE sp.name = 'o24act' AND p.permission_name = 'CONTROL SERVER'
)
BEGIN
    GRANT CONTROL SERVER TO [o24act];
END
GO
