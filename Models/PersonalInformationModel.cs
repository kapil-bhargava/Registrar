using System;

namespace Regis.Models
{
    public class PersonalInformationModel
    {
        public int ApplicationId { get; set; }         // 0 = new, >0 = edit
        public string ApplicationNo { get; set; }        // read-only, auto-generated

        // Core
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }

        // Basic Information
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DisplayNameFormat { get; set; }
        public string DisplayName { get; set; }
        public string MaritalStatus { get; set; }

        // Personal Information
        public string BirthState { get; set; }
        public string BirthPlace { get; set; }
        public string WhatsAppNumber { get; set; }          // FIXED: single field (e.g. "+91-9876543210")
        public string ReferralSource { get; set; }
        public bool PhysicallyChallenged { get; set; }
        public string BloodGroup { get; set; }
        public string IdentityMark { get; set; }
        public string MotherTongue { get; set; }
        public string AlternateMobileNumber { get; set; }   // FIXED: single field

        // Identity & Domicile
        public string Citizenship { get; set; }
        public string DomicileCountry { get; set; }
        public string DomicileState { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string Caste { get; set; }
        public string ABCId { get; set; }
        public string AntiRaggingId { get; set; }
    }

    public class PersonalInformationListItemModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationNo { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public DateTime RegisteredOn { get; set; }
        public string Stage { get; set; }
        public string Citizenship { get; set; }
        public string BloodGroup { get; set; }

        public string AcademicSessionName { get; set; }
        public string Degree { get; set; }
        public string Branch { get; set; }
        public DateTime? DateOfAdmission { get; set; }
    }
}