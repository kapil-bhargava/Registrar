CREATE OR ALTER PROCEDURE sp_FacultyMaster
    @Flag           NVARCHAR(20),
    @FacultyId      INT             = NULL,
    @FacultyCode    NVARCHAR(30)    = NULL,
    @FacultyName    NVARCHAR(200)   = NULL,
    @Description    NVARCHAR(500)   = NULL,
    @IsActive       BIT             = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        SELECT FacultyId, FacultyCode, FacultyName, Description, IsActive, CreatedDate
        FROM FacultyMaster
        ORDER BY FacultyName;

    ELSE IF @Flag = 'GETACTIVE'
        SELECT FacultyId, FacultyCode, FacultyName
        FROM FacultyMaster
        WHERE IsActive = 1
        ORDER BY FacultyName;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM FacultyMaster WHERE FacultyId = @FacultyId;

    ELSE IF @Flag = 'INSERT'
        INSERT INTO FacultyMaster (FacultyCode, FacultyName, Description, IsActive)
        VALUES (@FacultyCode, @FacultyName, @Description, ISNULL(@IsActive, 1));

    ELSE IF @Flag = 'UPDATE'
        UPDATE FacultyMaster
        SET FacultyCode = @FacultyCode, FacultyName = @FacultyName,
            Description = @Description, IsActive = @IsActive
        WHERE FacultyId = @FacultyId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM FacultyMaster WHERE FacultyId = @FacultyId;
END
GO

INSERT INTO FacultyMaster (FacultyCode, FacultyName, Description, IsActive) VALUES
('FOE', 'Faculty of Engineering', 'Engineering & Technology programs', 1),
('FOS', 'Faculty of Science', 'Pure & Applied Science programs', 1),
('FOM', 'Faculty of Management', 'Business & Management programs', 1);
GO