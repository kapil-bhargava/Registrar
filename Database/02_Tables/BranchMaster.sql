IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'sp_BranchMaster') AND type = 'P')
BEGIN
    EXEC('CREATE PROCEDURE sp_BranchMaster AS BEGIN SET NOCOUNT ON; END')
END
GO

ALTER PROCEDURE sp_BranchMaster
    @Flag               NVARCHAR(20),
    @BranchId           INT             = NULL,
    @BranchCode         NVARCHAR(30)    = NULL,
    @BranchName         NVARCHAR(200)   = NULL,
    @DepartmentId       INT             = NULL,
    @ProgramId          INT             = NULL,
    @CourseId           INT             = NULL,
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
            b.CourseId,
            c.CourseName,
            b.CampusName,
            b.IntakeCapacity,
            b.IsActive,
            b.CreatedDate
        FROM BranchMaster b
        INNER JOIN DepartmentMaster d
            ON b.DepartmentId = d.DepartmentId
        INNER JOIN ProgramMaster p
            ON b.ProgramId = p.ProgramId
        LEFT JOIN CourseMaster c
            ON b.CourseId = c.CourseId
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
            CourseId,
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
            ProgramId,
            CourseId
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
            ProgramId,
            CourseId
        FROM BranchMaster
        WHERE ProgramId = @ProgramId
          AND IsActive = 1
        ORDER BY BranchName;
    END

    -- GET BRANCHES BY COURSE
    ELSE IF @Flag = 'GETBYCOURSE'
    BEGIN
        SELECT
            BranchId,
            BranchCode,
            BranchName,
            DepartmentId,
            ProgramId,
            CourseId
        FROM BranchMaster
        WHERE CourseId = @CourseId
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
            CourseId,
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
            @CourseId,
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
            CourseId = @CourseId,
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


-- Step 1: BranchMaster mein CourseId column add karo
ALTER TABLE dbo.BranchMaster
ADD CourseId INT NULL;
GO

-- Step 2: CourseMaster se FK relationship banao
ALTER TABLE dbo.BranchMaster
ADD CONSTRAINT FK_Branch_Course
    FOREIGN KEY (CourseId)
    REFERENCES dbo.CourseMaster(CourseId);
GO