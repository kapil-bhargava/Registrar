IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'sp_Application')
    DROP PROCEDURE sp_Application
GO
CREATE PROCEDURE sp_Application
    @Flag                  NVARCHAR(30),
    @ApplicationId         INT = NULL,
    @FullName              NVARCHAR(150) = NULL,
    @Email                 NVARCHAR(150) = NULL,
    @Phone                 NVARCHAR(20) = NULL,
    @DOB                   DATE = NULL,
    @Gender                NVARCHAR(10) = NULL,
    @CategoryId            INT = NULL,
    @AdmissionModeId       INT = NULL,
    @AdmissionSetupId      INT = NULL,
    @PreviousPercentage    DECIMAL(5,2) = NULL,

    -- Document Verification (Step 5)
    @SubmittedDocumentIds  NVARCHAR(MAX) = NULL,   -- comma separated DocumentEnclosureIds actually submitted

    -- Counselling (Step 6)
    @CounsellingDate       DATE = NULL,
    @CounsellingTime       TIME = NULL,
    @CounsellingMode       NVARCHAR(20) = NULL,

    -- Fee Payment (Step 7)
    @FeeMode               NVARCHAR(20) = NULL,
    @FeeAmount             DECIMAL(10,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- =========================================================
    -- STEP 3 : STUDENT REGISTRATION -> creates the Application
    -- =========================================================
    IF @Flag = 'INSERT'
    BEGIN
        DECLARE @NextSeq INT, @NewApplicationNo NVARCHAR(20);
        SELECT @NextSeq = ISNULL(MAX(ApplicationId), 0) + 1 FROM Application;
        SET @NewApplicationNo = 'APP-' + CAST(YEAR(GETDATE()) AS NVARCHAR(4)) + '-' + RIGHT('000' + CAST(@NextSeq AS NVARCHAR(5)), 3);

        INSERT INTO Application
            (ApplicationNo, FullName, Email, Phone, DOB, Gender, CategoryId,
             AdmissionModeId, AdmissionSetupId, PreviousPercentage, Stage)
        VALUES
            (@NewApplicationNo, @FullName, @Email, @Phone, @DOB, @Gender, @CategoryId,
             @AdmissionModeId, @AdmissionSetupId, @PreviousPercentage, 'Registered');

        DECLARE @NewAppId INT = SCOPE_IDENTITY();

        -- seed the document checklist for this application from DocumentEnclosureMaster
        INSERT INTO ApplicationDocument (ApplicationId, DocumentEnclosureId, IsSubmitted)
        SELECT @NewAppId, DocumentEnclosureId, 0
        FROM DocumentEnclosureMaster
        WHERE IsActive = 1;

        SELECT @NewAppId AS ApplicationId, @NewApplicationNo AS ApplicationNo;
    END

    -- =========================================================
    -- STEP 4 : APPLICATION MANAGEMENT -> full list, all stages
    -- =========================================================
    ELSE IF @Flag = 'GETALL'
    BEGIN
        SELECT ap.ApplicationId, ap.ApplicationNo, ap.FullName, c.CourseName, cat.CategoryName,
               ap.RegisteredOn, ap.Stage, ap.DocVerified, ap.CounsellingDone, ap.FeePaid, ap.StudentId
        FROM Application ap
        INNER JOIN AdmissionSetup a ON a.AdmissionSetupId = ap.AdmissionSetupId
        INNER JOIN CourseMaster c ON c.CourseId = a.CourseId
        INNER JOIN Category cat ON cat.CategoryId = ap.CategoryId
        ORDER BY ap.ApplicationId DESC
    END

    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT ap.*, c.CourseName, cat.CategoryName, am.AdmissionModeName
        FROM Application ap
        INNER JOIN AdmissionSetup a ON a.AdmissionSetupId = ap.AdmissionSetupId
        INNER JOIN CourseMaster c ON c.CourseId = a.CourseId
        INNER JOIN Category cat ON cat.CategoryId = ap.CategoryId
        INNER JOIN AdmissionModeMaster am ON am.AdmissionModeId = ap.AdmissionModeId
        WHERE ap.ApplicationId = @ApplicationId
    END

    -- =========================================================
    -- STEP 5 : DOCUMENT VERIFICATION
    -- =========================================================
    ELSE IF @Flag = 'GETPENDINGDOCS'
    BEGIN
        SELECT ap.ApplicationId, ap.ApplicationNo, ap.FullName, c.CourseName
        FROM Application ap
        INNER JOIN AdmissionSetup a ON a.AdmissionSetupId = ap.AdmissionSetupId
        INNER JOIN CourseMaster c ON c.CourseId = a.CourseId
        WHERE ap.DocVerified = 0
        ORDER BY ap.ApplicationId
    END

    ELSE IF @Flag = 'GETDOCCHECKLIST'
    BEGIN
        -- Document Type Master (DocumentEnclosureMaster) joined with this application's submitted flags
        SELECT d.DocumentEnclosureId, d.DocumentName, d.IsMandatory,
               ISNULL(ad.IsSubmitted, 0) AS IsSubmitted
        FROM DocumentEnclosureMaster d
        LEFT JOIN ApplicationDocument ad
            ON ad.DocumentEnclosureId = d.DocumentEnclosureId AND ad.ApplicationId = @ApplicationId
        WHERE d.IsActive = 1
    END

    ELSE IF @Flag = 'VERIFYDOCS'
    BEGIN
        -- reset all to not-submitted, then mark the ones ticked in the UI
        UPDATE ApplicationDocument SET IsSubmitted = 0 WHERE ApplicationId = @ApplicationId;

        UPDATE ad
        SET ad.IsSubmitted = 1
        FROM ApplicationDocument ad
        INNER JOIN STRING_SPLIT(@SubmittedDocumentIds, ',') s ON TRY_CAST(s.value AS INT) = ad.DocumentEnclosureId
        WHERE ad.ApplicationId = @ApplicationId;

        DECLARE @AllMandatoryOk BIT = 1;
        IF EXISTS (
            SELECT 1
            FROM DocumentEnclosureMaster d
            LEFT JOIN ApplicationDocument ad
                ON ad.DocumentEnclosureId = d.DocumentEnclosureId AND ad.ApplicationId = @ApplicationId
            WHERE d.IsActive = 1 AND d.IsMandatory = 1 AND ISNULL(ad.IsSubmitted, 0) = 0
        )
            SET @AllMandatoryOk = 0;

        UPDATE Application
        SET DocVerified = @AllMandatoryOk,
            Stage = CASE WHEN @AllMandatoryOk = 1 THEN 'DocsVerified' ELSE 'Registered' END
        WHERE ApplicationId = @ApplicationId;

        SELECT @AllMandatoryOk AS AllMandatoryVerified;
    END

    -- =========================================================
    -- STEP 6 : COUNSELLING & SEAT ALLOTMENT
    -- =========================================================
    ELSE IF @Flag = 'GETPENDINGCOUNSELLING'
    BEGIN
        SELECT ap.ApplicationId, ap.ApplicationNo, ap.FullName, c.CourseName, cat.CategoryName
        FROM Application ap
        INNER JOIN AdmissionSetup a ON a.AdmissionSetupId = ap.AdmissionSetupId
        INNER JOIN CourseMaster c ON c.CourseId = a.CourseId
        INNER JOIN Category cat ON cat.CategoryId = ap.CategoryId
        WHERE ap.DocVerified = 1 AND ap.CounsellingDone = 0
        ORDER BY ap.ApplicationId
    END

    ELSE IF @Flag = 'SCHEDULECOUNSELLING'
    BEGIN
        DECLARE @CourseCode NVARCHAR(20), @CatShort NVARCHAR(3), @SeatNo NVARCHAR(50);

        SELECT @CourseCode = c.CourseCode, @CatShort = UPPER(LEFT(cat.CategoryName, 3))
        FROM Application ap
        INNER JOIN AdmissionSetup a ON a.AdmissionSetupId = ap.AdmissionSetupId
        INNER JOIN CourseMaster c ON c.CourseId = a.CourseId
        INNER JOIN Category cat ON cat.CategoryId = ap.CategoryId
        WHERE ap.ApplicationId = @ApplicationId;

        SET @SeatNo = @CatShort + '-' + @CourseCode + '-' + RIGHT('000' + CAST(@ApplicationId AS NVARCHAR(5)), 3);

        UPDATE Application
        SET CounsellingDone = 1, CounsellingDate = @CounsellingDate, CounsellingTime = @CounsellingTime,
            CounsellingMode = @CounsellingMode, SeatNumber = @SeatNo, Stage = 'CounsellingDone'
        WHERE ApplicationId = @ApplicationId;

        SELECT @SeatNo AS SeatNumber;
    END

    -- =========================================================
    -- STEP 7 : FEE PAYMENT
    -- =========================================================
    ELSE IF @Flag = 'GETPENDINGFEE'
    BEGIN
        SELECT ap.ApplicationId, ap.ApplicationNo, ap.FullName, c.CourseName, ap.SeatNumber
        FROM Application ap
        INNER JOIN AdmissionSetup a ON a.AdmissionSetupId = ap.AdmissionSetupId
        INNER JOIN CourseMaster c ON c.CourseId = a.CourseId
        WHERE ap.CounsellingDone = 1 AND ap.FeePaid = 0
        ORDER BY ap.ApplicationId
    END

    ELSE IF @Flag = 'COLLECTFEE'
    BEGIN
        DECLARE @ReceiptNo NVARCHAR(30) = 'RCPT-' + RIGHT(CAST(DATEDIFF(SECOND, '2020-01-01', GETDATE()) AS NVARCHAR(20)), 6);

        UPDATE Application
        SET FeePaid = 1, FeeReceiptNo = @ReceiptNo, FeeAmount = @FeeAmount, FeeMode = @FeeMode, Stage = 'FeePaid'
        WHERE ApplicationId = @ApplicationId;

        SELECT @ReceiptNo AS ReceiptNo;
    END

    -- =========================================================
    -- STEP 8 : ADMISSION FINAL -> creates the Student record
    -- =========================================================
    ELSE IF @Flag = 'GETPENDINGFINAL'
    BEGIN
        SELECT ap.ApplicationId, ap.ApplicationNo, ap.FullName, c.CourseName, ap.SeatNumber, ap.FeeReceiptNo
        FROM Application ap
        INNER JOIN AdmissionSetup a ON a.AdmissionSetupId = ap.AdmissionSetupId
        INNER JOIN CourseMaster c ON c.CourseId = a.CourseId
        WHERE ap.FeePaid = 1 AND ap.StudentId IS NULL
        ORDER BY ap.ApplicationId
    END

    ELSE IF @Flag = 'CONFIRMADMISSION'
    BEGIN
        DECLARE @NewStudentId NVARCHAR(20);
        DECLARE @NextStudentSeq INT;
        SELECT @NextStudentSeq = ISNULL(MAX(CAST(RIGHT(StudentId, 4) AS INT)), 999) + 1 FROM Student;
        SET @NewStudentId = 'STU-' + CAST(YEAR(GETDATE()) AS NVARCHAR(4)) + CAST(@NextStudentSeq AS NVARCHAR(4));

        INSERT INTO Student (StudentId, ApplicationId, FullName, CourseId, CategoryId, AcademicSessionId, SeatNumber)
        SELECT @NewStudentId, ap.ApplicationId, ap.FullName, a.CourseId, ap.CategoryId, a.AcademicSessionId, ap.SeatNumber
        FROM Application ap
        INNER JOIN AdmissionSetup a ON a.AdmissionSetupId = ap.AdmissionSetupId
        WHERE ap.ApplicationId = @ApplicationId;

        UPDATE Application SET StudentId = @NewStudentId, Stage = 'Admitted' WHERE ApplicationId = @ApplicationId;

        SELECT @NewStudentId AS StudentId;
    END
END
GO