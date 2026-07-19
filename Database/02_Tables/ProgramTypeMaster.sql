CREATE TABLE ProgramTypeMaster
(
    ProgramTypeId     INT IDENTITY(1,1) PRIMARY KEY,
    ProgramTypeCode   NVARCHAR(30)   NOT NULL,          -- e.g. UG, PG, DIP, CERT
    ProgramTypeName   NVARCHAR(100)  NOT NULL,           -- e.g. Undergraduate
    TypicalDuration   NVARCHAR(50)   NULL,                -- e.g. 3-4 Years
    DisplayOrder      INT            NULL,
    IsActive          BIT            NOT NULL DEFAULT 1,
    CreatedDate       DATETIME       NOT NULL DEFAULT GETDATE()
);