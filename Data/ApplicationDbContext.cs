using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static  class ApplicationDbContext
    {
        public static  string GetConnection(IConfiguration configuration)
        {
            return configuration.GetSection("ConnectionStrings").GetSection("ProductContext").Value;
        }
    }
}
