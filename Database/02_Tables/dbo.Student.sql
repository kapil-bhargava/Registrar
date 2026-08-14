-- =========================================================
-- 1) STUDENT MAPPING — Section + Semester assignment
-- =========================================================
IF OBJECT_ID('dbo.StudentMapping', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.StudentMapping
    (
        MappingId    INT IDENTITY(1,1) PRIMARY KEY,
        StudentId    NVARCHAR(20) NOT NULL,
        Section      NVARCHAR(10) NOT NULL,
        Semester     INT NOT NULL,
        CreatedDate  DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_StudentMapping_Student FOREIGN KEY (StudentId) REFERENCES dbo.Student(StudentId)
    );
END
GO

-- =========================================================
-- 2) IDENTITY GENERATION — Enrollment No / ID Card
-- =========================================================
IF OBJECT_ID('dbo.StudentIdentity', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.StudentIdentity
    (
        IdentityId     INT IDENTITY(1,1) PRIMARY KEY,
        StudentId      NVARCHAR(20) NOT NULL UNIQUE,
        EnrollmentNo   NVARCHAR(30) NOT NULL,
        IssuedOn       DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_StudentIdentity_Student FOREIGN KEY (StudentId) REFERENCES dbo.Student(StudentId)
    );
END
GO

-- =========================================================
-- 3) ACADEMIC PROGRESS — Semester-wise SGPA/Attendance
-- =========================================================
IF OBJECT_ID('dbo.AcademicProgress', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.AcademicProgress
    (
        ProgressId    INT IDENTITY(1,1) PRIMARY KEY,
        StudentId     NVARCHAR(20) NOT NULL,
        Semester      INT NOT NULL,
        SGPA          DECIMAL(4,2) NOT NULL,
        Attendance    DECIMAL(5,2) NULL,
        ResultStatus  NVARCHAR(20) NOT NULL DEFAULT 'Pending',
        CreatedDate   DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_AcademicProgress_Student FOREIGN KEY (StudentId) REFERENCES dbo.Student(StudentId)
    );
END
GO

-- =========================================================
-- 4) CERTIFICATE MANAGEMENT — Bonafide/Character/etc.
-- =========================================================
IF OBJECT_ID('dbo.CertificateIssued', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.CertificateIssued
    (
        CertificateId    INT IDENTITY(1,1) PRIMARY KEY,
        CertNo           NVARCHAR(30) NOT NULL,
        StudentId        NVARCHAR(20) NOT NULL,
        CertificateType  NVARCHAR(60) NOT NULL,
        Purpose          NVARCHAR(200) NULL,
        IssuedOn         DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_CertificateIssued_Student FOREIGN KEY (StudentId) REFERENCES dbo.Student(StudentId)
    );
END
GO

-- =========================================================
-- 5) ALUMNI INFO — Career details (only for Graduated students)
-- =========================================================
IF OBJECT_ID('dbo.AlumniInfo', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.AlumniInfo
    (
        AlumniInfoId  INT IDENTITY(1,1) PRIMARY KEY,
        StudentId     NVARCHAR(20) NOT NULL UNIQUE,
        Company       NVARCHAR(150) NULL,
        Designation   NVARCHAR(100) NULL,
        Email         NVARCHAR(150) NULL,
        LinkedInUrl   NVARCHAR(250) NULL,
        UpdatedDate   DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_AlumniInfo_Student FOREIGN KEY (StudentId) REFERENCES dbo.Student(StudentId)
    );
END
GO

-- =========================================================S
-- 6) STUDENT table me STATUS column add karo (agar nahi hai)
--    Active / Graduated / Suspended track karne ke liye
-- =========================================================
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Student') AND name = 'Status')
BEGIN
    ALTER TABLE dbo.Student ADD Status NVARCHAR(20) NOT NULL DEFAULT 'Active';
END
GO