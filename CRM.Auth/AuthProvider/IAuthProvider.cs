using CRM.Auth.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Auth.AuthProvider
{
    public interface IAuthProvider
    {
        /// <summary>
        /// Validate user credentials
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>role if username/password valid</returns>
        Task<LogInResult> Login(string username, string password);

        /// <summary>
        /// Creats new user
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>true if user created</returns>
        Task<RegistrationResult> Register(string username, string password, string role, string email, string departmentName = null, string positionName = null);

        /// <summary>
        /// Changes user password
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="oldpassword">oldpassword</param>
        /// <param name="newpassword">newpassword</param>
        /// <returns>true if user's password changed</returns>
        Task<IdentityResult> ChangePassword(string username, string oldpassword, string newpassword);

        /// <summary>
        /// Wipes user password
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>true if user's password wiped</returns>
        Task<IdentityResult> WipePassword(string username);

        /// <summary>
        /// logout
        /// </summary>
        /// <returns></returns>
        Task<LogInResult> Logout();

    }
}
