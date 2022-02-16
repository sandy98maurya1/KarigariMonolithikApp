using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class TokenUser
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public string? Device { get; set; }
    }
}
