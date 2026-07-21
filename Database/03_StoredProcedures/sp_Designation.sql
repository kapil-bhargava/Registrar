

CREATE OR ALTER PROCEDURE sp_Designation
(
    @Flag VARCHAR(20),
    @DesignationId INT = NULL,
    @DesignationName VARCHAR(100) = NULL,
    @DesignationCode VARCHAR(20) = NULL,
    @Level VARCHAR(20) = NULL,
    @IsActive INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT DesignationId, DesignationName, DesignationCode, Level, IsActive, CreatedDate
        FROM Designation
        ORDER BY DesignationName;
    END
    ELSE IF @Flag = 'GETACTIVE'
    BEGIN
        SELECT DesignationId, DesignationName, DesignationCode
        FROM Designation
        WHERE IsActive = 1
        ORDER BY DesignationName;
    END
    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT DesignationId, DesignationName, DesignationCode, Level, IsActive
        FROM Designation
        WHERE DesignationId = @DesignationId;
    END
    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO Designation (DesignationName, DesignationCode, Level, IsActive, CreatedDate)
        VALUES (@DesignationName, @DesignationCode, @Level, @IsActive, GETDATE());
    END
    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE Designation
        SET DesignationName = @DesignationName,
            DesignationCode = @DesignationCode,
            Level = @Level,
            IsActive = @IsActive
        WHERE DesignationId = @DesignationId;
    END
    ELSE IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM Designation WHERE DesignationId = @DesignationId;
    END
END
GO