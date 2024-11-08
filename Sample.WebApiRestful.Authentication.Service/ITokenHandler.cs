using Microsoft.AspNetCore.Authentication.JwtBearer;
using Sample.WebApiRestful.Domain.Entities;
using Sample.WebApiRestful.Model;

namespace Sample.WebApiRestful.Authentication.Service
{
    public interface ITokenHandler
    {
        Task<(string, DateTime)> CreateAccessToken(User user);
        Task<(string, string, DateTime)> CreateRefreshToken(User user);
        Task<JwtModel> ValidateRefreshToken(string refreshToken);
        Task ValidateToken(TokenValidatedContext context);
    }
}