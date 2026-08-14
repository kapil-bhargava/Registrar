


IF OBJECT_ID('dbo.sp_FeeHeadMaster', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_FeeHeadMaster;
GO

CREATE PROCEDURE dbo.sp_FeeHeadMaster
(
    @Flag NVARCHAR(20),
    @FeeHeadId INT = NULL,
    @FeeHeadCode NVARCHAR(20) = NULL,
    @FeeHeadName NVARCHAR(100) = NULL,
    @Description NVARCHAR(250) = NULL,
    @IsActive BIT = 1
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GETALL'
    BEGIN
        SELECT FeeHeadId, FeeHeadCode, FeeHeadName, Description, IsActive
        FROM dbo.FeeHeadMaster
        ORDER BY FeeHeadId DESC;
    END

    IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO dbo.FeeHeadMaster (FeeHeadCode, FeeHeadName, Description, IsActive)
        VALUES (@FeeHeadCode, @FeeHeadName, @Description, @IsActive);
    END

    IF @Flag = 'UPDATE'
    BEGIN
        UPDATE dbo.FeeHeadMaster
        SET FeeHeadCode = @FeeHeadCode, FeeHeadName = @FeeHeadName,
            Description = @Description, IsActive = @IsActive
        WHERE FeeHeadId = @FeeHeadId;
    END

    IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM dbo.FeeHeadMaster WHERE FeeHeadId = @FeeHeadId;
    END
END
GO


INSERT INTO dbo.FeeHeadMaster (FeeHeadCode, FeeHeadName, Description, IsActive) VALUES
('TUITION',   'Tuition Fee',          'Core academic tuition charged every semester', 1),
('EXAM',      'Exam Fee',             'Semester / annual examination fee', 1),
('HOSTEL',    'Hostel Fee',           'Hostel accommodation charges', 1),
('LIBRARY',   'Library Fee',          'Library membership and maintenance', 1),
('LAB',       'Laboratory Fee',       'Lab usage and equipment charges', 1),
('TRANSPORT', 'Transport Fee',        'College bus / transport charges', 1),
('DEV',       'Development Fee',      'Infrastructure and development charges', 1),
('CAUTION',   'Caution Money (Refundable)', 'Refundable security deposit at admission', 1),
('SPORTS',    'Sports Fee',           'Sports and extracurricular activities', 0),
('ID',        'ID Card Fee',          'Student ID card issuance charge', 1);
GO