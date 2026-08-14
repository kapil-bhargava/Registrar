-- =========================================================
-- SP 1 : STUDENT RECORDS (master source — list + status update)
-- =========================================================
IF OBJECT_ID('dbo.sp_StudentRecords', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_StudentRecords;
GO

CREATE PROCEDURE dbo.sp_StudentRecords
(
    @Flag       NVARCHAR(20),
    @StudentId  NVARCHAR(20) = NULL,
    @Status     NVARCHAR(20) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT s.StudentId, s.FullName AS Name, c.CourseName, cat.CategoryName AS Category,
               a.SessionName AS Session, s.SeatNumber, s.CreatedDate AS AdmittedOn, s.Status
        FROM dbo.Student s
        LEFT JOIN dbo.CourseMaster c ON c.CourseId = s.CourseId
        LEFT JOIN dbo.Category cat ON cat.CategoryId = s.CategoryId
        LEFT JOIN dbo.AcademicSession a ON a.AcademicSessionId = s.AcademicSessionId
        ORDER BY s.CreatedDate DESC;
    END

    IF @Flag = 'GETBYID'
    BEGIN
        SELECT s.StudentId, s.FullName AS Name, c.CourseName, cat.CategoryName AS Category,
               a.SessionName AS Session, s.SeatNumber, s.CreatedDate AS AdmittedOn, s.Status
        FROM dbo.Student s
        LEFT JOIN dbo.CourseMaster c ON c.CourseId = s.CourseId
        LEFT JOIN dbo.Category cat ON cat.CategoryId = s.CategoryId
        LEFT JOIN dbo.AcademicSession a ON a.AcademicSessionId = s.AcademicSessionId
        WHERE s.StudentId = @StudentId;
    END

    IF @Flag = 'UPDATESTATUS'
    BEGIN
        UPDATE dbo.Student SET Status = @Status WHERE StudentId = @StudentId;
    END
END
GO

-- =========================================================
-- SP 2 : STUDENT MAPPING (Section + Semester)
-- =========================================================
IF OBJECT_ID('dbo.sp_StudentMapping', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_StudentMapping;
GO

CREATE PROCEDURE dbo.sp_StudentMapping
(
    @Flag       NVARCHAR(20),
    @StudentId  NVARCHAR(20) = NULL,
    @Section    NVARCHAR(10) = NULL,
    @Semester   INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT s.StudentId, s.FullName AS Name, c.CourseName,
               m.Section, m.Semester,
               CASE WHEN m.MappingId IS NULL THEN 0 ELSE 1 END AS IsMapped
        FROM dbo.Student s
        LEFT JOIN dbo.CourseMaster c ON c.CourseId = s.CourseId
        LEFT JOIN dbo.StudentMapping m ON m.StudentId = s.StudentId
        WHERE s.Status = 'Active'
        ORDER BY s.CreatedDate DESC;
    END

    IF @Flag = 'SAVE'   -- upsert: replace existing mapping for this student
    BEGIN
        DELETE FROM dbo.StudentMapping WHERE StudentId = @StudentId;
        INSERT INTO dbo.StudentMapping (StudentId, Section, Semester)
        VALUES (@StudentId, @Section, @Semester);
    END
END
GO

-- =========================================================
-- SP 3 : IDENTITY GENERATION (Enrollment No / ID Card)
-- =========================================================
IF OBJECT_ID('dbo.sp_StudentIdentity', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_StudentIdentity;
GO

CREATE PROCEDURE dbo.sp_StudentIdentity
(
    @Flag       NVARCHAR(20),
    @StudentId  NVARCHAR(20) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT s.StudentId, s.FullName AS Name, c.CourseName,
               i.EnrollmentNo,
               CASE WHEN i.IdentityId IS NULL THEN 0 ELSE 1 END AS IsGenerated
        FROM dbo.Student s
        LEFT JOIN dbo.CourseMaster c ON c.CourseId = s.CourseId
        LEFT JOIN dbo.StudentIdentity i ON i.StudentId = s.StudentId
        WHERE s.Status = 'Active'
        ORDER BY s.CreatedDate DESC;
    END

    IF @Flag = 'GETONE'   -- for the ID card preview (course/session/studentId)
    BEGIN
        SELECT s.StudentId, s.FullName AS Name, c.CourseName, a.SessionName AS Session,
               i.EnrollmentNo
        FROM dbo.Student s
        LEFT JOIN dbo.CourseMaster c ON c.CourseId = s.CourseId
        LEFT JOIN dbo.AcademicSession a ON a.AcademicSessionId = s.AcademicSessionId
        LEFT JOIN dbo.StudentIdentity i ON i.StudentId = s.StudentId
        WHERE s.StudentId = @StudentId;
    END

    IF @Flag = 'GENERATE'
    BEGIN
        IF EXISTS (SELECT 1 FROM dbo.StudentIdentity WHERE StudentId = @StudentId)
        BEGIN
            SELECT EnrollmentNo, 0 AS IsNew FROM dbo.StudentIdentity WHERE StudentId = @StudentId;
        END
        ELSE
        BEGIN
            DECLARE @NextSeq INT, @NewEnrollNo NVARCHAR(30);
            SELECT @NextSeq = ISNULL(MAX(CAST(RIGHT(EnrollmentNo, 4) AS INT)), 0) + 1 FROM dbo.StudentIdentity;
            SET @NewEnrollNo = 'ENR-' + CAST(YEAR(GETDATE()) AS NVARCHAR(4)) + '-' + RIGHT('0000' + CAST(@NextSeq AS NVARCHAR(4)), 4);

            INSERT INTO dbo.StudentIdentity (StudentId, EnrollmentNo)
            VALUES (@StudentId, @NewEnrollNo);

            SELECT @NewEnrollNo AS EnrollmentNo, 1 AS IsNew;
        END
    END
END
GO

-- =========================================================
-- SP 4 : ACADEMIC PROGRESS (Semester-wise SGPA/Attendance)
-- =========================================================
IF OBJECT_ID('dbo.sp_AcademicProgress', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_AcademicProgress;
GO

CREATE PROCEDURE dbo.sp_AcademicProgress
(
    @Flag         NVARCHAR(20),
    @StudentId    NVARCHAR(20) = NULL,
    @Semester     INT = NULL,
    @SGPA         DECIMAL(4,2) = NULL,
    @Attendance   DECIMAL(5,2) = NULL,
    @ResultStatus NVARCHAR(20) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT p.ProgressId, p.StudentId, s.FullName AS Name, p.Semester, p.SGPA, p.Attendance, p.ResultStatus
        FROM dbo.AcademicProgress p
        INNER JOIN dbo.Student s ON s.StudentId = p.StudentId
        ORDER BY p.CreatedDate DESC;
    END

    IF @Flag = 'GETSTUDENTS'   -- for the "Select Student" dropdown (active students + their course)
    BEGIN
        SELECT s.StudentId, s.FullName AS Name, c.CourseName
        FROM dbo.Student s
        LEFT JOIN dbo.CourseMaster c ON c.CourseId = s.CourseId
        WHERE s.Status = 'Active'
        ORDER BY s.FullName;
    END

    IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO dbo.AcademicProgress (StudentId, Semester, SGPA, Attendance, ResultStatus)
        VALUES (@StudentId, @Semester, @SGPA, @Attendance, ISNULL(@ResultStatus, 'Pending'));

        SELECT SCOPE_IDENTITY() AS ProgressId;
    END
END
GO

-- =========================================================
-- SP 5 : CERTIFICATE MANAGEMENT
-- =========================================================
IF OBJECT_ID('dbo.sp_CertificateIssued', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CertificateIssued;
GO

CREATE PROCEDURE dbo.sp_CertificateIssued
(
    @Flag             NVARCHAR(20),
    @StudentId        NVARCHAR(20) = NULL,
    @CertificateType  NVARCHAR(60) = NULL,
    @Purpose          NVARCHAR(200) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT c.CertificateId, c.CertNo, c.StudentId, s.FullName AS Name,
               c.CertificateType, c.Purpose, c.IssuedOn
        FROM dbo.CertificateIssued c
        INNER JOIN dbo.Student s ON s.StudentId = c.StudentId
        ORDER BY c.IssuedOn DESC;
    END

    IF @Flag = 'GETSTUDENTS'
    BEGIN
        SELECT s.StudentId, s.FullName AS Name, c.CourseName
        FROM dbo.Student s
        LEFT JOIN dbo.CourseMaster c ON c.CourseId = s.CourseId
        ORDER BY s.FullName;
    END

    IF @Flag = 'INSERT'
    BEGIN
        DECLARE @NextSeq INT, @NewCertNo NVARCHAR(30);
        SELECT @NextSeq = ISNULL(MAX(CertificateId), 0) + 1 FROM dbo.CertificateIssued;
        SET @NewCertNo = 'CERT-' + CAST(YEAR(GETDATE()) AS NVARCHAR(4)) + '-' + RIGHT('0000' + CAST(@NextSeq AS NVARCHAR(4)), 4);

        INSERT INTO dbo.CertificateIssued (CertNo, StudentId, CertificateType, Purpose)
        VALUES (@NewCertNo, @StudentId, @CertificateType, @Purpose);

        SELECT @NewCertNo AS CertNo;
    END
END
GO

-- =========================================================
-- SP 6 : ALUMNI (only Graduated students)
-- =========================================================
IF OBJECT_ID('dbo.sp_AlumniInfo', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_AlumniInfo;
GO

CREATE PROCEDURE dbo.sp_AlumniInfo
(
    @Flag         NVARCHAR(20),
    @StudentId    NVARCHAR(20) = NULL,
    @Company      NVARCHAR(150) = NULL,
    @Designation  NVARCHAR(100) = NULL,
    @Email        NVARCHAR(150) = NULL,
    @LinkedInUrl  NVARCHAR(250) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT s.StudentId, s.FullName AS Name, c.CourseName, a.SessionName AS Session,
               ai.Company, ai.Designation
        FROM dbo.Student s
        LEFT JOIN dbo.CourseMaster c ON c.CourseId = s.CourseId
        LEFT JOIN dbo.AcademicSession a ON a.AcademicSessionId = s.AcademicSessionId
        LEFT JOIN dbo.AlumniInfo ai ON ai.StudentId = s.StudentId
        WHERE s.Status = 'Graduated'
        ORDER BY s.FullName;
    END

    IF @Flag = 'GETBYID'
    BEGIN
        SELECT StudentId, Company, Designation, Email, LinkedInUrl
        FROM dbo.AlumniInfo
        WHERE StudentId = @StudentId;
    END

    IF @Flag = 'SAVE'   -- upsert
    BEGIN
        IF EXISTS (SELECT 1 FROM dbo.AlumniInfo WHERE StudentId = @StudentId)
        BEGIN
            UPDATE dbo.AlumniInfo
            SET Company = @Company, Designation = @Designation, Email = @Email,
                LinkedInUrl = @LinkedInUrl, UpdatedDate = GETDATE()
            WHERE StudentId = @StudentId;
        END
        ELSE
        BEGIN
            INSERT INTO dbo.AlumniInfo (StudentId, Company, Designation, Email, LinkedInUrl)
            VALUES (@StudentId, @Company, @Designation, @Email, @LinkedInUrl);
        END
    END
END
GO