

CREATE OR ALTER PROCEDURE sp_AdmissionModeMaster
(
    @Flag VARCHAR(20),
    @AdmissionModeId INT = NULL,
    @AdmissionModeName VARCHAR(100) = NULL,
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
        INSERT INTO AdmissionModeMaster
        (AdmissionModeName, IsActive, CreatedBy, CreatedDate)
        VALUES
        (@AdmissionModeName, 1, @CreatedBy, GETDATE());
    END
    -- UPDATE
    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE AdmissionModeMaster
        SET AdmissionModeName = @AdmissionModeName,
            IsActive = @IsActive,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETDATE()
        WHERE AdmissionModeId = @AdmissionModeId;
    END
    -- SOFT DELETE / INACTIVE
    ELSE IF @Flag = 'DELETE'
    BEGIN
        UPDATE AdmissionModeMaster
        SET IsActive = 0,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETDATE()
        WHERE AdmissionModeId = @AdmissionModeId;
    END
    -- GET BY ID
    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT * FROM AdmissionModeMaster WHERE AdmissionModeId = @AdmissionModeId;
    END
    -- GET ALL (Active + Inactive both, list page needs everything)
    ELSE IF @Flag = 'GETALL'
    BEGIN
        SELECT * FROM AdmissionModeMaster ORDER BY AdmissionModeName;
    END
    -- GET ACTIVE ONLY (for dropdowns elsewhere)
    ELSE IF @Flag = 'GETACTIVE'
    BEGIN
        SELECT AdmissionModeId, AdmissionModeName
        FROM AdmissionModeMaster
        WHERE IsActive = 1
        ORDER BY AdmissionModeName;
    END
END;
GO

INSERT INTO AdmissionModeMaster (AdmissionModeName, IsActive, CreatedDate) VALUES
('Counselling', 1, GETDATE()),
('Management Seat', 1, GETDATE()),
('Direct Admission', 1, GETDATE());
GO