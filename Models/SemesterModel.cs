using System;

namespace Regis.Models
{
    public class SemesterModel
    {
        public int SemesterId { get; set; }

        public int CourseId { get; set; }               // FK -> Course
        public string CourseName { get; set; }            // display only (joined)

        public int AcademicSessionId { get; set; }       // FK -> AcademicSession
        public string SessionName { get; set; }            // display only (joined)

        public int SemesterNumber { get; set; }
        public string SemesterName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CreditLimit { get; set; }
        public string Status { get; set; }               // Upcoming / Active / Completed
        public DateTime? CreatedDate { get; set; }
    }


}