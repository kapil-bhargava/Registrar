USE [UniversityERP]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_FacultyAssignment]
(
    @Flag VARCHAR(20),
    @FacultyId INT = NULL,
    @EmployeeId INT = NULL,
    @DepartmentId INT = NULL,
    @BranchId INT = NULL,
    @SemesterId INT = NULL,
    @DesignationId INT = NULL,
    @Status VARCHAR(20) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    -- GET ALL
    IF @Flag = 'GETALL'
    BEGIN
        SELECT
            FA.FacultyId,
            FA.EmployeeId,
            E.FullName AS FacultyName,
            FA.DepartmentId,
            D.DepartmentName,
            FA.BranchId,
            B.BranchName,
            FA.SemesterId,
            S.SemesterName,
            FA.DesignationId,
            DS.DesignationName,
            FA.Status,
            FA.CreatedDate
        FROM FacultyAssignment FA
        INNER JOIN EmployeeMasters E          -- ✅ FIXED: plural, sahi table
            ON FA.EmployeeId = E.EmployeeId
        INNER JOIN DepartmentMaster D
            ON FA.DepartmentId = D.DepartmentId
        INNER JOIN BranchMaster B
            ON FA.BranchId = B.BranchId
        INNER JOIN SemesterMaster S
            ON FA.SemesterId = S.SemesterId
        INNER JOIN Designation DS
            ON FA.DesignationId = DS.DesignationId
        ORDER BY FA.FacultyId DESC;
        RETURN;
    END

    -- INSERT
    IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO FacultyAssignment
        (
            EmployeeId,
            DepartmentId,
            BranchId,
            SemesterId,
            DesignationId,
            Status
        )
        VALUES
        (
            @EmployeeId,
            @DepartmentId,
            @BranchId,
            @SemesterId,
            @DesignationId,
            ISNULL(@Status, 'Active')
        );
        RETURN;
    END

    -- UPDATE
    IF @Flag = 'UPDATE'
    BEGIN
        UPDATE FacultyAssignment
        SET
            EmployeeId = @EmployeeId,
            DepartmentId = @DepartmentId,
            BranchId = @BranchId,
            SemesterId = @SemesterId,
            DesignationId = @DesignationId,
            Status = ISNULL(@Status, 'Active')
        WHERE FacultyId = @FacultyId;
        RETURN;
    END

    -- DELETE
    IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM FacultyAssignment
        WHERE FacultyId = @FacultyId;
        RETURN;
    END
END