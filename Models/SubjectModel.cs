using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class SubjectModel
    {
       
            public int SubjectId { get; set; }
            public string SubjectCode { get; set; }
            public string SubjectName { get; set; }
            public string SubjectType { get; set; }          // Theory / Practical / Mixed

            public int SemesterId { get; set; }              // FK -> Semester
            public string SemesterName { get; set; }           // display only (joined)

            public int CourseId { get; set; }                 // FK -> Course
            public string CourseName { get; set; }              // display only (joined)

            public int? Credits { get; set; }
            public string Status { get; set; }
            public DateTime? CreatedDate { get; set; }
        
    }
}