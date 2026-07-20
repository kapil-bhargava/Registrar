


CREATE PROCEDURE sp_AcademicSession
    @Flag                NVARCHAR(20),
    @AcademicSessionId    INT             = NULL,
    @SessionName            NVARCHAR(150)   = NULL,
    @SessionCode              NVARCHAR(50)    = NULL,
    @SessionTypeId              INT             = NULL,
    @StartDate                    DATE            = NULL,
    @EndDate                        DATE            = NULL,
    @AcademicYear                     NVARCHAR(20)    = NULL,
    @MaxCredits                         INT             = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
        SELECT s.AcademicSessionId, s.SessionName, s.SessionCode, s.SessionTypeId,
               st.SessionTypeName, s.StartDate, s.EndDate, s.AcademicYear, s.Status, s.MaxCredits, s.CreatedDate
        FROM AcademicSession s
        INNER JOIN SessionType st ON s.SessionTypeId = st.SessionTypeId
        ORDER BY s.StartDate DESC;

    ELSE IF @Flag = 'GETACTIVE'      -- used as dropdown source by Semester
        SELECT AcademicSessionId, SessionName FROM AcademicSession WHERE Status = 'Active' ORDER BY StartDate DESC;

    ELSE IF @Flag = 'GETBYID'
        SELECT * FROM AcademicSession WHERE AcademicSessionId = @AcademicSessionId;

    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO AcademicSession (SessionName, SessionCode, SessionTypeId, StartDate, EndDate, AcademicYear, Status, MaxCredits)
        VALUES (@SessionName, @SessionCode, @SessionTypeId, @StartDate, @EndDate, @AcademicYear, 'Draft', @MaxCredits);

        SELECT SCOPE_IDENTITY() AS NewId;
    END

    ELSE IF @Flag = 'UPDATE'
        UPDATE AcademicSession
        SET SessionName = @SessionName, SessionCode = @SessionCode, SessionTypeId = @SessionTypeId,
            StartDate = @StartDate, EndDate = @EndDate, AcademicYear = @AcademicYear, MaxCredits = @MaxCredits
        WHERE AcademicSessionId = @AcademicSessionId;

    ELSE IF @Flag = 'DELETE'
        DELETE FROM AcademicSession WHERE AcademicSessionId = @AcademicSessionId;

    ELSE IF @Flag = 'ACTIVATE'       -- only one Active session at a time
    BEGIN
        UPDATE AcademicSession SET Status = 'Locked' WHERE Status = 'Active';
        UPDATE AcademicSession SET Status = 'Active' WHERE AcademicSessionId = @AcademicSessionId;
    END

    ELSE IF @Flag = 'CLOSE'
        UPDATE AcademicSession SET Status = 'Archived' WHERE AcademicSessionId = @AcademicSessionId;
END
GO

INSERT INTO AcademicSession (SessionName, SessionCode, SessionTypeId, StartDate, EndDate, AcademicYear, Status, MaxCredits) VALUES
('2025-26', 'AY-2025-26', 1, '2025-04-01', '2026-03-31', '2025-26', 'Active', 24);
