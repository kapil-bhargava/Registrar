
CREATE OR ALTER PROCEDURE sp_Category
(
    @Flag VARCHAR(20),
    @CategoryId INT = NULL,
    @CategoryCode VARCHAR(20) = NULL,
    @CategoryName VARCHAR(100) = NULL,
    @Description VARCHAR(250) = NULL,
    @FeeConcession DECIMAL(5,2) = NULL,
    @DisplayOrder INT = NULL,
    @IsActive INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    ----------------------------------------------------------
    -- GET ALL
    ----------------------------------------------------------
    IF @Flag = 'GETALL'
    BEGIN
        SELECT
            CategoryId,
            CategoryCode,
            CategoryName,
            Description,
            FeeConcession,
            DisplayOrder,
            IsActive,
            CreatedDate
        FROM Category
        ORDER BY DisplayOrder;
    END
    ----------------------------------------------------------
    -- GET ACTIVE ONLY (for dropdowns across the project)
    ----------------------------------------------------------
    ELSE IF @Flag = 'GETACTIVE'
    BEGIN
        SELECT
            CategoryId,
            CategoryCode,
            CategoryName
        FROM Category
        WHERE IsActive = 1
        ORDER BY DisplayOrder;
    END
    ----------------------------------------------------------
    -- GET BY ID
    ----------------------------------------------------------
    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT
            CategoryId,
            CategoryCode,
            CategoryName,
            Description,
            FeeConcession,
            DisplayOrder,
            IsActive
        FROM Category
        WHERE CategoryId = @CategoryId;
    END
    ----------------------------------------------------------
    -- INSERT
    ----------------------------------------------------------
    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO Category
        (
            CategoryCode,
            CategoryName,
            [Description],
            FeeConcession,
            DisplayOrder,
            IsActive,
            CreatedDate
        )
        VALUES
        (
            @CategoryCode,
            @CategoryName,
            @Description,
            @FeeConcession,
            @DisplayOrder,
            @IsActive,
            GETDATE()
        );
    END
    ----------------------------------------------------------
    -- UPDATE
    ----------------------------------------------------------
    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE Category
        SET
            CategoryCode = @CategoryCode,
            CategoryName = @CategoryName,
            Description = @Description,
            FeeConcession = @FeeConcession,
            DisplayOrder = @DisplayOrder,
            IsActive = @IsActive
        WHERE CategoryId = @CategoryId;
    END
    ----------------------------------------------------------
    -- DELETE
    ----------------------------------------------------------
    ELSE IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM Category
        WHERE CategoryId = @CategoryId;
    END
END
