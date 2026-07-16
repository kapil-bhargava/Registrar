using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class CampusModel
    {
        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public string CampusCode { get; set; }
        public int? CampusTypeId { get; set; }
        public string CampusTypeName { get; set; }   // filled by JOIN, not from form
        public int? Capacity { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Dean { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}