using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public decimal? FeeConcession { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}