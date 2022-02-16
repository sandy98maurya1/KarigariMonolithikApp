using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Company
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyType { get; set; }
        public string CompanyOwner { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNo { get; set; }
        public string CompanyEmail { get; set; }
        public string Password { get; set; }
        public string? BusinessContactNo { get; set; }
        public Address Address { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public bool disabled { get; set; }

    }
}
