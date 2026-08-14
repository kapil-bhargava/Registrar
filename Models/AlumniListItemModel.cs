using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regis.Models
{
    public class AlumniListItemModel
    {
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string CourseName { get; set; }
        public string Session { get; set; }
        public string Company { get; set; }
        public string Designation { get; set; }
    }

    public class AlumniInfoModel
    {
        public string StudentId { get; set; }
        public string Company { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string LinkedInUrl { get; set; }
    }
}
