using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class DocumentChecklistItemModel
    {
        public int DocumentEnclosureId { get; set; }
        public string DocumentName { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsSubmitted { get; set; }
    }
}