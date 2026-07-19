CREATE TABLE DepartmentMaster
(
    DepartmentId    INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentCode  NVARCHAR(30)   NOT NULL,
    DepartmentName  NVARCHAR(200)  NOT NULL,
    FacultyId       INT            NOT NULL,             -- FK -> FacultyMaster
    CampusName      NVARCHAR(100)  NULL,
    IsActive        BIT            NOT NULL DEFAULT 1,
    CreatedDate     DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_DepartmentMaster_FacultyMaster FOREIGN KEY (FacultyId) REFERENCES FacultyMaster(FacultyId)
);
GO