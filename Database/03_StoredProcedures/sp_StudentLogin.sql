CREATE PROCEDURE sp_StudentLogin
    @Flag           VARCHAR(20),
    @ApplicationId  INT           = NULL,
    @Username       VARCHAR(50)   = NULL,
    @PasswordHash   VARCHAR(64)   = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'VALIDATE'
    BEGIN
        SELECT sl.ApplicationId, sl.Username, a.FullName, a.StudentId
        FROM StudentLogin sl
        INNER JOIN Application a ON a.ApplicationId = sl.ApplicationId
        WHERE sl.Username = @Username
          AND sl.PasswordHash = @PasswordHash
          AND sl.Status = 'Active';
    END

    IF @Flag = 'GETBYAPPID'
    BEGIN
        SELECT * FROM StudentLogin WHERE ApplicationId = @ApplicationId;
    END

    IF @Flag = 'INSERT'
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM StudentLogin WHERE ApplicationId = @ApplicationId)
        BEGIN
            INSERT INTO StudentLogin (ApplicationId, Username, PasswordHash, Status)
            VALUES (@ApplicationId, @Username, @PasswordHash, 'Active');
        END
    END

    IF @Flag = 'CHANGEPASSWORD'
    BEGIN
        UPDATE StudentLogin SET PasswordHash = @PasswordHash WHERE ApplicationId = @ApplicationId;
    END
END
GO