
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Users] (
        [UserID]    INT            IDENTITY (1, 1) NOT NULL,
        [FirstName] NVARCHAR (50)  NULL,
        [LastName]  NVARCHAR (50)  NULL,
        [Email]     NVARCHAR (100) NULL,
        [Password]  NVARCHAR (50)  NULL, 
        [Role]      NVARCHAR (20)  DEFAULT ('User') NULL,
        PRIMARY KEY CLUSTERED ([UserID] ASC),
        UNIQUE NONCLUSTERED ([Email] ASC)
    );
END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'user_id' AND Object_ID = Object_ID(N'dbo.Tickets'))
BEGIN
    ALTER TABLE Tickets ADD user_id INT NULL;
END
GO

IF OBJECT_ID('dbo.sp_RegisterUser', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_RegisterUser;
GO

CREATE PROCEDURE sp_RegisterUser
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @Password NVARCHAR(50)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
    BEGIN
        SELECT 0; 
    END
    ELSE
    BEGIN
        INSERT INTO Users (FirstName, LastName, Email, Password, Role)
        VALUES (@FirstName, @LastName, @Email, @Password, 'User');
        SELECT 1; 
    END
END
GO

IF OBJECT_ID('dbo.sp_LoginUser', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_LoginUser;
GO

CREATE PROCEDURE sp_LoginUser
    @Email NVARCHAR(100),
    @Password NVARCHAR(50)
AS
BEGIN
    SELECT UserID, FirstName, LastName, Email, Role 
    FROM Users 
    WHERE Email = @Email AND Password = @Password;
END
GO