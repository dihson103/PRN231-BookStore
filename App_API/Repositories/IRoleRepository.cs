using App_API.Models;

namespace App_API.Repositories
{
    public interface IRoleRepository
    {
        bool IsRoleExist(int roleId);
        List<Role> GetRoles();
    }
}
