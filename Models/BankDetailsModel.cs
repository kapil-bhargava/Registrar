using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Models/BankDetailsModel.cs
// Models/BankDetailsModel.cs
namespace Regis.Models
{
    public class BankDetailsModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationNo { get; set; }

        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string PANNumber { get; set; }
    }
}