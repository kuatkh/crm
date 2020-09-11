using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigProject.Auth.IdentityProvider
{
    public interface IIdentityProvider
    {
        bool TryGetCurrentName(out string name);
        string CurrentName { get; }
    }
}
