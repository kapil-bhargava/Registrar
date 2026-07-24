using System;

namespace Regis.Models
{
    public class AcademicSessionModel
    {
        public int AcademicSessionId { get; set; }
        public string SessionName { get; set; }
        public string SessionCode { get; set; }

        public int SessionTypeId { get; set; }          // FK -> SessionType
        public string SessionTypeName { get; set; }       // display only (joined)

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AcademicYear { get; set; }
        public string Status { get; set; }               // Draft / Active / Locked / Archived
        public int? MaxCredits { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}