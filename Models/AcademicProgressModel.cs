using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regis.Models
{
    public class AcademicProgressModel
    {
        public int ProgressId { get; set; }
        public string StudentId { get; set; }
        public string Name { get; set; }
        public int Semester { get; set; }
        public decimal SGPA { get; set; }
        public decimal? Attendance { get; set; }
        public string ResultStatus { get; set; }
    }
}
