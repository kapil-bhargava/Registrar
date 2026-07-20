using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
   
        public class CourseModel
        {
            public int CourseId { get; set; }
            public string CourseCode { get; set; }
            public string CourseName { get; set; }

            public int ProgramId { get; set; }              // FK -> Program
            public string ProgramName { get; set; }          // display only (joined)

            public int DepartmentId { get; set; }            // FK -> Department
            public string DepartmentName { get; set; }        // display only (joined)

            public int? DurationYears { get; set; }
            public int? TotalSemesters { get; set; }
            public int? TotalCredits { get; set; }
            public int? IntakeCapacity { get; set; }
            public string Status { get; set; }
            public DateTime? CreatedDate { get; set; }
        
    }
}