using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigProject.Auth.Common
{
    public class Utilities
    {
        public static string TrimDomain(string username)
        {
            if (username.Contains("@"))
            {
                username = username.Split('@')[0];
            }
            if (username.Contains(@"\") && username.Split(@"\").Length > 1)
            {
                username = username.Split(@"\")[1];
            }

            return username;
        }
    }
}
