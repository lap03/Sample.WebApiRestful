using Sample.WebApiRestful.Domain.Entities;

namespace Sample.WebApiRestful.Service
{
    public interface IUserService
    {
        Task<User> CheckLogin(string username, string password);
        Task<User> FindUsername(string username);
    }
}