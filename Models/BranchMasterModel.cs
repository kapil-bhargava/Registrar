using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class BranchMasterModel
    {
        public int BranchId { get; set; }

        public string BranchCode { get; set; }

        public string BranchName { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int ProgramId { get; set; }

        public string ProgramName { get; set; }

        public string CampusName { get; set; }

        public int IntakeCapacity { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}