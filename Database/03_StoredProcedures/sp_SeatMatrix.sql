IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'sp_SeatMatrix')
    DROP PROCEDURE sp_SeatMatrix
GO
CREATE PROCEDURE sp_SeatMatrix
    @Flag              NVARCHAR(20),
    @SeatMatrixId      INT = NULL,
    @CourseId          INT = NULL,
    @AcademicSessionId INT = NULL,
    @TotalSeats        INT = NULL,
    @IsActive          BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        SELECT sm.SeatMatrixId, sm.CourseId, c.CourseName,
               sm.AcademicSessionId, s.SessionName,
               sm.TotalSeats, sm.IsActive, sm.CreatedDate
        FROM SeatMatrix sm
        INNER JOIN CourseMaster c ON c.CourseId = sm.CourseId
        INNER JOIN AcademicSession s ON s.AcademicSessionId = sm.AcademicSessionId
        ORDER BY sm.SeatMatrixId DESC;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM SeatMatrix WHERE SeatMatrixId = @SeatMatrixId;

    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO SeatMatrix (CourseId, AcademicSessionId, TotalSeats, IsActive)
        VALUES (@CourseId, @AcademicSessionId, @TotalSeats, ISNULL(@IsActive, 1));
    END

    ELSE IF @Flag = 'UPDATE'
        UPDATE SeatMatrix
        SET CourseId = @CourseId,
            AcademicSessionId = @AcademicSessionId,
            TotalSeats = @TotalSeats,
            IsActive = @IsActive
        WHERE SeatMatrixId = @SeatMatrixId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM SeatMatrix WHERE SeatMatrixId = @SeatMatrixId;
END