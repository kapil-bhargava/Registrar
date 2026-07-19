CREATE TABLE ProgramMaster
(
    ProgramId          INT IDENTITY(1,1) PRIMARY KEY,
    ProgramCode        NVARCHAR(30)   NOT NULL,
    ProgramName        NVARCHAR(200)  NOT NULL,
    ProgramType        NVARCHAR(50)   NOT NULL,          -- UG / PG / Diploma / Certificate
    TypicalDuration    NVARCHAR(50)   NULL,
    IsActive           BIT            NOT NULL DEFAULT 1,
    CreatedDate        DATETIME       NOT NULL DEFAULT GETDATE()
);