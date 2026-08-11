USE UniversityERP;
GO

CREATE PROCEDURE sp_RegistrarLogin_Validate
    @Username NVARCHAR(100),
    @PasswordHash NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        LoginId,
        Username
    FROM RegistrarLogin
    WHERE Username = @Username
      AND PasswordHash = @PasswordHash;
END
GO


USE UniversityERP;
GO

CREATE TABLE RegistrarLogin
(
    LoginId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(256) NOT NULL
);
GO



INSERT INTO RegistrarLogin
(
    Username,
    PasswordHash
)
VALUES
(
    'Registrar',
    '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'
);
GO