USE [UniversityERP]
GO
/****** Object:  StoredProcedure [dbo].[sp_UniversityType]    Script Date: 17-07-2026 14:38:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
=============================================================
Procedure Name : sp_UniversityType
Purpose        : Perform CRUD operations on UniversityType
Created By     : Kapil
=============================================================
*/
ALTER   PROCEDURE [dbo].[sp_UniversityType]
(
    @Flag VARCHAR(20),
    @UniversityTypeId INT = NULL,
    @UniversityTypeCode VARCHAR(20) = NULL,
    @UniversityTypeName VARCHAR(100) = NULL,
    @Description VARCHAR(250) = NULL,
    @DisplayOrder INT = NULL,
    @IsActive BIT = NULL,
    @CreatedDate DATETIME = NULL
)
AS
BEGIN
    SET NOCOUNT ON;
    ----------------------------------------------------------
    -- GET ALL
    ----------------------------------------------------------
    IF @Flag = 'GETALL'
    BEGIN
        SELECT
            UniversityTypeId,
            UniversityTypeCode,
            UniversityTypeName,
            Description,
            DisplayOrder,
            IsActive,
            CreatedDate
        FROM UniversityType
        ORDER BY DisplayOrder;
    END
    ----------------------------------------------------------
    -- GET ACTIVE (for dropdowns)
    ----------------------------------------------------------
    ELSE IF @Flag = 'GETACTIVE'
    BEGIN
        SELECT UniversityTypeId, UniversityTypeCode, UniversityTypeName
        FROM UniversityType
        WHERE IsActive = 1
        ORDER BY DisplayOrder;
    END
    ----------------------------------------------------------
    -- GET BY ID (Edit ke liye — ab IsActive aur CreatedDate bhi milega)
    ----------------------------------------------------------
    ELSE IF @Flag = 'GETBYID'
    BEGIN
        SELECT
            UniversityTypeId,
            UniversityTypeCode,
            UniversityTypeName,
            Description,
            DisplayOrder,
            IsActive,
            CreatedDate
        FROM UniversityType
        WHERE UniversityTypeId = @UniversityTypeId;
    END
    ----------------------------------------------------------
    -- INSERT
    ----------------------------------------------------------
    ELSE IF @Flag = 'INSERT'
    BEGIN
        INSERT INTO UniversityType
        (
            UniversityTypeCode,
            UniversityTypeName,
            [Description],
            DisplayOrder,
            IsActive,
            CreatedDate
        )
        VALUES
        (
            @UniversityTypeCode,
            @UniversityTypeName,
            @Description,
            @DisplayOrder,
            @IsActive,
            GETDATE()
        );
    END
    ----------------------------------------------------------
    -- UPDATE (ab IsActive bhi update hoga)
    ----------------------------------------------------------
    ELSE IF @Flag = 'UPDATE'
    BEGIN
        UPDATE UniversityType
        SET
            UniversityTypeCode = @UniversityTypeCode,
            UniversityTypeName = @UniversityTypeName,
            Description = @Description,
            DisplayOrder = @DisplayOrder,
            IsActive = @IsActive
        WHERE UniversityTypeId = @UniversityTypeId;
    END
    ----------------------------------------------------------
    -- DELETE
    ----------------------------------------------------------
    ELSE IF @Flag = 'DELETE'
    BEGIN
        DELETE FROM UniversityType
        WHERE UniversityTypeId = @UniversityTypeId;
    END
END