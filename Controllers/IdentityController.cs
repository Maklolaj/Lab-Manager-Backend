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
        [HttpPut(ApiRoutes.Identity.ChangePassword)]
        public async Task<IActionResult> ChangeUserPassword([FromBody] UserChangePasswordInfo request)
        {
            if (!InitialValidation().Result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Invalid request");
            }

            var currentUser = await _identityService.GetIdentityUserFromJWT(await HttpContext.GetTokenAsync("access_token"));
            var userUpdated = await _identityService.ChangePasswordAsync(request, currentUser);

            if (userUpdated.Errors != null && userUpdated.Errors.Any())
            {
                return BadRequest(userUpdated.Errors);
            }

            return Ok(userUpdated.Messages);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut(ApiRoutes.Identity.ChangeEmail)]
        public async Task<IActionResult> ChangeUserEmail([FromBody] UserChangeEmailInfo request)
        {
            if (!InitialValidation().Result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Invalid request");
            }

            var currentUser = await _identityService.GetIdentityUserFromJWT(await HttpContext.GetTokenAsync("access_token"));
            var userUpdated = await _identityService.ChangeEmailRequestAsync(request, currentUser);

            if (userUpdated.Errors != null && userUpdated.Errors.Any())
            {
                return BadRequest(userUpdated.Errors);
            }

            return Ok(userUpdated.Messages); //TODO: Send token confirmation via email.

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut(ApiRoutes.Identity.ConfirmChangeEmail)]
        public async Task<IActionResult> ConfirmChangeUserEmail([FromBody] UserChangeEmailInfo request)
        {
            if (!InitialValidation().Result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Invalid request");
            }

            var currentUser = await _identityService.GetIdentityUserFromJWT(await HttpContext.GetTokenAsync("access_token"));

            var userUpdated = await _identityService.ConfirmChangeEmailRequestAsync(request, currentUser);

            if (userUpdated.Errors != null && userUpdated.Errors.Any())
            {
                return BadRequest(userUpdated.Errors);
            }

            return Ok(userUpdated.Messages);

        }

        public async Task<bool> InitialValidation()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var currentUser = await _identityService.GetIdentityUserFromJWT(accessToken);
            return currentUser != null && ModelState.IsValid;
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