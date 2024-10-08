using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.WebApiRestful.Authentication.Service;
using Sample.WebApiRestful.Model;
using Sample.WebApiRestful.Service;

namespace Sample.WebApiRestful.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        IUserService _userService;
        ITokenHandler _tokenHandler;

        public AuthenticationController(IUserService userService, ITokenHandler tokenHandler)
        {
            _userService = userService;
            _tokenHandler = tokenHandler;
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
            return  await Task.Factory.StartNew(() =>
            {
                return Ok(_tokenHandler.CreateToken(user));
            });
        }
    }
}
