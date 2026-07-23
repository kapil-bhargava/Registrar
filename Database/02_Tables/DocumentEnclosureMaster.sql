BEGIN
    CREATE TABLE [dbo].[DocumentEnclosureMaster]
    (
        DocumentEnclosureId INT IDENTITY(1,1) PRIMARY KEY,
        DocumentName        NVARCHAR(200)  NOT NULL,
        ShortName           NVARCHAR(50)   NOT NULL,
        IsMandatory         BIT            NOT NULL DEFAULT (0),
        IsOriginalRequired  BIT            NOT NULL DEFAULT (0),
        IsActive            BIT            NOT NULL DEFAULT (1),
        CreatedDate         DATETIME       NOT NULL DEFAULT (GETDATE())
    );
END
GO