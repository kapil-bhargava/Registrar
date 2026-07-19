using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Views.Master
{
    public class Mastermodels
    {
        // =========================================================
        // FACULTY MASTER
        // =========================================================
       

        // =========================================================
        // DEPARTMENT MASTER
        // FacultyId -> FK FacultyMaster
        // =========================================================
        public class DepartmentMasterModel
        {
           
        }

        // =========================================================
        // PROGRAM MASTER
        // =========================================================
        public class ProgramMasterModel
        {
            public int ProgramId { get; set; }
            public string ProgramCode { get; set; }
            public string ProgramName { get; set; }
            public string ProgramType { get; set; }
            public string TypicalDuration { get; set; }
            public bool IsActive { get; set; }
            public DateTime? CreatedDate { get; set; }
        }

        // =========================================================
        // COURSE MASTER
        // ProgramId -> FK ProgramMaster, DepartmentId -> FK DepartmentMaster
        // =========================================================
        public class CourseMasterModel
        {
            public int CourseId { get; set; }
            public string CourseCode { get; set; }
            public string CourseName { get; set; }
            public int ProgramId { get; set; }
            public string ProgramName { get; set; }
            public int DepartmentId { get; set; }
            public string DepartmentName { get; set; }
            public int? DurationYears { get; set; }
            public int? TotalSemesters { get; set; }
            public int? TotalCredits { get; set; }
            public int? IntakeCapacity { get; set; }
            public string Status { get; set; }
            public DateTime? CreatedDate { get; set; }
        }

        // =========================================================
        // SEMESTER MASTER
        // CourseId -> FK CourseMaster, AcademicSessionId -> FK AcademicSession
        // =========================================================
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
        }

        // =========================================================
        // SUBJECT MASTER
        // SemesterId -> FK SemesterMaster, CourseId -> FK CourseMaster
        // =========================================================
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
}