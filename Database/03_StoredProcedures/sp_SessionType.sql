CREATE PROCEDURE sp_SessionType
    @Flag             NVARCHAR(20),
    @SessionTypeId     INT             = NULL,
    @SessionTypeCode    NVARCHAR(20)    = NULL,
    @SessionTypeName     NVARCHAR(100)   = NULL,
    @Description           NVARCHAR(255)   = NULL,
    @DisplayOrder            INT             = NULL,
    @IsActive                  BIT             = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT SessionTypeId, SessionTypeCode, SessionTypeName, Description, DisplayOrder, IsActive, CreatedDate
        FROM SessionType
        ORDER BY DisplayOrder, SessionTypeName;
    END

    ELSE IF @Flag = 'GETACTIVE'
    BEGIN
        SELECT SessionTypeId, SessionTypeCode, SessionTypeName
        FROM SessionType
        WHERE IsActive = 1
        ORDER BY DisplayOrder, SessionTypeName;
    END

    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT SessionTypeId, SessionTypeCode, SessionTypeName, Description, DisplayOrder, IsActive, CreatedDate
        FROM SessionType
        WHERE SessionTypeId = @SessionTypeId;
    END

    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO SessionType (SessionTypeCode, SessionTypeName, Description, DisplayOrder, IsActive)
        VALUES (@SessionTypeCode, @SessionTypeName, @Description, @DisplayOrder, @IsActive);
    END

    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE SessionType
        SET SessionTypeCode = @SessionTypeCode,
            SessionTypeName = @SessionTypeName,
            Description     = @Description,
            DisplayOrder    = @DisplayOrder,
            IsActive        = @IsActive
        WHERE SessionTypeId = @SessionTypeId;
    END

    ELSE IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM SessionType WHERE SessionTypeId = @SessionTypeId;
    END
END
GO

/* Seed data */
INSERT INTO SessionType (SessionTypeCode, SessionTypeName, Description, DisplayOrder, IsActive) VALUES
('ANNUAL', 'Annual', 'Annual pattern - B.A., B.Sc., B.Com', 1, 1),
('SEM', 'Semester', 'Semester pattern - B.Tech, BCA, MBA, MCA, B.Pharm', 2, 1),
('TRI', 'Trimester', 'Trimester pattern - Executive MBA, Management Programs', 3, 1),
('QTR', 'Quarter', 'Quarter system - International Universities', 4, 1),
('RES', 'Research', 'Research - Ph.D., M.Phil., Doctoral Programs', 5, 1);
