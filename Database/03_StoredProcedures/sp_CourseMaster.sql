
CREATE PROCEDURE sp_CourseMaster
    @Flag               NVARCHAR(20),
    @CourseId           INT             = NULL,
    @CourseCode         NVARCHAR(30)    = NULL,
    @CourseName         NVARCHAR(200)   = NULL,
    @ProgramId          INT             = NULL,
    @DepartmentId       INT             = NULL,
    @DurationYears      INT             = NULL,
    @TotalSemesters     INT             = NULL,
    @TotalCredits       INT             = NULL,
    @IntakeCapacity     INT             = NULL,
    @Status             NVARCHAR(20)    = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        SELECT c.CourseId, c.CourseCode, c.CourseName,
               c.ProgramId, p.ProgramName,
               c.DepartmentId, d.DepartmentName,
               c.DurationYears, c.TotalSemesters, c.TotalCredits, c.IntakeCapacity,
               c.Status, c.CreatedDate
        FROM CourseMaster c
        INNER JOIN ProgramMaster p ON c.ProgramId = p.ProgramId
        INNER JOIN DepartmentMaster d ON c.DepartmentId = d.DepartmentId
        ORDER BY d.DepartmentName, c.CourseName;

    ELSE IF @Flag = 'GETACTIVE'
        SELECT CourseId, CourseCode, CourseName, TotalSemesters
        FROM CourseMaster
        WHERE Status = 'Active'
        ORDER BY CourseName;

    ELSE IF @Flag = 'GETBYDEPARTMENT'   -- cascading: courses under one department
        SELECT CourseId, CourseCode, CourseName
        FROM CourseMaster
        WHERE DepartmentId = @DepartmentId AND Status = 'Active'
        ORDER BY CourseName;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM CourseMaster WHERE CourseId = @CourseId;

    ELSE IF @Flag = 'INSERT'
        INSERT INTO CourseMaster (CourseCode, CourseName, ProgramId, DepartmentId, DurationYears, TotalSemesters, TotalCredits, IntakeCapacity, Status)
        VALUES (@CourseCode, @CourseName, @ProgramId, @DepartmentId, @DurationYears, @TotalSemesters, @TotalCredits, @IntakeCapacity, ISNULL(@Status, 'Active'));

    ELSE IF @Flag = 'UPDATE'
        UPDATE CourseMaster
        SET CourseCode = @CourseCode, CourseName = @CourseName, ProgramId = @ProgramId,
            DepartmentId = @DepartmentId, DurationYears = @DurationYears,
            TotalSemesters = @TotalSemesters, TotalCredits = @TotalCredits,
            IntakeCapacity = @IntakeCapacity, Status = @Status
        WHERE CourseId = @CourseId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM CourseMaster WHERE CourseId = @CourseId;
END
GO

INSERT INTO CourseMaster (CourseCode, CourseName, ProgramId, DepartmentId, DurationYears, TotalSemesters, TotalCredits, IntakeCapacity, Status) VALUES
('BTECH-CS', 'Bachelor of Technology - Computer Science', 1, 1, 4, 8, 160, 120, 'Active'),
('BTECH-EC', 'Bachelor of Technology - Electronics', 1, 2, 4, 8, 160, 60, 'Active');
GO