CREATE TABLE CourseBranchSemesterMapping (
    MappingId   INT IDENTITY(1,1) PRIMARY KEY,
    CourseId    INT NOT NULL FOREIGN KEY REFERENCES CourseMaster(CourseId),
    BranchId    INT NOT NULL FOREIGN KEY REFERENCES BranchMaster(BranchId),
    SemesterId  INT NOT NULL FOREIGN KEY REFERENCES SemesterMaster(SemesterId),
    IsActive    BIT NOT NULL DEFAULT(1),
    CreatedDate DATETIME NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT UQ_CourseBranchSemester UNIQUE (CourseId, BranchId, SemesterId)
);
GO