using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regis.Models
{
    public class FeeHeadModel
    {
        public int FeeHeadId { get; set; }
        public string FeeHeadCode { get; set; }
        public string FeeHeadName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
