IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'sp_SeatCategoryMapping')
    DROP PROCEDURE sp_SeatCategoryMapping
GO
CREATE PROCEDURE sp_SeatCategoryMapping
    @Flag              NVARCHAR(20),
    @SeatMatrixId      INT = NULL,
    @CategoryIds       NVARCHAR(MAX) = NULL,   -- comma separated
    @Seats             NVARCHAR(MAX) = NULL    -- comma separated, same order as @CategoryIds
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT sm.SeatMatrixId, c.CourseName, c.CourseCode, s.SessionName, sm.TotalSeats,
               scm.CategoryId, cat.CategoryName, scm.AllocatedSeats
        FROM SeatMatrix sm
        INNER JOIN CourseMaster c ON c.CourseId = sm.CourseId
        INNER JOIN AcademicSession s ON s.AcademicSessionId = sm.AcademicSessionId
        LEFT JOIN SeatCategoryMapping scm ON scm.SeatMatrixId = sm.SeatMatrixId
        LEFT JOIN Category cat ON cat.CategoryId = scm.CategoryId
        ORDER BY sm.SeatMatrixId DESC;
    END

    ELSE IF @Flag = 'GETBYSEATMATRIX'
        SELECT CategoryId, AllocatedSeats FROM SeatCategoryMapping WHERE SeatMatrixId = @SeatMatrixId;

    ELSE IF @Flag = 'SAVE'
    BEGIN
        DELETE FROM SeatCategoryMapping WHERE SeatMatrixId = @SeatMatrixId;

        INSERT INTO SeatCategoryMapping (SeatMatrixId, CategoryId, AllocatedSeats)
        SELECT @SeatMatrixId,
               CAST(c.value AS INT),
               CAST(s.value AS INT)
        FROM (SELECT value, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS rn FROM STRING_SPLIT(@CategoryIds, ',')) c
        JOIN (SELECT value, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS rn FROM STRING_SPLIT(@Seats, ',')) s
             ON c.rn = s.rn;
    END

    ELSE IF @Flag = 'DELETE'
        DELETE FROM SeatCategoryMapping WHERE SeatMatrixId = @SeatMatrixId;
END