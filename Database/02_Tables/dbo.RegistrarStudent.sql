IF OBJECT_ID('dbo.RegistrarStudent', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.RegistrarStudent
    (
        RegistrarId               INT IDENTITY(1,1) PRIMARY KEY,
        StudentName                NVARCHAR(150)   NOT NULL,
        Email                       NVARCHAR(150)   NOT NULL,
        Mobile                      NVARCHAR(20)    NULL,
        CourseId                    INT             NOT NULL,
        BranchId                    INT             NULL,
        SemesterId                  INT             NULL,
        RequiredDocumentIdsCsv      NVARCHAR(MAX)   NULL,
        SubmittedDocumentIdsCsv     NVARCHAR(MAX)   NULL,
        IsActive                    BIT             NOT NULL DEFAULT (1),
        CreatedDate                 DATETIME        NOT NULL DEFAULT (GETDATE())
    );
END
GO