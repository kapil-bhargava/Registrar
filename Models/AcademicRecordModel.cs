using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Models/AcademicRecordModel.cs
namespace Regis.Models
{
    public class AcademicRecordModel
    {
        public int AcademicRecordId { get; set; }
        public int ApplicationId { get; set; }
        public string ExamPassed { get; set; }
        public string Board { get; set; }
        public string Institute { get; set; }
        public string Rank { get; set; }
        public string RollNumber { get; set; }
        public string PassingYear { get; set; }
        public string ResultType { get; set; }
        public string Percentage { get; set; }
        public string Stream { get; set; }
        public string EnrollmentNumber { get; set; }
        public string MarksObtained { get; set; }
        public string MarksOutOf { get; set; }
        public string Medium { get; set; }
        public string Mode { get; set; }
        public string GapYear { get; set; }
        public string GapReason { get; set; }
        public string ResultStatus { get; set; }
    }
}