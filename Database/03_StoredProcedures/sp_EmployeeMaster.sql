CREATE OR ALTER PROCEDURE sp_EmployeeMaster
(
    @Flag VARCHAR(20),
    @EmployeeId INT = NULL,
    @EmployeeCode VARCHAR(50) = NULL,
    @FullName VARCHAR(150) = NULL,
    @FatherName VARCHAR(150) = NULL,
    @Email VARCHAR(150) = NULL,
    @Phone VARCHAR(50) = NULL,
    @Gender VARCHAR(20) = NULL,
    @DateOfBirth DATE = NULL,
    @DateOfJoining DATE = NULL,
    @Address VARCHAR(300) = NULL,
    @Qualification VARCHAR(200) = NULL,
    @Experience VARCHAR(100) = NULL,
    @IsActive INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT 
            EmployeeId,
            EmployeeCode,
            FullName,
            FatherName,
            Email,
            Phone,
            Gender,
            DateOfBirth,
            DateOfJoining,
            Address,
            Qualification,
            Experience,
            IsActive,
            CreatedDate
        FROM EmployeeMasters
        ORDER BY FullName;
    END

    ELSE IF @Flag = 'GETACTIVE'
    BEGIN
        SELECT 
            EmployeeId,
            EmployeeCode,
            FullName
        FROM EmployeeMasters
        WHERE IsActive = 1
        ORDER BY FullName;
    END

    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT 
            EmployeeId,
            EmployeeCode,
            FullName,
            FatherName,
            Email,
            Phone,
            Gender,
            DateOfBirth,
            DateOfJoining,
            Address,
            Qualification,
            Experience,
            IsActive
        FROM EmployeeMasters
        WHERE EmployeeId = @EmployeeId;
    END

    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO EmployeeMasters
        (
            EmployeeCode,
            FullName,
            FatherName,
            Email,
            Phone,
            Gender,
            DateOfBirth,
            DateOfJoining,
            Address,
            Qualification,
            Experience,
            IsActive,
            CreatedDate
        )
        VALUES
        (
            @EmployeeCode,
            @FullName,
            @FatherName,
            @Email,
            @Phone,
            @Gender,
            @DateOfBirth,
            @DateOfJoining,
            @Address,
            @Qualification,
            @Experience,
            @IsActive,
            GETDATE()
        );
    END

    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE EmployeeMasters
        SET 
            EmployeeCode = @EmployeeCode,
            FullName = @FullName,
            FatherName = @FatherName,
            Email = @Email,
            Phone = @Phone,
            Gender = @Gender,
            DateOfBirth = @DateOfBirth,
            DateOfJoining = @DateOfJoining,
            Address = @Address,
            Qualification = @Qualification,
            Experience = @Experience,
            IsActive = @IsActive
        WHERE EmployeeId = @EmployeeId;
    END

    ELSE IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM EmployeeMasters
        WHERE EmployeeId = @EmployeeId;
    END
END
GO