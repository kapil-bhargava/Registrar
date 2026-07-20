

CREATE TABLE Subject
(
    SubjectId       INT IDENTITY(1,1) PRIMARY KEY,
    SubjectCode     NVARCHAR(30)   NOT NULL,
    SubjectName     NVARCHAR(200)  NOT NULL,
    SubjectType     NVARCHAR(20)   NOT NULL,             -- Theory / Practical / Mixed
    SemesterId      INT            NOT NULL,             -- FK -> Semester
    CourseId        INT            NOT NULL,             -- FK -> Course
    Credits         INT            NULL,
    Status          NVARCHAR(20)   NOT NULL DEFAULT 'Active',
    CreatedDate     DATETIME       NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Subject_Semester FOREIGN KEY (SemesterId) REFERENCES Semester(SemesterId),
    CONSTRAINT FK_Subject_Course FOREIGN KEY (CourseId) REFERENCES Course(CourseId)
);
