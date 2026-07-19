CREATE TABLE SemesterMaster
(
    SemesterId          INT IDENTITY(1,1) PRIMARY KEY,
    CourseId            INT            NOT NULL,         -- FK -> CourseMaster
    AcademicSessionId   INT            NOT NULL,         -- FK -> AcademicSession
    SemesterNumber      INT            NOT NULL,
    SemesterName        NVARCHAR(100)  NULL,
    StartDate           DATE           NULL,
    EndDate             DATE           NULL,
    CreditLimit         INT            NULL,
    Status              NVARCHAR(20)   NOT NULL DEFAULT 'Upcoming',
    CreatedDate         DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_SemesterMaster_CourseMaster FOREIGN KEY (CourseId) REFERENCES CourseMaster(CourseId),
    CONSTRAINT FK_SemesterMaster_AcademicSession FOREIGN KEY (AcademicSessionId) REFERENCES AcademicSession(AcademicSessionId)
);
