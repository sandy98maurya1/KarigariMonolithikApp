using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class JobApply
    {
        public long UserId { get; set; }
        public long CompanyId { get; set; }
        public int JobAppliedFor_JobId { get; set; }
        public DateTime AppliedDate { get; set; }

    }
}
