CREATE TABLE FacultyMaster
(
    FacultyId       INT IDENTITY(1,1) PRIMARY KEY,
    FacultyCode     NVARCHAR(30)   NOT NULL,
    FacultyName     NVARCHAR(200)  NOT NULL,
    Description     NVARCHAR(500)  NULL,
    IsActive        BIT            NOT NULL DEFAULT 1,
    CreatedDate     DATETIME       NOT NULL DEFAULT GETDATE()
);