/* ============================================================
   PROGRAM  (UG / PG / Diploma / Certificate — a broad category)
   ============================================================ */


CREATE PROCEDURE sp_Program
    @Flag              NVARCHAR(20),
    @ProgramId         INT             = NULL,
    @ProgramCode       NVARCHAR(20)    = NULL,
    @ProgramName       NVARCHAR(150)   = NULL,
    @ProgramType       NVARCHAR(30)    = NULL,
    @TypicalDuration   NVARCHAR(50)    = NULL,
    @IsActive          BIT             = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        SELECT * FROM Program ORDER BY ProgramName;

    ELSE IF @Flag = 'GETACTIVE'      -- used as dropdown source by Course
        SELECT ProgramId, ProgramCode, ProgramName, ProgramType FROM Program WHERE IsActive = 1 ORDER BY ProgramName;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM Program WHERE ProgramId = @ProgramId;

    ELSE IF @Flag = 'INSERT'
        INSERT INTO Program (ProgramCode, ProgramName, ProgramType, TypicalDuration, IsActive)
        VALUES (@ProgramCode, @ProgramName, @ProgramType, @TypicalDuration, @IsActive);

    ELSE IF @Flag = 'UPDATE'
        UPDATE Program
        SET ProgramCode = @ProgramCode, ProgramName = @ProgramName, ProgramType = @ProgramType,
            TypicalDuration = @TypicalDuration, IsActive = @IsActive
        WHERE ProgramId = @ProgramId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM Program WHERE ProgramId = @ProgramId;
END
GO

INSERT INTO Program (ProgramCode, ProgramName, ProgramType, TypicalDuration, IsActive) VALUES
('UG', 'Undergraduate', 'UG', '3-4 Years', 1),
('PG', 'Postgraduate', 'PG', '2 Years', 1),
('DIP', 'Diploma', 'Diploma', '1-3 Years', 1);
GO


/* ============================================================
   COURSE
   ProgramId    -> FK to Program    (mapping #3)
   DepartmentId -> FK to Department (mapping #4)
   ============================================================ */


