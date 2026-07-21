using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class DepartmentModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }

        public int FacultyId { get; set; }              // FK -> Faculty
        public string FacultyName { get; set; }          // display only (joined)

        public string CampusName { get; set; }

        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}