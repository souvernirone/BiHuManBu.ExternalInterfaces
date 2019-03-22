using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IAuthorityService
    {
        /// <summary>
        /// 判断是否是管理员
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        bool IsAdmin(int agentId);
    }
}
