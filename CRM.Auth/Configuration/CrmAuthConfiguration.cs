using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Auth.Configuration
{
    public class CrmAuthConfiguration
    {
        public int TokenLifeTimeInSec { get; set; }
        public ICollection<CrmAuthClientConfiguration> Clients { get; set; }
        public string CertPath { get; set; }
        public string CertSecret { get; set; }
        public string ConnectionString { get; set; }
    }
}
