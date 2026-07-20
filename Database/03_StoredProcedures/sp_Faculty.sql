
CREATE PROCEDURE sp_Faculty
    @Flag          NVARCHAR(20),
    @FacultyId     INT             = NULL,
    @FacultyCode   NVARCHAR(20)    = NULL,
    @FacultyName   NVARCHAR(150)   = NULL,
    @Description   NVARCHAR(255)   = NULL,
    @IsActive      BIT             = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        SELECT * FROM Faculty ORDER BY FacultyName;

    ELSE IF @Flag = 'GETACTIVE'      -- used as dropdown source by Department
        SELECT FacultyId, FacultyCode, FacultyName FROM Faculty WHERE IsActive = 1 ORDER BY FacultyName;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM Faculty WHERE FacultyId = @FacultyId;

    ELSE IF @Flag = 'INSERT'
        INSERT INTO Faculty (FacultyCode, FacultyName, Description, IsActive)
        VALUES (@FacultyCode, @FacultyName, @Description, @IsActive);

    ELSE IF @Flag = 'UPDATE'
        UPDATE Faculty
        SET FacultyCode = @FacultyCode, FacultyName = @FacultyName, Description = @Description, IsActive = @IsActive
        WHERE FacultyId = @FacultyId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM Faculty WHERE FacultyId = @FacultyId;
END
GO

INSERT INTO Faculty (FacultyCode, FacultyName, Description, IsActive) VALUES
('FOE', 'Faculty of Engineering', 'Engineering and technology programs', 1),
('FOS', 'Faculty of Science', 'Pure and applied science programs', 1),
('FOC', 'Faculty of Commerce', 'Commerce and management programs', 1),
('FOP', 'Faculty of Pharmacy', 'Pharmacy programs', 1);
GO
