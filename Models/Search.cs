using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Search
    {
        public int Id { get; set; }
        public int Duration { get; set; }
        public DateTime JobAvailableDate { get; set; }
        public string JobType { get; set; }
        public string Location { get; set; }
        public bool IsAccomodation { get; set; }
        public int NoOfPositions { get; set; }
        public string CompanyName { get; set; }
        public string ContactNo { get; set; }
        public string DayPayment { get; set; }
        

    }
}
