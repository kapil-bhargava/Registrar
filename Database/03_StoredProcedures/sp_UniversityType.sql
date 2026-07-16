/*
=============================================================
Procedure Name : sp_UniversityType
Purpose        : Perform CRUD operations on UniversityType
Created By     : Kapil
=============================================================
*/

CREATE OR ALTER PROCEDURE sp_UniversityType
(
    @Flag VARCHAR(20),

    @UniversityTypeId INT = NULL,
    @UniversityTypeCode VARCHAR(20) = NULL,
    @UniversityTypeName VARCHAR(100) = NULL,
    @Description VARCHAR(250) = NULL,
    @DisplayOrder INT = NULL,
	@IsActive INT = NULL,
	@CreatedDate DATETIME = NULL
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
        UniversityTypeId,
        UniversityTypeCode,
        UniversityTypeName,
        Description,
        DisplayOrder,
        IsActive,
        CreatedDate
    FROM UniversityType
    ORDER BY DisplayOrder;
    END

    ----------------------------------------------------------
    -- GET BY ID
    ----------------------------------------------------------
    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT
            UniversityTypeId,
            UniversityTypeCode,
            UniversityTypeName,
            Description,	
            DisplayOrder
        FROM UniversityType
        WHERE UniversityTypeId = @UniversityTypeId;
    END

    ----------------------------------------------------------
    -- INSERT
    ----------------------------------------------------------
    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO UniversityType
        (
            UniversityTypeCode,
            UniversityTypeName,
            [Description],
            DisplayOrder,
			IsActive,
			CreatedDate
        )
        VALUES
        (
            @UniversityTypeCode,
            @UniversityTypeName,
            @Description,
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
        UPDATE UniversityType
        SET
            UniversityTypeCode = @UniversityTypeCode,
            UniversityTypeName = @UniversityTypeName,
            Description = @Description,
            DisplayOrder = @DisplayOrder
        WHERE UniversityTypeId = @UniversityTypeId;
    END

    ----------------------------------------------------------
    -- DELETE
    ----------------------------------------------------------
    ELSE IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM UniversityType
        WHERE UniversityTypeId = @UniversityTypeId;
    END
END
GO



