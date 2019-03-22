using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface ILastInfoRepository
    {
        bx_lastinfo GetByBuid(long buid);
        /// <summary>
        /// 根据buid获取上一年商业险和交强险到期时间
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        InsuranceEndDate GetEndDate(long buid);
        /// <summary>
        /// 根据buid获取上一年商业险和交强险到期时间、出险次数
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        InsuranceEndDateAndClaim GetEndDateAndClaim(long buid);
    }
}
