CREATE TABLE AcademicSession
(
    AcademicSessionId   INT IDENTITY(1,1) PRIMARY KEY,
    SessionName          NVARCHAR(150)  NOT NULL,
    SessionCode           NVARCHAR(50)   NOT NULL,
    SessionTypeId          INT            NOT NULL,      -- FK -> SessionType
    StartDate                DATE           NOT NULL,
    EndDate                   DATE           NOT NULL,
    AcademicYear               NVARCHAR(20)   NULL,
    Status                      NVARCHAR(20)   NOT NULL DEFAULT 'Draft',  -- Draft/Active/Locked/Archived
    MaxCredits                   INT            NULL,
    CreatedDate                    DATETIME       NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_AcademicSession_SessionType FOREIGN KEY (SessionTypeId) REFERENCES SessionType(SessionTypeId)
);
