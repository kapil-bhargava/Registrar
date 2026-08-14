using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regis.Models
{
    public class StudentRecordModel
    {
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string CourseName { get; set; }
        public string Category { get; set; }
        public string Session { get; set; }
        public string SeatNumber { get; set; }
        public DateTime AdmittedOn { get; set; }
        public string Status { get; set; }
    }
}
