using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class SemesterMasterModel
    {
        public int SemesterId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int AcademicSessionId { get; set; }
        public string SessionName { get; set; }
        public int SemesterNumber { get; set; }
        public string SemesterName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CreditLimit { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        [NotMapped] // agar EF hai; warna sirf normal property
        public string AcademicSessionName { get; set; }   // form se aayega, DB column nahi hai
       
    }
}