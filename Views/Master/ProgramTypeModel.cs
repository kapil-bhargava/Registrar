using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Views.Master
{
    public class ProgramTypeModel
    {
        public int ProgramTypeId { get; set; }
        public string ProgramTypeCode { get; set; }
        public string ProgramTypeName { get; set; }
        public string TypicalDuration { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}