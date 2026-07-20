using System;

namespace Regis.Models
{
    public class ProgramModel
    {
        public int ProgramId { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public string ProgramType { get; set; }        // UG / PG / Diploma / Certificate
        public string TypicalDuration { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    
}