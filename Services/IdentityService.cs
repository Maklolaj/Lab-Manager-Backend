using LabManAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using LabManAPI.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using LabManAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace LabManAPI.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettigns;
        private readonly TokenValidationParameters _tokenValidationParametrs;
        private readonly ApplicationDbContext _dataContext;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettigns
            ,TokenValidationParameters tokenValidationParameters, ApplicationDbContext dataContext)
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

    
        private AuthenticationResult GenerateAuthenticationResultForUserAsync(IdentityUser user)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = ascii.GetBytes(_jwtSettigns.Secret);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id)  //Custom claim
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


    }
}