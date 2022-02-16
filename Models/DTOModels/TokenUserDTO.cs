using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class TokenUserDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? RefreshToken { get; set; }
    }
}
