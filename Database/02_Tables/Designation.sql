CREATE TABLE Designation
(
    DesignationId INT PRIMARY KEY IDENTITY(1,1),
    DesignationName VARCHAR(100),
    DesignationCode VARCHAR(20),
    Level VARCHAR(20),
    IsActive BIT,
    CreatedDate DATETIME
)
