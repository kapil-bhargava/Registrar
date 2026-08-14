using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Regis.Models
{
    // Used by GetAllPersonalInformation() — powers the list views on
    // Personal Information, Admission Information, and Address Information
    // pages. All fields come from the single dbo.Application table.
    public class ApplicationListItemModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationNo { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public DateTime? RegisteredOn { get; set; }
        public string Stage { get; set; }
        public string Citizenship { get; set; }
        public string BloodGroup { get; set; }

        // Admission Information summary (shown on the Admission list)
        public string AcademicSessionName { get; set; }
        public string Degree { get; set; }
        public string Branch { get; set; }
        public DateTime? DateOfAdmission { get; set; }

        // Address Information summary (shown on the Address list)
        public string PermanentCity { get; set; }
        public string PermanentState { get; set; }
        public string LocalCity { get; set; }
        public string LocalState { get; set; }
        // existing fields ke saath ye bhi add karo
        public string FatherFirstName { get; set; }
        public string FatherLastName { get; set; }
        public string MotherFirstName { get; set; }
        public string MotherLastName { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
    }
}
