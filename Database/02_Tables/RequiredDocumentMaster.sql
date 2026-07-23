CREATE TABLE RequiredDocumentMaster (
    RequiredDocumentId INT IDENTITY(1,1) PRIMARY KEY,
    AcademicSessionId  INT NOT NULL,
    AdmissionModeId    INT NOT NULL,
    ProgramId          INT NOT NULL,
    CourseId           INT NOT NULL,
    CategoryId         INT NOT NULL,
    IsActive           BIT DEFAULT 1,
    CreatedDate         DATETIME DEFAULT GETDATE()
);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_RDM_Session')
BEGIN
    ALTER TABLE RequiredDocumentMaster
    ADD CONSTRAINT FK_RDM_Session FOREIGN KEY (AcademicSessionId) REFERENCES AcademicSession(AcademicSessionId);
END

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_RDM_AdmissionMode')
BEGIN
    ALTER TABLE RequiredDocumentMaster
    ADD CONSTRAINT FK_RDM_AdmissionMode FOREIGN KEY (AdmissionModeId) REFERENCES AdmissionModeMaster(AdmissionModeId);
END

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_RDM_Program')
BEGIN
    ALTER TABLE RequiredDocumentMaster
    ADD CONSTRAINT FK_RDM_Program FOREIGN KEY (ProgramId) REFERENCES ProgramMaster(ProgramId);
END

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_RDM_Course')
BEGIN
    ALTER TABLE RequiredDocumentMaster
    ADD CONSTRAINT FK_RDM_Course FOREIGN KEY (CourseId) REFERENCES CourseMaster(CourseId);
END

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_RDM_Category')
BEGIN
    ALTER TABLE RequiredDocumentMaster
    ADD CONSTRAINT FK_RDM_Category FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId);
END
IF OBJECT_ID('RequiredDocumentDetail', 'U') IS NULL
BEGIN
    CREATE TABLE RequiredDocumentDetail (
        RequiredDocumentDetailId INT IDENTITY(1,1) PRIMARY KEY,
        RequiredDocumentId       INT NOT NULL,
        DocumentEnclosureId      INT NOT NULL,
        CONSTRAINT FK_RDD_Header FOREIGN KEY (RequiredDocumentId) REFERENCES RequiredDocumentMaster(RequiredDocumentId) ON DELETE CASCADE,
        CONSTRAINT FK_RDD_Document FOREIGN KEY (DocumentEnclosureId) REFERENCES DocumentEnclosureMaster(DocumentEnclosureId)
    );
END


SELECT name AS ForeignKeyName
FROM sys.foreign_keys
WHERE parent_object_id = OBJECT_ID('RequiredDocumentMaster');


SELECT name AS ForeignKeyName
FROM sys.foreign_keys
WHERE parent_object_id = OBJECT_ID('RequiredDocumentDetail');



