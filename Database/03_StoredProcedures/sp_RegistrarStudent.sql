CREATE OR ALTER PROCEDURE dbo.sp_RegistrarStudent
    @Flag                     VARCHAR(30),
    @RegistrarId              INT             = NULL,
    @StudentName               NVARCHAR(150)   = NULL,
    @Email                     NVARCHAR(150)   = NULL,
    @Mobile                    NVARCHAR(20)    = NULL,
    @CourseId                  INT             = NULL,
    @BranchId                  INT             = NULL,
    @SemesterId                INT             = NULL,
    @CategoryId                INT             = NULL,     -- NEW
    @RequiredDocumentIdsCsv    NVARCHAR(MAX)   = NULL,
    @SubmittedDocumentIdsCsv   NVARCHAR(MAX)   = NULL,
    @IsActive                  BIT             = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT
            r.RegistrarId, r.StudentName, r.Email, r.Mobile,
            r.CourseId, c.CourseName,
            r.BranchId, b.BranchName,
            r.SemesterId, sm.SemesterName,
            r.CategoryId, cat.CategoryName,                 -- NEW
            r.RequiredDocumentIdsCsv, r.SubmittedDocumentIdsCsv,
            reqDoc.RequiredDocumentNames, subDoc.SubmittedDocumentNames,
            reqDoc.RequiredDocumentCount, subDoc.SubmittedDocumentCount,
            r.IsActive, r.CreatedDate
        FROM dbo.RegistrarStudent r
        LEFT JOIN dbo.CourseMaster   c   ON c.CourseId    = r.CourseId
        LEFT JOIN dbo.BranchMaster   b   ON b.BranchId    = r.BranchId
        LEFT JOIN dbo.SemesterMaster sm  ON sm.SemesterId = r.SemesterId
        LEFT JOIN dbo.Category       cat ON cat.CategoryId = r.CategoryId   -- NEW
        OUTER APPLY (
            SELECT STRING_AGG(de.DocumentName, ', ') AS RequiredDocumentNames, COUNT(*) AS RequiredDocumentCount
            FROM STRING_SPLIT(r.RequiredDocumentIdsCsv, ',') s
            INNER JOIN dbo.DocumentEnclosureMaster de ON de.DocumentEnclosureId = TRY_CAST(s.value AS INT)
            WHERE r.RequiredDocumentIdsCsv IS NOT NULL AND LTRIM(RTRIM(r.RequiredDocumentIdsCsv)) <> ''
        ) reqDoc
        OUTER APPLY (
            SELECT STRING_AGG(de.DocumentName, ', ') AS SubmittedDocumentNames, COUNT(*) AS SubmittedDocumentCount
            FROM STRING_SPLIT(r.SubmittedDocumentIdsCsv, ',') s
            INNER JOIN dbo.DocumentEnclosureMaster de ON de.DocumentEnclosureId = TRY_CAST(s.value AS INT)
            WHERE r.SubmittedDocumentIdsCsv IS NOT NULL AND LTRIM(RTRIM(r.SubmittedDocumentIdsCsv)) <> ''
        ) subDoc
        ORDER BY r.RegistrarId DESC;
    END

    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT
            r.RegistrarId, r.StudentName, r.Email, r.Mobile,
            r.CourseId, c.CourseName,
            r.BranchId, b.BranchName,
            r.SemesterId, sm.SemesterName,
            r.CategoryId, cat.CategoryName,                 -- NEW
            r.RequiredDocumentIdsCsv, r.SubmittedDocumentIdsCsv,
            reqDoc.RequiredDocumentNames, subDoc.SubmittedDocumentNames,
            reqDoc.RequiredDocumentCount, subDoc.SubmittedDocumentCount,
            r.IsActive, r.CreatedDate
        FROM dbo.RegistrarStudent r
        LEFT JOIN dbo.CourseMaster   c   ON c.CourseId    = r.CourseId
        LEFT JOIN dbo.BranchMaster   b   ON b.BranchId    = r.BranchId
        LEFT JOIN dbo.SemesterMaster sm  ON sm.SemesterId = r.SemesterId
        LEFT JOIN dbo.Category       cat ON cat.CategoryId = r.CategoryId   -- NEW
        OUTER APPLY (
            SELECT STRING_AGG(de.DocumentName, ', ') AS RequiredDocumentNames, COUNT(*) AS RequiredDocumentCount
            FROM STRING_SPLIT(r.RequiredDocumentIdsCsv, ',') s
            INNER JOIN dbo.DocumentEnclosureMaster de ON de.DocumentEnclosureId = TRY_CAST(s.value AS INT)
            WHERE r.RequiredDocumentIdsCsv IS NOT NULL AND LTRIM(RTRIM(r.RequiredDocumentIdsCsv)) <> ''
        ) reqDoc
        OUTER APPLY (
            SELECT STRING_AGG(de.DocumentName, ', ') AS SubmittedDocumentNames, COUNT(*) AS SubmittedDocumentCount
            FROM STRING_SPLIT(r.SubmittedDocumentIdsCsv, ',') s
            INNER JOIN dbo.DocumentEnclosureMaster de ON de.DocumentEnclosureId = TRY_CAST(s.value AS INT)
            WHERE r.SubmittedDocumentIdsCsv IS NOT NULL AND LTRIM(RTRIM(r.SubmittedDocumentIdsCsv)) <> ''
        ) subDoc
        WHERE r.RegistrarId = @RegistrarId;
    END

    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO dbo.RegistrarStudent
            (StudentName, Email, Mobile, CourseId, BranchId, SemesterId, CategoryId,
             RequiredDocumentIdsCsv, SubmittedDocumentIdsCsv, IsActive, CreatedDate)
        VALUES
            (@StudentName, @Email, @Mobile, @CourseId, @BranchId, @SemesterId, @CategoryId,
             @RequiredDocumentIdsCsv, @SubmittedDocumentIdsCsv, ISNULL(@IsActive, 1), GETDATE());
    END

    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE dbo.RegistrarStudent
        SET StudentName             = @StudentName,
            Email                   = @Email,
            Mobile                  = @Mobile,
            CourseId                = @CourseId,
            BranchId                = @BranchId,
            SemesterId              = @SemesterId,
            CategoryId              = @CategoryId,          -- NEW
            RequiredDocumentIdsCsv  = @RequiredDocumentIdsCsv,
            SubmittedDocumentIdsCsv = @SubmittedDocumentIdsCsv,
            IsActive                = ISNULL(@IsActive, IsActive)
        WHERE RegistrarId = @RegistrarId;
    END

    ELSE IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM dbo.RegistrarStudent WHERE RegistrarId = @RegistrarId;
    END
END
GO


ALTER TABLE dbo.RegistrarStudent
ADD CategoryId INT NULL;
GO

SELECT COLUMN_NAME 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'RegistrarStudent';