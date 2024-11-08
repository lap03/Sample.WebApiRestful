﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sample.WebApiRestful.Domain.Entities;
using Sample.WebApiRestful.Model;
using Sample.WebApiRestful.Service.Abstract;
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
        IUserTokenService _userTokenService;
        public TokenHandler(IConfiguration configuration, IUserService userService, IUserTokenService userTokenService)
        {
            _configuration = configuration;
            _userService = userService;
            _userTokenService = userTokenService;
        }

        public async Task<(string, DateTime)> CreateAccessToken(User user)
        {
            DateTime expiredDate = DateTime.Now.AddMinutes(15);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), _configuration["TokenBear:Issuer"]), // key của claim
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _configuration["TokenBear:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["TokenBear:Issuer"], ClaimValueTypes.String, _configuration["TokenBear:Issuer"]), // issuer
                new Claim(JwtRegisteredClaimNames.Aud, _configuration["TokenBear:Audience"], ClaimValueTypes.String, _configuration["TokenBear:Issuer"]), // Audience
                new Claim(JwtRegisteredClaimNames.Exp, expiredDate.ToString("yyyy/MM/dd hh:mm:ss"), ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String, ""), ko nen de id len
                new Claim(ClaimTypes.Name, user.DisplayName, ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                new Claim("Username", user.UserName, ClaimValueTypes.String, _configuration["TokenBear:Issuer"])
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenBear:Key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenInfo = new JwtSecurityToken(
                    issuer: _configuration["TokenBear:Issuer"],
                    audience: _configuration["TokenBear:Audience"],
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: expiredDate,
                    signingCredentials: credential
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenInfo);

            return await Task.FromResult((token, expiredDate));
        }

        public async Task<(string, string, DateTime)> CreateRefreshToken(User user)
        {
            DateTime expiredDate = DateTime.Now.AddHours(3);
            string code = Guid.NewGuid().ToString();

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), _configuration["TokenBear:Issuer"]), // key của claim
                //new Claim(JwtRegisteredClaimNames.Iss, _configuration["TokenBear:Issuer"], ClaimValueTypes.String, _configuration["TokenBear:Issuer"]), // issuer
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.DateTime, _configuration["TokenBear:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Exp, expiredDate.ToString("yyyy/MM/dd hh:mm:ss"), ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                new Claim(ClaimTypes.SerialNumber, code, ClaimValueTypes.String, _configuration["TokenBear:Issuer"])
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenBear:Key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenInfo = new JwtSecurityToken(
                    issuer: _configuration["TokenBear:Issuer"],
                    audience: _configuration["TokenBear:Audience"],
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: expiredDate,
                    signingCredentials: credential
                );

            string refreshToken = new JwtSecurityTokenHandler().WriteToken(tokenInfo);

            return await Task.FromResult((code, refreshToken, expiredDate));
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

            if (identity.FindFirst("Username") != null)
            {
                string username = identity.FindFirst("Username").Value;

                User user = await _userService.FindUserByName(username);

                if (user == null)
                {
                    context.Fail($"Invalid username: {username}");
                    return;
                }
            }

            if (identity.FindFirst(JwtRegisteredClaimNames.Exp) != null)
            {
                var dateExp = identity.FindFirst(JwtRegisteredClaimNames.Exp).Value;

                long ticks = long.Parse(dateExp);
                var date = DateTimeOffset.FromUnixTimeSeconds(ticks).DateTime;

                var minutes = date.Subtract(DateTime.UtcNow).TotalMinutes;

                if (minutes < 0)
                {
                    context.Fail("This token is expired.");

                    throw new Exception("This token is expired.");
                    return;
                }
            }
        }

        public async Task<JwtModel> ValidateRefreshToken(string refreshToken)
        {
            var claimPriciple = new JwtSecurityTokenHandler().ValidateToken(
                refreshToken,
                new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenBear:Key"])),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                },
                out _
            );

            if (claimPriciple == null) return new();

            string serialNumber = claimPriciple.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value;

            if (serialNumber == null) return new();

            UserToken userToken = await _userTokenService.checkCodeRefreshToken(serialNumber);

            if (userToken != null)
            {
                User user = await _userService.FindUserById(userToken.UserId);

                (string newAccessToken, DateTime expiredDateAccess) = await CreateAccessToken(user);
                (string codeRefreshToken, string newRefreshToken, DateTime expiredDateRefresh) = await CreateRefreshToken(user);

                //modify userToken
                userToken.AccessToken = newAccessToken;
                userToken.RefreshToken = newRefreshToken;
                userToken.CodeRefreshToken = codeRefreshToken;
                userToken.ExpiredDateAccessToken = expiredDateAccess;
                userToken.ExpiredDateRefreshToken = expiredDateRefresh;
                userToken.CreateDate = DateTime.Now;

                await _userTokenService.modifyToken(userToken);

                return new JwtModel
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    FullName = user.DisplayName,
                    UserName = user.UserName,
                };
            }

            return new();
        }


    }
}
