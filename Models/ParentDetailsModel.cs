using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Models/ParentDetailsModel.cs
// Models/ParentDetailsModel.cs
namespace Regis.Models
{
    public class ParentDetailsModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationNo { get; set; }

        // Father
        public string FatherTitle { get; set; }
        public string FatherFirstName { get; set; }
        public string FatherLastName { get; set; }
        public string FatherEmail { get; set; }
        public string FatherMobile { get; set; }
        public string FatherOccupation { get; set; }
        public string FatherOrganization { get; set; }
        public string FatherDesignation { get; set; }
        public decimal? FatherAnnualIncome { get; set; }

        // Mother
        public string MotherTitle { get; set; }
        public string MotherFirstName { get; set; }
        public string MotherLastName { get; set; }
        public string MotherEmail { get; set; }
        public string MotherMobile { get; set; }
        public string MotherOccupation { get; set; }
        public string MotherOrganization { get; set; }
        public string MotherDesignation { get; set; }
        public decimal? MotherAnnualIncome { get; set; }

        // Local guardian
        public string GuardianTitle { get; set; }
        public string GuardianFirstName { get; set; }
        public string GuardianLastName { get; set; }
        public string GuardianEmail { get; set; }
        public string GuardianMobile { get; set; }
        public string GuardianOccupation { get; set; }
        public string GuardianOrganization { get; set; }
        public string GuardianDesignation { get; set; }
        public decimal? GuardianAnnualIncome { get; set; }
        public string GuardianFamilyIncome { get; set; }
        public string GuardianRelationship { get; set; }
    }
}