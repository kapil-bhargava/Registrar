CREATE TABLE UniversityType
(
    UniversityTypeId INT PRIMARY KEY IDENTITY(1,1),

    UniversityTypeCode VARCHAR(20),

    UniversityTypeName VARCHAR(100),

    Description VARCHAR(250),

    DisplayOrder INT,

    IsActive BIT,

    CreatedDate DATETIME
)

