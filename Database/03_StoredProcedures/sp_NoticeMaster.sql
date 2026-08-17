

CREATE PROCEDURE dbo.sp_NoticeMaster
(
    @Flag NVARCHAR(20),
    @NoticeId INT = NULL,
    @Title NVARCHAR(200) = NULL,
    @EventDate DATE = NULL,
    @Location NVARCHAR(200) = NULL,
    @Description NVARCHAR(500) = NULL,
    @IsActive BIT = 1
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT NoticeId, Title, EventDate, Location, Description, IsActive, CreatedDate
        FROM dbo.NoticeMaster
        ORDER BY EventDate DESC;
    END

    IF @Flag = 'GETUPCOMING'
    BEGIN
        -- Dashboard ke liye: aane wale 4 active events, date ke hisaab se
        SELECT TOP 4 NoticeId, Title, EventDate, Location, Description
        FROM dbo.NoticeMaster
        WHERE IsActive = 1 AND EventDate >= CAST(GETDATE() AS DATE)
        ORDER BY EventDate ASC;
    END

    IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO dbo.NoticeMaster (Title, EventDate, Location, Description, IsActive)
        VALUES (@Title, @EventDate, @Location, @Description, @IsActive);
    END

    IF @Flag = 'UPDATE'
    BEGIN
        UPDATE dbo.NoticeMaster
        SET Title = @Title, EventDate = @EventDate, Location = @Location,
            Description = @Description, IsActive = @IsActive
        WHERE NoticeId = @NoticeId;
    END

    IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM dbo.NoticeMaster WHERE NoticeId = @NoticeId;
    END
END
GO