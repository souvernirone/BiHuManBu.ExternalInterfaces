using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface ISubmitInfoRepository
    {
        bx_submit_info GetSubmitInfo(long buid, int source);

        /// <summary>
        /// 获取核保列表
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        List<bx_submit_info> GetSubmitInfoList(long buid);
        bool HasSubmitInfo(long buid);
    }
}
