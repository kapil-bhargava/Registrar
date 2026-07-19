CREATE TABLE CourseMaster
(
    CourseId        INT IDENTITY(1,1) PRIMARY KEY,
    CourseCode      NVARCHAR(30)   NOT NULL,
    CourseName      NVARCHAR(200)  NOT NULL,
    ProgramId       INT            NOT NULL,             -- FK -> ProgramMaster
    DepartmentId    INT            NOT NULL,             -- FK -> DepartmentMaster
    DurationYears   INT            NULL,
    TotalSemesters  INT            NULL,
    TotalCredits    INT            NULL,
    IntakeCapacity  INT            NULL,
    Status          NVARCHAR(20)   NOT NULL DEFAULT 'Active',
    CreatedDate     DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_CourseMaster_ProgramMaster FOREIGN KEY (ProgramId) REFERENCES ProgramMaster(ProgramId),
    CONSTRAINT FK_CourseMaster_DepartmentMaster FOREIGN KEY (DepartmentId) REFERENCES DepartmentMaster(DepartmentId)
);
GO