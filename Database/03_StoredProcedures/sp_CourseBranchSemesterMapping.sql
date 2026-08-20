

CREATE PROCEDURE sp_CourseBranchSemesterMapping
    @Flag        VARCHAR(30),
    @MappingId   INT = NULL,
    @CourseId    INT = NULL,
    @BranchId    INT = NULL,
    @SemesterId  INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Poori mapping list (grid ke liye)
    IF @Flag = 'GETALL'
    BEGIN
        SELECT m.MappingId, m.CourseId, c.CourseName, m.BranchId, b.BranchName,
               m.SemesterId, s.SemesterNumber, s.SemesterName, m.IsActive, m.CreatedDate
        FROM CourseBranchSemesterMapping m
        INNER JOIN CourseMaster   c ON c.CourseId  = m.CourseId
        INNER JOIN BranchMaster   b ON b.BranchId  = m.BranchId
        INNER JOIN SemesterMaster s ON s.SemesterId = m.SemesterId
        ORDER BY c.CourseName, b.BranchName, s.SemesterNumber;
    END

    -- Ek Course+Branch ke already-mapped semesters (checkbox pre-check ke liye)
    IF @Flag = 'GETBYCOURSEBRANCH'
    BEGIN
        SELECT m.MappingId, m.SemesterId, s.SemesterNumber, s.SemesterName
        FROM CourseBranchSemesterMapping m
        INNER JOIN SemesterMaster s ON s.SemesterId = m.SemesterId
        WHERE m.CourseId = @CourseId AND m.BranchId = @BranchId
        ORDER BY s.SemesterNumber;
    END

    -- Ek row insert (loop se multiple semesters ke liye call hoga)
    IF @Flag = 'INSERT'
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM CourseBranchSemesterMapping
                        WHERE CourseId = @CourseId AND BranchId = @BranchId AND SemesterId = @SemesterId)
        BEGIN
            INSERT INTO CourseBranchSemesterMapping (CourseId, BranchId, SemesterId, IsActive, CreatedDate)
            VALUES (@CourseId, @BranchId, @SemesterId, 1, GETDATE());
        END
    END

    -- Save se pehle purani selection clear karne ke liye
    IF @Flag = 'DELETEBYCOURSEBRANCH'
    BEGIN
        DELETE FROM CourseBranchSemesterMapping
        WHERE CourseId = @CourseId AND BranchId = @BranchId;
    END

    IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM CourseBranchSemesterMapping WHERE MappingId = @MappingId;
    END
END
GO