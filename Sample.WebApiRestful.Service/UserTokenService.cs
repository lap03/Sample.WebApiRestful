using Sample.WebApiRestful.Data.Abstract;
using Sample.WebApiRestful.Data.Migrations;
using Sample.WebApiRestful.Domain.Entities;
using Sample.WebApiRestful.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApiRestful.Service
{
    public class UserTokenService : IUserTokenService
    {
        IRepository<UserToken> _userTokenRepository;

        public UserTokenService(IRepository<UserToken> userTokenRepository)
        {
            _userTokenRepository = userTokenRepository;
        }

        public async Task SaveToken(UserToken userToken)
        {
            if (userToken != null)
            {
                await _userTokenRepository.Insert(userToken);
                await _userTokenRepository.CommitAsync();
            }
        }

        public async Task modifyToken(UserToken userToken)
        {
            if (userToken != null)
            {
                _userTokenRepository.Update(userToken);
                await _userTokenRepository.CommitAsync();
            }
        }

        public async Task<UserToken> checkCodeRefreshToken (string code)
        {
            return await _userTokenRepository.GetSingleByConditionAsynce(x => x.CodeRefreshToken == code);
        }
    }
}
