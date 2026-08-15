using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regis.Models
{
    public class FeeSummaryModel
    {
        public decimal TotalCollected { get; set; }
        public int PendingCount { get; set; }
        public decimal EstimatedPendingAmount { get; set; }
        public List<FeeMonthlyModel> MonthlyCollection { get; set; } = new List<FeeMonthlyModel>();
    }

    public class FeeMonthlyModel
    {
        public string MonthName { get; set; }
        public decimal Collected { get; set; }
    }
}
