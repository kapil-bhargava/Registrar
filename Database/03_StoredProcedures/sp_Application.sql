USE [UniversityERP]
GO
/****** Object:  StoredProcedure [dbo].[sp_Application] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Application]
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

    -- Personal Information (Student Registration — new full form)
    @Title                 NVARCHAR(20)  = NULL,
    @FirstName             NVARCHAR(60)  = NULL,
    @MiddleName            NVARCHAR(60)  = NULL,
    @LastName              NVARCHAR(60)  = NULL,
    @DisplayNameFormat     NVARCHAR(30)  = NULL,
    @DisplayName           NVARCHAR(150) = NULL,
    @MaritalStatus         NVARCHAR(20)  = NULL,
    @BirthState            NVARCHAR(60)  = NULL,
    @BirthPlace            NVARCHAR(100) = NULL,
    @WhatsAppNumber        NVARCHAR(15)  = NULL,
    @ReferralSource        NVARCHAR(100) = NULL,
    @PhysicallyChallenged  BIT           = 0,
    @BloodGroup            NVARCHAR(5)   = NULL,
    @IdentityMark          NVARCHAR(150) = NULL,
    @MotherTongue          NVARCHAR(50)  = NULL,
    @InstituteEmail        NVARCHAR(150) = NULL,
    @AlternateMobileNumber NVARCHAR(15)  = NULL,
    @Citizenship           NVARCHAR(50)  = NULL,
    @DomicileCountry       NVARCHAR(50)  = NULL,
    @DomicileState         NVARCHAR(60)  = NULL,
    @Nationality           NVARCHAR(50)  = NULL,
    @Religion              NVARCHAR(50)  = NULL,
    @Caste                 NVARCHAR(50)  = NULL,
    @ABCId                 NVARCHAR(30)  = NULL,
    @AntiRaggingId         NVARCHAR(30)  = NULL,

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
    -- (full Personal Information form)
    -- =========================================================
    IF @Flag = 'INSERT'
    BEGIN
        DECLARE @NextSeq INT, @NewApplicationNo NVARCHAR(20);
        SELECT @NextSeq = ISNULL(MAX(ApplicationId), 0) + 1 FROM Application;
        SET @NewApplicationNo = 'APP-' + CAST(YEAR(GETDATE()) AS NVARCHAR(4)) + '-' + RIGHT('000' + CAST(@NextSeq AS NVARCHAR(5)), 3);

        -- FullName kept for backward-compat with existing joins/grids
        DECLARE @ComputedFullName NVARCHAR(150) =
            LTRIM(RTRIM(ISNULL(@FirstName,'') + ' ' + ISNULL(@MiddleName + ' ', '') + ISNULL(@LastName,'')));

        DECLARE @ComputedDisplayName NVARCHAR(150) = ISNULL(NULLIF(@DisplayName, ''), @ComputedFullName);

        INSERT INTO Application
            (ApplicationNo, FullName, Email, Phone, DOB, Gender, CategoryId,
             AdmissionModeId, AdmissionSetupId, PreviousPercentage, Stage,
             Title, FirstName, MiddleName, LastName, DisplayNameFormat, DisplayName, MaritalStatus,
             BirthState, BirthPlace, WhatsAppNumber, ReferralSource, PhysicallyChallenged,
             BloodGroup, IdentityMark, MotherTongue,
             InstituteEmail, AlternateMobileNumber,
             Citizenship, DomicileCountry, DomicileState,
             Nationality, Religion, Caste, ABCId, AntiRaggingId)
        VALUES
            (@NewApplicationNo, @ComputedFullName, @Email, @Phone, @DOB, @Gender, @CategoryId,
             @AdmissionModeId, @AdmissionSetupId, @PreviousPercentage, 'Registered',
             @Title, @FirstName, @MiddleName, @LastName, @DisplayNameFormat, @ComputedDisplayName, @MaritalStatus,
             @BirthState, @BirthPlace, @WhatsAppNumber, @ReferralSource, @PhysicallyChallenged,
             @BloodGroup, @IdentityMark, @MotherTongue,
             @InstituteEmail, @AlternateMobileNumber,
             @Citizenship, @DomicileCountry, @DomicileState,
             @Nationality, @Religion, @Caste, @ABCId, @AntiRaggingId);

        DECLARE @NewAppId INT = SCOPE_IDENTITY();

        -- seed the document checklist for this application from DocumentEnclosureMaster
        INSERT INTO ApplicationDocument (ApplicationId, DocumentEnclosureId, IsSubmitted)
        SELECT @NewAppId, DocumentEnclosureId, 0
        FROM DocumentEnclosureMaster
        WHERE IsActive = 1;

        SELECT @NewAppId AS ApplicationId, @NewApplicationNo AS ApplicationNo;
    END

    -- =========================================================
    -- NEW: EDIT an existing application (list + edit on one page)
    -- =========================================================
    ELSE IF @Flag = 'UPDATE'
    BEGIN
        DECLARE @UpdFullName NVARCHAR(150) =
            LTRIM(RTRIM(ISNULL(@FirstName,'') + ' ' + ISNULL(@MiddleName + ' ', '') + ISNULL(@LastName,'')));
        DECLARE @UpdDisplayName NVARCHAR(150) = ISNULL(NULLIF(@DisplayName, ''), @UpdFullName);

        UPDATE Application SET
            FullName = @UpdFullName, Email = @Email, Phone = @Phone, DOB = @DOB, Gender = @Gender,
            CategoryId = @CategoryId, AdmissionModeId = @AdmissionModeId, AdmissionSetupId = @AdmissionSetupId,
            PreviousPercentage = @PreviousPercentage,
            Title = @Title, FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName,
            DisplayNameFormat = @DisplayNameFormat, DisplayName = @UpdDisplayName, MaritalStatus = @MaritalStatus,
            BirthState = @BirthState, BirthPlace = @BirthPlace, WhatsAppNumber = @WhatsAppNumber,
            ReferralSource = @ReferralSource, PhysicallyChallenged = @PhysicallyChallenged,
            BloodGroup = @BloodGroup, IdentityMark = @IdentityMark, MotherTongue = @MotherTongue,
            InstituteEmail = @InstituteEmail, AlternateMobileNumber = @AlternateMobileNumber,
            Citizenship = @Citizenship, DomicileCountry = @DomicileCountry, DomicileState = @DomicileState,
            Nationality = @Nationality, Religion = @Religion, Caste = @Caste,
            ABCId = @ABCId, AntiRaggingId = @AntiRaggingId
        WHERE ApplicationId = @ApplicationId;

        SELECT @ApplicationId AS ApplicationId;
    END

    -- =========================================================
    -- STEP 4 : APPLICATION MANAGEMENT -> full list, all stages
    -- (also used as the Student Registration LIST view)
    -- =========================================================
    ELSE IF @Flag = 'GETALL'
    BEGIN
        SELECT ap.ApplicationId, ap.ApplicationNo, ap.FullName, ap.DisplayName, ap.Email, ap.Phone,
               c.CourseName, cat.CategoryName,
               ap.RegisteredOn, ap.Stage, ap.DocVerified, ap.CounsellingDone, ap.FeePaid, ap.StudentId
        FROM Application ap
        INNER JOIN AdmissionSetup a ON a.AdmissionSetupId = ap.AdmissionSetupId
        INNER JOIN CourseMaster c ON c.CourseId = a.CourseId
        INNER JOIN Category cat ON cat.CategoryId = ap.CategoryId
        ORDER BY ap.ApplicationId DESC
    END

    ELSE IF @Flag = 'GETBYID'
    BEGIN
        -- ap.* already returns every new Personal Information column too
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
        SELECT d.DocumentEnclosureId, d.DocumentName, d.IsMandatory,
               ISNULL(ad.IsSubmitted, 0) AS IsSubmitted
        FROM DocumentEnclosureMaster d
        LEFT JOIN ApplicationDocument ad
            ON ad.DocumentEnclosureId = d.DocumentEnclosureId AND ad.ApplicationId = @ApplicationId
        WHERE d.IsActive = 1
    END

    ELSE IF @Flag = 'VERIFYDOCS'
    BEGIN
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
        SET FeePaid = 1, FeeReceiptNo = @ReceiptNo, FeeAmount = @FeeAmount, FeeMode = @FeeMode,
            FeePaymentDate = GETDATE(), Stage = 'FeePaid'
        WHERE ApplicationId = @ApplicationId;

        SELECT @ReceiptNo AS ReceiptNo;
    END

    -- =========================================================
    -- STEP 8 : ADMISSION FINAL -> creates the Student record
    -- (also auto-generates RegistrationNumber, UniversityEnrollmentNumber, InstituteEmail)
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

        -- Auto-generate Registration Number, University/Enrollment No., Institute Email
        DECLARE @RegNo NVARCHAR(30) = 'REG-' + CAST(YEAR(GETDATE()) AS NVARCHAR(4)) + '-' + RIGHT('0000' + CAST(@ApplicationId AS NVARCHAR(5)), 4);
        DECLARE @UnivEnrollNo NVARCHAR(30) = 'ENR' + CAST(YEAR(GETDATE()) AS NVARCHAR(4)) + RIGHT('0000' + CAST(@ApplicationId AS NVARCHAR(5)), 4);
        DECLARE @InstEmail NVARCHAR(150);

        SELECT @InstEmail = LOWER(REPLACE(ISNULL(FirstName, LEFT(FullName, CHARINDEX(' ', FullName + ' ') - 1)), ' ', ''))
                             + CAST(@ApplicationId AS NVARCHAR(10)) + '@college.edu'
        FROM Application WHERE ApplicationId = @ApplicationId;

        UPDATE Application
        SET RegistrationNumber = @RegNo,
            UniversityEnrollmentNumber = @UnivEnrollNo,
            InstituteEmail = @InstEmail
        WHERE ApplicationId = @ApplicationId;

        INSERT INTO Student (StudentId, ApplicationId, FullName, CourseId, CategoryId, AcademicSessionId, SeatNumber)
        SELECT @NewStudentId, ap.ApplicationId, ap.FullName, a.CourseId, ap.CategoryId, a.AcademicSessionId, ap.SeatNumber
        FROM Application ap
        INNER JOIN AdmissionSetup a ON a.AdmissionSetupId = ap.AdmissionSetupId
        WHERE ap.ApplicationId = @ApplicationId;

        UPDATE Application SET StudentId = @NewStudentId, Stage = 'Admitted' WHERE ApplicationId = @ApplicationId;

        SELECT @NewStudentId AS StudentId, @RegNo AS RegistrationNumber, @UnivEnrollNo AS UniversityEnrollmentNumber, @InstEmail AS InstituteEmail;
    END

    -- =========================================================
    -- DASHBOARD : FEE COLLECTION SUMMARY
    -- Result set 1: totals (TotalCollected, PendingCount, EstimatedPendingAmount)
    -- Result set 2: month-wise collected amount (last 6 months)
    -- =========================================================
    ELSE IF @Flag = 'GETFEESUMMARY'
    BEGIN
        SELECT
            ISNULL(SUM(FeeAmount), 0) AS TotalCollected,
            (SELECT COUNT(*) FROM Application WHERE CounsellingDone = 1 AND FeePaid = 0) AS PendingCount,
            (SELECT ISNULL(SUM(fh.Amount), 0) FROM FeeHeadMaster fh WHERE fh.IsActive = 1)
                * (SELECT COUNT(*) FROM Application WHERE CounsellingDone = 1 AND FeePaid = 0) AS EstimatedPendingAmount
        FROM Application
        WHERE FeePaid = 1;

        SELECT
            MONTH(FeePaymentDate) AS MonthNum,
            DATENAME(MONTH, FeePaymentDate) AS MonthName,
            SUM(FeeAmount) AS Collected
        FROM Application
        WHERE FeePaid = 1 AND FeePaymentDate >= DATEADD(MONTH, -6, GETDATE())
        GROUP BY MONTH(FeePaymentDate), DATENAME(MONTH, FeePaymentDate)
        ORDER BY MonthNum;
    END
END


UPDATE Application
SET FeePaymentDate = GETDATE()
WHERE FeePaid = 1 AND FeePaymentDate IS NULL;