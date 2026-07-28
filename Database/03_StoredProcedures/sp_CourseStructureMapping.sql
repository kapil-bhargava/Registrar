

CREATE OR ALTER PROCEDURE sp_CourseStructureMapping
    @Flag NVARCHAR(20),
    @MappingId INT = NULL,
    @CourseId INT = NULL,
    @BranchId INT = NULL,
    @SemesterNumber INT = NULL,
    @IsActive BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT m.MappingId, m.CourseId, c.CourseName, m.BranchId, b.BranchName,
               m.SemesterNumber, m.IsActive, m.CreatedDate
        FROM CourseStructureMapping m
        INNER JOIN CourseMaster c ON c.CourseId = m.CourseId
        INNER JOIN BranchMaster b ON b.BranchId = m.BranchId
        ORDER BY m.MappingId DESC;
    END

    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO CourseStructureMapping (CourseId, BranchId, SemesterNumber, IsActive)
        VALUES (@CourseId, @BranchId, @SemesterNumber, ISNULL(@IsActive, 1));
    END

    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE CourseStructureMapping
        SET CourseId = @CourseId,
            BranchId = @BranchId,
            SemesterNumber = @SemesterNumber,
            IsActive = @IsActive
        WHERE MappingId = @MappingId;
    END

    ELSE IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM CourseStructureMapping WHERE MappingId = @MappingId;
    END

    -- Cascading: Course select hote hi uske Department ke Branches
    ELSE IF @Flag = 'GETBRANCHESBYCOURSE'
    BEGIN
        SELECT b.BranchId, b.BranchCode, b.BranchName
        FROM BranchMaster b
        INNER JOIN CourseMaster c ON c.DepartmentId = b.DepartmentId
        WHERE c.CourseId = @CourseId AND b.IsActive = 1;
    END
END
GO



UPDATE CourseMaster SET TotalSemesters = 6 WHERE CourseId = 2; -- BCA (3 years)
UPDATE CourseMaster SET TotalSemesters = 8 WHERE CourseId = 6; -- B.Tech (4 years)
UPDATE CourseMaster SET TotalSemesters = 4 WHERE CourseId = 5; -- MBA (2 years)
UPDATE CourseMaster SET TotalSemesters = 4 WHERE CourseId = 3; -- MCA (2 years)
UPDATE CourseMaster SET TotalSemesters = 4 WHERE CourseId = 4; -- M.Tech (2 years)



SELECT CourseId, CourseName, TotalSemesters FROM CourseMaster ORDER BY CourseName