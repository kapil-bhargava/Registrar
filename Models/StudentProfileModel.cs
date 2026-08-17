using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regis.Models
{
    public class StudentProfileModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationNo { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CourseName { get; set; }
        public string Branch { get; set; }
        public string Semester { get; set; }
        public string CategoryName { get; set; }
        public DateTime? RegisteredOn { get; set; }
        public string Stage { get; set; }
        public bool DocVerified { get; set; }
        public int RequiredDocCount { get; set; }
        public int SubmittedDocCount { get; set; }

        public decimal? FeeAmount { get; set; }
        public bool FeePaid { get; set; }
        public string FeeReceiptNo { get; set; }
        public string FeeMode { get; set; }
        public DateTime? FeePaymentDate { get; set; }
        public string AdmissionModeName { get; set; }
        public string AdmissionStatus { get; set; }

        public List<DocumentChecklistItemModel> Documents { get; set; } = new List<DocumentChecklistItemModel>();
    }
}
