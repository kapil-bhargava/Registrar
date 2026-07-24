using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class SeatMatrixModel
    {

        public int SeatMatrixId { get; set; }

        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }

        public int AcademicSessionId { get; set; }
        public string SessionName { get; set; }

        public int TotalSeats { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}