using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sample.WebApiRestful.Domain.Entities;
using Sample.WebApiRestful.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApiRestful.Authentication.Service
{
    public class TokenHandler : ITokenHandler
    {
        IConfiguration _configuration;
        IUserService _userService;
        public TokenHandler(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<string> CreateToken(User user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), _configuration["TokenBear:Issuer"]), // key của claim
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["TokenBear:Issuer"], ClaimValueTypes.String, _configuration["TokenBear:Issuer"]), // issuer
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToString(), ClaimValueTypes.Integer64, _configuration["TokenBear:Issuer"]), // tg tạo token
                new Claim(JwtRegisteredClaimNames.Aud, "WebApiRestful - .Netchannel", ClaimValueTypes.String, _configuration["TokenBear:Issuer"]), // Audience
                new Claim(JwtRegisteredClaimNames.Exp, DateTime.Now.AddHours(3).ToString("yyyy/MM/dd hh/mm/ss"), ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String, ""), ko nen de id len
                new Claim(ClaimTypes.Name, user.DisplayName, ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                new Claim("Username", user.UserName, ClaimValueTypes.String, _configuration["TokenBear:Issuer"])
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenBear:SignatureKey"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenInfo = new JwtSecurityToken(
                    issuer: _configuration["TokenBear:Issuer"],
                    audience: _configuration["TokenBear:Audience"],
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(3),
                    credential
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenInfo);

            return await Task.FromResult(token);
        }

        public async Task ValidateToken(TokenValidatedContext context)
        {
            var claims = context.Principal.Claims.ToList();

            if (claims.Count == 0)
            {
                context.Fail("This token contains no information");
                return;
            }

            var identity = context.Principal.Identity as ClaimsIdentity;

            if (identity.FindFirst(JwtRegisteredClaimNames.Iss) == null)
            {
                context.Fail("This token is not issued by point entry");
                return;
            }

            if(identity.FindFirst("Username") != null)
            {
                string username = identity.FindFirst("Username").Value;

                User user = await _userService.FindUsername(username);

                if (user == null)
                {
                    context.Fail($"Invalid username: {username}");
                    return;
                }
            }

            if(identity.FindFirst(JwtRegisteredClaimNames.Exp) == null)
            {
                var dateExp = identity.FindFirst(JwtRegisteredClaimNames.Exp).Value;

                long ticks = long.Parse(dateExp);
                var date = DateTimeOffset.FromUnixTimeSeconds(ticks).DateTime;

                var minutes = date.Subtract(DateTime.Now).TotalMinutes;

                if (minutes < 0)
                {
                    context.Fail("This token is expired.");

                    throw new Exception("This token is expired.");
                    return;
                }
            }
        }
    }
}
