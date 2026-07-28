using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class CourseStructureMappingModel
    {
        public int MappingId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int SemesterNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}