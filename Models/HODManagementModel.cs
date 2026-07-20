using System;
using System.Collections.Generic;

namespace Regis.Models
{
    /// <summary>Raw row from HODManagement table (current + relieved history rows).</summary>
    public class HODManagementModel
    {
        public int HODId { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string FacultyName { get; set; }

        public string HODName { get; set; }
        public string EmployeeId { get; set; }
        public string Designation { get; set; }
        public string Qualification { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? TenureEndDate { get; set; }
        public string ReasonForChange { get; set; }
        public string Status { get; set; }               // Active / Relieved
        public DateTime? CreatedDate { get; set; }
    }

    /// <summary>
    /// One row per Department, shaped to match the "departments" JS array
    /// already used in HODManagement.cshtml — current HOD + full history.
    /// This is what gets JSON-serialized into the view.
    /// </summary>


    public class HODHistoryEntry
    {
        public string Name { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }
}