using System;

namespace Regis.Models
{
    public class FacultyModel
    {
        public int FacultyId { get; set; }
        public string FacultyCode { get; set; }
        public string FacultyName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? CampusId { get; set; }      // NEW
        public string CampusName { get; set; }  // NEW (display ke liye)
        public DateTime? CreatedDate { get; set; }
    }

   
}