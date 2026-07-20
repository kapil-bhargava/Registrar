using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class DepartmentHODOverviewModel
    {
        public int DeptId { get; set; }
        public string Dept { get; set; }
        public string Faculty { get; set; }

        public string Hod { get; set; }             // "-" if vacant
        public string EmpId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Qualification { get; set; }
        public string Designation { get; set; }
        public string Date { get; set; }             // EffectiveDate as yyyy-MM-dd string, "-" if vacant
        public string TenureEnd { get; set; }
        public string Status { get; set; }           // Active / Vacant / Pending

        public List<HODHistoryEntry> History { get; set; } = new List<HODHistoryEntry>();
    }
}