IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'sp_RequiredDocumentMaster')
    DROP PROCEDURE sp_RequiredDocumentMaster
GO
CREATE PROCEDURE sp_RequiredDocumentMaster
    @Flag NVARCHAR(20),
    @RequiredDocumentId INT = NULL,
    @AcademicSessionId INT = NULL,
    @AdmissionModeId INT = NULL,
    @ProgramId INT = NULL,
    @CourseId INT = NULL,
    @CategoryId INT = NULL,
    @IsActive BIT = NULL,
    @DocumentEnclosureIds NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT
            r.RequiredDocumentId, r.AcademicSessionId, s.SessionName,
            r.AdmissionModeId, am.AdmissionModeName,
            r.ProgramId, p.ProgramName,
            r.CourseId, c.CourseName,
            r.CategoryId, cat.CategoryName,
            r.IsActive, r.CreatedDate,
            STRING_AGG(CAST(d.DocumentEnclosureId AS NVARCHAR(10)), ',') AS DocumentEnclosureIdsCsv,
            STRING_AGG(de.DocumentName, ', ') AS RequiredDocumentNames
        FROM RequiredDocumentMaster r
        JOIN AcademicSession s        ON s.AcademicSessionId = r.AcademicSessionId
        JOIN AdmissionModeMaster am   ON am.AdmissionModeId = r.AdmissionModeId
        JOIN ProgramMaster p          ON p.ProgramId = r.ProgramId
        JOIN CourseMaster c           ON c.CourseId = r.CourseId
        JOIN Category cat             ON cat.CategoryId = r.CategoryId
        LEFT JOIN RequiredDocumentDetail d ON d.RequiredDocumentId = r.RequiredDocumentId
        LEFT JOIN DocumentEnclosureMaster de ON de.DocumentEnclosureId = d.DocumentEnclosureId
        GROUP BY r.RequiredDocumentId, r.AcademicSessionId, s.SessionName,
                 r.AdmissionModeId, am.AdmissionModeName, r.ProgramId, p.ProgramName,
                 r.CourseId, c.CourseName, r.CategoryId, cat.CategoryName,
                 r.IsActive, r.CreatedDate
        ORDER BY r.RequiredDocumentId DESC;
    END

    IF @Flag = 'GETBYID'
    BEGIN
        SELECT * FROM RequiredDocumentMaster WHERE RequiredDocumentId = @RequiredDocumentId;

        SELECT DocumentEnclosureId
        FROM RequiredDocumentDetail
        WHERE RequiredDocumentId = @RequiredDocumentId;
    END

    IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO RequiredDocumentMaster
            (AcademicSessionId, AdmissionModeId, ProgramId, CourseId, CategoryId, IsActive)
        VALUES
            (@AcademicSessionId, @AdmissionModeId, @ProgramId, @CourseId, @CategoryId, ISNULL(@IsActive,1));

        DECLARE @NewId INT = SCOPE_IDENTITY();

        INSERT INTO RequiredDocumentDetail (RequiredDocumentId, DocumentEnclosureId)
        SELECT @NewId, CAST(value AS INT)
        FROM STRING_SPLIT(@DocumentEnclosureIds, ',');
    END

    IF @Flag = 'UPDATE'
    BEGIN
        UPDATE RequiredDocumentMaster
        SET AcademicSessionId = @AcademicSessionId,
            AdmissionModeId   = @AdmissionModeId,
            ProgramId         = @ProgramId,
            CourseId          = @CourseId,
            CategoryId        = @CategoryId,
            IsActive          = @IsActive
        WHERE RequiredDocumentId = @RequiredDocumentId;

        DELETE FROM RequiredDocumentDetail WHERE RequiredDocumentId = @RequiredDocumentId;

        INSERT INTO RequiredDocumentDetail (RequiredDocumentId, DocumentEnclosureId)
        SELECT @RequiredDocumentId, CAST(value AS INT)
        FROM STRING_SPLIT(@DocumentEnclosureIds, ',');
    END

    IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM RequiredDocumentMaster WHERE RequiredDocumentId = @RequiredDocumentId;
    END
END
GO