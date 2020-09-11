using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigProject.Auth.Configuration
{
    public class BpAuthConfiguration
    {
        public int TokenLifeTimeInSec { get; set; }
        public ICollection<BpAuthClientConfiguration> Clients { get; set; }
        public string CertPath { get; set; }
        public string CertSecret { get; set; }
        public string ConnectionString { get; set; }
    }
}
