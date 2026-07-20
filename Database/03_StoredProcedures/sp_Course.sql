CREATE PROCEDURE sp_Course
    @Flag              NVARCHAR(20),
    @CourseId          INT             = NULL,
    @CourseCode        NVARCHAR(30)    = NULL,
    @CourseName        NVARCHAR(200)   = NULL,
    @ProgramId         INT             = NULL,
    @DepartmentId      INT             = NULL,
    @DurationYears     INT             = NULL,
    @TotalSemesters    INT             = NULL,
    @TotalCredits      INT             = NULL,
    @IntakeCapacity    INT             = NULL,
    @Status            NVARCHAR(20)    = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        SELECT c.CourseId, c.CourseCode, c.CourseName, c.ProgramId, p.ProgramName,
               c.DepartmentId, d.DepartmentName, c.DurationYears, c.TotalSemesters,
               c.TotalCredits, c.IntakeCapacity, c.Status, c.CreatedDate
        FROM Course c
        INNER JOIN Program p ON c.ProgramId = p.ProgramId
        INNER JOIN Department d ON c.DepartmentId = d.DepartmentId
        ORDER BY c.CourseName;

    ELSE IF @Flag = 'GETACTIVE'      -- used as dropdown source by Semester
        SELECT CourseId, CourseCode, CourseName, TotalSemesters FROM Course WHERE Status = 'Active' ORDER BY CourseName;

    ELSE IF @Flag = 'GETBYDEPARTMENT'  -- cascading dropdown: courses under one Department
        SELECT CourseId, CourseCode, CourseName FROM Course WHERE DepartmentId = @DepartmentId AND Status = 'Active' ORDER BY CourseName;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM Course WHERE CourseId = @CourseId;

    ELSE IF @Flag = 'INSERT'
        INSERT INTO Course (CourseCode, CourseName, ProgramId, DepartmentId, DurationYears, TotalSemesters, TotalCredits, IntakeCapacity, Status)
        VALUES (@CourseCode, @CourseName, @ProgramId, @DepartmentId, @DurationYears, @TotalSemesters, @TotalCredits, @IntakeCapacity, @Status);

    ELSE IF @Flag = 'UPDATE'
        UPDATE Course
        SET CourseCode = @CourseCode, CourseName = @CourseName, ProgramId = @ProgramId,
            DepartmentId = @DepartmentId, DurationYears = @DurationYears, TotalSemesters = @TotalSemesters,
            TotalCredits = @TotalCredits, IntakeCapacity = @IntakeCapacity, Status = @Status
        WHERE CourseId = @CourseId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM Course WHERE CourseId = @CourseId;
END
GO

INSERT INTO Course (CourseCode, CourseName, ProgramId, DepartmentId, DurationYears, TotalSemesters, TotalCredits, IntakeCapacity, Status) VALUES
('BTECH-CS', 'Bachelor of Technology - Computer Science', 1, 1, 4, 8, 160, 120, 'Active'),
('MBA', 'Master of Business Administration', 2, 3, 2, 4, 80, 60, 'Active'),
('BCA', 'Bachelor of Computer Applications', 1, 1, 3, 6, 120, 120, 'Active');
GO