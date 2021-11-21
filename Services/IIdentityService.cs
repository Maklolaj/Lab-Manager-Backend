
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabManAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace LabManAPI.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);

        Task<AuthenticationResult> LoginAsync(string email, string password);

        Task<IdentityUser> GetIdentityUserFromJWT(string jwt);

    }
}