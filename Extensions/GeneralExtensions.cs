using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using LabManAPI.Services;
using System;
using System.Threading.Tasks;

namespace LabManAPI.Extensions
{
    public class GeneralExtensions : IGeneralExtensions
    {
        private readonly IIdentityService _identityService;
        public GeneralExtensions(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<bool> isAdmin(string accessToken)
        {
            var currentUser = await _identityService.GetIdentityUserFromJWT(accessToken);

            if (currentUser != null && currentUser.PhoneNumber == "606")
            {
                return true;
            }
            return false;
        }
    }
}
