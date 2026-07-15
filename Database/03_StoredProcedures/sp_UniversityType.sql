CREATE PROCEDURE sp_UniversityType
(
    @Flag INT,
    @UniversityTypeId INT = NULL,
    @UniversityTypeCode VARCHAR(20)=NULL,
    @UniversityTypeName VARCHAR(100)=NULL
)
AS
BEGIN

    IF(@Flag=1)
    BEGIN
        INSERT INTO UniversityType
        (
            UniversityTypeCode,
            UniversityTypeName
        )
        VALUES
        (
            @UniversityTypeCode,
            @UniversityTypeName
        )
    END

    IF(@Flag=2)
    BEGIN
        SELECT *
        FROM UniversityType
    END

END