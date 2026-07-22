using System;

namespace Regis.Models
{
    public class FacultyModel
    {
        public int FacultyId { get; set; }

        // EmployeeMaster se dropdown
        public int EmployeeId { get; set; }
        public string FacultyName { get; set; }

        // DepartmentMaster se dropdown
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        // BranchMaster se dropdown
        public int BranchId { get; set; }
        public string BranchName { get; set; }

        // SemesterMaster se dropdown
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }

        // DesignationMaster se dropdown
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }

        // Active / Inactive/
        public string Status { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}