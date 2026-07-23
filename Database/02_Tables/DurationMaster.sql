CREATE TABLE DurationMaster
(
    DurationId INT IDENTITY(1,1) PRIMARY KEY,
    DurationName VARCHAR(100) NOT NULL,
    DurationMonth INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedBy INT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedBy INT NULL,
    ModifiedDate DATETIME NULL
);