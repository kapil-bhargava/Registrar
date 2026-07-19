using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class CourseMasterModel
    {
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? DurationYears { get; set; }
        public int? TotalSemesters { get; set; }
        public int? TotalCredits { get; set; }
        public int? IntakeCapacity { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}