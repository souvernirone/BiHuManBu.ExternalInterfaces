using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IQuoteResultRepository
    {
        bx_quoteresult GetQuoteResultByBuid(long buid, int source);
        bx_quoteresult GetQuoteResultByBuid(long buid);

        /// <summary>
        /// 获取报价返回信息列表
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        List<bx_quoteresult> GetQuoteResultList(long buid);

        /// <summary>
        /// 根据buid获取商业险和交强险开始时间
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        InsuranceStartDate GetStartDate(long buid);
        /// <summary>
        /// 修改行信息
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        int Update(bx_quoteresult item);
    }
}
