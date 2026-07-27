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
        public string SeatNumber { get; set; }
        public string FeeReceiptNo { get; set; }
        public string StudentId { get; set; }
    }
}