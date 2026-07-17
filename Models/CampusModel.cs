using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class CampusModel
    {
        public int CampusId { get; set; }

        [Required(ErrorMessage = "Campus Name is required")]
        public string CampusName { get; set; }

        [Required(ErrorMessage = "Campus Code is required")]
        public string CampusCode { get; set; }

        public int? CampusTypeId { get; set; }
        public string CampusTypeName { get; set; }   // filled by JOIN, not from form

        public int? Capacity { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }

        public string Dean { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}