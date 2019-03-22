using System;
using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public interface IOrderRepository
    {
        Int64 Add(bx_car_order order);
        int Update(bx_car_order order);
        CarOrderModel GetOrderByBuid(int buid, int topAgentId);
        bx_car_order GetOrderByBuidAgent(long buid, int topAgentId);
        /// <summary>
        /// 根据buid查订单状态
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        CarOrderStatusModel GetOrderStatus(long buid);
        bx_car_order FindBy(long orderId);
        bx_car_order FindBy(long orderId, int topagent);
        bx_car_order FindBy(string orderNo);
        bx_car_order FindBy(long orderId, string openId);
        bx_car_order FindBy(string licenseNo, string openId, int topAgent);
        /// <summary>
        /// 根据车牌号、当前代理查订单，2017228新增
        /// </summary>
        /// <param name="licenseNo"></param>
        /// <param name="curAgent"></param>
        /// <returns></returns>
        bx_car_order FindOrder(string licenseNo, int curAgent);

        List<CarOrderModel> FindListBy(int status,bool isAgent, List<bx_agent> sonself, string openid, int agentId, string search, int pageIndex, int pageSize, out int totalCount);
        //List<bx_car_order> FindListBy(long user_id, int top_agent);
        //List<bx_car_order> FindByUserId(long user_id, int top_agent);
        CarOrderModel FindCarOrderBy(long orderId, int topagent);//, string openId

        long CreateOrder(bx_car_order order, bx_address address, bx_lastinfo lastinfo, bx_userinfo userinfo, bx_savequote savequote, bx_submit_info submitInfo, bx_quoteresult quoteresult, bx_quoteresult_carinfo carInfo, List<bx_claim_detail> claimDetails);

        /// <summary>
        /// 获取我的预约单总数
        /// </summary>
        /// <param name="isAgent"></param>
        /// <param name="sonself"></param>
        /// <param name="strPass"></param>
        /// <param name="agent">顶级代理</param>
        /// <returns></returns>
        int CountYuYue(bool isAgent, List<bx_agent> sonself, string strPass, int agent);

        /// <summary>
        /// 获取我的已出单总数
        /// </summary>
        /// <param name="isAgent"></param>
        /// <param name="sonself"></param>
        /// <param name="strPass"></param>
        /// <param name="agent">顶级代理</param>
        /// <returns></returns>
        int CountChuDan(bool isAgent, List<bx_agent> sonself, string strPass, int agent);

        bx_order_quoteresult GetQuoteResult(long orderId);
        bx_order_submit_info GetSubmitInfo(long orderId);
    }
}
