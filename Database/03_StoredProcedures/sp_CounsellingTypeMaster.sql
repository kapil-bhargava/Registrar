
CREATE OR ALTER PROCEDURE sp_CounsellingTypeMaster
(
    @Flag VARCHAR(20),
    @CounsellingTypeId INT = NULL,
    @CounsellingTypeName VARCHAR(100) = NULL,
    @IsActive BIT = NULL,
    @CreatedBy INT = NULL,
    @ModifiedBy INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    -- INSERT
    IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO CounsellingTypeMaster
        (CounsellingTypeName, IsActive, CreatedBy, CreatedDate)
        VALUES
        (@CounsellingTypeName, 1, @CreatedBy, GETDATE());
    END
    -- UPDATE
    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE CounsellingTypeMaster
        SET CounsellingTypeName = @CounsellingTypeName,
            IsActive = @IsActive,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETDATE()
        WHERE CounsellingTypeId = @CounsellingTypeId;
    END
    -- SOFT DELETE / INACTIVE
    ELSE IF @Flag = 'DELETE'
    BEGIN
        UPDATE CounsellingTypeMaster
        SET IsActive = 0,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETDATE()
        WHERE CounsellingTypeId = @CounsellingTypeId;
    END
    -- GET BY ID
    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT * FROM CounsellingTypeMaster WHERE CounsellingTypeId = @CounsellingTypeId;
    END
    -- GET ALL (Active + Inactive both)
    ELSE IF @Flag = 'GETALL'
    BEGIN
        SELECT * FROM CounsellingTypeMaster ORDER BY CounsellingTypeName;
    END
    -- GET ACTIVE ONLY (for dropdowns elsewhere)
    ELSE IF @Flag = 'GETACTIVE'
    BEGIN
        SELECT CounsellingTypeId, CounsellingTypeName
        FROM CounsellingTypeMaster
        WHERE IsActive = 1
        ORDER BY CounsellingTypeName;
    END
END;
GO

INSERT INTO CounsellingTypeMaster (CounsellingTypeName, IsActive, CreatedDate) VALUES
('State Counselling', 1, GETDATE()),
('University Counselling', 1, GETDATE()),
('Central Counselling', 1, GETDATE()),
('Institute Level Counselling', 1, GETDATE());
GO