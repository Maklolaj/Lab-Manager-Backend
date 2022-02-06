
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabManAPI.Models;
using Microsoft.AspNetCore.Identity;
using LabManAPI.Contracts.Responses;
using LabManAPI.Contracts.Requests;

namespace LabManAPI.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);

        Task<AuthenticationResult> LoginAsync(string email, string password);

        Task<UpdateUserResponse> ChangePasswordAsync(UserChangePasswordInfo request, IdentityUser user);

        Task<UpdateUserResponse> ChangeEmailRequestAsync(UserChangeEmailInfo request, IdentityUser user);

        Task<UpdateUserResponse> ConfirmChangeEmailRequestAsync(UserChangeEmailInfo request, IdentityUser user);

        Task<IdentityUser> GetIdentityUserFromJWT(string jwt);

        Task<List<AllUsersResponse>> GetAllUsersAsync();

    }
}