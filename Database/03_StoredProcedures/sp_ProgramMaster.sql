CREATE OR ALTER PROCEDURE sp_ProgramMaster
    @Flag               NVARCHAR(20),
    @ProgramId          INT             = NULL,
    @ProgramCode        NVARCHAR(30)    = NULL,
    @ProgramName        NVARCHAR(200)   = NULL,
    @ProgramType        NVARCHAR(50)    = NULL,
    @TypicalDuration    NVARCHAR(50)    = NULL,
    @IsActive           BIT             = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        SELECT ProgramId, ProgramCode, ProgramName, ProgramType, TypicalDuration, IsActive, CreatedDate
        FROM ProgramMaster
        ORDER BY ProgramName;

    ELSE IF @Flag = 'GETACTIVE'
        SELECT ProgramId, ProgramCode, ProgramName, ProgramType
        FROM ProgramMaster
        WHERE IsActive = 1
        ORDER BY ProgramName;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM ProgramMaster WHERE ProgramId = @ProgramId;

    ELSE IF @Flag = 'INSERT'
        INSERT INTO ProgramMaster (ProgramCode, ProgramName, ProgramType, TypicalDuration, IsActive)
        VALUES (@ProgramCode, @ProgramName, @ProgramType, @TypicalDuration, ISNULL(@IsActive, 1));

    ELSE IF @Flag = 'UPDATE'
        UPDATE ProgramMaster
        SET ProgramCode = @ProgramCode, ProgramName = @ProgramName, ProgramType = @ProgramType,
            TypicalDuration = @TypicalDuration, IsActive = @IsActive
        WHERE ProgramId = @ProgramId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM ProgramMaster WHERE ProgramId = @ProgramId;
END
GO

INSERT INTO ProgramMaster (ProgramCode, ProgramName, ProgramType, TypicalDuration, IsActive) VALUES
('UG', 'Undergraduate', 'UG', '3-4 Years', 1),
('PG', 'Postgraduate', 'PG', '2 Years', 1),
('DIP', 'Diploma', 'Diploma', '1-3 Years', 1);
GO
