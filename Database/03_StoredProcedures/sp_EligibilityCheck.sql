
-- ============================================================
-- sp_EligibilityCheck
-- Used by: AdmissionService.cs
--   CheckEligibility(name, admissionSetupId, pct) -> @Flag = CHECK
--   GetRecentEligibilityChecks()                  -> @Flag = GETRECENT
-- ============================================================
IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'sp_EligibilityCheck')
    DROP PROCEDURE sp_EligibilityCheck
GO
CREATE PROCEDURE sp_EligibilityCheck
    @Flag                NVARCHAR(30),
    @ApplicantName        NVARCHAR(150) = NULL,
    @AdmissionSetupId     INT = NULL,
    @PercentageObtained   DECIMAL(5,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- =========================================================
    -- CHECK -> runs the eligibility comparison, logs it, returns result
    -- =========================================================
    IF @Flag = 'CHECK'
    BEGIN
        DECLARE @MinPct DECIMAL(5,2), @IsEligible BIT;

        SELECT @MinPct = MinEligibilityPct
        FROM AdmissionSetup
        WHERE AdmissionSetupId = @AdmissionSetupId;

        IF @MinPct IS NULL
        BEGIN
            -- invalid AdmissionSetupId -> return no rows;
            -- service will see result == null and report "Invalid Admission Setup selected."
            RETURN;
        END

        SET @IsEligible = CASE WHEN @PercentageObtained >= @MinPct THEN 1 ELSE 0 END;

        INSERT INTO EligibilityCheckLog (ApplicantName, AdmissionSetupId, PercentageObtained, IsEligible, CheckedOn)
        VALUES (@ApplicantName, @AdmissionSetupId, @PercentageObtained, @IsEligible, GETDATE());

        SELECT
            @ApplicantName                  AS ApplicantName,
            c.CourseName                    AS CourseName,
            s.SessionName                   AS SessionName,
            a.EligibilityCriteria           AS EligibilityCriteria,
            a.MinEligibilityPct             AS MinEligibilityPct,
            @PercentageObtained             AS PercentageObtained,
            @IsEligible                     AS IsEligible
        FROM AdmissionSetup a
        INNER JOIN CourseMaster c ON c.CourseId = a.CourseId
        INNER JOIN AcademicSession s ON s.AcademicSessionId = a.AcademicSessionId
        WHERE a.AdmissionSetupId = @AdmissionSetupId;
    END

    -- =========================================================
    -- GETRECENT -> last 10 checks for the "Recent Checks" table
    -- =========================================================
    ELSE IF @Flag = 'GETRECENT'
    BEGIN
        SELECT TOP 10
            l.ApplicantName,
            c.CourseName,
            l.PercentageObtained,
            l.IsEligible,
            l.CheckedOn
        FROM EligibilityCheckLog l
        INNER JOIN AdmissionSetup a ON a.AdmissionSetupId = l.AdmissionSetupId
        INNER JOIN CourseMaster c ON c.CourseId = a.CourseId
        ORDER BY l.CheckedOn DESC
    END
END
GO