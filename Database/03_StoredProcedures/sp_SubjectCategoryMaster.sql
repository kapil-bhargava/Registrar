CREATE OR ALTER PROCEDURE sp_SubjectCategoryMaster
(
    @Flag VARCHAR(20),
    @CategoryId INT = NULL,
    @CategoryName VARCHAR(100) = NULL,
    @CategoryType VARCHAR(100) = NULL,
    @CreditApplicable BIT = NULL,
    @MarksApplicable BIT = NULL,
    @PassingMarksRequired BIT = NULL,
    @IsActive BIT = NULL,
    @DisplayOrder INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    -- GET ALL
    IF @Flag = 'GETALL'
    BEGIN
        SELECT
            CategoryId,
            CategoryName,
            CategoryType,
            CreditApplicable,
            MarksApplicable,
            PassingMarksRequired,
            IsActive,
            DisplayOrder,
            CreatedDate
        FROM SubjectCategoryMaster
        ORDER BY DisplayOrder, CategoryName;
    END

    -- GET ACTIVE
    ELSE IF @Flag = 'GETACTIVE'
    BEGIN
        SELECT
            CategoryId,
            CategoryName,
            CategoryType
        FROM SubjectCategoryMaster
        WHERE IsActive = 1
        ORDER BY DisplayOrder, CategoryName;
    END

    -- GET BY ID
    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT
            CategoryId,
            CategoryName,
            CategoryType,
            CreditApplicable,
            MarksApplicable,
            PassingMarksRequired,
            IsActive,
            DisplayOrder
        FROM SubjectCategoryMaster
        WHERE CategoryId = @CategoryId;
    END

    -- INSERT
    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO SubjectCategoryMaster
        (
            CategoryName,
            CategoryType,
            CreditApplicable,
            MarksApplicable,
            PassingMarksRequired,
            IsActive,
            DisplayOrder,
            CreatedDate
        )
        VALUES
        (
            @CategoryName,
            @CategoryType,
            @CreditApplicable,
            @MarksApplicable,
            @PassingMarksRequired,
            @IsActive,
            @DisplayOrder,
            GETDATE()
        );
    END

    -- UPDATE
    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE SubjectCategoryMaster
        SET
            CategoryName = @CategoryName,
            CategoryType = @CategoryType,
            CreditApplicable = @CreditApplicable,
            MarksApplicable = @MarksApplicable,
            PassingMarksRequired = @PassingMarksRequired,
            IsActive = @IsActive,
            DisplayOrder = @DisplayOrder
        WHERE CategoryId = @CategoryId;
    END

    -- DELETE
    ELSE IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM SubjectCategoryMaster
        WHERE CategoryId = @CategoryId;
    END
END
GO