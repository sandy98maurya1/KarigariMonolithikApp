using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public partial class TokenResponse
    {
        public bool IsSuccess { get; set; }
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public int Expires_in { get; set; }
        public string UserName { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Expires { get; set; }
        public string Error { get; set; }
        public string Error_description { get; set; }
        public bool IsAccountLock { get; set; }
        public string AccessRights { get; set; }
        public string LoginId { get; set; }
        public bool IsPasswordTemporary { get; set; }
        public int ScreenTimeOutMins { get; set; }
        public int TimeOutMins { get; set; }
        public string RefreshToken { get; set; }
    }
}
