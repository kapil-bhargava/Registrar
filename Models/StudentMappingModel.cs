using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regis.Models
{
    public class StudentMappingModel
    {
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string CourseName { get; set; }
        public string Section { get; set; }
        public int? Semester { get; set; }
        public bool IsMapped { get; set; }
    }
}
