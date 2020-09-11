using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Auth.Responses
{
    public class ValidationResponse
    {
        public bool Valid { get; set; }
        public List<CookieResponse> CookieList { get; set; }
    }

    public class CookieResponse
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public string Expires { get; set; }
        public bool HTTP { get; set; }
        public bool Secure { get; set; }
    }
}
