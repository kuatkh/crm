using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigProject.Auth.Models
{
    public class RegistrationResult
    {
        public RegistrationResult()
        {
            IsRegistered = false;
            Message = string.Empty;
            Roles = string.Empty;
        }

        public RegistrationResult(bool isRegistered, string message, string roles)
        {
            IsRegistered = isRegistered;
            Message = message;
            Roles = roles;
        }

        public RegistrationResult(string message, string roles)
        {
            Message = message;
            Roles = roles;
        }

        public RegistrationResult(string message)
        {
            Message = message;
        }

        public bool IsRegistered { get; set; } = false;
        public string Message { get; set; }
        public string Roles { get; set; }
    }
}
