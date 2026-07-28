CREATE TABLE CourseStructureMapping
(
    MappingId       INT IDENTITY(1,1) PRIMARY KEY,
    CourseId        INT NOT NULL,          -- FK -> CourseMaster
    BranchId        INT NOT NULL,          -- FK -> BranchMaster
    SemesterNumber  INT NOT NULL,          -- 1,2,3... TotalSemesters tak
    IsActive        BIT NOT NULL DEFAULT 1,
    CreatedDate     DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_CSM_Course FOREIGN KEY (CourseId) REFERENCES CourseMaster(CourseId),
    CONSTRAINT FK_CSM_Branch FOREIGN KEY (BranchId) REFERENCES BranchMaster(BranchId)
);
GO


