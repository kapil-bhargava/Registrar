CREATE PROCEDURE sp_BranchMaster
    @Flag               NVARCHAR(20),
    @BranchId           INT             = NULL,
    @BranchCode         NVARCHAR(30)    = NULL,
    @BranchName         NVARCHAR(200)   = NULL,
    @DepartmentId       INT             = NULL,
    @ProgramId          INT             = NULL,
    @CampusName         NVARCHAR(100)   = NULL,
    @IntakeCapacity     INT             = NULL,
    @IsActive           BIT             = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- GET ALL BRANCHES
    IF @Flag = 'GETALL'
    BEGIN
        SELECT
            b.BranchId,
            b.BranchCode,
            b.BranchName,
            b.DepartmentId,
            d.DepartmentName,
            b.ProgramId,
            p.ProgramName,
            b.CampusName,
            b.IntakeCapacity,
            b.IsActive,
            b.CreatedDate
        FROM BranchMaster b
        INNER JOIN DepartmentMaster d
            ON b.DepartmentId = d.DepartmentId
        INNER JOIN ProgramMaster p
            ON b.ProgramId = p.ProgramId
        ORDER BY b.BranchName;
    END

    -- GET ACTIVE BRANCHES
    ELSE IF @Flag = 'GETACTIVE'
    BEGIN
        SELECT
            BranchId,
            BranchCode,
            BranchName,
            DepartmentId,
            ProgramId,
            CampusName,
            IntakeCapacity
        FROM BranchMaster
        WHERE IsActive = 1
        ORDER BY BranchName;
    END

    -- GET BRANCH BY ID
    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT *
        FROM BranchMaster
        WHERE BranchId = @BranchId;
    END

    -- GET BRANCHES BY DEPARTMENT
    ELSE IF @Flag = 'GETBYDEPARTMENT'
    BEGIN
        SELECT
            BranchId,
            BranchCode,
            BranchName,
            DepartmentId,
            ProgramId
        FROM BranchMaster
        WHERE DepartmentId = @DepartmentId
          AND IsActive = 1
        ORDER BY BranchName;
    END

    -- GET BRANCHES BY PROGRAM
    ELSE IF @Flag = 'GETBYPROGRAM'
    BEGIN
        SELECT
            BranchId,
            BranchCode,
            BranchName,
            DepartmentId,
            ProgramId
        FROM BranchMaster
        WHERE ProgramId = @ProgramId
          AND IsActive = 1
        ORDER BY BranchName;
    END

    -- INSERT
    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO BranchMaster
        (
            BranchCode,
            BranchName,
            DepartmentId,
            ProgramId,
            CampusName,
            IntakeCapacity,
            IsActive
        )
        VALUES
        (
            @BranchCode,
            @BranchName,
            @DepartmentId,
            @ProgramId,
            @CampusName,
            @IntakeCapacity,
            ISNULL(@IsActive, 1)
        );
    END

    -- UPDATE
    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE BranchMaster
        SET
            BranchCode = @BranchCode,
            BranchName = @BranchName,
            DepartmentId = @DepartmentId,
            ProgramId = @ProgramId,
            CampusName = @CampusName,
            IntakeCapacity = @IntakeCapacity,
            IsActive = @IsActive
        WHERE BranchId = @BranchId;
    END

    -- DELETE
    ELSE IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM BranchMaster
        WHERE BranchId = @BranchId;
    END
END
GO

INSERT INTO BranchMaster
(
    BranchCode,
    BranchName,
    DepartmentId,
    ProgramId,
    CampusName,
    IntakeCapacity,
    IsActive
)
VALUES
('CSE-AI', 'Computer Science & Engineering - AI', 1, 1, 'Main Campus', 120, 1),
('CSE-DS', 'Computer Science & Engineering - Data Science', 1, 1, 'Main Campus', 60, 1),
('ECE', 'Electronics & Communication Engineering', 2, 1, 'Main Campus', 60, 1);
GO