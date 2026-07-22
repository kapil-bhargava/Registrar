using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public string FatherName { get; set; }
        public string Qualification { get; set; }
        public string Experience { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}