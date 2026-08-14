using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Models/AdditionalDetailModel.cs
namespace Regis.Models
{
    public class AdditionalDetailModel
    {
        public int AdditionalDetailId { get; set; }
        public int ApplicationId { get; set; }
        public string Level { get; set; }
        public string ParticipationLevel { get; set; }
        public string Category { get; set; }
        public string AwardingInstitution { get; set; }
        public string AwardName { get; set; }
        public string ReceivedWhen { get; set; }
        public string Reason { get; set; }
    }
}