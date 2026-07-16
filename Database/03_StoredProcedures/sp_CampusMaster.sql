
CREATE OR ALTER PROCEDURE sp_CampusCategory
(
    @Flag VARCHAR(20),
    @CampusCategoryId INT = NULL,
    @CampusCategoryCode VARCHAR(20) = NULL,
    @CampusCategoryName VARCHAR(100) = NULL,
    @Description VARCHAR(250) = NULL,
    @DisplayOrder INT = NULL,
    @IsActive INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT CampusCategoryId, CampusCategoryCode, CampusCategoryName,
               Description, DisplayOrder, IsActive, CreatedDate
        FROM CampusCategory
        ORDER BY DisplayOrder;
    END
    ELSE IF @Flag = 'GETACTIVE'
    BEGIN
        SELECT CampusCategoryId, CampusCategoryCode, CampusCategoryName
        FROM CampusCategory
        WHERE IsActive = 1
        ORDER BY DisplayOrder;
    END
    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT CampusCategoryId, CampusCategoryCode, CampusCategoryName,
               Description, DisplayOrder, IsActive
        FROM CampusCategory
        WHERE CampusCategoryId = @CampusCategoryId;
    END
    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO CampusCategory
        (CampusCategoryCode, CampusCategoryName, Description, DisplayOrder, IsActive, CreatedDate)
        VALUES
        (@CampusCategoryCode, @CampusCategoryName, @Description, @DisplayOrder, @IsActive, GETDATE());
    END
    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE CampusCategory
        SET CampusCategoryCode = @CampusCategoryCode,
            CampusCategoryName = @CampusCategoryName,
            Description = @Description,
            DisplayOrder = @DisplayOrder,
            IsActive = @IsActive
        WHERE CampusCategoryId = @CampusCategoryId;
    END
    ELSE IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM CampusCategory WHERE CampusCategoryId = @CampusCategoryId;
    END
END
