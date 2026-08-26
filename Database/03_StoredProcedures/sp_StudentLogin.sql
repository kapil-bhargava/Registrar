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






-- (jab tak password plain ASCII/UTF8 ho, jaise phone numbers hote hain)
-- FIX: Phone agar nvarchar hai to HASHBYTES use nahi Unicode bytes leta hai,
-- jabki C# ka Encoding.UTF8.GetBytes() single-byte UTF8 leta hai — mismatch isi wajah se ho raha tha.
-- CONVERT(VARCHAR(50), a.Phone) explicitly single-byte string me convert karta hai — ab hash match karega.
UPDATE sl
SET
    sl.Username = a.Email,
    sl.PasswordHash = LOWER(CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', CONVERT(VARCHAR(50), a.Phone)), 2))
FROM StudentLogin sl
INNER JOIN Application a ON a.ApplicationId = sl.ApplicationId
WHERE a.Email IS NOT NULL AND a.Phone IS NOT NULL;

-- Check karne ke liye:
SELECT * FROM StudentLogin;