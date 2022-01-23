using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LabManAPI.Services;
using LabManAPI.Contracts;
using LabManAPI.Contracts.Requests;
using LabManAPI.Contracts.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace LabManAPI.Controllers
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;

        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid) //is email model state valid
            {
                return BadRequest(new AuthFailResponse
                {
                    Errors = ModelState.Values.SelectMany(x =>
                    x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);
            if (!authResponse.Succes)
            {
                return BadRequest(new AuthFailResponse
                {
                    Errors = authResponse.Errors
                });
            }
            return Ok(new AuthSuccesResponse
            {
                Token = authResponse.Token,
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut(ApiRoutes.Identity.UpdateUser)]
        public async Task<IActionResult> Update([FromBody] UserUpdateProfileInfo request)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var currentUser = await _identityService.GetIdentityUserFromJWT(accessToken);

            if (currentUser == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "User does not exist");
            }

            var userUpdated = await _identityService.UpdateUserAsync(request, currentUser);

            if (userUpdated.Errors != null && userUpdated.Errors.Any())
            {
                return BadRequest(userUpdated.Errors);
            }

            return Ok(userUpdated.Messages);
        }


        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);
            if (!authResponse.Succes)
            {
                return BadRequest(new AuthFailResponse
                {
                    Errors = authResponse.Errors
                });
            }
            return Ok(new AuthSuccesResponse
            {
                Token = authResponse.Token,
            });
        }

        [HttpGet(ApiRoutes.Identity.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _identityService.GetAllUsersAsync());
        }
    }
}