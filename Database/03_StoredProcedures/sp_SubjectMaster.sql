CREATE PROCEDURE sp_SubjectMaster
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
        FROM SubjectMaster sub
        INNER JOIN SemesterMaster sem ON sub.SemesterId = sem.SemesterId
        INNER JOIN CourseMaster c ON sub.CourseId = c.CourseId
        ORDER BY c.CourseName, sem.SemesterNumber, sub.SubjectName;

    ELSE IF @Flag = 'GETBYSEMESTER'  -- subjects for one Semester
        SELECT SubjectId, SubjectCode, SubjectName, SubjectType, Credits
        FROM SubjectMaster
        WHERE SemesterId = @SemesterId AND Status = 'Active';

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM SubjectMaster WHERE SubjectId = @SubjectId;

    ELSE IF @Flag = 'INSERT'
        INSERT INTO SubjectMaster (SubjectCode, SubjectName, SubjectType, SemesterId, CourseId, Credits, Status)
        VALUES (@SubjectCode, @SubjectName, @SubjectType, @SemesterId, @CourseId, @Credits, ISNULL(@Status, 'Active'));

    ELSE IF @Flag = 'UPDATE'
        UPDATE SubjectMaster
        SET SubjectCode = @SubjectCode, SubjectName = @SubjectName, SubjectType = @SubjectType,
            SemesterId = @SemesterId, CourseId = @CourseId, Credits = @Credits, Status = @Status
        WHERE SubjectId = @SubjectId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM SubjectMaster WHERE SubjectId = @SubjectId;
END
GO

INSERT INTO SubjectMaster (SubjectCode, SubjectName, SubjectType, SemesterId, CourseId, Credits, Status) VALUES
('CS201', 'Data Structures', 'Theory', 1, 1, 4, 'Active'),
('EC202L', 'Electronics Lab', 'Practical', 1, 1, 2, 'Active');
GO