IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'EligibilityCheckLog')
BEGIN
    CREATE TABLE EligibilityCheckLog
    (
        EligibilityCheckId  INT IDENTITY(1,1) PRIMARY KEY,
        ApplicantName       NVARCHAR(150)   NOT NULL,
        AdmissionSetupId    INT             NOT NULL,
        PercentageObtained  DECIMAL(5,2)    NOT NULL,
        IsEligible          BIT             NOT NULL,
        CheckedOn           DATETIME        NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_EligibilityCheckLog_AdmissionSetup
            FOREIGN KEY (AdmissionSetupId) REFERENCES AdmissionSetup(AdmissionSetupId)
    );
END
GO