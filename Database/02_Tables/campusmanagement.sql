CREATE TABLE CampusCategory
(
    CampusCategoryId INT PRIMARY KEY IDENTITY(1,1),
    CampusCategoryCode VARCHAR(20),
    CampusCategoryName VARCHAR(100),
    Description VARCHAR(250),
    DisplayOrder INT,
    IsActive BIT,
    CreatedDate DATETIME
)

