CREATE TABLE BranchMaster
(
    BranchId INT IDENTITY(1,1) PRIMARY KEY,

    BranchCode NVARCHAR(30) NOT NULL,

    BranchName NVARCHAR(200) NOT NULL,

    DepartmentId INT NOT NULL,

    ProgramId INT NOT NULL,

    CampusName NVARCHAR(100) NULL,

    IntakeCapacity INT NULL,

    IsActive BIT NOT NULL DEFAULT 1,

    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Branch_Department
        FOREIGN KEY (DepartmentId)
        REFERENCES DepartmentMaster(DepartmentId),

    CONSTRAINT FK_Branch_Program
        FOREIGN KEY (ProgramId)
        REFERENCES ProgramMaster(ProgramId)
);