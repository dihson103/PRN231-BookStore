using App_API.Dtos.Common;
using App_API.Dtos.Users;
using App_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserAuthRequest userAuthRequest)
        {
            var userResponse = _userService.Login(userAuthRequest);
            return Ok(new SuccessResponse() { Message = "Login success!", Data = userResponse });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserAuthRequest userAuthRequest)
        {
            _userService.Registration(userAuthRequest);
            return Ok(new SuccessResponse() { Message = "Register user success!" });
        }

        [HttpPatch("{id}/change-password")]
        public IActionResult ChangePassword([FromRoute] int id, [FromBody] UserChangePasswordRequest changePasswordRequest)
        {
            _userService.ChangePassord(id, changePasswordRequest);
            return Ok(new SuccessResponse() { Message = $"Change password of user has id: {id} success!" });
        }
    }
}
