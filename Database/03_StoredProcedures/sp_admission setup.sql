CREATE OR ALTER PROCEDURE dbo.sp_AdmissionSetup
    @Flag                  VARCHAR(30),
    @AdmissionSetupId      INT             = NULL,
    @CourseId              INT             = NULL,
    @AcademicSessionId     INT             = NULL,
    @TotalSeats            INT             = NULL,
    @MinEligibilityPct     DECIMAL(5,2)    = NULL,
    @ApplicationStartDate  DATE            = NULL,
    @ApplicationEndDate    DATE            = NULL,
    @EligibilityCriteria   NVARCHAR(500)   = NULL,
    @Status                NVARCHAR(20)    = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT
            a.AdmissionSetupId,
            a.CourseId, c.CourseName,
            a.AcademicSessionId, ase.SessionName,
            a.TotalSeats,
            a.MinEligibilityPct,
            a.ApplicationStartDate,
            a.ApplicationEndDate,
            a.EligibilityCriteria,
            a.Status,
            a.CreatedDate
        FROM dbo.AdmissionSetup a
        LEFT JOIN dbo.CourseMaster      c   ON c.CourseId = a.CourseId
        LEFT JOIN dbo.AcademicSession   ase ON ase.AcademicSessionId = a.AcademicSessionId
        ORDER BY a.AdmissionSetupId DESC;
    END

    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT
            a.AdmissionSetupId,
            a.CourseId, c.CourseName,
            a.AcademicSessionId, ase.SessionName,
            a.TotalSeats,
            a.MinEligibilityPct,
            a.ApplicationStartDate,
            a.ApplicationEndDate,
            a.EligibilityCriteria,
            a.Status,
            a.CreatedDate
        FROM dbo.AdmissionSetup a
        LEFT JOIN dbo.CourseMaster      c   ON c.CourseId = a.CourseId
        LEFT JOIN dbo.AcademicSession   ase ON ase.AcademicSessionId = a.AcademicSessionId
        WHERE a.AdmissionSetupId = @AdmissionSetupId;
    END

    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO dbo.AdmissionSetup
            (CourseId, AcademicSessionId, TotalSeats, MinEligibilityPct,
             ApplicationStartDate, ApplicationEndDate, EligibilityCriteria, Status, CreatedDate)
        VALUES
            (@CourseId, @AcademicSessionId, @TotalSeats, @MinEligibilityPct,
             @ApplicationStartDate, @ApplicationEndDate, @EligibilityCriteria,
             ISNULL(@Status, 'Open'), GETDATE());
    END

    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE dbo.AdmissionSetup
        SET CourseId              = @CourseId,
            AcademicSessionId     = @AcademicSessionId,
            TotalSeats            = @TotalSeats,
            MinEligibilityPct     = @MinEligibilityPct,
            ApplicationStartDate  = @ApplicationStartDate,
            ApplicationEndDate    = @ApplicationEndDate,
            EligibilityCriteria   = @EligibilityCriteria,
            Status                = ISNULL(@Status, Status)
        WHERE AdmissionSetupId = @AdmissionSetupId;
    END

    ELSE IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM dbo.AdmissionSetup WHERE AdmissionSetupId = @AdmissionSetupId;
    END

    ELSE IF @Flag = 'CLOSE'
    BEGIN
        UPDATE dbo.AdmissionSetup
        SET Status = 'Closed'
        WHERE AdmissionSetupId = @AdmissionSetupId;
    END
END
GO