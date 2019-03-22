using BiHuManBu.ExternalInterfaces.Models;
using System;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces
{
    public interface IGetDateNewService
    {
        /// <summary>
        /// 获取商业险起保时间和交强险起保时间
        /// </summary>
        /// <param name="listquoteresult"></param>
        /// <returns>1商业2交强</returns>
        Tuple<string, string> GetDate(List<bx_quoteresult> listquoteresult);
    }
}
