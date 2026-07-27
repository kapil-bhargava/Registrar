using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class AdmissionSetupModel
    {

        public int AdmissionSetupId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int AcademicSessionId { get; set; }
        public string SessionName { get; set; }
        public int TotalSeats { get; set; }
        public decimal MinEligibilityPct { get; set; }
        public DateTime ApplicationStartDate { get; set; }
        public DateTime ApplicationEndDate { get; set; }
        public string EligibilityCriteria { get; set; }
        public string Status { get; set; }
    }
}