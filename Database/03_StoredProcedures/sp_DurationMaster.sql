CREATE OR ALTER PROCEDURE sp_DurationMaster
(
    @Flag VARCHAR(20),
    @DurationId INT = NULL,
    @DurationName VARCHAR(100) = NULL,
    @DurationMonth INT = NULL,
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
        INSERT INTO DurationMaster
        (
            DurationName,
            DurationMonth,
            IsActive,
            CreatedBy,
            CreatedDate
        )
        VALUES
        (
            @DurationName,
            @DurationMonth,
            1,
            @CreatedBy,
            GETDATE()
        );
    END
    -- UPDATE
    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE DurationMaster
        SET
            DurationName = @DurationName,
            DurationMonth = @DurationMonth,
            IsActive = @IsActive,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETDATE()
        WHERE DurationId = @DurationId;
    END
    -- DELETE / INACTIVE (soft delete)
    ELSE IF @Flag = 'DELETE'
    BEGIN
        UPDATE DurationMaster
        SET
            IsActive = 0,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETDATE()
        WHERE DurationId = @DurationId;
    END
    -- GET BY ID
    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT *
        FROM DurationMaster
        WHERE DurationId = @DurationId;
    END
    -- GET ALL (Active + Inactive both — list page needs to show everything)
    ELSE IF @Flag = 'GETALL'
    BEGIN
        SELECT *
        FROM DurationMaster
        ORDER BY DurationMonth;
    END
    -- GET ACTIVE ONLY (for dropdowns elsewhere in the app)
    ELSE IF @Flag = 'GETACTIVE'
    BEGIN
        SELECT DurationId, DurationName, DurationMonth
        FROM DurationMaster
        WHERE IsActive = 1
        ORDER BY DurationMonth;
    END
END;
GO