using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Auth.IdentityProvider
{
    public class IdentityProvider : IIdentityProvider
    {
        public string CurrentName => "localhost";

        public bool TryGetCurrentName(out string name)
        {
            name = null;
            return true;
        }
    }
}
