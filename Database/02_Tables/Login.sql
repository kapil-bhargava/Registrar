CREATE TABLE RegistrarLogin (
    LoginId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE()
);


CREATE PROCEDURE sp_RegistrarLogin_Validate
    @Username NVARCHAR(100),
    @PasswordHash NVARCHAR(256)
AS
BEGIN
    SELECT LoginId, Username
    FROM RegistrarLogin
    WHERE Username = @Username
      AND PasswordHash = @PasswordHash
      AND IsActive = 1
END



INSERT INTO RegistrarLogin (Username, PasswordHash)
VALUES ('Registrar', '4321');


select * from RegistrarLogin 

DELETE FROM RegistrarLogin 
WHERE LoginId = 1;


UPDATE RegistrarLogin 
SET PasswordHash = 'R#4321'
WHERE Username = 'Registrar';

UPDATE RegistrarLogin
SET PasswordHash = '5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5'
WHERE Username = 'Registrar';