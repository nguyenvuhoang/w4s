-- o24dwh
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'o24dwh')
BEGIN
    CREATE DATABASE [o24dwh] CONTAINMENT = NONE;
END
GO

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.sql_logins WHERE name = 'o24dwh')
BEGIN
    CREATE LOGIN [o24dwh] WITH PASSWORD = N'o24dwh', 
        DEFAULT_DATABASE = [o24dwh], 
        DEFAULT_LANGUAGE = [us_english], 
        CHECK_EXPIRATION = OFF, 
        CHECK_POLICY = OFF;
END
GO

USE o24dwh;
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'o24dwh')
BEGIN
    CREATE USER [o24dwh] FOR LOGIN [o24dwh];
END
GO

USE master;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.server_permissions p
    JOIN sys.server_principals sp ON p.grantee_principal_id = sp.principal_id
    WHERE sp.name = 'o24dwh' AND p.permission_name = 'CONTROL SERVER'
)
BEGIN
    GRANT CONTROL SERVER TO [o24dwh];
END
GO