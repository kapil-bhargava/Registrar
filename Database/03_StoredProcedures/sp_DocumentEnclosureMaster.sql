


/****** Object:  StoredProcedure [dbo].[sp_DocumentEnclosureMaster] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT 1 FROM sys.procedures WHERE name = 'sp_DocumentEnclosureMaster')
    DROP PROCEDURE [dbo].[sp_DocumentEnclosureMaster]
GO

CREATE PROCEDURE [dbo].[sp_DocumentEnclosureMaster]
    @Flag                  NVARCHAR(20),
    @DocumentEnclosureId   INT             = NULL,
    @DocumentName          NVARCHAR(200)   = NULL,
    @ShortName             NVARCHAR(50)    = NULL,
    @IsMandatory           BIT             = NULL,
    @IsOriginalRequired    BIT             = NULL,
    @IsActive              BIT             = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        SELECT DocumentEnclosureId, DocumentName, ShortName,
               IsMandatory, IsOriginalRequired, IsActive, CreatedDate
        FROM DocumentEnclosureMaster
        ORDER BY DocumentName;

    ELSE IF @Flag = 'GETACTIVE'   -- feeds dropdowns wherever this master is used (e.g. Admission enclosures)
        SELECT DocumentEnclosureId, DocumentName, ShortName,
               IsMandatory, IsOriginalRequired
        FROM DocumentEnclosureMaster
        WHERE IsActive = 1
        ORDER BY DocumentName;

    ELSE IF @Flag = 'GETBYID'
        SELECT DocumentEnclosureId, DocumentName, ShortName,
               IsMandatory, IsOriginalRequired, IsActive, CreatedDate
        FROM DocumentEnclosureMaster
        WHERE DocumentEnclosureId = @DocumentEnclosureId;

    ELSE IF @Flag = 'INSERT'
        INSERT INTO DocumentEnclosureMaster
            (DocumentName, ShortName, IsMandatory, IsOriginalRequired, IsActive)
        VALUES
            (@DocumentName, @ShortName, ISNULL(@IsMandatory, 0),
             ISNULL(@IsOriginalRequired, 0), ISNULL(@IsActive, 1));

    ELSE IF @Flag = 'UPDATE'
        UPDATE DocumentEnclosureMaster
        SET DocumentName       = @DocumentName,
            ShortName          = @ShortName,
            IsMandatory        = @IsMandatory,
            IsOriginalRequired = @IsOriginalRequired,
            IsActive           = @IsActive
        WHERE DocumentEnclosureId = @DocumentEnclosureId;

    -- Only status change is allowed here — deliberately NO 'DELETE' flag.
    ELSE IF @Flag = 'TOGGLESTATUS'
        UPDATE DocumentEnclosureMaster
        SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END
        WHERE DocumentEnclosureId = @DocumentEnclosureId;

END
GO
