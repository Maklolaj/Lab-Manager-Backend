using LabManAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using LabManAPI.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using LabManAPI.Data;
using LabManAPI.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using LabManAPI.Contracts.Requests;

namespace LabManAPI.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettigns;
        private readonly TokenValidationParameters _tokenValidationParametrs;
        private readonly ApplicationDbContext _dataContext;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettigns
            , TokenValidationParameters tokenValidationParameters, ApplicationDbContext dataContext)
        {
            _userManager = userManager;
            _jwtSettigns = jwtSettigns;
            _tokenValidationParametrs = tokenValidationParameters;
            _dataContext = dataContext;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist" }
                };
            }

            var id = await _userManager.GetUserIdAsync(user);

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "login/password combiantion is invalid" }
                };
            }

            return GenerateAuthenticationResultForUserAsync(user);

        }


        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existtingUser = await _userManager.FindByEmailAsync(email);

            if (existtingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this email adress already exists" }
                };
            }

            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            return GenerateAuthenticationResultForUserAsync(newUser);

        }

        public async Task<UpdateUserResponse> UpdateUserAsync(UserUpdateProfileInfo request, IdentityUser user)
        {
            List<string> messages = new List<string>();
            List<string> errors = new List<string>();

            if (!request.Email.Equals(user.Email))
            {
                if (await _userManager.FindByEmailAsync(request.Email) == null)
                {
                    var token = await _userManager.GenerateChangeEmailTokenAsync(user, request.Email);
                    await _userManager.ChangeEmailAsync(user, request.Email, token);
                    messages.Add("Email has changed");
                }
                else
                {
                    errors.Add("User with given email already exists");
                }
            }

            if (request.Password.Equals(request.RepeatedPassword))
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, request.Password);
                messages.Add("Password has changed");
            }
            else
            {
                errors.Add("Passwords are not the same");
            }

            return new UpdateUserResponse
            {
                Messages = messages,
                Errors = errors,
            };
        }

        public async Task<IdentityUser> GetIdentityUserFromJWT(string accessToken)
        {
            //var accessToken = await HttpContext.GetTokenAsync("access_token");

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(accessToken);

            var userId = jwtSecurityToken.Claims.Where(c => c.Type == "id").Select(value => value.Value).SingleOrDefault();

            var user = await _userManager.FindByIdAsync(userId);

            return user;
        }


        private AuthenticationResult GenerateAuthenticationResultForUserAsync(IdentityUser user)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = ascii.GetBytes(_jwtSettigns.Secret);

            if (user.PhoneNumber == null)
            {
                user.PhoneNumber = "0";
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id), //Custom claim
                    new Claim(JwtRegisteredClaimNames.Prn, user.PhoneNumber)  //Custom claim
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);


            return new AuthenticationResult
            {
                Succes = true,
                Token = tokenHandler.WriteToken(token),
            };
        }


        public async Task<List<AllUsersResponse>> GetAllUsersAsync()
        {
            var allUsersResponse = new List<AllUsersResponse>();

            IQueryable<IdentityUser> users = _userManager.Users;

            await users.ForEachAsync((user) =>
            {
                allUsersResponse.Add(new AllUsersResponse
                {
                    userName = user.UserName,
                    userEmail = user.Email,
                    userPhoneNumber = user.PhoneNumber,
                });
            });
            return allUsersResponse;

        }


    }
}