using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class UniversityProfileModel
    {

        public int UniversityId { get; set; }

        // Step 1: Profile
        public string UniversityName { get; set; }
        public string ShortName { get; set; }
        public string UniversityCode { get; set; }
        public int? UniversityTypeId { get; set; }
        public int? EstablishedYear { get; set; }
        public string Accreditation { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TaxNumber { get; set; }
        public string AffiliatedUniversity { get; set; }
        public bool IsActive { get; set; }

        // Step 2: Details
        public string Motto { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }
        public string Description { get; set; }
        public string AboutUniversity { get; set; }
        public string RegistrationNumber { get; set; }
        public string UGCApprovalNumber { get; set; }
        public string AICTEApproval { get; set; }
        public string NAACGrade { get; set; }
        public string NIRFRank { get; set; }
        public string ISOCertification { get; set; }
        public string OfficeNumber { get; set; }
        public string FaxNumber { get; set; }
        public string DetailsEmail { get; set; }
        public string DetailsWebsite { get; set; }
        public string Facebook { get; set; }
        public string LinkedIn { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }

        // Step 3: Logos — stored paths (from DB)
        public string LogoMainPath { get; set; }
        public string LogoDarkPath { get; set; }
        public string LogoLightPath { get; set; }
        public string LogoSidebarPath { get; set; }
        public string FaviconPath { get; set; }

        // Step 3: Logos — uploaded files (from form, not stored directly)
        public HttpPostedFileBase LogoMainFile { get; set; }
        public HttpPostedFileBase LogoDarkFile { get; set; }
        public HttpPostedFileBase LogoLightFile { get; set; }
        public HttpPostedFileBase LogoSidebarFile { get; set; }
        public HttpPostedFileBase FaviconFile { get; set; }

        // Step 4: Address
        public string Country { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string PostalCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        // Step 5: Contact
        public string ContOffice { get; set; }
        public string ContMobile { get; set; }
        public string ContAlternate { get; set; }
        public string ContAdmissionHelpline { get; set; }
        public string ContRegistrarOffice { get; set; }
        public string ContEmail { get; set; }
        public string ContSupportEmail { get; set; }
        public string ContWebsite { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
