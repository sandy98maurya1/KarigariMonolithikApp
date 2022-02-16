using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public partial class AppSettings
    {
        public string Secret { get; set; } = "ThisIsKarigariServiceSecret";
        public string Issuer { get; set; } = "https://localhost:44378/";
        public string Audiance { get; set; } = "PostmanClient";

    }
}
