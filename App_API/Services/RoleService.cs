using App_API.Dtos.Roles;
using App_API.Exceptions;
using App_API.Models;
using App_API.Repositories;
using AutoMapper;
using System.Net;

namespace App_API.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository= roleRepository;
            _mapper= mapper;
        }
        public void ChekRoleExist(int roleId)
        {
            var isRoleExist = _roleRepository.IsRoleExist(roleId);
            if (!isRoleExist)
                throw new MyException((int)HttpStatusCode.BadRequest, $"RoleService:: Can not find role has id: {roleId}");
        }

        public List<RoleResponse> GetRoles()
        {
            var roles = _roleRepository.GetRoles();

            if(roles == null || roles.Count == 0)
            {
                throw new MyException((int)HttpStatusCode.NotFound, "RoleService:: Can not find any role!");
            }

            var roleResponses = _mapper.Map<List<RoleResponse>>(roles);

            return roleResponses;
        }
    }
}
