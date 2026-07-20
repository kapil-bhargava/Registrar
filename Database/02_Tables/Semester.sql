CREATE TABLE Semester
(
    SemesterId           INT IDENTITY(1,1) PRIMARY KEY,
    CourseId             INT            NOT NULL,        -- FK -> Course
    AcademicSessionId    INT            NOT NULL,        -- FK -> AcademicSession
    SemesterNumber       INT            NOT NULL,
    SemesterName         NVARCHAR(50)   NULL,
    StartDate            DATE           NULL,
    EndDate              DATE           NULL,
    CreditLimit          INT            NULL,
    Status               NVARCHAR(20)   NOT NULL DEFAULT 'Upcoming',   -- Upcoming/Active/Completed
    CreatedDate          DATETIME       NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Semester_Course FOREIGN KEY (CourseId) REFERENCES Course(CourseId),
    CONSTRAINT FK_Semester_Session FOREIGN KEY (AcademicSessionId) REFERENCES AcademicSession(AcademicSessionId)
);
GO