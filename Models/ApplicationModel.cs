using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class ApplicationModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationNo { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int AdmissionModeId { get; set; }
        public string AdmissionModeName { get; set; }
        public int AdmissionSetupId { get; set; }
        public string CourseName { get; set; }
        public decimal? PreviousPercentage { get; set; }

        public DateTime RegisteredOn { get; set; }
        public string Stage { get; set; }
        public bool DocVerified { get; set; }
        public bool CounsellingDone { get; set; }
        public bool FeePaid { get; set; }
        public string StudentId { get; set; }

        public string SeatNumber { get; set; }
        public DateTime? CounsellingDate { get; set; }
        public TimeSpan? CounsellingTime { get; set; }
        public string CounsellingMode { get; set; }

        public string FeeReceiptNo { get; set; }
        public decimal? FeeAmount { get; set; }
        public string FeeMode { get; set; }

        // ---- NEW: Basic information ----
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DisplayNameFormat { get; set; }
        public string DisplayName { get; set; }
        public string MaritalStatus { get; set; }

        // ---- NEW: Admin information (auto-generated at Admission Final) ----
        public string RegistrationNumber { get; set; }
        public string UniversityEnrollmentNumber { get; set; }

        // ---- NEW: Personal information ----
        public string BirthState { get; set; }
        public string BirthPlace { get; set; }
        public string WhatsAppNumber { get; set; }
        public string ReferralSource { get; set; }
        public bool PhysicallyChallenged { get; set; }
        public string BloodGroup { get; set; }
        public string IdentityMark { get; set; }
        public string MotherTongue { get; set; }

        // ---- NEW: Contact details ----
        public string InstituteEmail { get; set; }
        public string AlternateMobileNumber { get; set; }

        // ---- NEW: Identity & domicile ----
        public string Citizenship { get; set; }
        public string DomicileCountry { get; set; }
        public string DomicileState { get; set; }

        // ---- NEW: Other information ----
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string Caste { get; set; }
        public string ABCId { get; set; }
        public string AntiRaggingId { get; set; }
    }
}


 
  
