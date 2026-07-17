

CREATE OR ALTER PROCEDURE sp_Campus
(
    @Flag VARCHAR(20),
    @CampusId INT = NULL,
    @CampusName VARCHAR(150) = NULL,
    @CampusCode VARCHAR(20) = NULL,
    @CampusTypeId INT = NULL,
    @Capacity INT = NULL,
    @Address VARCHAR(500) = NULL,
    @ContactNumber VARCHAR(20) = NULL,
    @Email VARCHAR(100) = NULL,
    @Dean VARCHAR(150) = NULL,
    @IsActive INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT
            c.CampusId, c.CampusName, c.CampusCode, c.CampusTypeId,
            cc.CampusCategoryName AS CampusTypeName,
            c.Capacity, c.Address, c.ContactNumber, c.Email, c.Dean,
            c.IsActive, c.CreatedDate
        FROM Campus c
        LEFT JOIN CampusCategory cc ON cc.CampusCategoryId = c.CampusTypeId
        ORDER BY c.CampusId;
    END
    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT
            c.CampusId, c.CampusName, c.CampusCode, c.CampusTypeId,
            cc.CampusCategoryName AS CampusTypeName,
            c.Capacity, c.Address, c.ContactNumber, c.Email, c.Dean, c.IsActive
        FROM Campus c
        LEFT JOIN CampusCategory cc ON cc.CampusCategoryId = c.CampusTypeId
        WHERE c.CampusId = @CampusId;
    END
    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO Campus
        (CampusName, CampusCode, CampusTypeId, Capacity, Address, ContactNumber, Email, Dean, IsActive, CreatedDate)
        VALUES
        (@CampusName, @CampusCode, @CampusTypeId, @Capacity, @Address, @ContactNumber, @Email, @Dean, @IsActive, GETDATE());
    END
    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE Campus
        SET CampusName = @CampusName,
            CampusCode = @CampusCode,
            CampusTypeId = @CampusTypeId,
            Capacity = @Capacity,
            Address = @Address,
            ContactNumber = @ContactNumber,
            Email = @Email,
            Dean = @Dean,
            IsActive = @IsActive
        WHERE CampusId = @CampusId;
    END
    ELSE IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM Campus WHERE CampusId = @CampusId;
    END
END
GO

SELECT * FROM Campus