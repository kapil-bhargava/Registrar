using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class SubjectMasterModel
    {
        public int SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string SubjectType { get; set; }
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int? Credits { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}