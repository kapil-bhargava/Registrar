
CREATE TABLE Course
(
    CourseId          INT IDENTITY(1,1) PRIMARY KEY,
    CourseCode        NVARCHAR(30)   NOT NULL,
    CourseName        NVARCHAR(200)  NOT NULL,
    ProgramId         INT            NOT NULL,          -- FK -> Program
    DepartmentId      INT            NOT NULL,          -- FK -> Department
    DurationYears     INT            NULL,
    TotalSemesters    INT            NULL,
    TotalCredits      INT            NULL,
    IntakeCapacity    INT            NULL,
    Status            NVARCHAR(20)   NOT NULL DEFAULT 'Active',
    CreatedDate       DATETIME       NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Course_Program FOREIGN KEY (ProgramId) REFERENCES Program(ProgramId),
    CONSTRAINT FK_Course_Department FOREIGN KEY (DepartmentId) REFERENCES Department(DepartmentId)
);
GO