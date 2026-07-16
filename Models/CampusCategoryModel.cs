using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class CampusCategoryModel
    {
        public int CampusCategoryId { get; set; }
        public string CampusCategoryCode { get; set; }
        public string CampusCategoryName { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}