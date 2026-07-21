/* ================================================================
   sp_SemesterMaster — ALTER version (procedure already exists,
   isliye CREATE nahi, ALTER use karo — koi "already exists" error
   nahi aayega)
   ================================================================ */

CREATE OR ALTER PROCEDURE sp_SemesterMaster
    @Flag               NVARCHAR(20),
    @SemesterId         INT             = NULL,
    @CourseId           INT             = NULL,
    @AcademicSessionId  INT             = NULL,
    @SemesterNumber     INT             = NULL,
    @SemesterName       NVARCHAR(100)   = NULL,
    @StartDate          DATE            = NULL,
    @EndDate            DATE            = NULL,
    @CreditLimit        INT             = NULL,
    @Status             NVARCHAR(20)    = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        SELECT sm.SemesterId, sm.CourseId, c.CourseName,
               sm.AcademicSessionId, s.SessionName,
               sm.SemesterNumber, sm.SemesterName, sm.StartDate, sm.EndDate,
               sm.CreditLimit, sm.Status, sm.CreatedDate
        FROM SemesterMaster sm
        INNER JOIN CourseMaster c ON sm.CourseId = c.CourseId
        INNER JOIN AcademicSession s ON sm.AcademicSessionId = s.AcademicSessionId
        ORDER BY c.CourseName, sm.SemesterNumber;

    ELSE IF @Flag = 'GETBYCOURSE'   -- cascading: semesters under one course (used by Subject)
        SELECT SemesterId, SemesterNumber, SemesterName
        FROM SemesterMaster
        WHERE CourseId = @CourseId
        ORDER BY SemesterNumber;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM SemesterMaster WHERE SemesterId = @SemesterId;

    ELSE IF @Flag = 'INSERT'
        INSERT INTO SemesterMaster (CourseId, AcademicSessionId, SemesterNumber, SemesterName, StartDate, EndDate, CreditLimit, Status)
        VALUES (@CourseId, @AcademicSessionId, @SemesterNumber, @SemesterName, @StartDate, @EndDate, @CreditLimit, ISNULL(@Status, 'Upcoming'));

    ELSE IF @Flag = 'UPDATE'
        UPDATE SemesterMaster
        SET CourseId = @CourseId, AcademicSessionId = @AcademicSessionId,
            SemesterNumber = @SemesterNumber, SemesterName = @SemesterName,
            StartDate = @StartDate, EndDate = @EndDate, CreditLimit = @CreditLimit, Status = @Status
        WHERE SemesterId = @SemesterId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM SemesterMaster WHERE SemesterId = @SemesterId;
END
GO


/* ================================================================
   Corrected seed insert — CourseId ab 2 use kiya hai (tumhare
   CourseMaster me yehi ID maujood hai). AcademicSessionId abhi bhi
   1 hi rakha hai — pehle check kar lena:
       SELECT AcademicSessionId, SessionName FROM AcademicSession;
   Agar wahan bhi 1 na mile to yahan sahi ID daal dena.
   ================================================================ */

INSERT INTO SemesterMaster (CourseId, AcademicSessionId, SemesterNumber, SemesterName, StartDate, EndDate, CreditLimit, Status) VALUES
(2, 1, 3, 'Semester 3', '2026-01-01', '2026-06-30', 24, 'Active'),
(2, 1, 4, 'Semester 4', '2026-07-01', '2026-12-31', 24, 'Upcoming');
GO