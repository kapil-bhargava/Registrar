CREATE TABLE Category
(
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    CategoryCode VARCHAR(20),
    CategoryName VARCHAR(100),
    Description VARCHAR(250),
    FeeConcession DECIMAL(5,2),
    DisplayOrder INT,
    IsActive BIT,
    CreatedDate DATETIME
)