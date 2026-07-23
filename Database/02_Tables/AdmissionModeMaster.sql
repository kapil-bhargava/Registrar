CREATE TABLE AdmissionModeMaster
(
    AdmissionModeId INT PRIMARY KEY IDENTITY(1,1),
    AdmissionModeName VARCHAR(100),
    IsActive BIT,
    CreatedBy INT NULL,
    CreatedDate DATETIME,
    ModifiedBy INT NULL,
    ModifiedDate DATETIME NULL
)
GO