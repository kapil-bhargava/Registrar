CREATE TABLE CounsellingTypeMaster
(
    CounsellingTypeId INT PRIMARY KEY IDENTITY(1,1),
    CounsellingTypeName VARCHAR(100),
    IsActive BIT,
    CreatedBy INT NULL,
    CreatedDate DATETIME,
    ModifiedBy INT NULL,
    ModifiedDate DATETIME NULL
)
GO
