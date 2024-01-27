using App_API.Dtos.Roles;
using App_API.Models;

namespace App_API.Services
{
    public interface IRoleService
    {
        void ChekRoleExist(int roleId);
        List<RoleResponse> GetRoles();
    }
}
