using Microsoft.AspNetCore.Authentication.JwtBearer;
using Sample.WebApiRestful.Domain.Entities;

namespace Sample.WebApiRestful.Authentication.Service
{
    public interface ITokenHandler
    {
        Task<string> CreateToken(User user);
        Task ValidateToken(TokenValidatedContext context);
    }
}