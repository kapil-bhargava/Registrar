using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regis.Models
{
    public class CertificateIssuedModel
    {
        public int CertificateId { get; set; }
        public string CertNo { get; set; }
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string CertificateType { get; set; }
        public string Purpose { get; set; }
        public DateTime IssuedOn { get; set; }
    }

}
