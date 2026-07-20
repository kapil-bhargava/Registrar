CREATE TABLE HODManagement
(
    HODId             INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentId      INT            NOT NULL,          -- FK -> Department
    HODName           NVARCHAR(150)  NOT NULL,
    EmployeeId        NVARCHAR(30)   NULL,
    Designation       NVARCHAR(100)  NULL,
    Qualification     NVARCHAR(150)  NULL,
    Email             NVARCHAR(150)  NULL,
    Phone             NVARCHAR(20)   NULL,
    EffectiveDate     DATE           NULL,
    TenureEndDate     DATE           NULL,
    ReasonForChange   NVARCHAR(255)  NULL,               -- reason recorded when THIS record was relieved
    Status            NVARCHAR(20)   NOT NULL DEFAULT 'Active',   -- Active / Relieved / Vacant / Pending
    CreatedDate       DATETIME       NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_HOD_Department FOREIGN KEY (DepartmentId) REFERENCES Department(DepartmentId)
);