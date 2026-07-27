-- 1) ADMISSION SETUP  (Step 1)
IF OBJECT_ID('AdmissionSetup') IS NULL
BEGIN
    CREATE TABLE AdmissionSetup
	(
        AdmissionSetupId     INT IDENTITY(1,1) PRIMARY KEY,
        CourseId             INT NOT NULL FOREIGN KEY REFERENCES CourseMaster(CourseId),
        AcademicSessionId    INT NOT NULL FOREIGN KEY REFERENCES AcademicSession(AcademicSessionId),
        TotalSeats           INT NOT NULL,
        MinEligibilityPct    DECIMAL(5,2) NOT NULL,
        ApplicationStartDate DATE NOT NULL,
        ApplicationEndDate   DATE NOT NULL,
        EligibilityCriteria  NVARCHAR(500) NULL,
        Status               NVARCHAR(20) NOT NULL DEFAULT 'Open',   -- Open / Closed
        CreatedDate          DATETIME NOT NULL DEFAULT GETDATE()
    )
END
GO