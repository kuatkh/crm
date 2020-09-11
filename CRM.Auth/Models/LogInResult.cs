using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigProject.Auth.Models
{
    public class LogInResult
    {
        public LogInResult()
        {
            IsLoggedIn = false;
            Message = string.Empty;
            Roles = string.Empty;
        }

        public LogInResult(bool isLoggedIn, string message, string roles)
        {
            IsLoggedIn = isLoggedIn;
            Message = message;
            Roles = roles;
        }

        public LogInResult(string message, string roles)
        {
            Message = message;
            Roles = roles;
        }

        public LogInResult(string message)
        {
            Message = message;
        }

        public bool IsLoggedIn { get; set; } = false;
        public string Message { get; set; }
        public string Roles { get; set; }
    }
}
