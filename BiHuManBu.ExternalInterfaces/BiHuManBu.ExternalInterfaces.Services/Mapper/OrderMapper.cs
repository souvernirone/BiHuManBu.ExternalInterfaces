using System.Collections.Generic;
using System.Net;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Repository;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class OrderMapper
    {
        public static OrderDetail ConverToViewModel(this bx_car_order carOrder, int agent)
        {
            OrderDetail order = new OrderDetail();
            IAgentRepository _agent = new AgentRepository();
            if (carOrder != null)
            {
                order.OrderId = carOrder.id;
                order.InsuredName = carOrder.insured_name;
                order.LicenseNo = carOrder.LicenseNo;
                int s = carOrder.source.HasValue ? carOrder.source.Value : 0;
                order.Source = SourceGroupAlgorithm.GetNewSource(s);
                order.OrderStatus = carOrder.order_status.HasValue ? carOrder.order_status.Value : 0;
                order.CreateTime = carOrder.create_time.HasValue ? carOrder.create_time.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                int curAgent = carOrder.cur_agent.HasValue ? carOrder.cur_agent.Value : agent;
                order.Agent = curAgent;
                order.AgentName = _agent.GetAgentName(curAgent);
            }
            IOrderRepository _order = new OrderRepository();

            bx_order_submit_info orderSubmitInfo = new bx_order_submit_info();
            orderSubmitInfo = _order.GetSubmitInfo(carOrder.id);
            bx_order_quoteresult orderQuoteResult = new bx_order_quoteresult();
            orderQuoteResult = _order.GetQuoteResult(carOrder.id);
            if (orderSubmitInfo != null)
            {
                order.BizSysRate = orderSubmitInfo.biz_rate.HasValue ? orderSubmitInfo.biz_rate.Value : 0;
                order.ForceSysRate = orderSubmitInfo.force_rate.HasValue ? orderSubmitInfo.force_rate.Value : 0;
                order.BizTno = orderSubmitInfo.biz_tno;
                order.ForceTno = orderSubmitInfo.force_tno;
            }
            if (orderQuoteResult != null)
            {
                order.BizTotal = orderQuoteResult.BizTotal.HasValue ? orderQuoteResult.BizTotal.Value : 0;
                order.ForceTotal = orderQuoteResult.ForceTotal.HasValue ? orderQuoteResult.ForceTotal.Value : 0;
                order.TaxTotal = orderQuoteResult.TaxTotal.HasValue ? orderQuoteResult.TaxTotal.Value : 0;
            }
            return order;
        }

        public static OrderDetailResponse ConverToViewModel(this OrderCacheResponse orderModel)
        {
            OrderDetailResponse orderDetail=new OrderDetailResponse();
            #region 实例化

            //新实例start
            var carOrder = new CarOrder();
            var userInfo = new UserInfo();
            var claimDetails = new List<ClaimDetail>();
            var precisePrice = new PrecisePrice();
            //新实例end
            //原有实例start
            bx_car_order bxCarOrder = orderModel.BxCarOrder;
            bx_userinfo bxUserInfo = orderModel.BxUserInfo;
            bx_quoteresult bxQuoteResult = orderModel.BxQuoteResult;
            bx_savequote bxSaveQuote = orderModel.BxSaveQuote;
            bx_submit_info bxSubmitInfo = orderModel.BxSubmitInfo;
            bx_lastinfo bxLastInfo = orderModel.BxLastInfo;
            bx_quoteresult_carinfo bxCarInfo = orderModel.BxCarInfo;
            List<bx_claim_detail> bxClaimDetails = orderModel.BxClaimDetails;
            InsuranceStartDate qrStartDate = orderModel.QrStartDate;
            //原有实例end

            #endregion

            #region CarOrder转换

            if (bxCarOrder != null)
            {
                carOrder.AgentName = string.Empty;
                carOrder.AgentMobile = string.Empty;
                if (bxCarOrder.cur_agent.HasValue)
                {
                    carOrder.Agent = bxCarOrder.cur_agent.Value;
                    //调用代理人列表
                    IAgentRepository agentRepository=new AgentRepository();
                    var curAgent = agentRepository.GetAgent(bxCarOrder.cur_agent.Value);
                    if (curAgent != null)
                    {
                        carOrder.AgentName = curAgent.AgentName;
                        carOrder.AgentMobile = curAgent.Mobile;
                    }
                }
                carOrder.OpenId = bxCarOrder.openid;
                carOrder.Mobile = bxCarOrder.mobile;
                carOrder.ContactsName = bxCarOrder.contacts_name;
                carOrder.OrderStatus = bxCarOrder.order_status.HasValue ? bxCarOrder.order_status.Value : 0;
                carOrder.GetOrderTime = bxCarOrder.GetOrderTime.ToString();
                carOrder.CreateTime = bxCarOrder.create_time.ToString();
                carOrder.OrderId = bxCarOrder.id;
                carOrder.Buid = bxCarOrder.buid.HasValue ? bxCarOrder.buid.Value : 0;
                carOrder.Source = bxCarOrder.source.HasValue ? bxCarOrder.source.Value : 0;
                carOrder.Source = SourceGroupAlgorithm.GetNewSource((int)carOrder.Source);
                carOrder.InsuredName = bxCarOrder.insured_name;
                carOrder.IdType = bxCarOrder.id_type.HasValue ? bxCarOrder.id_type.Value : 2; //默认空时，赋值个“其他”
                carOrder.IdNum = bxCarOrder.id_num;
                carOrder.IdImgFirst = bxCarOrder.id_img_firs;
                carOrder.IdImgSecond = bxCarOrder.id_img_secd;
                carOrder.ImageUrls = bxCarOrder.imageUrls;
                carOrder.ProvinceName = string.Empty;
                carOrder.CityName = string.Empty;
                carOrder.AreaName = string.Empty;
                carOrder.DistributionAddress = bxCarOrder.distribution_address;
                carOrder.DistributionName = bxCarOrder.distribution_name;
                carOrder.DistributionPhone = bxCarOrder.distribution_phone;
                carOrder.DistributionTime = bxCarOrder.distribution_time.HasValue
                    ? bxCarOrder.distribution_time.Value.ToString("yyyy-MM-dd HH:mm:ss")
                    : string.Empty;
                carOrder.InsurancePrice = bxCarOrder.insurance_price.HasValue
                    ? (double)bxCarOrder.insurance_price.Value
                    : 0;
                carOrder.CarriagePrice = bxCarOrder.carriage_price.HasValue
                    ? (double)bxCarOrder.carriage_price.Value
                    : 0;
                carOrder.TotalPrice = bxCarOrder.total_price.HasValue
                    ? (double)bxCarOrder.total_price.Value
                    : 0;
                carOrder.Receipt = bxCarOrder.receipt_title;
                carOrder.ReceiptHead = bxCarOrder.receipt_head.HasValue ? bxCarOrder.receipt_head.Value : 0;
                carOrder.PayType = bxCarOrder.pay_type.HasValue ? bxCarOrder.pay_type.Value : 0;
                carOrder.DistributionType = bxCarOrder.distribution_type.HasValue
                    ? bxCarOrder.distribution_type.Value
                    : 0;
            }

            #endregion

            #region UserInfo转换

            if (bxUserInfo != null)
            {
                userInfo.LicenseNo = bxUserInfo.LicenseNo;
                userInfo.LicenseOwner = bxUserInfo.LicenseOwner;
                userInfo.IdType = bxUserInfo.OwnerIdCardType; //默认空时，赋值个“其他”
                userInfo.InsuredName = bxUserInfo.InsuredName;
                userInfo.InsuredMobile = bxUserInfo.InsuredMobile;
                userInfo.InsuredAddress = bxUserInfo.InsuredAddress;
                userInfo.InsuredIdType = bxUserInfo.InsuredIdType.HasValue ? bxUserInfo.InsuredIdType.Value : 6;
                //默认空时，赋值个“其他”
                userInfo.CredentislasNum = bxUserInfo.InsuredIdCard;
                userInfo.Email = bxUserInfo.Email;
                userInfo.CityCode = !string.IsNullOrEmpty(bxUserInfo.CityCode)
                    ? int.Parse(bxUserInfo.CityCode)
                    : 0;
                //CarUsedType如下
                userInfo.EngineNo = bxUserInfo.EngineNo;
                userInfo.CarVin = bxUserInfo.CarVIN;
                //PurchasePrice如下
                //SeatCount如下
                userInfo.ModleName = bxUserInfo.MoldName;
                userInfo.RegisterDate = bxUserInfo.RegisterDate;
                //LastEndDate如下
                //LastBusinessEndDdate如下
                //ForceStartDate如下
                //BizStartDate如下
                //ClaimCount如下
            }

            #endregion

            #region QuoteResultCarInfo转换

            if (bxCarInfo != null)
            {
                userInfo.CarUsedType = bxCarInfo.car_used_type.HasValue ? bxCarInfo.car_used_type.Value : 0;
                userInfo.PurchasePrice = bxCarInfo.purchase_price.HasValue ? (double)bxCarInfo.purchase_price.Value : 0;
                userInfo.SeatCount = bxCarInfo.seat_count.HasValue ? bxCarInfo.seat_count.Value : 0;
            }

            #endregion

            #region ClaimDetails转换

            if (bxClaimDetails != null)
            {
                userInfo.ClaimCount = bxClaimDetails.Count;
                var claim = new ClaimDetail();
                var claimDetail = new bx_claim_detail();
                for (int i = 0; i < userInfo.ClaimCount; i++)
                {
                    claim = new ClaimDetail();
                    claimDetail = new bx_claim_detail();
                    claimDetail = bxClaimDetails[i];
                    claim.EndcaseTime = claimDetail.endcase_time.HasValue
                        ? claimDetail.endcase_time.Value.ToString()
                        : string.Empty;
                    claim.LossTime = claimDetail.loss_time.HasValue
                        ? claimDetail.loss_time.Value.ToString()
                        : string.Empty;
                    claim.PayAmount = claimDetail.pay_amount.HasValue ? claimDetail.pay_amount.Value : 0;
                    claim.PayCompanyName = claimDetail.pay_company_name;
                    claimDetails.Add(claim);
                }
            }

            #endregion

            #region SaveQuote、QuoteResult、SubmitInfo转换

            if (bxSaveQuote != null)
            {
                bool isquoteresult = true;
                if (bxQuoteResult == null)
                {
                    isquoteresult = false;
                    bxQuoteResult = new bx_quoteresult();
                }
                precisePrice.BizTotal = bxQuoteResult.BizTotal.HasValue ? bxQuoteResult.BizTotal.Value : 0;
                precisePrice.ForceTotal = bxQuoteResult.ForceTotal ?? 0;
                precisePrice.TaxTotal = bxQuoteResult.TaxTotal ?? 0;
                precisePrice.Source = bxQuoteResult.Source ?? 0;
                precisePrice.Source = SourceGroupAlgorithm.GetNewSource((int)precisePrice.Source);
                precisePrice.JiaoQiang = bxSaveQuote.JiaoQiang.HasValue ? bxSaveQuote.JiaoQiang.Value : 1;
                if (bxSubmitInfo != null)
                {
                    precisePrice.QuoteStatus = bxSubmitInfo.quote_status.HasValue
                        ? bxSubmitInfo.quote_status.Value
                        : 0;
                    precisePrice.QuoteResult = bxSubmitInfo.quote_result;
                    precisePrice.SubmitStatus = bxSubmitInfo.submit_status.HasValue
                        ? bxSubmitInfo.submit_status.Value
                        : 0;
                    precisePrice.SubmitResult = bxSubmitInfo.submit_result;
                    precisePrice.BizTno = bxSubmitInfo.biz_tno;
                    precisePrice.ForceTno = bxSubmitInfo.force_pno;
                    precisePrice.BizSysRate = bxSubmitInfo.biz_rate.HasValue ? (double)bxSubmitInfo.biz_rate.Value : 0;
                    precisePrice.ForceSysRate = bxSubmitInfo.force_rate.HasValue ? (double)bxSubmitInfo.force_rate.Value : 0;
                    //BenefitRate
                }
                precisePrice.CheSun = new XianZhongUnit
                {
                    BaoE =
                        isquoteresult
                            ? (bxQuoteResult.CheSunBE.HasValue ? bxQuoteResult.CheSunBE.Value : 0)
                            : (bxSaveQuote.CheSun.HasValue ? bxSaveQuote.CheSun.Value : 0),
                    BaoFei = bxQuoteResult.CheSun.HasValue ? bxQuoteResult.CheSun.Value : 0
                };
                precisePrice.SanZhe = new XianZhongUnit
                {
                    BaoE = bxSaveQuote.SanZhe.HasValue ? bxSaveQuote.SanZhe.Value : 0,
                    BaoFei = bxQuoteResult.SanZhe.HasValue ? bxQuoteResult.SanZhe.Value : 0
                };
                precisePrice.DaoQiang = new XianZhongUnit
                {
                    BaoE =
                        isquoteresult
                            ? (bxQuoteResult.DaoQiangBE.HasValue ? bxQuoteResult.DaoQiangBE.Value : 0)
                            : (bxSaveQuote.DaoQiang.HasValue ? bxSaveQuote.DaoQiang.Value : 0),
                    BaoFei = bxQuoteResult.DaoQiang.HasValue ? bxQuoteResult.DaoQiang.Value : 0
                };
                precisePrice.SiJi = new XianZhongUnit
                {
                    BaoE = bxSaveQuote.SiJi.HasValue ? bxSaveQuote.SiJi.Value : 0,
                    BaoFei = bxQuoteResult.SiJi.HasValue ? bxQuoteResult.SiJi.Value : 0
                };
                precisePrice.ChengKe = new XianZhongUnit
                {
                    BaoE = bxSaveQuote.ChengKe.HasValue ? bxSaveQuote.ChengKe.Value : 0,
                    BaoFei = bxQuoteResult.ChengKe.HasValue ? bxQuoteResult.ChengKe.Value : 0
                };
                precisePrice.BoLi = new XianZhongUnit
                {
                    BaoE = bxSaveQuote.BoLi.HasValue ? bxSaveQuote.BoLi.Value : 0,
                    BaoFei = bxQuoteResult.BoLi.HasValue ? bxQuoteResult.BoLi.Value : 0
                };
                precisePrice.HuaHen = new XianZhongUnit
                {
                    BaoE = bxSaveQuote.HuaHen.HasValue ? bxSaveQuote.HuaHen.Value : 0,
                    BaoFei = bxQuoteResult.HuaHen.HasValue ? bxQuoteResult.HuaHen.Value : 0
                };
                precisePrice.BuJiMianCheSun = new XianZhongUnit
                {
                    BaoE = bxSaveQuote.BuJiMianCheSun.HasValue ? bxSaveQuote.BuJiMianCheSun.Value : 0,
                    BaoFei = bxQuoteResult.BuJiMianCheSun.HasValue ? bxQuoteResult.BuJiMianCheSun.Value : 0
                };
                precisePrice.BuJiMianSanZhe = new XianZhongUnit
                {
                    BaoE = bxSaveQuote.BuJiMianSanZhe.HasValue ? bxSaveQuote.BuJiMianSanZhe.Value : 0,
                    BaoFei = bxQuoteResult.BuJiMianSanZhe.HasValue ? bxQuoteResult.BuJiMianSanZhe.Value : 0
                };
                precisePrice.BuJiMianDaoQiang = new XianZhongUnit
                {
                    BaoE = bxSaveQuote.BuJiMianDaoQiang.HasValue ? bxSaveQuote.BuJiMianDaoQiang.Value : 0,
                    BaoFei = bxQuoteResult.BuJiMianDaoQiang.HasValue ? bxQuoteResult.BuJiMianDaoQiang.Value : 0
                };
                precisePrice.BuJiMianChengKe = new XianZhongUnit()
                {
                    BaoE = bxSaveQuote.BuJiMianChengKe.HasValue ? bxSaveQuote.BuJiMianChengKe.Value : 0,
                    BaoFei = bxQuoteResult.BuJiMianChengKe.HasValue ? bxQuoteResult.BuJiMianChengKe.Value : 0
                };
                precisePrice.BuJiMianSiJi = new XianZhongUnit()
                {
                    BaoE = bxSaveQuote.BuJiMianSiJi.HasValue ? bxSaveQuote.BuJiMianSiJi.Value : 0,
                    BaoFei = bxQuoteResult.BuJiMianSiJi.HasValue ? bxQuoteResult.BuJiMianSiJi.Value : 0
                };
                precisePrice.BuJiMianHuaHen = new XianZhongUnit()
                {
                    BaoE = bxSaveQuote.BuJiMianHuaHen.HasValue ? bxSaveQuote.BuJiMianHuaHen.Value : 0,
                    BaoFei = bxQuoteResult.BuJiMianHuaHen.HasValue ? bxQuoteResult.BuJiMianHuaHen.Value : 0
                };
                precisePrice.BuJiMianSheShui = new XianZhongUnit()
                {
                    BaoE = bxSaveQuote.BuJiMianSheShui.HasValue ? bxSaveQuote.BuJiMianSheShui.Value : 0,
                    BaoFei = bxQuoteResult.BuJiMianSheShui.HasValue ? bxQuoteResult.BuJiMianSheShui.Value : 0
                };
                precisePrice.BuJiMianZiRan = new XianZhongUnit()
                {
                    BaoE = bxSaveQuote.BuJiMianZiRan.HasValue ? bxSaveQuote.BuJiMianZiRan.Value : 0,
                    BaoFei = bxQuoteResult.BuJiMianZiRan.HasValue ? bxQuoteResult.BuJiMianZiRan.Value : 0
                };
                precisePrice.BuJiMianJingShenSunShi = new XianZhongUnit()
                {
                    BaoE =
                        bxSaveQuote.BuJiMianJingShenSunShi.HasValue
                            ? bxSaveQuote.BuJiMianJingShenSunShi.Value
                            : 0,
                    BaoFei =
                        bxQuoteResult.BuJiMianJingShenSunShi.HasValue
                            ? bxQuoteResult.BuJiMianJingShenSunShi.Value
                            : 0
                };
                precisePrice.SheShui = new XianZhongUnit
                {
                    BaoE = bxSaveQuote.SheShui.HasValue ? bxSaveQuote.SheShui.Value : 0,
                    BaoFei = bxQuoteResult.SheShui.HasValue ? bxQuoteResult.SheShui.Value : 0
                };
                precisePrice.ZiRan = new XianZhongUnit
                {
                    BaoE =
                        isquoteresult
                            ? (bxQuoteResult.ZiRanBE.HasValue ? bxQuoteResult.ZiRanBE.Value : 0)
                            : (bxSaveQuote.ZiRan.HasValue ? bxSaveQuote.ZiRan.Value : 0),
                    BaoFei = bxQuoteResult.ZiRan.HasValue ? bxQuoteResult.ZiRan.Value : 0
                };
                precisePrice.HcSheBeiSunshi = new XianZhongUnit
                {
                    BaoE = bxSaveQuote.HcSheBeiSunshi.HasValue ? bxSaveQuote.HcSheBeiSunshi.Value : 0,
                    BaoFei = bxQuoteResult.HcSheBeiSunshi.HasValue ? bxQuoteResult.HcSheBeiSunshi.Value : 0
                };
                precisePrice.HcHuoWuZeRen = new XianZhongUnit
                {
                    BaoE = bxSaveQuote.HcHuoWuZeRen.HasValue ? bxSaveQuote.HcHuoWuZeRen.Value : 0,
                    BaoFei = bxQuoteResult.HcHuoWuZeRen.HasValue ? bxQuoteResult.HcHuoWuZeRen.Value : 0
                };
                precisePrice.HcFeiYongBuChang = new XianZhongUnit
                {
                    BaoE = bxSaveQuote.HcFeiYongBuChang.HasValue ? bxSaveQuote.HcFeiYongBuChang.Value : 0,
                    BaoFei = bxQuoteResult.HcFeiYongBuChang.HasValue ? bxQuoteResult.HcFeiYongBuChang.Value : 0
                };
                precisePrice.HcJingShenSunShi = new XianZhongUnit
                {
                    BaoE = bxSaveQuote.HcJingShenSunShi.HasValue ? bxSaveQuote.HcJingShenSunShi.Value : 0,
                    BaoFei = bxQuoteResult.HcJingShenSunShi.HasValue ? bxQuoteResult.HcJingShenSunShi.Value : 0
                };
                precisePrice.HcSanFangTeYue = new XianZhongUnit
                {
                    BaoE = bxSaveQuote.HcSanFangTeYue.HasValue ? bxSaveQuote.HcSanFangTeYue.Value : 0,
                    BaoFei = bxQuoteResult.HcSanFangTeYue.HasValue ? bxQuoteResult.HcSanFangTeYue.Value : 0
                };
                precisePrice.HcXiuLiChang = new XianZhongUnit
                {
                    BaoE = bxSaveQuote.HcXiuLiChang.HasValue ? bxSaveQuote.HcXiuLiChang.Value : 0,
                    BaoFei = bxQuoteResult.HcXiuLiChang.HasValue ? bxQuoteResult.HcXiuLiChang.Value : 0
                };
                precisePrice.HcXiuLiChangType = bxSaveQuote.HcXiuLiChangType.HasValue
                    ? bxSaveQuote.HcXiuLiChangType.Value.ToString()
                    : "-1";
                precisePrice.RateFactor1 = bxQuoteResult.RateFactor1.HasValue
                    ? (double)bxQuoteResult.RateFactor1.Value
                    : 0;
                precisePrice.RateFactor2 = bxQuoteResult.RateFactor2.HasValue
                    ? (double)bxQuoteResult.RateFactor2.Value
                    : 0;
                precisePrice.RateFactor3 = bxQuoteResult.RateFactor3.HasValue
                    ? (double)bxQuoteResult.RateFactor3.Value
                    : 0;
                precisePrice.RateFactor4 = bxQuoteResult.RateFactor4.HasValue
                    ? (double)bxQuoteResult.RateFactor4.Value
                    : 0;
            }

            #endregion

            #region LastInfo转换

            if (bxLastInfo != null)
            {
                userInfo.LastEndDate = bxLastInfo.last_end_date;
                userInfo.LastBusinessEndDdate = bxLastInfo.last_business_end_date;
            }

            #endregion

            #region 保险起始时间

            if (qrStartDate != null)
            {
                userInfo.ForceStartDate = qrStartDate.ForceStartDate.HasValue
                    ? qrStartDate.ForceStartDate.Value.ToString()
                    : string.Empty;
                userInfo.BizStartDate = qrStartDate.BizStartDate.HasValue
                    ? qrStartDate.BizStartDate.Value.ToString()
                    : string.Empty;
            }

            #endregion

            orderDetail.CarOrder = carOrder;
            orderDetail.ClaimDetail = claimDetails;
            orderDetail.PrecisePrice = precisePrice;
            orderDetail.UserInfo = userInfo;

            return orderDetail;
        }
    }
}
