using Sample.WebApiRestful.Domain.Entities;

namespace Sample.WebApiRestful.Service.Abstract
{
    public interface IUserService
    {
        Task<User> CheckLogin(string username, string password);
        Task<User> FindUserById(int userId);
        Task<User> FindUserByName(string username);
    }
}