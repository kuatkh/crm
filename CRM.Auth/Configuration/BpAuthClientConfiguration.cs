using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigProject.Auth.Configuration
{
    public class BpAuthClientConfiguration
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public ICollection<string> Scopes { get; set; }
    }
}
