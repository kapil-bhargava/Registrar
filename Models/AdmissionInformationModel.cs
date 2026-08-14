using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regis.Models
{
    public class AdmissionInformationModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationNo { get; set; }

        public string AcademicStream { get; set; }
        public string AcademicSessionName { get; set; }
        public string Degree { get; set; }
        public string Branch { get; set; }
        public string AcademicBatch { get; set; }
        public string Enrollment { get; set; }
        public string AcademicYear { get; set; }
        public string Semester { get; set; }
        public string Scheme { get; set; }
        public string ClassSection { get; set; }
        public string RollNumber { get; set; }

        public DateTime? DateOfAdmission { get; set; }
        public string AdmissionCategory { get; set; }
        public string FeesCategory { get; set; }
        public string Shift { get; set; }
        public string EntranceExamRegNo { get; set; }
        public string EntranceExamMeritNo { get; set; }
        public string ReferenceName { get; set; }

        public int AdmissionSetupId { get; set; }
        public int CategoryId { get; set; }
    }
}