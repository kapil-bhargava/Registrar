IF OBJECT_ID('dbo.FeeHeadMaster', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.FeeHeadMaster
    (
        FeeHeadId    INT IDENTITY(1,1) PRIMARY KEY,
        FeeHeadCode  NVARCHAR(20)  NOT NULL,
        FeeHeadName  NVARCHAR(100) NOT NULL,
        Description  NVARCHAR(250) NULL,
        IsActive     BIT NOT NULL DEFAULT 1,
        CreatedOn    DATETIME NOT NULL DEFAULT GETDATE()
    );
END