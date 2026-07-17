CREATE TABLE Campus
(
    CampusId INT PRIMARY KEY IDENTITY(1,1),
    CampusName VARCHAR(150),
    CampusCode VARCHAR(20),
    CampusTypeId INT NULL,
    Capacity INT NULL,
    Address VARCHAR(500),
    ContactNumber VARCHAR(20),
    Email VARCHAR(100),
    Dean VARCHAR(150),
    IsActive BIT,
    CreatedDate DATETIME,
    CONSTRAINT FK_Campus_CampusCategory FOREIGN KEY (CampusTypeId) REFERENCES CampusCategory(CampusCategoryId)
)
