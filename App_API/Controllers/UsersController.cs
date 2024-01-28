using App_API.Dtos.Common;
using App_API.Dtos.Users;
using App_API.Models;
using App_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var user = _userService.GetAllUser();
            return Ok(new SuccessResponse() { Message = "Get all users success!", Data = user });
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]int id)
        {
            var user = _userService.GetUserById(id);
            return Ok(new SuccessResponse() { Message = $"Get user has id: {id} success!", Data= user });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            _userService.DeleteUser(id);
            return Ok(new SuccessResponse() { Message = $"Delete user has id: {id} success!"});
        }

        [HttpPatch("{id}/change-user-role/{roleId}")]
        public IActionResult ChangeUserRole([FromRoute] int id, [FromRoute]int roleId)
        {
            _userService.ChangeUserRole(id, roleId);
            return Ok(new SuccessResponse() { Message = $"Update user's role has id: {id} success!" });
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody]UserAdminCreateRequest userAdminCreateRequest)
        {
            _userService.CreateUser(userAdminCreateRequest);
            return Ok(new SuccessResponse() { Message = "Create user success!" });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser([FromRoute]int id, [FromBody] UserUpdateProfile userUpdateProfile)
        {
            _userService.UpdateUser(id, userUpdateProfile);
            return Ok(new SuccessResponse() { Message = $"Update user has id: {id} success!" });
        }
    }
}
