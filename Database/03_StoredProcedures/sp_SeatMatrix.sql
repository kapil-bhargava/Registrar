--IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'sp_SeatMatrix')
--    DROP PROCEDURE sp_SeatMatrix
--GO
--CREATE PROCEDURE sp_SeatMatrix
--    @Flag              NVARCHAR(20),
--    @SeatMatrixId      INT = NULL,
--    @CourseId          INT = NULL,
--    @AcademicSessionId INT = NULL,
--    @TotalSeats        INT = NULL,
--    @IsActive          BIT = NULL
--AS
--BEGIN
--    SET NOCOUNT ON;

--    IF @Flag = 'GETALL'
--        SELECT sm.SeatMatrixId, sm.CourseId, c.CourseName,
--               sm.AcademicSessionId, s.SessionName,
--               sm.TotalSeats, sm.IsActive, sm.CreatedDate
--        FROM SeatMatrix sm
--        INNER JOIN CourseMaster c ON c.CourseId = sm.CourseId
--        INNER JOIN AcademicSession s ON s.AcademicSessionId = sm.AcademicSessionId
--        ORDER BY sm.SeatMatrixId DESC;

--    ELSE IF @Flag = 'GETBYID'
--        SELECT * FROM SeatMatrix WHERE SeatMatrixId = @SeatMatrixId;

--    ELSE IF @Flag = 'INSERT'
--    BEGIN
--        INSERT INTO SeatMatrix (CourseId, AcademicSessionId, TotalSeats, IsActive)
--        VALUES (@CourseId, @AcademicSessionId, @TotalSeats, ISNULL(@IsActive, 1));
--    END

--    ELSE IF @Flag = 'UPDATE'
--        UPDATE SeatMatrix
--        SET CourseId = @CourseId,
--            AcademicSessionId = @AcademicSessionId,
--            TotalSeats = @TotalSeats,
--            IsActive = @IsActive
--        WHERE SeatMatrixId = @SeatMatrixId;

--    ELSE IF @Flag = 'DELETE'
--        DELETE FROM SeatMatrix WHERE SeatMatrixId = @SeatMatrixId;
--END

-- =========================================================
-- SEAT MATRIX
-- =========================================================
CREATE OR ALTER PROCEDURE dbo.sp_SeatMatrix
    @Flag               VARCHAR(30),
    @SeatMatrixId       INT             = NULL,
    @CourseId           INT             = NULL,
    @AcademicSessionId  INT             = NULL,
    @TotalSeats         INT             = NULL,
    @IsActive           BIT             = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT
            sm.SeatMatrixId,
            sm.CourseId, c.CourseName, c.CourseCode,
            sm.AcademicSessionId, ase.SessionName,
            sm.TotalSeats,
            sm.IsActive,
            sm.CreatedDate
        FROM dbo.SeatMatrix sm
        LEFT JOIN dbo.CourseMaster     c   ON c.CourseId = sm.CourseId
        LEFT JOIN dbo.AcademicSession  ase ON ase.AcademicSessionId = sm.AcademicSessionId
        ORDER BY sm.SeatMatrixId DESC;
    END

    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT
            sm.SeatMatrixId,
            sm.CourseId, c.CourseName, c.CourseCode,
            sm.AcademicSessionId, ase.SessionName,
            sm.TotalSeats,
            sm.IsActive,
            sm.CreatedDate
        FROM dbo.SeatMatrix sm
        LEFT JOIN dbo.CourseMaster     c   ON c.CourseId = sm.CourseId
        LEFT JOIN dbo.AcademicSession  ase ON ase.AcademicSessionId = sm.AcademicSessionId
        WHERE sm.SeatMatrixId = @SeatMatrixId;
    END

    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO dbo.SeatMatrix (CourseId, AcademicSessionId, TotalSeats, IsActive, CreatedDate)
        VALUES (@CourseId, @AcademicSessionId, @TotalSeats, ISNULL(@IsActive, 1), GETDATE());
    END

    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE dbo.SeatMatrix
        SET CourseId          = @CourseId,
            AcademicSessionId = @AcademicSessionId,
            TotalSeats        = @TotalSeats,
            IsActive           = ISNULL(@IsActive, IsActive)
        WHERE SeatMatrixId = @SeatMatrixId;
    END

    ELSE IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM dbo.SeatMatrix WHERE SeatMatrixId = @SeatMatrixId;
    END
END
GO


-- =========================================================
-- SEAT CATEGORY MAPPING  (category-wise split of a SeatMatrix's TotalSeats)
-- =========================================================
CREATE OR ALTER PROCEDURE dbo.sp_SeatCategoryMapping
    @Flag           VARCHAR(30),
    @SeatMatrixId   INT             = NULL,
    @CategoryIds    NVARCHAR(MAX)   = NULL,   -- CSV, e.g. '1,2,3'
    @Seats          NVARCHAR(MAX)   = NULL    -- CSV, e.g. '30,20,10' (same order/position as @CategoryIds)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT
            sm.SeatMatrixId,
            c.CourseName, c.CourseCode,
            ase.SessionName,
            sm.TotalSeats,
            scm.CategoryId, cat.CategoryName,
            scm.AllocatedSeats
        FROM dbo.SeatMatrix sm
        LEFT JOIN dbo.CourseMaster     c   ON c.CourseId = sm.CourseId
        LEFT JOIN dbo.AcademicSession  ase ON ase.AcademicSessionId = sm.AcademicSessionId
        LEFT JOIN dbo.SeatCategoryMapping scm ON scm.SeatMatrixId = sm.SeatMatrixId
        LEFT JOIN dbo.Category         cat ON cat.CategoryId = scm.CategoryId
        ORDER BY sm.SeatMatrixId DESC;
    END

    ELSE IF @Flag = 'GETBYSEATMATRIX'
    BEGIN
        SELECT
            scm.CategoryId,
            scm.AllocatedSeats
        FROM dbo.SeatCategoryMapping scm
        WHERE scm.SeatMatrixId = @SeatMatrixId;
    END

    ELSE IF @Flag = 'SAVE'
    BEGIN
        -- Replace-all pattern: wipe existing rows for this SeatMatrixId, then re-insert from the CSVs.
        DELETE FROM dbo.SeatCategoryMapping WHERE SeatMatrixId = @SeatMatrixId;

        ;WITH cats AS (
            SELECT value AS CategoryId, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS rn
            FROM STRING_SPLIT(@CategoryIds, ',')
        ),
        seats AS (
            SELECT value AS AllocatedSeats, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS rn
            FROM STRING_SPLIT(@Seats, ',')
        )
        INSERT INTO dbo.SeatCategoryMapping (SeatMatrixId, CategoryId, AllocatedSeats)
        SELECT @SeatMatrixId, TRY_CAST(cats.CategoryId AS INT), TRY_CAST(seats.AllocatedSeats AS INT)
        FROM cats
        INNER JOIN seats ON seats.rn = cats.rn;
    END
END
GO