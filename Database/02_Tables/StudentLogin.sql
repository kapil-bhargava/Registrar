CREATE TABLE StudentLogin
(
    StudentLoginId INT IDENTITY(1,1) PRIMARY KEY,
    ApplicationId  INT NOT NULL UNIQUE,     -- FK -> Application.ApplicationId
    Username       VARCHAR(50) NOT NULL UNIQUE,
    PasswordHash   VARCHAR(64) NOT NULL,
    Status         VARCHAR(20) NOT NULL DEFAULT 'Active',
    CreatedOn      DATETIME NOT NULL DEFAULT GETDATE()
);
GO