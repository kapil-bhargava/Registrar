using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class SubjectCategoryModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryType { get; set; }

        public bool CreditApplicable { get; set; }

        public bool MarksApplicable { get; set; }

        public bool PassingMarksRequired { get; set; }

        public bool IsActive { get; set; }

        public int? DisplayOrder { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}