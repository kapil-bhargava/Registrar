/* ============================================================
   HOD MANAGEMENT (extended to match HODManagement.cshtml fields)
   DepartmentId -> FK to Department
   History = every past row for a Department (Status='Relieved'),
   ordered by EffectiveDate desc — no separate history table needed.
   ============================================================ */



CREATE PROCEDURE sp_HODManagement
    @Flag              NVARCHAR(20),
    @HODId             INT             = NULL,
    @DepartmentId      INT             = NULL,
    @HODName           NVARCHAR(150)   = NULL,
    @EmployeeId        NVARCHAR(30)    = NULL,
    @Designation       NVARCHAR(100)   = NULL,
    @Qualification     NVARCHAR(150)   = NULL,
    @Email             NVARCHAR(150)   = NULL,
    @Phone             NVARCHAR(20)    = NULL,
    @EffectiveDate     DATE            = NULL,
    @TenureEndDate     DATE            = NULL,
    @ReasonForChange   NVARCHAR(255)   = NULL,
    @Status            NVARCHAR(20)    = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        -- Every HOD row (current + relieved), joined with Department/Faculty — used to build the full dashboard
        SELECT h.HODId, h.DepartmentId, d.DepartmentName, f.FacultyName,
               h.HODName, h.EmployeeId, h.Designation, h.Qualification, h.Email, h.Phone,
               h.EffectiveDate, h.TenureEndDate, h.ReasonForChange, h.Status, h.CreatedDate
        FROM HODManagement h
        INNER JOIN Department d ON h.DepartmentId = d.DepartmentId
        INNER JOIN Faculty f ON d.FacultyId = f.FacultyId
        ORDER BY d.DepartmentName, h.EffectiveDate DESC;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM HODManagement WHERE HODId = @HODId;

    ELSE IF @Flag = 'INSERT'
    BEGIN
        -- Relieve whoever currently holds this department (if anyone), recording the reason
        UPDATE HODManagement
        SET Status = 'Relieved', ReasonForChange = @ReasonForChange
        WHERE DepartmentId = @DepartmentId AND Status = 'Active';

        INSERT INTO HODManagement
            (DepartmentId, HODName, EmployeeId, Designation, Qualification, Email, Phone, EffectiveDate, TenureEndDate, Status)
        VALUES
            (@DepartmentId, @HODName, @EmployeeId, @Designation, @Qualification, @Email, @Phone, @EffectiveDate, @TenureEndDate, ISNULL(@Status, 'Active'));
    END

    ELSE IF @Flag = 'UPDATE'
        UPDATE HODManagement
        SET HODName = @HODName, EmployeeId = @EmployeeId, Designation = @Designation,
            Qualification = @Qualification, Email = @Email, Phone = @Phone,
            EffectiveDate = @EffectiveDate, TenureEndDate = @TenureEndDate, Status = @Status
        WHERE HODId = @HODId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM HODManagement WHERE HODId = @HODId;
END
GO

INSERT INTO HODManagement (DepartmentId, HODName, EmployeeId, Designation, Qualification, Email, Phone, EffectiveDate, TenureEndDate, Status) VALUES
(1, 'Dr. Ramesh Kumar', 'EMP-1042', 'Professor', 'Ph.D. (Computer Science)', 'ramesh.kumar@university.edu', '+1 234 567 890', '2024-07-01', '2027-06-30', 'Active'),
(2, 'Dr. Sunita Verma', 'EMP-0871', 'Professor', 'Ph.D. (Physics)', 'sunita.verma@university.edu', '+1 234 567 891', '2023-01-15', '2026-01-14', 'Active');
GO