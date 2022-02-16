using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class JobProfile:ApiResponse
    {
        public int Duration { get; set; }

        public DateTime AvailableDate;
        public int JobTypeID { get; set; }
        public int LocationID { get; set; }
        public int UserID { get; set; }
    }
}
