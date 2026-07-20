
CREATE TABLE Program
(
    ProgramId         INT IDENTITY(1,1) PRIMARY KEY,
    ProgramCode       NVARCHAR(20)   NOT NULL,
    ProgramName       NVARCHAR(150)  NOT NULL,
    ProgramType       NVARCHAR(30)   NOT NULL,          -- UG / PG / Diploma / Certificate
    TypicalDuration   NVARCHAR(50)   NULL,
    IsActive          BIT            NOT NULL DEFAULT 1,
    CreatedDate       DATETIME       NOT NULL DEFAULT GETDATE()
);
