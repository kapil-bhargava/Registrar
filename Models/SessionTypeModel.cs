using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class SessionTypeModel
    {
        public int SessionTypeId { get; set; }
        public string SessionTypeCode { get; set; }
        public string SessionTypeName { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}