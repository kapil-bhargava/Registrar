
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