using System;

namespace Regis.Models
{
    public class UniversityTypeModel
    {
        public int UniversityTypeId { get; set; }
        public string UniversityTypeCode { get; set; }
        public string UniversityTypeName { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}