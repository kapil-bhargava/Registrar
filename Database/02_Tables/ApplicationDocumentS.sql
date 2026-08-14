CREATE TABLE dbo.ApplicationDocument
(
    ApplicationDocumentId INT IDENTITY(1,1) PRIMARY KEY,

    ApplicationId INT NOT NULL,

    DocumentEnclosureId INT NOT NULL,

    IsSubmitted BIT NOT NULL DEFAULT 0,

    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_ApplicationDocument_Application
        FOREIGN KEY (ApplicationId)
        REFERENCES dbo.Application(ApplicationId),

    CONSTRAINT FK_ApplicationDocument_DocumentEnclosure
        FOREIGN KEY (DocumentEnclosureId)
        REFERENCES dbo.DocumentEnclosureMaster(DocumentEnclosureId),

    CONSTRAINT UQ_ApplicationDocument
        UNIQUE (ApplicationId, DocumentEnclosureId)
);

SELECT OBJECT_ID('dbo.ApplicationDocument') AS ApplicationDocumentId;

SELECT *
FROM dbo.ApplicationDocument;

INSERT INTO dbo.ApplicationDocument
(
    ApplicationId,
    DocumentEnclosureId,
    IsSubmitted,
    CreatedDate
)
SELECT
    a.ApplicationId,
    d.DocumentEnclosureId,
    0,
    GETDATE()
FROM dbo.Application a
CROSS JOIN dbo.DocumentEnclosureMaster d
WHERE d.IsActive = 1
AND NOT EXISTS
(
    SELECT 1
    FROM dbo.ApplicationDocument ad
    WHERE ad.ApplicationId = a.ApplicationId
      AND ad.DocumentEnclosureId = d.DocumentEnclosureId
);


SELECT *
FROM dbo.ApplicationDocument
ORDER BY ApplicationId, DocumentEnclosureId;