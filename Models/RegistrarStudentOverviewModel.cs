using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regis.Models
{
    public  class RegistrarStudentOverviewModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationNo { get; set; }
        public string FullName { get; set; }
        public string RegistrationNumber { get; set; }
        public string UniversityEnrollmentNumber { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Branch { get; set; }
        public int AcademicSessionId { get; set; }
        public string SessionName { get; set; }
        public string Semester { get; set; }
        public string Stage { get; set; }
        public bool DocVerified { get; set; }
        public int RequiredDocCount { get; set; }
        public int SubmittedDocCount { get; set; }

        // Derived (set in service, not DB)
        public string DocumentStatus { get; set; }      // Complete / Pending / Deficient
        public string VerificationStatus { get; set; }  // Verified / Verification Pending
        public string RegistrationStatus { get; set; }  // Active / Incomplete
    }
}
