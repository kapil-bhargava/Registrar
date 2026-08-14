using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regis.Models
{
    public class AddressInformationModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationNo { get; set; }

        public string PermanentAddress { get; set; }
        public string PermanentCountry { get; set; }
        public string PermanentState { get; set; }
        public string PermanentDistrict { get; set; }
        public string PermanentCity { get; set; }
        public string PermanentPinCode { get; set; }

        public bool LocalSameAsPermanent { get; set; }
        public string LocalAddress { get; set; }
        public string LocalCountry { get; set; }
        public string LocalState { get; set; }
        public string LocalDistrict { get; set; }
        public string LocalCity { get; set; }
        public string LocalPinCode { get; set; }
    }
}