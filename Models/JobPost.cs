using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class JobPost
    {
        public int Id { get; set; }
        public int Duration { get; set; }
        public DateTime? JobAvailableDate { get; set; }
        public int JobTypeID { get; set; } //Will insert job type in db.
        public int LocationID { get; set; }
        public int UserID { get; set; }
        public bool IsAccomodation { get; set; }
        public int NoOfPositions { get; set; }
        public int CompanyId { get; set; }
    }
}
