/* ============================================================
   FACULTY  (e.g. Faculty of Engineering, Faculty of Science)
   Top of the Academic Setup hierarchy — Departments belong to a Faculty.
   ============================================================ */




/* ============================================================
   DEPARTMENT
   FacultyId -> FK to Faculty (mapping #1)
   ============================================================ */

CREATE PROCEDURE sp_Department
    @Flag             NVARCHAR(20),
    @DepartmentId     INT             = NULL,
    @DepartmentCode   NVARCHAR(20)    = NULL,
    @DepartmentName   NVARCHAR(150)   = NULL,
    @FacultyId        INT             = NULL,
    @CampusName       NVARCHAR(100)   = NULL,
    @IsActive         BIT             = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        -- Joined with Faculty so the list page can show the Faculty name, not just its Id
        SELECT d.DepartmentId, d.DepartmentCode, d.DepartmentName, d.FacultyId,
               f.FacultyName, d.CampusName, d.IsActive, d.CreatedDate
        FROM Department d
        INNER JOIN Faculty f ON d.FacultyId = f.FacultyId
        ORDER BY d.DepartmentName;

    ELSE IF @Flag = 'GETACTIVE'      -- used as dropdown source by Course
        SELECT DepartmentId, DepartmentCode, DepartmentName, FacultyId FROM Department WHERE IsActive = 1 ORDER BY DepartmentName;

    ELSE IF @Flag = 'GETBYFACULTY'   -- cascading dropdown: departments under one Faculty
        SELECT DepartmentId, DepartmentCode, DepartmentName FROM Department WHERE FacultyId = @FacultyId AND IsActive = 1 ORDER BY DepartmentName;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM Department WHERE DepartmentId = @DepartmentId;

    ELSE IF @Flag = 'INSERT'
        INSERT INTO Department (DepartmentCode, DepartmentName, FacultyId, CampusName, IsActive)
        VALUES (@DepartmentCode, @DepartmentName, @FacultyId, @CampusName, @IsActive);

    ELSE IF @Flag = 'UPDATE'
        UPDATE Department
        SET DepartmentCode = @DepartmentCode, DepartmentName = @DepartmentName,
            FacultyId = @FacultyId, CampusName = @CampusName, IsActive = @IsActive
        WHERE DepartmentId = @DepartmentId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM Department WHERE DepartmentId = @DepartmentId;
END
GO

INSERT INTO Department (DepartmentCode, DepartmentName, FacultyId, CampusName, IsActive) VALUES
('CS', 'Computer Science', 1, 'Main Campus', 1),
('PHY', 'Physics', 2, 'Main Campus', 1),
('COM', 'Accounting', 3, 'Gomti Nagar Campus', 1);
GO