CREATE TABLE FacultyAssignment
(
    FacultyId INT IDENTITY(1,1) PRIMARY KEY,

    EmployeeId INT NOT NULL,
    DepartmentId INT NOT NULL,
    BranchId INT NOT NULL,
    SemesterId INT NOT NULL,
    DesignationId INT NOT NULL,

    Status VARCHAR(20) NOT NULL DEFAULT 'Active',

    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_FacultyAssignment_Employee
        FOREIGN KEY (EmployeeId)
        REFERENCES EmployeeMaster(EmployeeId),

    CONSTRAINT FK_FacultyAssignment_Department
        FOREIGN KEY (DepartmentId)
        REFERENCES DepartmentMaster(DepartmentId),

    CONSTRAINT FK_FacultyAssignment_Branch
        FOREIGN KEY (BranchId)
        REFERENCES BranchMaster(BranchId),

    CONSTRAINT FK_FacultyAssignment_Semester
        FOREIGN KEY (SemesterId)
        REFERENCES SemesterMaster(SemesterId),

    CONSTRAINT FK_FacultyAssignment_Designation
        FOREIGN KEY (DesignationId)
        REFERENCES Designation(DesignationId)
);

--