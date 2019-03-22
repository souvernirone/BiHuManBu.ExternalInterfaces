using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class PrecisePriceMapper
    {

        /// <summary>
        /// 对外报价接口
        /// </summary>
        /// <param name="userinfo"></param>
        /// <param name="savequote"></param>
        /// <param name="quoteresult"></param>
        /// <param name="submitInfo"></param>
        /// <returns></returns>
        public static PrecisePriceItemViewModel ConvertToViewModel(this bx_userinfo userinfo,
            bx_savequote savequote, bx_quoteresult quoteresult, bx_submit_info submitInfo, List<bx_ywxdetail> jiayi)
        {
            var isquoteresult = true;
            if (quoteresult == null)
            {
                isquoteresult = false;
                quoteresult = new bx_quoteresult();
            }
            var model = new PrecisePriceItemViewModel()
            {
                BizRate = (double)(submitInfo.biz_rate.HasValue ? submitInfo.biz_rate.Value : 0),
                ForceRate = (double)(submitInfo.force_rate.HasValue ? submitInfo.force_rate.Value : 0),
                BizTotal = quoteresult.BizTotal.HasValue ? quoteresult.BizTotal.Value : 0,
                ForceTotal = quoteresult.ForceTotal ?? 0,
                TaxTotal = quoteresult.TaxTotal ?? 0,
                Source = quoteresult.Source ?? 0,
                QuoteStatus = submitInfo.quote_status.HasValue ? submitInfo.quote_status.Value : 0,
                CheSun = new XianZhongUnit
                {
                    //BaoE = savequote.CheSun.HasValue ? savequote.CheSun.Value : 0,
                    BaoE = isquoteresult ? (quoteresult.CheSunBE.HasValue ? quoteresult.CheSunBE.Value : 0) : (savequote.CheSun.HasValue ? savequote.CheSun.Value : 0),
                    BaoFei = quoteresult.CheSun.HasValue ? quoteresult.CheSun.Value : 0
                },
                SanZhe = new XianZhongUnit
                {
                    BaoE = savequote.SanZhe.HasValue ? savequote.SanZhe.Value : 0,
                    BaoFei = quoteresult.SanZhe.HasValue ? quoteresult.SanZhe.Value : 0
                },
                DaoQiang = new XianZhongUnit
                {
                    BaoE = isquoteresult ? (quoteresult.DaoQiangBE.HasValue ? quoteresult.DaoQiangBE.Value : 0) : (savequote.DaoQiang.HasValue ? savequote.DaoQiang.Value : 0),
                    BaoFei = quoteresult.DaoQiang.HasValue ? quoteresult.DaoQiang.Value : 0
                },
                SiJi = new XianZhongUnit
                {
                    BaoE = savequote.SiJi.HasValue ? savequote.SiJi.Value : 0,
                    BaoFei = quoteresult.SiJi.HasValue ? quoteresult.SiJi.Value : 0
                },
                ChengKe = new XianZhongUnit
                {
                    BaoE = savequote.ChengKe.HasValue ? savequote.ChengKe.Value : 0,
                    BaoFei = quoteresult.ChengKe.HasValue ? quoteresult.ChengKe.Value : 0
                },
                BoLi = new XianZhongUnit
                {
                    BaoE = savequote.BoLi.HasValue ? savequote.BoLi.Value : 0,
                    BaoFei = quoteresult.BoLi.HasValue ? quoteresult.BoLi.Value : 0
                },
                HuaHen = new XianZhongUnit
                {
                    BaoE = savequote.HuaHen.HasValue ? savequote.HuaHen.Value : 0,
                    BaoFei = quoteresult.HuaHen.HasValue ? quoteresult.HuaHen.Value : 0
                },


                BuJiMianCheSun = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianCheSun.HasValue ? savequote.BuJiMianCheSun.Value : 0,
                    BaoFei = quoteresult.BuJiMianCheSun.HasValue ? quoteresult.BuJiMianCheSun.Value : 0
                },
                BuJiMianSanZhe = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianSanZhe.HasValue ? savequote.BuJiMianSanZhe.Value : 0,
                    BaoFei = quoteresult.BuJiMianSanZhe.HasValue ? quoteresult.BuJiMianSanZhe.Value : 0
                },
                BuJiMianDaoQiang = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianDaoQiang.HasValue ? savequote.BuJiMianDaoQiang.Value : 0,
                    BaoFei = quoteresult.BuJiMianDaoQiang.HasValue ? quoteresult.BuJiMianDaoQiang.Value : 0
                },
                //BuJiMianRenYuan = new XianZhongUnit
                //{
                //    BaoE = savequote.BuJiMianRenYuan.HasValue ? savequote.BuJiMianRenYuan.Value : 0,
                //    BaoFei = quoteresult.BuJiMianRenYuan.HasValue ? quoteresult.BuJiMianRenYuan.Value : 0
                //},

                BuJiMianFuJia = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianFuJian.HasValue ? savequote.BuJiMianFuJian.Value : 0,
                    BaoFei = quoteresult.BuJiMianFuJian.HasValue ? quoteresult.BuJiMianFuJian.Value : 0
                },

                //2.1.5版本 修改 增加6个字段
                BuJiMianChengKe = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianChengKe.HasValue ? savequote.BuJiMianChengKe.Value : 0,
                    BaoFei = quoteresult.BuJiMianChengKe.HasValue ? quoteresult.BuJiMianChengKe.Value : 0
                },
                BuJiMianSiJi = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianSiJi.HasValue ? savequote.BuJiMianSiJi.Value : 0,
                    BaoFei = quoteresult.BuJiMianSiJi.HasValue ? quoteresult.BuJiMianSiJi.Value : 0
                },
                BuJiMianHuaHen = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianHuaHen.HasValue ? savequote.BuJiMianHuaHen.Value : 0,
                    BaoFei = quoteresult.BuJiMianHuaHen.HasValue ? quoteresult.BuJiMianHuaHen.Value : 0
                },
                BuJiMianSheShui = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianSheShui.HasValue ? savequote.BuJiMianSheShui.Value : 0,
                    BaoFei = quoteresult.BuJiMianSheShui.HasValue ? quoteresult.BuJiMianSheShui.Value : 0
                },
                BuJiMianZiRan = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianZiRan.HasValue ? savequote.BuJiMianZiRan.Value : 0,
                    BaoFei = quoteresult.BuJiMianZiRan.HasValue ? quoteresult.BuJiMianZiRan.Value : 0
                },
                BuJiMianJingShenSunShi = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianJingShenSunShi.HasValue ? savequote.BuJiMianJingShenSunShi.Value : 0,
                    BaoFei = quoteresult.BuJiMianJingShenSunShi.HasValue ? quoteresult.BuJiMianJingShenSunShi.Value : 0
                },
                //2.1.5修改结束

                SheShui = new XianZhongUnit
                {
                    BaoE = savequote.SheShui.HasValue ? savequote.SheShui.Value : 0,
                    BaoFei = quoteresult.SheShui.HasValue ? quoteresult.SheShui.Value : 0
                },
                //CheDeng = new XianZhongUnit
                //{
                //    BaoE = savequote.CheDeng.HasValue ? savequote.CheDeng.Value : 0,
                //    BaoFei = quoteresult.CheDeng.HasValue ? quoteresult.CheDeng.Value : 0
                //},
                ZiRan = new XianZhongUnit
                {
                    BaoE = isquoteresult ? (quoteresult.ZiRanBE.HasValue ? quoteresult.ZiRanBE.Value : 0) : (savequote.ZiRan.HasValue ? savequote.ZiRan.Value : 0),
                    BaoFei = quoteresult.ZiRan.HasValue ? quoteresult.ZiRan.Value : 0
                },
                HcSheBeiSunshi = new XianZhongUnit
                {
                    BaoE = savequote.HcSheBeiSunshi.HasValue ? savequote.HcSheBeiSunshi.Value : 0,
                    BaoFei = quoteresult.HcSheBeiSunshi.HasValue ? quoteresult.HcSheBeiSunshi.Value : 0
                },
                HcHuoWuZeRen = new XianZhongUnit
                {
                    BaoE = savequote.HcHuoWuZeRen.HasValue ? savequote.HcHuoWuZeRen.Value : 0,
                    BaoFei = quoteresult.HcHuoWuZeRen.HasValue ? quoteresult.HcHuoWuZeRen.Value : 0
                },
                //HcFeiYongBuChang = new XianZhongUnit
                //{
                //    BaoE = savequote.HcFeiYongBuChang.HasValue ? savequote.HcFeiYongBuChang.Value : 0,
                //    BaoFei = quoteresult.HcFeiYongBuChang.HasValue ? quoteresult.HcFeiYongBuChang.Value : 0
                //},
                HcJingShenSunShi = new XianZhongUnit
                {
                    BaoE = savequote.HcJingShenSunShi.HasValue ? savequote.HcJingShenSunShi.Value : 0,
                    BaoFei = quoteresult.HcJingShenSunShi.HasValue ? quoteresult.HcJingShenSunShi.Value : 0
                },
                HcSanFangTeYue = new XianZhongUnit
                {
                    BaoE = savequote.HcSanFangTeYue.HasValue ? savequote.HcSanFangTeYue.Value : 0,
                    BaoFei = quoteresult.HcSanFangTeYue.HasValue ? quoteresult.HcSanFangTeYue.Value : 0
                },
                HcXiuLiChang = new XianZhongUnit
                {
                    BaoE = savequote.HcXiuLiChang ?? 0,
                    BaoFei = quoteresult.HcXiuLiChang ?? 0
                },
                HcXiuLiChangType = savequote.HcXiuLiChangType.HasValue ? savequote.HcXiuLiChangType.Value.ToString() : "-1",
                RateFactor1 = quoteresult.RateFactor1 ?? 0,
                RateFactor2 = quoteresult.RateFactor2 ?? 0,
                RateFactor3 = quoteresult.RateFactor3 ?? 0,
                RateFactor4 = quoteresult.RateFactor4 ?? 0,
                TotalRate = (quoteresult.TotalRate ?? 0).ToString(CultureInfo.InvariantCulture),
                //太平洋实际折扣系数20180725bygpj
                ActualDiscounts = (quoteresult.ActualDiscounts ?? 0).ToString(CultureInfo.InvariantCulture),
                Fybc = new XianZhongUnit
                {
                    BaoE = savequote.HcFeiYongBuChang ?? 0,
                    BaoFei = quoteresult.HcFeiYongBuChang ?? 0
                },
                FybcDays = new XianZhongUnit()
                {
                    BaoE = savequote.FeiYongBuChangDays ?? 0,
                    BaoFei = savequote.FeiYongBuChangDays ?? 0
                },
                SheBeiSunShi = new XianZhongUnit
                {
                    BaoE = savequote.HcSheBeiSunshi ?? 0,
                    BaoFei = quoteresult.HcSheBeiSunshi ?? 0
                },
                BjmSheBeiSunShi = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianSheBeiSunshi ?? 0,
                    BaoFei = quoteresult.BuJiMianSheBeiSunshi ?? 0
                }
            };
            //安心三者，划痕特殊处理
            if (quoteresult.Source == 12)
            {
                if (model.SanZhe.BaoE > 3000000)
                {
                    model.SanZhe.BaoE = 3000000;
                }
                //安心的国产车如果上了进口，则改为国产玻璃
                if (model.BoLi.BaoE == 2)
                {
                    if (!string.IsNullOrEmpty(userinfo.CarVIN))
                    {
                        if (userinfo.CarVIN.StartsWith("L"))
                        {
                            model.BoLi.BaoE = 1;
                        }
                    }
                    else { model.BoLi.BaoE = 1; }
                }
            }
            List<SheBei> sheBeis = new List<SheBei>();
            if (!string.IsNullOrWhiteSpace(savequote.SheBeiSunShiConfig))
            {
                var items = savequote.SheBeiSunShiConfig.FromJson<List<bx_devicedetail>>();

                foreach (bx_devicedetail devicedetail in items)
                {
                    var sb = new SheBei()
                    {
                        DN = string.IsNullOrWhiteSpace(devicedetail.device_name) ? string.Empty : devicedetail.device_name,
                        DA = devicedetail.device_amount ?? 0,
                        DD = devicedetail.device_depreciationamount ?? devicedetail.device_depreciationamount.Value,
                        DQ = devicedetail.device_quantity ?? devicedetail.device_quantity.Value,
                        DT = devicedetail.device_type ?? devicedetail.device_type.Value,
                        PD = devicedetail.purchase_date.HasValue ? devicedetail.purchase_date.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty
                    };
                    sheBeis.Add(sb);
                }
            }
            model.SheBeis = sheBeis;
            if (submitInfo != null)
            {
                if (userinfo.Agent == "4405")
                {
                    model.QuoteResult = submitInfo.quote_result_toc ?? "";
                }
                else
                {
                    model.QuoteResult = submitInfo.quote_result ?? "";
                }
                //新增错误编码和结果
                model.QuoteErrorCode = submitInfo.ErrorCodeToC.HasValue ? submitInfo.ErrorCodeToC.Value.ToString() : "";
                model.QuoteErrorResult = submitInfo.quote_result_toc ?? "";
            }
            if (submitInfo.source == 0 || submitInfo.source == 3)
            {
                if (savequote.HcXiuLiChangType == 1)
                {
                    if (savequote.HcXiuLiChang < 0.15)
                    {
                        model.HcXiuLiChang.BaoE = 0.15;
                    }
                }
            }
            model.PingAnScore = quoteresult.PingAnScore.HasValue ? quoteresult.PingAnScore.Value.ToString(CultureInfo.InvariantCulture) : "0";
            model.RepeatSubmitResult = (submitInfo.is_repeat_submit ?? 0).ToString();
            model.ExpectedLossRate = (quoteresult.biz_expected_loss_rate ?? 0).ToString(CultureInfo.InvariantCulture);
            if (submitInfo.source == 2)
            {
                model.VersionType = submitInfo.VersionType == "Rb_Marketing_version" ? "1" : "0";
            }
            else { model.VersionType = "0"; }
            #region 是否为人保版本第三代
            model.IsRB3Version = "0";
            if (!string.IsNullOrWhiteSpace(submitInfo.VersionType))
            {
                switch (submitInfo.VersionType)
                {
                    case "Rb_Marketing_version":
                        model.IsRB3Version = "0";
                        break;
                    case "Rb_VFour_JL_Artificial_version":
                        model.IsRB3Version = "0";
                        break;
                    case "Rb_VFour_LN_Artificial_version":
                        model.IsRB3Version = "0";
                        break;
                    case "Rb_VFour_HuBei_Artificial_version":
                        model.IsRB3Version = "0";
                        break;
                    case "Rb_VFour_HB_Artificial_version":
                        model.IsRB3Version = "0";
                        break;
                    case "Rb_VFour_CQ_Artificial_version":
                        model.IsRB3Version = "0";
                        break;
                    default:
                        model.IsRB3Version = "1";
                        break;
                }
            }
            #endregion
            model.BizCpicScore = (quoteresult.BizCpicScore ?? 0).ToString(CultureInfo.InvariantCulture);
            model.TotalCpicScore = (quoteresult.TotalCpicScore ?? 0).ToString(CultureInfo.InvariantCulture);
            model.TotalEcompensationRate =
                (quoteresult.TotalEcompensationRate ?? 0).ToString(CultureInfo.InvariantCulture);
            //20181123驾意险
            double jiayitotal = 0;
            int quotestatus = submitInfo != null ? submitInfo.quote_status ?? 0 : 0;
            model.JiaYi = jiayi.ConvertViewModel(quotestatus, out jiayitotal);
            return model;
        }

        /// <summary>
        /// 对内报价单详情和列表
        /// </summary>
        /// <param name="source"></param>
        /// <param name="savequote"></param>
        /// <param name="quoteresult"></param>
        /// <param name="submitInfo"></param>
        /// <param name="quoteStatus"></param>
        /// <returns></returns>
        public static MyPrecisePriceItemViewModel ConvertToViewModelNew(int source, bx_savequote savequote, bx_quoteresult quoteresult, bx_submit_info submitInfo, int quoteStatus, List<AgentConfigNameModel> agentChannelList, string carVin, List<bx_ywxdetail> jiayi, string strRate = null)
        {
            double? newRate = null;
            if (!string.IsNullOrEmpty(strRate) && source == 3)
            {
                newRate = double.Parse(strRate);
            }

            if (savequote == null)
            {
                savequote = new bx_savequote();
            }
            if (submitInfo == null)
            {
                submitInfo = new bx_submit_info();
            }
            var isquoteresult = true;
            if (quoteresult == null)
            {
                isquoteresult = false;
                quoteresult = new bx_quoteresult();
            }

            var model = new MyPrecisePriceItemViewModel()
            {
                BizRate = (double)(submitInfo.biz_rate.HasValue ? submitInfo.biz_rate.Value : 0),
                ForceRate = (double)(submitInfo.force_rate.HasValue ? submitInfo.force_rate.Value : 0),
                BizTotal = quoteresult.BizTotal.HasValue ? (!newRate.HasValue ? quoteresult.BizTotal.Value : System.Math.Round((double)quoteresult.BizTotal.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0,
                ForceTotal = quoteresult.ForceTotal ?? 0,
                TaxTotal = quoteresult.TaxTotal ?? 0,
                Source = SourceGroupAlgorithm.GetNewSource(source),
                QuoteStatus = submitInfo.quote_status.HasValue ? submitInfo.quote_status.Value : 0,
                QuoteResult = !string.IsNullOrEmpty(submitInfo.quote_result) ? submitInfo.quote_result : "",
                SubmitStatus = submitInfo.submit_status.HasValue ? submitInfo.submit_status.Value : 0,
                SubmitResult = !string.IsNullOrEmpty(submitInfo.submit_result) ? submitInfo.submit_result : "",
                JiaoQiang = savequote.JiaoQiang.HasValue ? savequote.JiaoQiang.Value : 1,
                CheSun = new XianZhongUnit
                {
                    //BaoE = savequote.CheSun.HasValue ? savequote.CheSun.Value : 0,
                    BaoE =
                        isquoteresult
                            ? (quoteresult.CheSunBE.HasValue ? quoteresult.CheSunBE.Value : 0)
                            : (savequote.CheSun.HasValue ? savequote.CheSun.Value : 0),
                    BaoFei = quoteresult.CheSun.HasValue ? (!newRate.HasValue ? quoteresult.CheSun.Value : System.Math.Round((double)quoteresult.CheSun.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                SanZhe = new XianZhongUnit
                {
                    BaoE = savequote.SanZhe.HasValue ? savequote.SanZhe.Value : 0,
                    BaoFei = quoteresult.SanZhe.HasValue ? (!newRate.HasValue ? quoteresult.SanZhe.Value : System.Math.Round((double)quoteresult.SanZhe.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                DaoQiang = new XianZhongUnit
                {
                    BaoE =
                        isquoteresult
                            ? (quoteresult.DaoQiangBE.HasValue ? quoteresult.DaoQiangBE.Value : 0)
                            : (savequote.DaoQiang.HasValue ? savequote.DaoQiang.Value : 0),
                    BaoFei = quoteresult.DaoQiang.HasValue ? (!newRate.HasValue ? quoteresult.DaoQiang.Value : System.Math.Round((double)quoteresult.DaoQiang.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                SanZheJieJiaRi = new XianZhongUnit
                {
                    BaoE = (savequote.SanZheJieJiaRi.HasValue ? savequote.SanZheJieJiaRi.Value : 0) > 0 ? 1 : 0,
                    BaoFei = quoteresult.SanZheJieJiaRi.HasValue ? (!newRate.HasValue ? quoteresult.SanZheJieJiaRi.Value : System.Math.Round((double)quoteresult.SanZheJieJiaRi.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                SiJi = new XianZhongUnit
                {
                    BaoE = savequote.SiJi.HasValue ? savequote.SiJi.Value : 0,
                    BaoFei = quoteresult.SiJi.HasValue ? (!newRate.HasValue ? quoteresult.SiJi.Value : System.Math.Round((double)quoteresult.SiJi.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                ChengKe = new XianZhongUnit
                {
                    BaoE = savequote.ChengKe.HasValue ? savequote.ChengKe.Value : 0,
                    BaoFei = quoteresult.ChengKe.HasValue ? (!newRate.HasValue ? quoteresult.ChengKe.Value : System.Math.Round((double)quoteresult.ChengKe.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BoLi = new XianZhongUnit
                {
                    BaoE = savequote.BoLi.HasValue ? savequote.BoLi.Value : 0,
                    BaoFei = quoteresult.BoLi.HasValue ? (!newRate.HasValue ? quoteresult.BoLi.Value : System.Math.Round((double)quoteresult.BoLi.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                HuaHen = new XianZhongUnit
                {
                    BaoE = savequote.HuaHen.HasValue ? savequote.HuaHen.Value : 0,
                    BaoFei = quoteresult.HuaHen.HasValue ? (!newRate.HasValue ? quoteresult.HuaHen.Value : System.Math.Round((double)quoteresult.HuaHen.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },

                BuJiMianCheSun = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianCheSun.HasValue ? savequote.BuJiMianCheSun.Value : 0,
                    BaoFei = quoteresult.BuJiMianCheSun.HasValue ? (!newRate.HasValue ? quoteresult.BuJiMianCheSun.Value : System.Math.Round((double)quoteresult.BuJiMianCheSun.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianSanZhe = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianSanZhe.HasValue ? savequote.BuJiMianSanZhe.Value : 0,
                    BaoFei = quoteresult.BuJiMianSanZhe.HasValue ? (!newRate.HasValue ? quoteresult.BuJiMianSanZhe.Value : System.Math.Round((double)quoteresult.BuJiMianSanZhe.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianDaoQiang = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianDaoQiang.HasValue ? savequote.BuJiMianDaoQiang.Value : 0,
                    BaoFei = quoteresult.BuJiMianDaoQiang.HasValue ? (!newRate.HasValue ? quoteresult.BuJiMianDaoQiang.Value : System.Math.Round((double)quoteresult.BuJiMianDaoQiang.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianRenYuan = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianRenYuan.HasValue ? savequote.BuJiMianRenYuan.Value : 0,
                    BaoFei = quoteresult.BuJiMianRenYuan.HasValue ? (!newRate.HasValue ? quoteresult.BuJiMianRenYuan.Value : System.Math.Round((double)quoteresult.BuJiMianRenYuan.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },

                BuJiMianFuJia = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianFuJian.HasValue ? savequote.BuJiMianFuJian.Value : 0,
                    BaoFei = quoteresult.BuJiMianFuJian.HasValue ? (!newRate.HasValue ? quoteresult.BuJiMianFuJian.Value : System.Math.Round((double)quoteresult.BuJiMianFuJian.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },

                //2.1.5版本 修改 增加6个字段
                BuJiMianChengKe = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianChengKe.HasValue ? savequote.BuJiMianChengKe.Value : 0,
                    BaoFei = quoteresult.BuJiMianChengKe.HasValue ? (!newRate.HasValue ? quoteresult.BuJiMianChengKe.Value : System.Math.Round((double)quoteresult.BuJiMianChengKe.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianSiJi = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianSiJi.HasValue ? savequote.BuJiMianSiJi.Value : 0,
                    BaoFei = quoteresult.BuJiMianSiJi.HasValue ? (!newRate.HasValue ? quoteresult.BuJiMianSiJi.Value : System.Math.Round((double)quoteresult.BuJiMianSiJi.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianHuaHen = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianHuaHen.HasValue ? savequote.BuJiMianHuaHen.Value : 0,
                    BaoFei = quoteresult.BuJiMianHuaHen.HasValue ? (!newRate.HasValue ? quoteresult.BuJiMianHuaHen.Value : System.Math.Round((double)quoteresult.BuJiMianHuaHen.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianSheShui = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianSheShui.HasValue ? savequote.BuJiMianSheShui.Value : 0,
                    BaoFei = quoteresult.BuJiMianSheShui.HasValue ? (!newRate.HasValue ? quoteresult.BuJiMianSheShui.Value : System.Math.Round((double)quoteresult.BuJiMianSheShui.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianZiRan = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianZiRan.HasValue ? savequote.BuJiMianZiRan.Value : 0,
                    BaoFei = quoteresult.BuJiMianZiRan.HasValue ? (!newRate.HasValue ? quoteresult.BuJiMianZiRan.Value : System.Math.Round((double)quoteresult.BuJiMianZiRan.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianJingShenSunShi = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianJingShenSunShi.HasValue ? savequote.BuJiMianJingShenSunShi.Value : 0,
                    BaoFei = quoteresult.BuJiMianJingShenSunShi.HasValue ? (!newRate.HasValue ? quoteresult.BuJiMianJingShenSunShi.Value : System.Math.Round((double)quoteresult.BuJiMianJingShenSunShi.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                //2.1.5修改结束

                //2.1.5修改补充
                HcSheBeiSunshi = new XianZhongUnit
                {
                    BaoE = savequote.HcSheBeiSunshi.HasValue ? savequote.HcSheBeiSunshi.Value : 0,
                    BaoFei = quoteresult.HcSheBeiSunshi.HasValue ? (!newRate.HasValue ? quoteresult.HcSheBeiSunshi.Value : System.Math.Round((double)quoteresult.HcSheBeiSunshi.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                HcHuoWuZeRen = new XianZhongUnit
                {
                    BaoE = savequote.HcHuoWuZeRen.HasValue ? savequote.HcHuoWuZeRen.Value : 0,
                    BaoFei = quoteresult.HcHuoWuZeRen.HasValue ? (!newRate.HasValue ? quoteresult.HcHuoWuZeRen.Value : System.Math.Round((double)quoteresult.HcHuoWuZeRen.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                //HcFeiYongBuChang = new XianZhongUnit
                //{
                //    BaoE = savequote.HcFeiYongBuChang.HasValue ? savequote.HcFeiYongBuChang.Value : 0,
                //    BaoFei = quoteresult.HcFeiYongBuChang.HasValue ? quoteresult.HcFeiYongBuChang.Value : 0
                //},
                HcJingShenSunShi = new XianZhongUnit
                {
                    BaoE = savequote.HcJingShenSunShi.HasValue ? savequote.HcJingShenSunShi.Value : 0,
                    BaoFei = quoteresult.HcJingShenSunShi.HasValue ? (!newRate.HasValue ? quoteresult.HcJingShenSunShi.Value : System.Math.Round((double)quoteresult.HcJingShenSunShi.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                HcSanFangTeYue = new XianZhongUnit
                {
                    BaoE = savequote.HcSanFangTeYue.HasValue ? savequote.HcSanFangTeYue.Value : 0,
                    BaoFei = quoteresult.HcSanFangTeYue.HasValue ? (!newRate.HasValue ? quoteresult.HcSanFangTeYue.Value : System.Math.Round((double)quoteresult.HcSanFangTeYue.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                HcXiuLiChang = new XianZhongUnit
                {
                    BaoE = savequote.HcXiuLiChang.HasValue ? savequote.HcXiuLiChang.Value : 0,
                    BaoFei = quoteresult.HcXiuLiChang.HasValue ? (!newRate.HasValue ? quoteresult.HcXiuLiChang.Value : System.Math.Round((double)quoteresult.HcXiuLiChang.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                HcXiuLiChangType =
                    savequote.HcXiuLiChangType.HasValue ? savequote.HcXiuLiChangType.Value.ToString() : string.Empty,
                //2.1.5修改补充结束

                SheShui = new XianZhongUnit
                {
                    BaoE = savequote.SheShui.HasValue ? savequote.SheShui.Value : 0,
                    BaoFei = quoteresult.SheShui.HasValue ? (!newRate.HasValue ? quoteresult.SheShui.Value : System.Math.Round((double)quoteresult.SheShui.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                CheDeng = new XianZhongUnit
                {
                    BaoE = savequote.CheDeng.HasValue ? savequote.CheDeng.Value : 0,
                    BaoFei = quoteresult.CheDeng.HasValue ? (!newRate.HasValue ? quoteresult.CheDeng.Value : System.Math.Round((double)quoteresult.CheDeng.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                ZiRan = new XianZhongUnit
                {
                    BaoE =
                        isquoteresult
                            ? (quoteresult.ZiRanBE.HasValue ? quoteresult.ZiRanBE.Value : 0)
                            : (savequote.ZiRan.HasValue ? savequote.ZiRan.Value : 0),
                    BaoFei = quoteresult.ZiRan.HasValue ? (!newRate.HasValue ? quoteresult.ZiRan.Value : System.Math.Round((double)quoteresult.ZiRan.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                RateFactor1 = quoteresult.RateFactor1.HasValue ? quoteresult.RateFactor1.Value : 0,
                RateFactor2 = quoteresult.RateFactor2.HasValue ? quoteresult.RateFactor2.Value : 0,
                RateFactor3 = quoteresult.RateFactor3.HasValue ? quoteresult.RateFactor3.Value : 0,
                RateFactor4 = quoteresult.RateFactor4.HasValue ? quoteresult.RateFactor4.Value : 0,
                TotalRate = (quoteresult.TotalRate ?? 0).ToString(CultureInfo.InvariantCulture),
                //太平洋实际折扣系数20180725bygpj
                ActualDiscounts = (quoteresult.ActualDiscounts ?? 0).ToString(CultureInfo.InvariantCulture),
                BizTno = string.IsNullOrEmpty(submitInfo.biz_tno) ? string.Empty : submitInfo.biz_tno,
                ForceTno = string.IsNullOrEmpty(submitInfo.force_tno) ? string.Empty : submitInfo.force_tno,
                Fybc = new XianZhongUnit
                {
                    BaoE = savequote.HcFeiYongBuChang.HasValue ? savequote.HcFeiYongBuChang.Value : 0,
                    BaoFei = quoteresult.HcFeiYongBuChang.HasValue ? quoteresult.HcFeiYongBuChang.Value : 0
                },
                FybcDays = new XianZhongUnit()
                {
                    BaoE = savequote.FeiYongBuChangDays.HasValue ? savequote.FeiYongBuChangDays.Value : 0,
                    BaoFei = savequote.FeiYongBuChangDays.HasValue ? savequote.FeiYongBuChangDays.Value : 0
                },
                SheBeiSunShi = new XianZhongUnit
                {
                    BaoE = savequote.HcSheBeiSunshi ?? 0,
                    BaoFei = quoteresult.HcSheBeiSunshi ?? 0
                },
                BjmSheBeiSunShi = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianSheBeiSunshi ?? 0,
                    BaoFei = quoteresult.BuJiMianSheBeiSunshi ?? 0
                },
                OrderNo = string.IsNullOrEmpty(submitInfo.orderNo) ? "" : submitInfo.orderNo
            };
            //安心三者，划痕特殊处理
            if (quoteresult.Source == 12)
            {
                if (model.SanZhe.BaoE > 3000000)
                {
                    model.SanZhe.BaoE = 3000000;
                }
                //安心的国产车如果上了进口，则改为国产玻璃
                if (model.BoLi.BaoE == 2)
                {
                    if (!string.IsNullOrEmpty(carVin))
                    {
                        if (carVin.StartsWith("L"))
                        {
                            model.BoLi.BaoE = 1;
                        }
                    }
                    else { model.BoLi.BaoE = 1; }
                }
            }
            if (!quoteresult.TotalRate.HasValue)
            {
                model.TotalRate =
                    (model.RateFactor1 * model.RateFactor2 * model.RateFactor3 * model.RateFactor4).ToString("f4");
            }
            List<SheBei> sheBeis = new List<SheBei>();
            if (!string.IsNullOrWhiteSpace(savequote.SheBeiSunShiConfig))
            {
                var items = savequote.SheBeiSunShiConfig.FromJson<List<bx_devicedetail>>();

                foreach (bx_devicedetail devicedetail in items)
                {
                    var sb = new SheBei()
                    {
                        DN = string.IsNullOrWhiteSpace(devicedetail.device_name) ? string.Empty : devicedetail.device_name,
                        DA = devicedetail.device_amount ?? 0,
                        DD = devicedetail.device_depreciationamount ?? devicedetail.device_depreciationamount.Value,
                        DQ = devicedetail.device_quantity ?? devicedetail.device_quantity.Value,
                        DT = devicedetail.device_type ?? devicedetail.device_type.Value,
                        PD = devicedetail.purchase_date.HasValue ? devicedetail.purchase_date.Value.ToString("yyyy-MM-dd") : string.Empty
                    };
                    sheBeis.Add(sb);
                }
            }
            model.SheBeis = sheBeis;
            if (submitInfo.source == 0 || submitInfo.source == 3)
            {
                if (savequote.HcXiuLiChangType == 1)
                {
                    if (savequote.HcXiuLiChang < 0.15)
                    {
                        model.HcXiuLiChang.BaoE = 0.15;
                    }
                }
            }
            //取核保渠道
            if (submitInfo.channel_id.HasValue && agentChannelList.Any())
            {
                var channelmodel = agentChannelList.FirstOrDefault(s => s.Id == submitInfo.channel_id.Value);
                if (channelmodel != null)
                {
                    var channel = new ChannelInfo
                    {
                        ChannelId = submitInfo.channel_id.Value,
                        ChannelName = channelmodel.ConfigUkeyName ?? "",
                        IsPaicApi = channelmodel.IsPaicApi.ToString()
                    };
                    model.Channel = channel;
                }
            }
            if (model.Channel == null)
            {//如果没给model.Channel赋值，默认实例化
                model.Channel = new ChannelInfo() { ChannelId = 0, ChannelName = "", IsPaicApi = "0" };
            }
            //20181123驾意险
            double jiayitotal = 0;
            model.JiaYi = jiayi.ConvertViewModel(model.QuoteStatus, out jiayitotal);
            model.JiaYiTotal = jiayitotal.ToString();

            if (quoteStatus == 0)
            {
                model.QuoteResult = model.QuoteStatus == 1 ? "报价失败" : (model.QuoteResult ?? "");
                model.SubmitResult = model.QuoteStatus == 1 ? "报价失败未核保" : (model.SubmitResult ?? "");
                model.QuoteStatus = 0;
                model.SubmitStatus = 5;//报价失败未核保
                //险种
                //model.BizRate  = 0;
                //model.ForceRate = 0;
                model.BizTotal = 0;
                model.ForceTotal = 0;
                model.TaxTotal = 0;
                model.CheSun.BaoFei = 0;
                model.SanZhe.BaoFei = 0;
                model.DaoQiang.BaoFei = 0;
                model.SiJi.BaoFei = 0;
                model.ChengKe.BaoFei = 0;
                model.BoLi.BaoFei = 0;
                model.HuaHen.BaoFei = 0;
                model.BuJiMianCheSun.BaoFei = 0;
                model.BuJiMianSanZhe.BaoFei = 0;
                model.BuJiMianDaoQiang.BaoFei = 0;
                model.BuJiMianRenYuan.BaoFei = 0;
                model.BuJiMianFuJia.BaoFei = 0;
                model.BuJiMianChengKe.BaoFei = 0;
                model.BuJiMianSiJi.BaoFei = 0;
                model.BuJiMianHuaHen.BaoFei = 0;
                model.BuJiMianSheShui.BaoFei = 0;
                model.BuJiMianZiRan.BaoFei = 0;
                model.BuJiMianJingShenSunShi.BaoFei = 0;
                model.HcSheBeiSunshi.BaoFei = 0;
                model.HcHuoWuZeRen.BaoFei = 0;
                //model.HcFeiYongBuChang.BaoFei = 0;
                model.HcJingShenSunShi.BaoFei = 0;
                model.HcSanFangTeYue.BaoFei = 0;
                model.HcXiuLiChang.BaoFei = 0;
                model.SheShui.BaoFei = 0;
                model.CheDeng.BaoFei = 0;
                model.ZiRan.BaoFei = 0;
                //model.RateFactor1 = 0;
                //model.RateFactor2 = 0;
                //model.RateFactor3  = 0;
                //model.RateFactor4  = 0;
                //model.BizTno = "",
                //model.ForceTno = "",
                model.Fybc.BaoFei = 0;
                model.FybcDays.BaoFei = 0;
                model.SheBeiSunShi.BaoFei = 0;
                model.BjmSheBeiSunShi.BaoFei = 0;
            }
            model.PingAnScore = (quoteresult.PingAnScore ?? 0).ToString(CultureInfo.InvariantCulture);
            model.RepeatSubmitResult = (submitInfo.is_repeat_submit ?? 0).ToString();
            model.ExpectedLossRate = (quoteresult.biz_expected_loss_rate ?? 0).ToString();
            if (submitInfo.source == 2)
            {
                model.VersionType = submitInfo.VersionType == "Rb_Marketing_version" ? "1" : "0";
            }
            else { model.VersionType = "0"; }
            #region 是否为人保版本第三代
            model.IsRB3Version = "0";
            if (!string.IsNullOrWhiteSpace(submitInfo.VersionType))
            {
                switch (submitInfo.VersionType)
                {
                    case "Rb_Marketing_version":
                        model.IsRB3Version = "0";
                        break;
                    case "Rb_VFour_JL_Artificial_version":
                        model.IsRB3Version = "0";
                        break;
                    case "Rb_VFour_LN_Artificial_version":
                        model.IsRB3Version = "0";
                        break;
                    case "Rb_VFour_HuBei_Artificial_version":
                        model.IsRB3Version = "0";
                        break;
                    case "Rb_VFour_HB_Artificial_version":
                        model.IsRB3Version = "0";
                        break;
                    case "Rb_VFour_CQ_Artificial_version":
                        model.IsRB3Version = "0";
                        break;
                    default:
                        model.IsRB3Version = "1";
                        break;
                }
            }
            #endregion
            model.ValidateCar = new ValidateCar()
            {
                BizValidateCar = (submitInfo.BizcInspectorNme ?? 0).ToString(),
                ForceValidateCar = (submitInfo.ForcecInspectorNme ?? 0).ToString(),
                IsValidateCar = ((submitInfo.ForcecInspectorNme ?? 0) | (submitInfo.BizcInspectorNme ?? 0)).ToString()
            };
            //新增错误编码和结果
            model.QuoteErrorCode = submitInfo.ErrorCodeToC.HasValue ? submitInfo.ErrorCodeToC.Value.ToString() : "";
            model.QuoteErrorResult = submitInfo.quote_result_toc ?? "";

            return model;
        }
        /// <summary>
        /// 对内报价详情 带有Buid
        /// </summary>
        /// <param name="userinfo"></param>
        /// <param name="savequote"></param>
        /// <param name="quoteresult"></param>
        /// <param name="submitInfo"></param>
        /// <returns></returns>
        public static PrecisePriceItemViewModelWithBuid ConvertToViewModelWithBuid(this bx_userinfo userinfo,
          bx_savequote savequote, bx_quoteresult quoteresult, bx_submit_info submitInfo, List<bx_ywxdetail> jiayi)
        {
            var isquoteresult = true;
            if (quoteresult == null)
            {
                isquoteresult = false;
                quoteresult = new bx_quoteresult();
            }
            var model = new PrecisePriceItemViewModelWithBuid()
            {
                BuId = userinfo.Id,
                BizRate = (double)(submitInfo.biz_rate.HasValue ? submitInfo.biz_rate.Value : 0),
                ForceRate = (double)(submitInfo.force_rate.HasValue ? submitInfo.force_rate.Value : 0),
                BizTotal = quoteresult.BizTotal.HasValue ? quoteresult.BizTotal.Value : 0,
                ForceTotal = quoteresult.ForceTotal ?? 0,
                TaxTotal = quoteresult.TaxTotal ?? 0,
                Source = quoteresult.Source ?? 0,

                //SubmitStatus = 1,
                //SubmitResult = "",
                QuoteStatus = submitInfo.quote_status.HasValue ? submitInfo.quote_status.Value : 0,
                //QuoteResult = "",

                CheSun = new XianZhongUnit
                {
                    //BaoE = savequote.CheSun.HasValue ? savequote.CheSun.Value : 0,
                    BaoE = isquoteresult ? (quoteresult.CheSunBE.HasValue ? quoteresult.CheSunBE.Value : 0) : (savequote.CheSun.HasValue ? savequote.CheSun.Value : 0),
                    BaoFei = quoteresult.CheSun.HasValue ? quoteresult.CheSun.Value : 0
                },
                SanZhe = new XianZhongUnit
                {
                    BaoE = savequote.SanZhe.HasValue ? savequote.SanZhe.Value : 0,
                    BaoFei = quoteresult.SanZhe.HasValue ? quoteresult.SanZhe.Value : 0
                },
                DaoQiang = new XianZhongUnit
                {
                    BaoE = isquoteresult ? (quoteresult.DaoQiangBE.HasValue ? quoteresult.DaoQiangBE.Value : 0) : (savequote.DaoQiang.HasValue ? savequote.DaoQiang.Value : 0),
                    BaoFei = quoteresult.DaoQiang.HasValue ? quoteresult.DaoQiang.Value : 0
                },
                SiJi = new XianZhongUnit
                {
                    BaoE = savequote.SiJi.HasValue ? savequote.SiJi.Value : 0,
                    BaoFei = quoteresult.SiJi.HasValue ? quoteresult.SiJi.Value : 0
                },
                ChengKe = new XianZhongUnit
                {
                    BaoE = savequote.ChengKe.HasValue ? savequote.ChengKe.Value : 0,
                    BaoFei = quoteresult.ChengKe.HasValue ? quoteresult.ChengKe.Value : 0
                },
                BoLi = new XianZhongUnit
                {
                    BaoE = savequote.BoLi.HasValue ? savequote.BoLi.Value : 0,
                    BaoFei = quoteresult.BoLi.HasValue ? quoteresult.BoLi.Value : 0
                },
                HuaHen = new XianZhongUnit
                {
                    BaoE = savequote.HuaHen ?? 0,
                    BaoFei = quoteresult.HuaHen ?? 0
                },


                BuJiMianCheSun = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianCheSun ?? 0,
                    BaoFei = quoteresult.BuJiMianCheSun ?? 0
                },
                BuJiMianSanZhe = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianSanZhe ?? 0,
                    BaoFei = quoteresult.BuJiMianSanZhe ?? 0
                },
                BuJiMianDaoQiang = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianDaoQiang ?? 0,
                    BaoFei = quoteresult.BuJiMianDaoQiang ?? 0
                },
                BuJiMianRenYuan = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianRenYuan ?? 0,
                    BaoFei = quoteresult.BuJiMianRenYuan ?? 0
                },

                BuJiMianFuJia = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianFuJian ?? 0,
                    BaoFei = quoteresult.BuJiMianFuJian ?? 0
                },

                //2.1.5版本 修改 增加6个字段
                BuJiMianChengKe = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianChengKe ?? 0,
                    BaoFei = quoteresult.BuJiMianChengKe ?? 0
                },
                BuJiMianSiJi = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianSiJi ?? 0,
                    BaoFei = quoteresult.BuJiMianSiJi ?? 0
                },
                BuJiMianHuaHen = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianHuaHen ?? 0,
                    BaoFei = quoteresult.BuJiMianHuaHen ?? 0
                },
                BuJiMianSheShui = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianSheShui ?? 0,
                    BaoFei = quoteresult.BuJiMianSheShui ?? 0
                },
                BuJiMianZiRan = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianZiRan ?? 0,
                    BaoFei = quoteresult.BuJiMianZiRan ?? 0
                },
                BuJiMianJingShenSunShi = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianJingShenSunShi ?? 0,
                    BaoFei = quoteresult.BuJiMianJingShenSunShi ?? 0
                },
                //2.1.5修改结束
                HcSheBeiSunshi = new XianZhongUnit
                {
                    BaoE = savequote.HcSheBeiSunshi ?? 0,
                    BaoFei = quoteresult.HcSheBeiSunshi ?? 0
                },
                HcHuoWuZeRen = new XianZhongUnit
                {
                    BaoE = savequote.HcHuoWuZeRen ?? 0,
                    BaoFei = quoteresult.HcHuoWuZeRen ?? 0
                },
                HcFeiYongBuChang = new XianZhongUnit
                {
                    BaoE = savequote.HcFeiYongBuChang ?? 0,
                    BaoFei = quoteresult.HcFeiYongBuChang ?? 0
                },
                HcJingShenSunShi = new XianZhongUnit
                {
                    BaoE = savequote.HcJingShenSunShi ?? 0,
                    BaoFei = quoteresult.HcJingShenSunShi ?? 0
                },
                HcSanFangTeYue = new XianZhongUnit
                {
                    BaoE = savequote.HcSanFangTeYue ?? 0,
                    BaoFei = quoteresult.HcSanFangTeYue ?? 0
                },
                HcXiuLiChang = new XianZhongUnit
                {
                    BaoE = savequote.HcXiuLiChang ?? 0,
                    BaoFei = quoteresult.HcXiuLiChang ?? 0
                },
                HcXiuLiChangType = savequote.HcXiuLiChangType.HasValue ? savequote.HcXiuLiChangType.Value.ToString() : "-1",

                SheShui = new XianZhongUnit
                {
                    BaoE = savequote.SheShui ?? 0,
                    BaoFei = quoteresult.SheShui ?? 0
                },
                CheDeng = new XianZhongUnit
                {
                    BaoE = savequote.CheDeng ?? 0,
                    BaoFei = quoteresult.CheDeng ?? 0
                },
                ZiRan = new XianZhongUnit
                {
                    BaoE = isquoteresult ? (quoteresult.ZiRanBE ?? 0) : (savequote.ZiRan ?? 0),
                    BaoFei = quoteresult.ZiRan ?? 0
                },
                RateFactor1 = quoteresult.RateFactor1 ?? 0,
                RateFactor2 = quoteresult.RateFactor2 ?? 0,
                RateFactor3 = quoteresult.RateFactor3 ?? 0,
                RateFactor4 = quoteresult.RateFactor4 ?? 0,
                TotalRate = (quoteresult.TotalRate ?? 0).ToString(CultureInfo.InvariantCulture),
                //太平洋实际折扣系数20180725bygpj
                ActualDiscounts = (quoteresult.ActualDiscounts ?? 0).ToString(CultureInfo.InvariantCulture),
                Fybc = new XianZhongUnit
                {
                    BaoE = savequote.HcFeiYongBuChang ?? 0,
                    BaoFei = quoteresult.HcFeiYongBuChang ?? 0
                },
                FybcDays = new XianZhongUnit()
                {
                    BaoE = savequote.FeiYongBuChangDays.HasValue ? savequote.FeiYongBuChangDays.Value : 0,
                    BaoFei = savequote.FeiYongBuChangDays.HasValue ? savequote.FeiYongBuChangDays.Value : 0
                },
                SheBeiSunShi = new XianZhongUnit
                {
                    BaoE = savequote.HcSheBeiSunshi ?? 0,
                    BaoFei = quoteresult.HcSheBeiSunshi ?? 0
                },
                BjmSheBeiSunShi = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianSheBeiSunshi ?? 0,
                    BaoFei = quoteresult.BuJiMianSheBeiSunshi ?? 0
                },
                SanZheJieJiaRi = new XianZhongUnit
                {
                    BaoE = savequote.SanZheJieJiaRi ?? 0,
                    BaoFei = quoteresult.SanZheJieJiaRi ?? 0
                }
            };
            //安心三者，划痕特殊处理
            if (quoteresult.Source == 12)
            {
                if (model.SanZhe.BaoE > 3000000)
                {
                    model.SanZhe.BaoE = 3000000;
                }
                //安心的国产车如果上了进口，则改为国产玻璃
                if (model.BoLi.BaoE == 2)
                {
                    if (!string.IsNullOrEmpty(userinfo.CarVIN))
                    {
                        if (userinfo.CarVIN.StartsWith("L"))
                        {
                            model.BoLi.BaoE = 1;
                        }
                    }
                    else { model.BoLi.BaoE = 1; }
                }
            }
            if (!quoteresult.TotalRate.HasValue)
            {
                model.TotalRate =
                    (model.RateFactor1 * model.RateFactor2 * model.RateFactor3 * model.RateFactor4).ToString("f4");
            }
            List<SheBei> sheBeis = new List<SheBei>();
            if (!string.IsNullOrWhiteSpace(savequote.SheBeiSunShiConfig))
            {
                var items = savequote.SheBeiSunShiConfig.FromJson<List<bx_devicedetail>>();

                foreach (bx_devicedetail devicedetail in items)
                {
                    var sb = new SheBei()
                    {
                        DN = string.IsNullOrWhiteSpace(devicedetail.device_name) ? string.Empty : devicedetail.device_name,
                        DA = devicedetail.device_amount ?? 0,
                        DD = devicedetail.device_depreciationamount ?? devicedetail.device_depreciationamount.Value,
                        DQ = devicedetail.device_quantity ?? devicedetail.device_quantity.Value,
                        DT = devicedetail.device_type ?? devicedetail.device_type.Value,
                        PD = devicedetail.purchase_date.HasValue ? devicedetail.purchase_date.Value.ToString("yyyy-MM-dd") : string.Empty
                    };
                    sheBeis.Add(sb);
                }
            }
            model.SheBeis = sheBeis;
            if (submitInfo != null)
            {
                model.QuoteResult = submitInfo.quote_result ?? "";
                model.QuoteStatus = submitInfo.quote_status.HasValue ? submitInfo.quote_status.Value : 0;
                //新增错误编码和结果
                model.QuoteErrorCode = submitInfo.ErrorCodeToC.HasValue ? submitInfo.ErrorCodeToC.Value.ToString() : "";
                model.QuoteErrorResult = submitInfo.quote_result_toc ?? "";
            }
            if (submitInfo.source == 0 || submitInfo.source == 3)
            {
                if (savequote.HcXiuLiChangType == 1)
                {
                    if (savequote.HcXiuLiChang < 0.15)
                    {
                        model.HcXiuLiChang.BaoE = 0.15;
                    }
                }
            }
            model.PingAnScore = (quoteresult.PingAnScore ?? 0).ToString(CultureInfo.InvariantCulture);

            model.RepeatSubmitResult = (submitInfo.is_repeat_submit ?? 0).ToString();
            model.ExpectedLossRate = (quoteresult.biz_expected_loss_rate ?? 0).ToString();
            if (submitInfo.source == 2)
            {
                model.VersionType = submitInfo.VersionType == "Rb_Marketing_version" ? "1" : "0";
            }
            else { model.VersionType = "0"; }
            #region 是否为人保版本第三代
            model.IsRB3Version = "0";
            if (!string.IsNullOrWhiteSpace(submitInfo.VersionType))
            {
                switch (submitInfo.VersionType)
                {
                    case "Rb_Marketing_version":
                        model.IsRB3Version = "0";
                        break;
                    case "Rb_VFour_JL_Artificial_version":
                        model.IsRB3Version = "0";
                        break;
                    case "Rb_VFour_LN_Artificial_version":
                        model.IsRB3Version = "0";
                        break;
                    case "Rb_VFour_HuBei_Artificial_version":
                        model.IsRB3Version = "0";
                        break;
                    case "Rb_VFour_HB_Artificial_version":
                        model.IsRB3Version = "0";
                        break;
                    case "Rb_VFour_CQ_Artificial_version":
                        model.IsRB3Version = "0";
                        break;
                    default:
                        model.IsRB3Version = "1";
                        break;
                }
            }
            #endregion
            //20181123驾意险
            double jiayitotal = 0;
            int quotestatus = submitInfo != null ? submitInfo.quote_status ?? 0 : 0;
            model.JiaYi = jiayi.ConvertViewModel(quotestatus, out jiayitotal);
            model.JiaYiTotal = jiayitotal.ToString();
            //20181222增加交强商业区分，app用
            model.JiaoQiang = savequote.JiaoQiang ?? 0;
            return model;
        }
    }
}