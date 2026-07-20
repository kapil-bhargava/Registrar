
CREATE PROCEDURE sp_Semester
    @Flag                 NVARCHAR(20),
    @SemesterId           INT             = NULL,
    @CourseId             INT             = NULL,
    @AcademicSessionId    INT             = NULL,
    @SemesterNumber       INT             = NULL,
    @SemesterName         NVARCHAR(50)    = NULL,
    @StartDate            DATE            = NULL,
    @EndDate              DATE            = NULL,
    @CreditLimit          INT             = NULL,
    @Status               NVARCHAR(20)    = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        SELECT sem.SemesterId, sem.CourseId, c.CourseName, sem.AcademicSessionId, s.SessionName,
               sem.SemesterNumber, sem.SemesterName, sem.StartDate, sem.EndDate, sem.CreditLimit, sem.Status, sem.CreatedDate
        FROM Semester sem
        INNER JOIN Course c ON sem.CourseId = c.CourseId
        INNER JOIN AcademicSession s ON sem.AcademicSessionId = s.AcademicSessionId
        ORDER BY c.CourseName, sem.SemesterNumber;

    ELSE IF @Flag = 'GETBYCOURSE'    -- cascading dropdown: semesters under one Course (used by Subject)
        SELECT SemesterId, SemesterNumber, SemesterName FROM Semester WHERE CourseId = @CourseId ORDER BY SemesterNumber;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM Semester WHERE SemesterId = @SemesterId;

    ELSE IF @Flag = 'INSERT'
        INSERT INTO Semester (CourseId, AcademicSessionId, SemesterNumber, SemesterName, StartDate, EndDate, CreditLimit, Status)
        VALUES (@CourseId, @AcademicSessionId, @SemesterNumber, @SemesterName, @StartDate, @EndDate, @CreditLimit, @Status);

    ELSE IF @Flag = 'UPDATE'
        UPDATE Semester
        SET CourseId = @CourseId, AcademicSessionId = @AcademicSessionId, SemesterNumber = @SemesterNumber,
            SemesterName = @SemesterName, StartDate = @StartDate, EndDate = @EndDate,
            CreditLimit = @CreditLimit, Status = @Status
        WHERE SemesterId = @SemesterId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM Semester WHERE SemesterId = @SemesterId;
END
GO

INSERT INTO Semester (CourseId, AcademicSessionId, SemesterNumber, SemesterName, StartDate, EndDate, CreditLimit, Status) VALUES
(1, 1, 1, 'Semester 1', '2025-07-01', '2025-12-15', 22, 'Completed'),
(1, 1, 2, 'Semester 2', '2026-01-05', '2026-05-20', 24, 'Completed'),
(1, 1, 3, 'Semester 3', '2026-07-01', '2026-12-15', 24, 'Active');
GO
