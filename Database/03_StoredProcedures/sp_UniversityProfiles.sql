CREATE OR ALTER PROCEDURE sp_UniversityProfiles
(
    @Flag VARCHAR(20),
    @UniversityName VARCHAR(200) = NULL,
    @ShortName VARCHAR(50) = NULL,
    @UniversityCode VARCHAR(50) = NULL,
    @UniversityTypeId INT = NULL,
    @EstablishedYear INT = NULL,
    @Accreditation VARCHAR(250) = NULL,
    @Website VARCHAR(200) = NULL,
    @Email VARCHAR(150) = NULL,
    @Phone VARCHAR(50) = NULL,
    @TaxNumber VARCHAR(100) = NULL,
    @AffiliatedUniversity VARCHAR(200) = NULL,
    @IsActive INT = NULL,

    @Motto VARCHAR(250) = NULL,
    @Vision VARCHAR(500) = NULL,
    @Mission VARCHAR(1000) = NULL,
    @Description VARCHAR(2000) = NULL,
    @AboutUniversity VARCHAR(4000) = NULL,
    @RegistrationNumber VARCHAR(100) = NULL,
    @UGCApprovalNumber VARCHAR(100) = NULL,
    @AICTEApproval VARCHAR(100) = NULL,
    @NAACGrade VARCHAR(20) = NULL,
    @NIRFRank VARCHAR(20) = NULL,
    @ISOCertification VARCHAR(100) = NULL,
    @OfficeNumber VARCHAR(50) = NULL,
    @FaxNumber VARCHAR(50) = NULL,
    @DetailsEmail VARCHAR(150) = NULL,
    @DetailsWebsite VARCHAR(200) = NULL,
    @Facebook VARCHAR(250) = NULL,
    @LinkedIn VARCHAR(250) = NULL,
    @Instagram VARCHAR(250) = NULL,
    @Twitter VARCHAR(250) = NULL,

    @LogoMainPath VARCHAR(300) = NULL,
    @LogoDarkPath VARCHAR(300) = NULL,
    @LogoLightPath VARCHAR(300) = NULL,
    @LogoSidebarPath VARCHAR(300) = NULL,
    @FaviconPath VARCHAR(300) = NULL,

    @Country VARCHAR(100) = NULL,
    @State VARCHAR(100) = NULL,
    @District VARCHAR(100) = NULL,
    @City VARCHAR(100) = NULL,
    @Area VARCHAR(150) = NULL,
    @Street VARCHAR(200) = NULL,
    @BuildingNumber VARCHAR(50) = NULL,
    @PostalCode VARCHAR(20) = NULL,
    @Latitude VARCHAR(30) = NULL,
    @Longitude VARCHAR(30) = NULL,

    @ContOffice VARCHAR(50) = NULL,
    @ContMobile VARCHAR(50) = NULL,
    @ContAlternate VARCHAR(50) = NULL,
    @ContAdmissionHelpline VARCHAR(50) = NULL,
    @ContRegistrarOffice VARCHAR(50) = NULL,
    @ContEmail VARCHAR(150) = NULL,
    @ContSupportEmail VARCHAR(150) = NULL,
    @ContWebsite VARCHAR(200) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Flag = 'GET'
    BEGIN
        SELECT TOP 1 * FROM UniversityProfiles ORDER BY UniversityId;
    END
    ELSE IF @Flag = 'SAVE'
    BEGIN
        IF EXISTS (SELECT 1 FROM UniversityProfiles)
        BEGIN
            UPDATE TOP (1) UniversityProfiles
            SET
                UniversityName = @UniversityName, ShortName = @ShortName, UniversityCode = @UniversityCode,
                UniversityTypeId = @UniversityTypeId, EstablishedYear = @EstablishedYear, Accreditation = @Accreditation,
                Website = @Website, Email = @Email, Phone = @Phone, TaxNumber = @TaxNumber,
                AffiliatedUniversity = @AffiliatedUniversity, IsActive = @IsActive,

                Motto = @Motto, Vision = @Vision, Mission = @Mission, Description = @Description,
                AboutUniversity = @AboutUniversity, RegistrationNumber = @RegistrationNumber,
                UGCApprovalNumber = @UGCApprovalNumber, AICTEApproval = @AICTEApproval, NAACGrade = @NAACGrade,
                NIRFRank = @NIRFRank, ISOCertification = @ISOCertification, OfficeNumber = @OfficeNumber,
                FaxNumber = @FaxNumber, DetailsEmail = @DetailsEmail, DetailsWebsite = @DetailsWebsite,
                Facebook = @Facebook, LinkedIn = @LinkedIn, Instagram = @Instagram, Twitter = @Twitter,

                LogoMainPath = ISNULL(@LogoMainPath, LogoMainPath),
                LogoDarkPath = ISNULL(@LogoDarkPath, LogoDarkPath),
                LogoLightPath = ISNULL(@LogoLightPath, LogoLightPath),
                LogoSidebarPath = ISNULL(@LogoSidebarPath, LogoSidebarPath),
                FaviconPath = ISNULL(@FaviconPath, FaviconPath),

                Country = @Country, State = @State, District = @District, City = @City, Area = @Area,
                Street = @Street, BuildingNumber = @BuildingNumber, PostalCode = @PostalCode,
                Latitude = @Latitude, Longitude = @Longitude,

                ContOffice = @ContOffice, ContMobile = @ContMobile, ContAlternate = @ContAlternate,
                ContAdmissionHelpline = @ContAdmissionHelpline, ContRegistrarOffice = @ContRegistrarOffice,
                ContEmail = @ContEmail, ContSupportEmail = @ContSupportEmail, ContWebsite = @ContWebsite,

                UpdatedDate = GETDATE();
        END
        ELSE
        BEGIN
            INSERT INTO UniversityProfiles
            (
                UniversityName, ShortName, UniversityCode, UniversityTypeId, EstablishedYear, Accreditation,
                Website, Email, Phone, TaxNumber, AffiliatedUniversity, IsActive,

                Motto, Vision, Mission, Description, AboutUniversity, RegistrationNumber, UGCApprovalNumber,
                AICTEApproval, NAACGrade, NIRFRank, ISOCertification, OfficeNumber, FaxNumber, DetailsEmail,
                DetailsWebsite, Facebook, LinkedIn, Instagram, Twitter,

                LogoMainPath, LogoDarkPath, LogoLightPath, LogoSidebarPath, FaviconPath,

                Country, State, District, City, Area, Street, BuildingNumber, PostalCode, Latitude, Longitude,

                ContOffice, ContMobile, ContAlternate, ContAdmissionHelpline, ContRegistrarOffice,
                ContEmail, ContSupportEmail, ContWebsite,

                CreatedDate, UpdatedDate
            )
            VALUES
            (
                @UniversityName, @ShortName, @UniversityCode, @UniversityTypeId, @EstablishedYear, @Accreditation,
                @Website, @Email, @Phone, @TaxNumber, @AffiliatedUniversity, @IsActive,

                @Motto, @Vision, @Mission, @Description, @AboutUniversity, @RegistrationNumber, @UGCApprovalNumber,
                @AICTEApproval, @NAACGrade, @NIRFRank, @ISOCertification, @OfficeNumber, @FaxNumber, @DetailsEmail,
                @DetailsWebsite, @Facebook, @LinkedIn, @Instagram, @Twitter,

                @LogoMainPath, @LogoDarkPath, @LogoLightPath, @LogoSidebarPath, @FaviconPath,

                @Country, @State, @District, @City, @Area, @Street, @BuildingNumber, @PostalCode, @Latitude, @Longitude,

                @ContOffice, @ContMobile, @ContAlternate, @ContAdmissionHelpline, @ContRegistrarOffice,
                @ContEmail, @ContSupportEmail, @ContWebsite,

                GETDATE(), GETDATE()
            );
        END
    END
END
