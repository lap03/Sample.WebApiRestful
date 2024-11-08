using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.WebApiRestful.Authentication.Service;
using Sample.WebApiRestful.Domain.Entities;
using Sample.WebApiRestful.Model;
using Sample.WebApiRestful.Service.Abstract;

namespace Sample.WebApiRestful.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        IUserService _userService;
        ITokenHandler _tokenHandler;
        IUserTokenService _userTokenService;

        public AuthenticationController(IUserService userService, ITokenHandler tokenHandler, IUserTokenService userTokenService)
        {
            _userService = userService;
            _tokenHandler = tokenHandler;
            _userTokenService = userTokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous] // http này ai cũng có thể gọi được
        public async Task<IActionResult> Login(AccountModel accountModel)
        {
            if (accountModel == null)
            {
                return BadRequest("user i snot exist");
            }

            var user = await _userService.CheckLogin(accountModel.Username,accountModel.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            //create token
            (string accessToken, DateTime expiredDateAccess) = await _tokenHandler.CreateAccessToken(user);
            (string codeRefreshToken, string  refreshToken, DateTime expiredDateRefresh) = await _tokenHandler.CreateRefreshToken(user);

            await _userTokenService.SaveToken(new UserToken
            {
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiredDateAccessToken = expiredDateAccess,
                ExpiredDateRefreshToken = expiredDateRefresh,
                CodeRefreshToken = codeRefreshToken,
                CreateDate = DateTime.Now,
                IsActive = true
            });

            return Ok(new JwtModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserName = user.UserName,
                FullName = user.DisplayName,
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel refreshTokenModel)
        {
            if (refreshTokenModel == null) return BadRequest("Could not get refresh token");

            return Ok( await _tokenHandler.ValidateRefreshToken(refreshTokenModel.RefreshToken));
        }

    }
}
