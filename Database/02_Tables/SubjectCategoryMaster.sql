CREATE TABLE SubjectCategoryMaster
(
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    CategoryName VARCHAR(100) NOT NULL,
    CategoryType VARCHAR(100) NOT NULL,
    CreditApplicable BIT NOT NULL,
    MarksApplicable BIT NOT NULL,
    PassingMarksRequired BIT NOT NULL,
    IsActive BIT NOT NULL,
    DisplayOrder INT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
)