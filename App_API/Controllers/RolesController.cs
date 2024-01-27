using App_API.Dtos.Common;
using App_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var roles = _roleService.GetRoles();
            return Ok(new SuccessResponse() { Message = "Get all roles success!", Data = roles });
        }
    }
}
