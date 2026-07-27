IF OBJECT_ID('Student') IS NULL
BEGIN
    CREATE TABLE Student (
        StudentId          NVARCHAR(20) PRIMARY KEY,       -- e.g. STU-20261000
        ApplicationId      INT NOT NULL FOREIGN KEY REFERENCES Application(ApplicationId),
        FullName           NVARCHAR(150 NOT NULL,
        CourseId           INT NOT NULL FOREIGN KEY REFERENCES CourseMaster(CourseId),
        CategoryId         INT NOT NULL FOREIGN KEY REFERENCES CategoryMa(CategoryId),
        AcademicSessionId  INT NOT NULL FOREIGN KEY REFERENCES AcademicSession(AcademicSessionId),
        SeatNumber         NVARCHAR(50) NULL,
        AdmittedOn         DATETIME NOT NULL DEFAULT GETDATE()
    )
END
GO