CREATE TABLE Faculty
(
    FacultyId     INT IDENTITY(1,1) PRIMARY KEY,
    FacultyCode   NVARCHAR(20)   NOT NULL,
    FacultyName   NVARCHAR(150)  NOT NULL,
    Description   NVARCHAR(255)  NULL,
    IsActive      BIT            NOT NULL DEFAULT 1,
    CreatedDate   DATETIME       NOT NULL DEFAULT GETDATE()
);
