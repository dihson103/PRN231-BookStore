using App_API.Models;

namespace App_API.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly Assignment2Prn231Context _context;
        public RoleRepository(Assignment2Prn231Context context)
        {
            _context = context;
        }
        public bool IsRoleExist(int roleId)
        {
            return _context.Roles.Any(x => x.RoleId == roleId);
        }
    }
}
