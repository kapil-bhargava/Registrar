CREATE TABLE Department
(
    DepartmentId    INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentCode  NVARCHAR(20)   NOT NULL,
    DepartmentName  NVARCHAR(150)  NOT NULL,
    FacultyId       INT            NOT NULL,          -- FK -> Faculty
    CampusName      NVARCHAR(100)  NULL,
    IsActive        BIT            NOT NULL DEFAULT 1,
    CreatedDate     DATETIME       NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Department_Faculty FOREIGN KEY (FacultyId) REFERENCES Faculty(FacultyId)
);