using Sample.WebApiRestful.Domain.Entities;

namespace Sample.WebApiRestful.Service.Abstract
{
    public interface IUserTokenService
    {
        Task<UserToken> checkCodeRefreshToken(string code);
        Task modifyToken(UserToken userToken);
        Task SaveToken(UserToken userToken);
    }
}