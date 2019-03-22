using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves
{
    public class SetPrecisePriceItemNewService : ISetPrecisePriceItemNewService
    {
        private IAgentConfigRepository _agentConfigRepository;
        private IQuoteResultRepository _quoteResultRepository;
        private IPictureRepository _pictureRepository;
        private ISaveQuoteRepository _saveQuoteRepository;
        private ISetPrecisePriceItem _setPrecisePriceItem;
        private ISubmitInfoRepository _submitInfoRepository;
        private IHebaoDianweiRepository _hebaoDianweiRepository;
        private ILog logErr;

        public SetPrecisePriceItemNewService(IAgentConfigRepository agentConfigService, IQuoteResultRepository quoteResultRepository, IPictureRepository pictureRepository, ISaveQuoteRepository saveQuoteRepository, ISubmitInfoRepository submitInfoRepository, IHebaoDianweiRepository hebaoDianweiRepository, ISetPrecisePriceItem setPrecisePriceItem)
        {
            _setPrecisePriceItem = setPrecisePriceItem;
            _hebaoDianweiRepository = hebaoDianweiRepository;
            _submitInfoRepository = submitInfoRepository;
            _saveQuoteRepository = saveQuoteRepository;
            _pictureRepository = pictureRepository;
            _quoteResultRepository = quoteResultRepository;
            _agentConfigRepository = agentConfigService;
            logErr = LogManager.GetLogger("ERROR");
        }
        public MyBaoJiaViewModel SetPrecisePriceItem(MyBaoJiaViewModel my, bx_userinfo userinfo, 
            List<bx_quoteresult_carinfo> quoteresultCarinfo,
            List<long> listquote01,bool allfail,
            bx_savequote sq, List<bx_quoteresult> qrList, List<bx_submit_info> siList)
        {
            string newRate = string.Empty;
            //var sourceList = _setPrecisePriceItem.FindSource(userinfo, request);
            //获取图片bx_picture
            //List<bx_picture> picList = _pictureRepository.GetAllList(o => o.b_uid == userinfo.Id);
            //报价单资源对象
            //bx_savequote sq = _saveQuoteRepository.GetSavequoteByBuid(userinfo.Id);
            //List<bx_quoteresult> qrList = _quoteResultRepository.GetQuoteResultList(userinfo.Id);
            //List<bx_submit_info> siList = _submitInfoRepository.GetSubmitInfoList(userinfo.Id);
            //渠道列表
            List<AgentConfigNameModel> agentChannelList = new List<AgentConfigNameModel>();
            if (siList.Any())
                agentChannelList = _agentConfigRepository.FindListById(siList.Select(l => l.channel_id).Join(","));
            //图片url
            List<IsUploadImg> isUploadImg = new List<IsUploadImg>();
            //报价信息模型，包括险种和报价
            List<MyPrecisePriceItemViewModel> listquoteTotal = new List<MyPrecisePriceItemViewModel>();
            
           var hebaodianweis = _hebaoDianweiRepository.FindList(userinfo.Id, listquote01.ToArray());
            if (listquote01.Any())
            {
                foreach (int oit in listquote01)
                {
                    //20160905修改source1248=>0123，传入的新数据转换
                    var submit = siList.FirstOrDefault(i => i.source == oit);
                    var qr = qrList.FirstOrDefault(i => i.Source == oit);
                    if (oit == 3)
                    {
                        if (!string.IsNullOrEmpty(newRate) && qr != null)
                        {
                            qr.NewRate = decimal.Parse(newRate);
                            _quoteResultRepository.Update(qr);
                        }
                        else if (string.IsNullOrEmpty(newRate) && qr != null && qr.NewRate != 0 && qr.NewRate != null)
                        {
                            newRate = qr.NewRate.ToString();
                        }
                        if (!string.IsNullOrEmpty(newRate))
                        {
                            my.NewRate = double.Parse(newRate).ToString("#0.00000");
                        }

                    }
                    var model = ConvertToViewModelNew(oit, sq,
                        qr, submit, allfail, agentChannelList, userinfo.CarVIN, newRate);

                    var hebaodianwei = hebaodianweis.FirstOrDefault(heb => heb.source == oit);
                    if (hebaodianwei != null && my.IsShowCalc == 0)
                    {
                        model.BizSysRate = hebaodianwei.system_biz_rate.HasValue
                            ? Convert.ToDecimal(hebaodianwei.system_biz_rate.Value)
                            : 0;
                        model.ForceSysRate = hebaodianwei.system_force_rate.HasValue
                            ? Convert.ToDecimal(hebaodianwei.system_force_rate.Value)
                            : 0;
                        //优惠费率
                        model.BenefitRate = hebaodianwei.agent_id == hebaodianwei.parent_agent_id
                            ? Convert.ToDecimal(hebaodianwei.zhike_biz_rate.Value)
                            : Convert.ToDecimal(hebaodianwei.agent_biz_rate.Value);
                    }
                    var qcinfo = quoteresultCarinfo.FirstOrDefault(l => l.source == oit);
                    if (qcinfo != null)
                    {
                        model.CarUsedType = qcinfo.car_used_type.HasValue ? qcinfo.car_used_type.Value : 0;
                    }
                    model.JingSuanKouJing = submit != null ? (submit.RbJSKJ ?? "") : "";
                    listquoteTotal.Add(model);
                }
                my.PrecisePriceItem = listquoteTotal;
                my.IsUploadImg = new List<IsUploadImg>();
            }
            else
            {
                my.PrecisePriceItem = new List<MyPrecisePriceItemViewModel>();
            }
            return my;
        }

        public MyPrecisePriceItemViewModel ConvertToViewModelNew(int source, bx_savequote savequote, bx_quoteresult quoteresult, bx_submit_info submitInfo, bool allfail, List<AgentConfigNameModel> agentChannelList, string carVin, string strRate = null)
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
                BizTotal = quoteresult.BizTotal.HasValue ? (newRate == null ? quoteresult.BizTotal.Value : System.Math.Round((double)quoteresult.BizTotal.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0,
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
                    BaoFei = quoteresult.CheSun.HasValue ? (newRate == null ? quoteresult.CheSun.Value : System.Math.Round((double)quoteresult.CheSun.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                SanZhe = new XianZhongUnit
                {
                    BaoE = savequote.SanZhe.HasValue ? savequote.SanZhe.Value : 0,
                    BaoFei = quoteresult.SanZhe.HasValue ? (newRate == null ? quoteresult.SanZhe.Value : System.Math.Round((double)quoteresult.SanZhe.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                DaoQiang = new XianZhongUnit
                {
                    BaoE =
                        isquoteresult
                            ? (quoteresult.DaoQiangBE.HasValue ? quoteresult.DaoQiangBE.Value : 0)
                            : (savequote.DaoQiang.HasValue ? savequote.DaoQiang.Value : 0),
                    BaoFei = quoteresult.DaoQiang.HasValue ? (newRate == null ? quoteresult.DaoQiang.Value : System.Math.Round((double)quoteresult.DaoQiang.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                SanZheJieJiaRi = new XianZhongUnit
                {
                    BaoE = (savequote.SanZheJieJiaRi.HasValue ? savequote.SanZheJieJiaRi.Value : 0) > 0 ? 1 : 0,
                    BaoFei = quoteresult.SanZheJieJiaRi.HasValue ? (newRate == null ? quoteresult.SanZheJieJiaRi.Value : System.Math.Round((double)quoteresult.SanZheJieJiaRi.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                SiJi = new XianZhongUnit
                {
                    BaoE = savequote.SiJi.HasValue ? savequote.SiJi.Value : 0,
                    BaoFei = quoteresult.SiJi.HasValue ? (newRate == null ? quoteresult.SiJi.Value : System.Math.Round((double)quoteresult.SiJi.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                ChengKe = new XianZhongUnit
                {
                    BaoE = savequote.ChengKe.HasValue ? savequote.ChengKe.Value : 0,
                    BaoFei = quoteresult.ChengKe.HasValue ? (newRate == null ? quoteresult.ChengKe.Value : System.Math.Round((double)quoteresult.ChengKe.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BoLi = new XianZhongUnit
                {
                    BaoE = savequote.BoLi.HasValue ? savequote.BoLi.Value : 0,
                    BaoFei = quoteresult.BoLi.HasValue ? (newRate == null ? quoteresult.BoLi.Value : System.Math.Round((double)quoteresult.BoLi.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                HuaHen = new XianZhongUnit
                {
                    BaoE = savequote.HuaHen.HasValue ? savequote.HuaHen.Value : 0,
                    BaoFei = quoteresult.HuaHen.HasValue ? (newRate == null ? quoteresult.HuaHen.Value : System.Math.Round((double)quoteresult.HuaHen.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },

                BuJiMianCheSun = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianCheSun.HasValue ? savequote.BuJiMianCheSun.Value : 0,
                    BaoFei = quoteresult.BuJiMianCheSun.HasValue ? (newRate == null ? quoteresult.BuJiMianCheSun.Value : System.Math.Round((double)quoteresult.BuJiMianCheSun.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianSanZhe = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianSanZhe.HasValue ? savequote.BuJiMianSanZhe.Value : 0,
                    BaoFei = quoteresult.BuJiMianSanZhe.HasValue ? (newRate == null ? quoteresult.BuJiMianSanZhe.Value : System.Math.Round((double)quoteresult.BuJiMianSanZhe.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianDaoQiang = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianDaoQiang.HasValue ? savequote.BuJiMianDaoQiang.Value : 0,
                    BaoFei = quoteresult.BuJiMianDaoQiang.HasValue ? (newRate == null ? quoteresult.BuJiMianDaoQiang.Value : System.Math.Round((double)quoteresult.BuJiMianDaoQiang.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianRenYuan = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianRenYuan.HasValue ? savequote.BuJiMianRenYuan.Value : 0,
                    BaoFei = quoteresult.BuJiMianRenYuan.HasValue ? (newRate == null ? quoteresult.BuJiMianRenYuan.Value : System.Math.Round((double)quoteresult.BuJiMianRenYuan.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },

                BuJiMianFuJia = new XianZhongUnit
                {
                    BaoE = savequote.BuJiMianFuJian.HasValue ? savequote.BuJiMianFuJian.Value : 0,
                    BaoFei = quoteresult.BuJiMianFuJian.HasValue ? (newRate == null ? quoteresult.BuJiMianFuJian.Value : System.Math.Round((double)quoteresult.BuJiMianFuJian.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },

                //2.1.5版本 修改 增加6个字段
                BuJiMianChengKe = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianChengKe.HasValue ? savequote.BuJiMianChengKe.Value : 0,
                    BaoFei = quoteresult.BuJiMianChengKe.HasValue ? (newRate == null ? quoteresult.BuJiMianChengKe.Value : System.Math.Round((double)quoteresult.BuJiMianChengKe.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianSiJi = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianSiJi.HasValue ? savequote.BuJiMianSiJi.Value : 0,
                    BaoFei = quoteresult.BuJiMianSiJi.HasValue ? (newRate == null ? quoteresult.BuJiMianSiJi.Value : System.Math.Round((double)quoteresult.BuJiMianSiJi.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianHuaHen = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianHuaHen.HasValue ? savequote.BuJiMianHuaHen.Value : 0,
                    BaoFei = quoteresult.BuJiMianHuaHen.HasValue ? (newRate == null ? quoteresult.BuJiMianHuaHen.Value : System.Math.Round((double)quoteresult.BuJiMianHuaHen.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianSheShui = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianSheShui.HasValue ? savequote.BuJiMianSheShui.Value : 0,
                    BaoFei = quoteresult.BuJiMianSheShui.HasValue ? (newRate == null ? quoteresult.BuJiMianSheShui.Value : System.Math.Round((double)quoteresult.BuJiMianSheShui.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianZiRan = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianZiRan.HasValue ? savequote.BuJiMianZiRan.Value : 0,
                    BaoFei = quoteresult.BuJiMianZiRan.HasValue ? (newRate == null ? quoteresult.BuJiMianZiRan.Value : System.Math.Round((double)quoteresult.BuJiMianZiRan.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                BuJiMianJingShenSunShi = new XianZhongUnit()
                {
                    BaoE = savequote.BuJiMianJingShenSunShi.HasValue ? savequote.BuJiMianJingShenSunShi.Value : 0,
                    BaoFei = quoteresult.BuJiMianJingShenSunShi.HasValue ? (newRate == null ? quoteresult.BuJiMianJingShenSunShi.Value : System.Math.Round((double)quoteresult.BuJiMianJingShenSunShi.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                //2.1.5修改结束

                //2.1.5修改补充
                HcSheBeiSunshi = new XianZhongUnit
                {
                    BaoE = savequote.HcSheBeiSunshi.HasValue ? savequote.HcSheBeiSunshi.Value : 0,
                    BaoFei = quoteresult.HcSheBeiSunshi.HasValue ? (newRate == null ? quoteresult.HcSheBeiSunshi.Value : System.Math.Round((double)quoteresult.HcSheBeiSunshi.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                HcHuoWuZeRen = new XianZhongUnit
                {
                    BaoE = savequote.HcHuoWuZeRen.HasValue ? savequote.HcHuoWuZeRen.Value : 0,
                    BaoFei = quoteresult.HcHuoWuZeRen.HasValue ? (newRate == null ? quoteresult.HcHuoWuZeRen.Value : System.Math.Round((double)quoteresult.HcHuoWuZeRen.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                //HcFeiYongBuChang = new XianZhongUnit
                //{
                //    BaoE = savequote.HcFeiYongBuChang.HasValue ? savequote.HcFeiYongBuChang.Value : 0,
                //    BaoFei = quoteresult.HcFeiYongBuChang.HasValue ? quoteresult.HcFeiYongBuChang.Value : 0
                //},
                HcJingShenSunShi = new XianZhongUnit
                {
                    BaoE = savequote.HcJingShenSunShi.HasValue ? savequote.HcJingShenSunShi.Value : 0,
                    BaoFei = quoteresult.HcJingShenSunShi.HasValue ? (newRate == null ? quoteresult.HcJingShenSunShi.Value : System.Math.Round((double)quoteresult.HcJingShenSunShi.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                HcSanFangTeYue = new XianZhongUnit
                {
                    BaoE = savequote.HcSanFangTeYue.HasValue ? savequote.HcSanFangTeYue.Value : 0,
                    BaoFei = quoteresult.HcSanFangTeYue.HasValue ? (newRate == null ? quoteresult.HcSanFangTeYue.Value : System.Math.Round((double)quoteresult.HcSanFangTeYue.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                HcXiuLiChang = new XianZhongUnit
                {
                    BaoE = savequote.HcXiuLiChang.HasValue ? savequote.HcXiuLiChang.Value : 0,
                    BaoFei = quoteresult.HcXiuLiChang.HasValue ? (newRate == null ? quoteresult.HcXiuLiChang.Value : System.Math.Round((double)quoteresult.HcXiuLiChang.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                HcXiuLiChangType =
                    savequote.HcXiuLiChangType.HasValue ? savequote.HcXiuLiChangType.Value.ToString() : string.Empty,
                //2.1.5修改补充结束

                SheShui = new XianZhongUnit
                {
                    BaoE = savequote.SheShui.HasValue ? savequote.SheShui.Value : 0,
                    BaoFei = quoteresult.SheShui.HasValue ? (newRate == null ? quoteresult.SheShui.Value : System.Math.Round((double)quoteresult.SheShui.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                CheDeng = new XianZhongUnit
                {
                    BaoE = savequote.CheDeng.HasValue ? savequote.CheDeng.Value : 0,
                    BaoFei = quoteresult.CheDeng.HasValue ? (newRate == null ? quoteresult.CheDeng.Value : System.Math.Round((double)quoteresult.CheDeng.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                ZiRan = new XianZhongUnit
                {
                    BaoE =
                        isquoteresult
                            ? (quoteresult.ZiRanBE.HasValue ? quoteresult.ZiRanBE.Value : 0)
                            : (savequote.ZiRan.HasValue ? savequote.ZiRan.Value : 0),
                    BaoFei = quoteresult.ZiRan.HasValue ? (newRate == null ? quoteresult.ZiRan.Value : System.Math.Round((double)quoteresult.ZiRan.Value / (double)quoteresult.TotalRate * (double)newRate, 2)) : 0
                },
                RateFactor1 = quoteresult.RateFactor1.HasValue ? quoteresult.RateFactor1.Value : 0,
                RateFactor2 = quoteresult.RateFactor2.HasValue ? quoteresult.RateFactor2.Value : 0,
                RateFactor3 = quoteresult.RateFactor3.HasValue ? quoteresult.RateFactor3.Value : 0,
                RateFactor4 = quoteresult.RateFactor4.HasValue ? quoteresult.RateFactor4.Value : 0,

                TotalRate = (quoteresult.TotalRate ?? 0).ToString(CultureInfo.InvariantCulture),

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
                if (model.HuaHen.BaoE > 0)
                {
                    model.HuaHen.BaoE = 10000;
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
                        IsPaicApi=channelmodel.IsPaicApi.ToString()
                    };
                    model.Channel = channel;
                }
            }
            if (model.Channel == null)
            {//如果没给model.Channel赋值，默认实例化
                model.Channel = new ChannelInfo() { ChannelId = 0, ChannelName = "",IsPaicApi="0" };
            }
            if (allfail)
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



    }
}
