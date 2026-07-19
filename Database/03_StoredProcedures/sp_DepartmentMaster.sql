CREATE PROCEDURE sp_DepartmentMaster
    @Flag               NVARCHAR(20),
    @DepartmentId       INT             = NULL,
    @DepartmentCode     NVARCHAR(30)    = NULL,
    @DepartmentName     NVARCHAR(200)   = NULL,
    @FacultyId          INT             = NULL,
    @CampusName         NVARCHAR(100)   = NULL,
    @IsActive           BIT             = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        SELECT d.DepartmentId, d.DepartmentCode, d.DepartmentName,
               d.FacultyId, f.FacultyName, d.CampusName, d.IsActive, d.CreatedDate
        FROM DepartmentMaster d
        INNER JOIN FacultyMaster f ON d.FacultyId = f.FacultyId
        ORDER BY f.FacultyName, d.DepartmentName;

    ELSE IF @Flag = 'GETACTIVE'
        SELECT DepartmentId, DepartmentCode, DepartmentName, FacultyId
        FROM DepartmentMaster
        WHERE IsActive = 1
        ORDER BY DepartmentName;

    ELSE IF @Flag = 'GETBYFACULTY'   -- cascading: departments under one faculty
        SELECT DepartmentId, DepartmentCode, DepartmentName
        FROM DepartmentMaster
        WHERE FacultyId = @FacultyId AND IsActive = 1
        ORDER BY DepartmentName;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM DepartmentMaster WHERE DepartmentId = @DepartmentId;

    ELSE IF @Flag = 'INSERT'
        INSERT INTO DepartmentMaster (DepartmentCode, DepartmentName, FacultyId, CampusName, IsActive)
        VALUES (@DepartmentCode, @DepartmentName, @FacultyId, @CampusName, ISNULL(@IsActive, 1));

    ELSE IF @Flag = 'UPDATE'
        UPDATE DepartmentMaster
        SET DepartmentCode = @DepartmentCode, DepartmentName = @DepartmentName,
            FacultyId = @FacultyId, CampusName = @CampusName, IsActive = @IsActive
        WHERE DepartmentId = @DepartmentId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM DepartmentMaster WHERE DepartmentId = @DepartmentId;
END
GO

INSERT INTO DepartmentMaster (DepartmentCode, DepartmentName, FacultyId, CampusName, IsActive) VALUES
('CSE', 'Computer Science & Engineering', 1, 'Main Campus', 1),
('ECE', 'Electronics & Communication', 1, 'Main Campus', 1),
('PHY', 'Physics', 2, 'Main Campus', 1);
GO
