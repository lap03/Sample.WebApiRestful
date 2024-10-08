using Sample.WebApiRestful.Data.Abstract;
using Sample.WebApiRestful.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApiRestful.Service
{
    public class UserService : IUserService
    {
        IRepository<User> _userRepository;
        public UserService(IRepository<User> userRepository) { _userRepository = userRepository; }

        public async Task<User> CheckLogin(string username, string password)
        {
            return await _userRepository.GetSingleByConditionAsynce(x => x.UserName.Equals(username) && x.Password.Equals(password));
        }

        public async Task<User> FindUsername(string username)
        {
            return await _userRepository.GetSingleByConditionAsynce(x => x.UserName == username);
        }
    }
}
