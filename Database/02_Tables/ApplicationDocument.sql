IF OBJECT_ID('ApplicationDocument') IS NULL
BEGIN
    CREATE TABLE ApplicationDocument (
        ApplicationDocumentId INT IDENTITY(1,1) PRIMARY KEY,
        ApplicationId         INT NOT NULL FOREIGN KEY REFERENCES Application(ApplicationId),
        DocumentEnclosureId   INT NOT NULL FOREIGN KEY REFERENCES DocumentEnclosureMaster(DocumentEnclosureId),
        IsSubmitted           BIT NOT NULL DEFAULT 0,
        CONSTRAINT UQ_AppDoc UNIQUE (ApplicationId, DocumentEnclosureId)
    )
END
GO