using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class EligibilityCheckModel
    {
        public string ApplicantName { get; set; }
        public string CourseName { get; set; }
        public string SessionName { get; set; }
        public string EligibilityCriteria { get; set; }
        public decimal MinEligibilityPct { get; set; }
        public decimal PercentageObtained { get; set; }
        public bool IsEligible { get; set; }
        public DateTime CheckedOn { get; set; }
    }
}