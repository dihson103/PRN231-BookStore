using App_API.Exceptions;
using App_API.Repositories;
using System.Net;

namespace App_API.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository= roleRepository;
        }
        public void ChekRoleExist(int roleId)
        {
            var isRoleExist = _roleRepository.IsRoleExist(roleId);
            if (!isRoleExist)
                throw new MyException((int)HttpStatusCode.BadRequest, $"RoleService:: Can not find role has id: {roleId}");
        }
    }
}
