using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Regis.Models
{
    public class SeatCategoryMappingModel
    {
        public int SeatMatrixId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string SessionName { get; set; }
        public int TotalSeats { get; set; }

        public List<CategorySeatEntry> CategoryAllocations { get; set; } = new List<CategorySeatEntry>();
        public string CategoryIdsCsv { get; set; }
        public string SeatsCsv { get; set; }
    }
    public class CategorySeatEntry
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int AllocatedSeats { get; set; }
    }
}