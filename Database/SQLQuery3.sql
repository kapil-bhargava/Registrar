CREATE PROCEDURE sp_CourseBranchMapping
    @Flag        VARCHAR(20),
    @CourseId    INT = NULL,
    @BranchIds   VARCHAR(MAX) = NULL   -- comma separated: '1,2,3' (SAVE ke liye)
AS
BEGIN
    SET NOCOUNT ON;

    -- =========================================================
    -- GETALL : Course wise saari mapped branches (comma joined)
    -- =========================================================
    IF @Flag = 'GETALL'
    BEGIN
        SELECT 
            cm.CourseId,
            cm.CourseName,
            STRING_AGG(bm.BranchName, ', ') AS MappedBranches,
            COUNT(cbm.BranchId) AS TotalBranches
        FROM CourseMaster cm
        LEFT JOIN CourseBranchMapping cbm 
               ON cm.CourseId = cbm.CourseId AND cbm.IsActive = 1
        LEFT JOIN BranchMaster bm 
               ON cbm.BranchId = bm.BranchId
        GROUP BY cm.CourseId, cm.CourseName
        ORDER BY cm.CourseName
    END

    -- =========================================================
    -- GETBYCOURSE : Ek course ki mapped branch ids (edit ke liye)
    -- =========================================================
    IF @Flag = 'GETBYCOURSE'
    BEGIN
        SELECT 
            cbm.MappingId,
            cbm.BranchId,
            bm.BranchName,
            bm.BranchCode
        FROM CourseBranchMapping cbm
        INNER JOIN BranchMaster bm ON cbm.BranchId = bm.BranchId
        WHERE cbm.CourseId = @CourseId
          AND cbm.IsActive = 1
    END

    -- =========================================================
    -- SAVE : Purani mapping hata ke naya set insert (course ke liye)
    -- =========================================================
    IF @Flag = 'SAVE'
    BEGIN
        DELETE FROM CourseBranchMapping WHERE CourseId = @CourseId;

        IF @BranchIds IS NOT NULL AND LEN(@BranchIds) > 0
        BEGIN
            INSERT INTO CourseBranchMapping (CourseId, BranchId)
            SELECT @CourseId, CAST(value AS INT)
            FROM STRING_SPLIT(@BranchIds, ',')
            WHERE LTRIM(RTRIM(value)) <> '';
        END
    END

    -- =========================================================
    -- DELETE : Ek particular mapping row hatani ho
    -- =========================================================
    IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM CourseBranchMapping WHERE CourseId = @CourseId;
    END
END