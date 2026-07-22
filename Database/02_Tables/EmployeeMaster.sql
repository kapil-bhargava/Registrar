CREATE TABLE EmployeeMasters
(
    EmployeeId INT PRIMARY KEY IDENTITY(1,1),
    EmployeeCode VARCHAR(50),
    FullName VARCHAR(150),
    FatherName VARCHAR(150),
    Email VARCHAR(150),
    Phone VARCHAR(50),
    Gender VARCHAR(20),
    DateOfBirth DATE NULL,
    DateOfJoining DATE NULL,
    Address VARCHAR(300),
    Qualification VARCHAR(200),
    Experience VARCHAR(100),
    IsActive BIT,
    CreatedDate DATETIME
)

