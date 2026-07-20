/* ============================================================
   SEMESTER
   CourseId          -> FK to Course           (mapping #5)
   AcademicSessionId -> FK to AcademicSession   (mapping #6)
   ============================================================ */







CREATE PROCEDURE sp_Subject
    @Flag           NVARCHAR(20),
    @SubjectId      INT             = NULL,
    @SubjectCode    NVARCHAR(30)    = NULL,
    @SubjectName    NVARCHAR(200)   = NULL,
    @SubjectType    NVARCHAR(20)    = NULL,
    @SemesterId     INT             = NULL,
    @CourseId       INT             = NULL,
    @Credits        INT             = NULL,
    @Status         NVARCHAR(20)    = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        SELECT sub.SubjectId, sub.SubjectCode, sub.SubjectName, sub.SubjectType,
               sub.SemesterId, sem.SemesterName, sub.CourseId, c.CourseName,
               sub.Credits, sub.Status, sub.CreatedDate
        FROM Subject sub
        INNER JOIN Semester sem ON sub.SemesterId = sem.SemesterId
        INNER JOIN Course c ON sub.CourseId = c.CourseId
        ORDER BY c.CourseName, sem.SemesterNumber, sub.SubjectName;

    ELSE IF @Flag = 'GETBYSEMESTER'  -- subjects for one Semester
        SELECT SubjectId, SubjectCode, SubjectName, SubjectType, Credits FROM Subject WHERE SemesterId = @SemesterId AND Status = 'Active';

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM Subject WHERE SubjectId = @SubjectId;

    ELSE IF @Flag = 'INSERT'
        INSERT INTO Subject (SubjectCode, SubjectName, SubjectType, SemesterId, CourseId, Credits, Status)
        VALUES (@SubjectCode, @SubjectName, @SubjectType, @SemesterId, @CourseId, @Credits, @Status);

    ELSE IF @Flag = 'UPDATE'
        UPDATE Subject
        SET SubjectCode = @SubjectCode, SubjectName = @SubjectName, SubjectType = @SubjectType,
            SemesterId = @SemesterId, CourseId = @CourseId, Credits = @Credits, Status = @Status
        WHERE SubjectId = @SubjectId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM Subject WHERE SubjectId = @SubjectId;
END
GO

INSERT INTO Subject (SubjectCode, SubjectName, SubjectType, SemesterId, CourseId, Credits, Status) VALUES
('CS201', 'Data Structures', 'Theory', 3, 1, 4, 'Active'),
('EC202L', 'Electronics Lab', 'Practical', 3, 1, 2, 'Active');
GO