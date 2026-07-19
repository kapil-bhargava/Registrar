

CREATE PROCEDURE sp_ProgramTypeMaster
    @Flag              NVARCHAR(20),
    @ProgramTypeId     INT             = NULL,
    @ProgramTypeCode   NVARCHAR(30)    = NULL,
    @ProgramTypeName   NVARCHAR(100)   = NULL,
    @TypicalDuration   NVARCHAR(50)    = NULL,
    @DisplayOrder      INT             = NULL,
    @IsActive          BIT             = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        SELECT ProgramTypeId, ProgramTypeCode, ProgramTypeName, TypicalDuration,
               DisplayOrder, IsActive, CreatedDate
        FROM ProgramTypeMaster
        ORDER BY ISNULL(DisplayOrder, 999), ProgramTypeName;

    ELSE IF @Flag = 'GETACTIVE'    -- used as dropdown source by Program Management
        SELECT ProgramTypeId, ProgramTypeCode, ProgramTypeName, TypicalDuration
        FROM ProgramTypeMaster
        WHERE IsActive = 1
        ORDER BY ISNULL(DisplayOrder, 999), ProgramTypeName;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM ProgramTypeMaster WHERE ProgramTypeId = @ProgramTypeId;

    ELSE IF @Flag = 'INSERT'
        INSERT INTO ProgramTypeMaster (ProgramTypeCode, ProgramTypeName, TypicalDuration, DisplayOrder, IsActive)
        VALUES (@ProgramTypeCode, @ProgramTypeName, @TypicalDuration, @DisplayOrder, ISNULL(@IsActive, 1));

    ELSE IF @Flag = 'UPDATE'
        UPDATE ProgramTypeMaster
        SET ProgramTypeCode = @ProgramTypeCode,
            ProgramTypeName = @ProgramTypeName,
            TypicalDuration = @TypicalDuration,
            DisplayOrder    = @DisplayOrder,
            IsActive        = ISNULL(@IsActive, IsActive)
        WHERE ProgramTypeId = @ProgramTypeId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM ProgramTypeMaster WHERE ProgramTypeId = @ProgramTypeId;
END
GO

-- Seed data (the 4 types that were hardcoded in the view before)
INSERT INTO ProgramTypeMaster (ProgramTypeCode, ProgramTypeName, TypicalDuration, DisplayOrder, IsActive) VALUES
('UG',   'Undergraduate',  '3-4 Years',    1, 1),
('PG',   'Postgraduate',   '2 Years',      2, 1),
('DIP',  'Diploma',        '1-3 Years',    3, 1),
('CERT', 'Certificate',    '3-12 Months',  4, 1);
GO