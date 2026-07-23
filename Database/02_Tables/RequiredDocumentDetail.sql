CREATE TABLE RequiredDocumentDetail (
    RequiredDocumentDetailId INT IDENTITY(1,1) PRIMARY KEY,
    RequiredDocumentId       INT NOT NULL,
    DocumentEnclosureId      INT NOT NULL,
    CONSTRAINT FK_RDD_Header FOREIGN KEY (RequiredDocumentId) REFERENCES RequiredDocumentMaster(RequiredDocumentId) ON DELETE CASCADE,
    CONSTRAINT FK_RDD_Document FOREIGN KEY (DocumentEnclosureId) REFERENCES DocumentEnclosureMaster(DocumentEnclosureId)
);