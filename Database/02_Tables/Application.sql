
-- 2) APPLICATION  (Steps 2–8: created at Student Registration, updated at every later step)
IF OBJECT_ID('Application') IS NULL
BEGIN
    CREATE TABLE Application (
        ApplicationId        INT IDENTITY(1,1) PRIMARY KEY,
        ApplicationNo        NVARCHAR(20) NOT NULL UNIQUE,
        FullName             NVARCHAR(150) NOT NULL,
        Email                NVARCHAR(150) NOT NULL,
        Phone                NVARCHAR(20) NOT NULL,
        DOB                  DATE NULL,
        Gender               NVARCHAR(10) NULL,
        CategoryId           INT NOT NULL FOREIGN KEY REFERENCES CategoryMaster(CategoryId),
        AdmissionModeId      INT NOT NULL FOREIGN KEY REFERENCES AdmissionModeMaster(AdmissionModeId),
        AdmissionSetupId     INT NOT NULL FOREIGN KEY REFERENCES AdmissionSetup(AdmissionSetupId),
        PreviousPercentage   DECIMAL(5,2) NULL,
        RegisteredOn         DATETIME NOT NULL DEFAULT GETDATE(),
        Stage                NVARCHAR(30) NOT NULL DEFAULT 'Registered',
                             -- Registered / DocsVerified / CounsellingDone / FeePaid / Admitted
        DocVerified          BIT NOT NULL DEFAULT 0,
        CounsellingDone      BIT NOT NULL DEFAULT 0,
        CounsellingDate      DATE NULL,
        CounsellingTime      TIME NULL,
        CounsellingMode      NVARCHAR(20) NULL,          -- In-Person / Online
        SeatNumber           NVARCHAR(50) NULL,
        FeePaid              BIT NOT NULL DEFAULT 0,
        FeeReceiptNo         NVARCHAR(30) NULL,
        FeeAmount            DECIMAL(10,2) NULL,
        FeeMode              NVARCHAR(20) NULL,          -- Online / Cash / DD
        StudentId            NVARCHAR(20) NULL
    )
END
GO


---- ============================================================
---- STEP A: Add new columns to Application table
---- ============================================================
--ALTER TABLE Application ADD
--    Title                   NVARCHAR(20)  NULL,
--    FirstName               NVARCHAR(60)  NULL,
--    MiddleName              NVARCHAR(60)  NULL,
--    LastName                NVARCHAR(60)  NULL,
--    DisplayNameFormat       NVARCHAR(30)  NULL,
--    DisplayName             NVARCHAR(150) NULL,
--    MaritalStatus           NVARCHAR(20)  NULL,

--    -- Personal information
--    BirthState              NVARCHAR(60)  NULL,
--    BirthPlace              NVARCHAR(100) NULL,
--    WhatsAppNumber          NVARCHAR(15)  NULL,
--    ReferralSource          NVARCHAR(100) NULL,
--    PhysicallyChallenged    BIT           NULL DEFAULT 0,
--    BloodGroup              NVARCHAR(5)   NULL,
--    IdentityMark            NVARCHAR(150) NULL,
--    MotherTongue            NVARCHAR(50)  NULL,

--    -- Contact details
--    InstituteEmail          NVARCHAR(150) NULL,
--    AlternateMobileNumber   NVARCHAR(15)  NULL,

--    -- Identity & domicile
--    Citizenship             NVARCHAR(50)  NULL,
--    DomicileCountry         NVARCHAR(50)  NULL,
--    DomicileState           NVARCHAR(60)  NULL,

--    -- Other information
--    Nationality             NVARCHAR(50)  NULL,
--    Religion                NVARCHAR(50)  NULL,
--    Caste                   NVARCHAR(50)  NULL,
--    ABCId                   NVARCHAR(30)  NULL,
--    AntiRaggingId           NVARCHAR(30)  NULL,

--    -- Auto-generated at Admission (Final) step — added now, filled later
--    RegistrationNumber          NVARCHAR(30) NULL,
--    UniversityEnrollmentNumber  NVARCHAR(30) NULL;new add kro run kro
--GO


IF COL_LENGTH('dbo.Application', 'FeeAmount') IS NULL
    ALTER TABLE dbo.Application ADD FeeAmount DECIMAL(10,2) NULL;

IF COL_LENGTH('dbo.Application', 'FeePaymentDate') IS NULL
    ALTER TABLE dbo.Application ADD FeePaymentDate DATETIME NULL;


    IF COL_LENGTH('dbo.Application', 'FeePaymentDate') IS NULL
    ALTER TABLE dbo.Application ADD FeePaymentDate DATETIME NULL;
GO