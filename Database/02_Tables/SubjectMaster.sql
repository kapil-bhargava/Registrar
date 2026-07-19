CREATE TABLE SubjectMaster
(
    SubjectId       INT IDENTITY(1,1) PRIMARY KEY,
    SubjectCode     NVARCHAR(30)   NOT NULL,
    SubjectName     NVARCHAR(200)  NOT NULL,
    SubjectType     NVARCHAR(20)   NOT NULL,             -- Theory / Practical / Mixed
    SemesterId      INT            NOT NULL,             -- FK -> SemesterMaster
    CourseId        INT            NOT NULL,             -- FK -> CourseMaster
    Credits         INT            NULL,
    Status          NVARCHAR(20)   NOT NULL DEFAULT 'Active',
    CreatedDate     DATETIME       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_SubjectMaster_SemesterMaster FOREIGN KEY (SemesterId) REFERENCES SemesterMaster(SemesterId),
    CONSTRAINT FK_SubjectMaster_CourseMaster FOREIGN KEY (CourseId) REFERENCES CourseMaster(CourseId)
);
