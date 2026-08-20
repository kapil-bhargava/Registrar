CREATE TABLE CourseBranchMapping (
    MappingId   INT IDENTITY(1,1) PRIMARY KEY,
    CourseId    INT NOT NULL,
    BranchId    INT NOT NULL,
    IsActive    BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_CBM_Course FOREIGN KEY (CourseId) REFERENCES CourseMaster(CourseId),
    CONSTRAINT FK_CBM_Branch FOREIGN KEY (BranchId) REFERENCES BranchMaster(BranchId),
    CONSTRAINT UQ_CBM_Course_Branch UNIQUE (CourseId, BranchId)
)