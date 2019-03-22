using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class AuthorityService : IAuthorityService
    {
        /// <summary>
        /// 系统默认角色
        /// </summary>
        private static int[] SystemRoleType = { 0, 3, 4, 5 };
        private readonly IManagerRoleRepository _managerRoleRepository;

        public AuthorityService(
            IManagerRoleRepository managerRoleRepository)
        {
            _managerRoleRepository = managerRoleRepository;
        }


        public bool IsAdmin(int agentId)
        {
            var result = false;
            var roleTyoe = _managerRoleRepository.GetRoleTypeByAgentId(agentId);
            if (roleTyoe == 3 || roleTyoe == 4)
                result = true;
            return result;
        }
    }
}
