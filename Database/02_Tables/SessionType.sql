

CREATE TABLE SessionType
(
    SessionTypeId    INT IDENTITY(1,1) PRIMARY KEY,
    SessionTypeCode  NVARCHAR(20)   NOT NULL,          -- e.g. ANNUAL, SEM, TRI
    SessionTypeName  NVARCHAR(100)  NOT NULL,          -- e.g. Annual, Semester, Trimester
    Description      NVARCHAR(255)  NULL,
    DisplayOrder      INT            NULL,
    IsActive           BIT            NOT NULL DEFAULT 1,
    CreatedDate         DATETIME       NOT NULL DEFAULT GETDATE()
);

