using IdentityModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Auth.IdentityProvider
{
    public class WindowsIdentityProvider : IIdentityProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public WindowsIdentityProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public bool TryGetCurrentName(out string name)
        {
            name = null;
            if (!_contextAccessor.HttpContext.User.Identities.Any(i => i.IsAuthenticated)) return false;
            name = _contextAccessor.HttpContext.User.FindFirst(JwtClaimTypes.Subject).Value;
            return true;
        }

        public string CurrentName => "localhost";
    }
}
