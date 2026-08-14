-- ============================================================
-- STEP 1: Table mein IsActive column add karo (agar exist nahi karta)
-- ============================================================
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.AcademicSession')
      AND name = N'IsActive'
)
BEGIN
    ALTER TABLE AcademicSession
    ADD IsActive BIT NOT NULL DEFAULT 1;
    PRINT 'Added IsActive column.';
END
ELSE
BEGIN
    PRINT 'IsActive column already exists — skipping.';
END
GO

-- ============================================================
-- STEP 2: Proc create-if-not-exists guard
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_AcademicSession') AND type = 'P')
BEGIN
    EXEC('CREATE PROCEDURE sp_AcademicSession AS BEGIN SET NOCOUNT ON; END')
END
GO

-- ============================================================
-- STEP 3: Proc ko IsActive ke saath ALTER karo
-- ============================================================
ALTER PROCEDURE sp_AcademicSession
    @Flag                NVARCHAR(20),
    @AcademicSessionId    INT             = NULL,
    @SessionName            NVARCHAR(150)   = NULL,
    @SessionCode              NVARCHAR(50)    = NULL,
    @SessionTypeId              INT             = NULL,
    @StartDate                    DATE            = NULL,
    @EndDate                        DATE            = NULL,
    @AcademicYear                     NVARCHAR(20)    = NULL,
    @MaxCredits                         INT             = NULL,
    @Status                              NVARCHAR(20)    = NULL,
    @IsActive                             BIT             = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        SELECT s.AcademicSessionId, s.SessionName, s.SessionCode, s.SessionTypeId,
               st.SessionTypeName, s.StartDate, s.EndDate, s.AcademicYear, s.Status,
               s.MaxCredits, s.IsActive, s.CreatedDate
        FROM AcademicSession s
        INNER JOIN SessionType st ON s.SessionTypeId = st.SessionTypeId
        ORDER BY s.StartDate DESC;

    ELSE IF @Flag = 'GETACTIVE'      -- used as dropdown source by Semester
        SELECT AcademicSessionId, SessionName FROM AcademicSession WHERE Status = 'Active' ORDER BY StartDate DESC;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM AcademicSession WHERE AcademicSessionId = @AcademicSessionId;

    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO AcademicSession (SessionName, SessionCode, SessionTypeId, StartDate, EndDate, AcademicYear, Status, MaxCredits, IsActive)
        VALUES (@SessionName, @SessionCode, @SessionTypeId, @StartDate, @EndDate, @AcademicYear, ISNULL(@Status, 'Draft'), @MaxCredits, ISNULL(@IsActive, 1));

        SELECT SCOPE_IDENTITY() AS NewId;
    END

    ELSE IF @Flag = 'UPDATE'
        UPDATE AcademicSession
        SET SessionName = @SessionName, SessionCode = @SessionCode, SessionTypeId = @SessionTypeId,
            StartDate = @StartDate, EndDate = @EndDate, AcademicYear = @AcademicYear, MaxCredits = @MaxCredits,
            Status = ISNULL(@Status, Status), IsActive = ISNULL(@IsActive, IsActive)
        WHERE AcademicSessionId = @AcademicSessionId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM AcademicSession WHERE AcademicSessionId = @AcademicSessionId;

    ELSE IF @Flag = 'ACTIVATE'       -- only one Active session at a time
    BEGIN
        UPDATE AcademicSession SET Status = 'Locked' WHERE Status = 'Active';
        UPDATE AcademicSession SET Status = 'Active' WHERE AcademicSessionId = @AcademicSessionId;
    END

    ELSE IF @Flag = 'CLOSE'
        UPDATE AcademicSession SET Status = 'Archived' WHERE AcademicSessionId = @AcademicSessionId;
END
GO

-- ============================================================
-- STEP 4: Sample insert (agar table khali hai)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM AcademicSession)
BEGIN
    INSERT INTO AcademicSession (SessionName, SessionCode, SessionTypeId, StartDate, EndDate, AcademicYear, Status, MaxCredits, IsActive) VALUES
    ('2025-26', 'AY-2025-26', 1, '2025-04-01', '2026-03-31', '2025-26', 'Active', 24, 1);
END
GO