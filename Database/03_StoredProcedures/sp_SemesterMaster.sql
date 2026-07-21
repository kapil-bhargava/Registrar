-- Step 1: FK constraint hatao (sirf yeh relationship tootega, baaki kuch nahi)
ALTER TABLE dbo.SemesterMaster
DROP CONSTRAINT FK_SemesterMaster_AcademicSession;

-- Step 2: naya text column add karo (asal session text yahan save hoga)
ALTER TABLE dbo.SemesterMaster
ADD AcademicSessionName NVARCHAR(50) NULL;

-- Step 3: purane AcademicSessionId column ko optional bana do (FK gaya, ab isko required rakhne ki zaroorat nahi)
ALTER TABLE dbo.SemesterMaster
ALTER COLUMN AcademicSessionId INT NULL;


ALTER PROCEDURE sp_SemesterMaster
    @Flag               NVARCHAR(20),
    @SemesterId         INT             = NULL,
    @CourseId           INT             = NULL,
    @AcademicSessionName NVARCHAR(50)   = NULL,
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
               sm.AcademicSessionName,
               sm.SemesterNumber, sm.SemesterName, sm.StartDate, sm.EndDate,
               sm.CreditLimit, sm.Status, sm.CreatedDate
        FROM SemesterMaster sm
        INNER JOIN CourseMaster c ON sm.CourseId = c.CourseId
        ORDER BY c.CourseName, sm.SemesterNumber;

    ELSE IF @Flag = 'GETBYCOURSE'
        SELECT SemesterId, SemesterNumber, SemesterName
        FROM SemesterMaster
        WHERE CourseId = @CourseId
        ORDER BY SemesterNumber;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM SemesterMaster WHERE SemesterId = @SemesterId;

    -- NAYA: purane save kiye gaye Academic Session names ki distinct list (datalist ke liye)
    ELSE IF @Flag = 'GETDISTINCTSESSIONS'
        SELECT DISTINCT AcademicSessionName
        FROM SemesterMaster
        WHERE AcademicSessionName IS NOT NULL AND AcademicSessionName <> ''
        ORDER BY AcademicSessionName;

    ELSE IF @Flag = 'INSERT'
        INSERT INTO SemesterMaster (CourseId, AcademicSessionName, SemesterNumber, SemesterName, StartDate, EndDate, CreditLimit, Status)
        VALUES (@CourseId, @AcademicSessionName, @SemesterNumber, @SemesterName, @StartDate, @EndDate, @CreditLimit, ISNULL(@Status, 'Upcoming'));

    ELSE IF @Flag = 'UPDATE'
        UPDATE SemesterMaster
        SET CourseId = @CourseId, AcademicSessionName = @AcademicSessionName,
            SemesterNumber = @SemesterNumber, SemesterName = @SemesterName,
            StartDate = @StartDate, EndDate = @EndDate, CreditLimit = @CreditLimit, Status = @Status
        WHERE SemesterId = @SemesterId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM SemesterMaster WHERE SemesterId = @SemesterId;
END
GO