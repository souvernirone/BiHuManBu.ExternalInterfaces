
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class BjdItemMapper
    {

        public static BaojiaItemViewModel ConvertToViewModel(this bj_baodanxinxi xinxi, bj_baodanxianzhong xianzhong, List<bx_claim_detail> claimDetail, bx_savequote savequote, AgentViewModelByBJ AgentDetail, List<bx_preferential_activity> Activitys)
        {
            var item = new BaojiaItemViewModel();

            #region BaoJiaInfo
            item.BaoJiaInfo = new BaojiaInfoViewModel
            {
                CarOwner = xinxi.CarOwner,
                BizStartDate = xinxi.BizStartDate.HasValue ? xinxi.BizStartDate.Value.Date == DateTime.MinValue.Date ? "" : xinxi.BizStartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                ForceStartDate = xinxi.ForceStartDate.HasValue ? xinxi.ForceStartDate.Value.Date == DateTime.MinValue.Date ? "" : xinxi.ForceStartDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                CarBrandModel = xinxi.CarBrandModel,
                CarLicense = xinxi.CarLicense,
                CompanyId = xinxi.CompanyId.HasValue ? SourceGroupAlgorithm.GetNewSource(xinxi.CompanyId.Value) : 0,
                ChannelId = xinxi.ChannelId.HasValue ? xinxi.ChannelId.Value : 0,
                InsureIdType = xinxi.InsureIdType,
                InsureIdNum = xinxi.InsureIdNum,
                //新增的4个费率
                NonClaimRate = xinxi.NonClaimRate.HasValue ? xinxi.NonClaimRate.Value : 0,
                MultiDiscountRate = xinxi.MultiDiscountRate.HasValue ? xinxi.MultiDiscountRate.Value : 0,
                AvgMileRate = xinxi.AvgMileRate.HasValue ? xinxi.AvgMileRate.Value : 0,
                RiskRate = xinxi.RiskRate.HasValue ? xinxi.RiskRate.Value : 0,
                BizNum = xinxi.BizNum,
                ForceNum = xinxi.ForceNum,
                //座位数
                CarSeat = xinxi.CarSeated,
                JiaoQiang = xianzhong.JiaoQiang.HasValue ? xianzhong.JiaoQiang.Value : 1,
                VehicleInfo = xinxi.VehicleInfo,
                JqVehicleClaimType = xinxi.JqVehicleClaimType,
                SyVehicleClaimType = xinxi.SyVehicleClaimType,

                LastBizEndDate = xinxi.BizEndDate.HasValue ? xinxi.BizEndDate.Value.Date == DateTime.MinValue.Date ? "" : xinxi.BizEndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                LastForceEndDate = xinxi.ForceEndDate.HasValue ? xinxi.ForceEndDate.Value.Date == DateTime.MinValue.Date ? "" : xinxi.ForceEndDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                ActivityContent = string.IsNullOrEmpty(xinxi.activity_content) ? "" : xinxi.activity_content
            };
            if (!string.IsNullOrEmpty(xinxi.TotalRate))
            {
                item.BaoJiaInfo.TotalRate = xinxi.TotalRate;
            }
            else
            {
                item.BaoJiaInfo.TotalRate = (item.BaoJiaInfo.NonClaimRate * item.BaoJiaInfo.MultiDiscountRate *
                                            item.BaoJiaInfo.AvgMileRate * item.BaoJiaInfo.RiskRate).ToString("f4");
            }
            #endregion

            #region XianZhongInfo
            item.XianZhongInfo = new BaoxianXianZhongViewModel
            {
                BizRate = xinxi.ManualBizRate.HasValue ? xinxi.ManualBizRate.Value : 0,
                ForceRate = xinxi.ManualForceRate.HasValue ? xinxi.ManualForceRate.Value : 0,
                //20170221新增增值税
                AddValueTaxRate = xinxi.AddValueTaxRate.HasValue ? xinxi.AddValueTaxRate.Value : 0,
                BizTotal = xianzhong.BizTotal.HasValue ? xianzhong.BizTotal.Value : 0,

                BoLi = new XianZhongUnit
                {
                    BaoE = xianzhong.BoLiBaoE.HasValue ? xianzhong.BoLiBaoE.Value : 0,
                    BaoFei = xianzhong.BoLiBaoFei.HasValue ? xianzhong.BoLiBaoFei.Value : 0
                },
                BuJiMianCheSun = new XianZhongUnit
                {
                    BaoE = (xianzhong.BuJiMianCheSun.HasValue ? xianzhong.BuJiMianCheSun.Value : 0) > 0 ? 1 : 0,
                    BaoFei = xianzhong.BuJiMianCheSun.HasValue ? xianzhong.BuJiMianCheSun.Value : 0
                },
                BuJiMianDaoQiang = new XianZhongUnit
                {
                    BaoE = (xianzhong.BuJiMianDaoQiang.HasValue ? xianzhong.BuJiMianDaoQiang.Value : 0) > 0 ? 1 : 0,
                    BaoFei = xianzhong.BuJiMianDaoQiang.HasValue ? xianzhong.BuJiMianDaoQiang.Value : 0
                },
                BuJiMianFuJia = new XianZhongUnit
                {
                    BaoE = (xianzhong.BuJiMianFuJian.HasValue ? xianzhong.BuJiMianFuJian.Value : 0) > 0 ? 1 : 0,
                    BaoFei = xianzhong.BuJiMianFuJian.HasValue ? xianzhong.BuJiMianFuJian.Value : 0
                },
                BuJiMianRenYuan = new XianZhongUnit
                {
                    BaoE = (xianzhong.BuJiMianRenYuan.HasValue ? xianzhong.BuJiMianRenYuan.Value : 0) > 0 ? 1 : 0,
                    BaoFei = xianzhong.BuJiMianRenYuan.HasValue ? xianzhong.BuJiMianRenYuan.Value : 0
                },
                BuJiMianSanZhe = new XianZhongUnit
                {
                    BaoE = (xianzhong.BuJiMianSanZhe.HasValue ? xianzhong.BuJiMianSanZhe.Value : 0) > 0 ? 1 : 0,
                    BaoFei = xianzhong.BuJiMianSanZhe.HasValue ? xianzhong.BuJiMianSanZhe.Value : 0
                },
                CheDeng = new XianZhongUnit
                {
                    BaoE = xianzhong.CheDengBaoE.HasValue ? xianzhong.CheDengBaoE.Value : 0,
                    BaoFei = xianzhong.CheDengBaoFei.HasValue ? xianzhong.CheDengBaoFei.Value : 0
                },
                ChengKe = new XianZhongUnit
                {
                    BaoE = xianzhong.ChengKeBaoE.HasValue ? xianzhong.ChengKeBaoE.Value : 0,
                    BaoFei = xianzhong.ChengKeBaoFei.HasValue ? xianzhong.ChengKeBaoFei.Value : 0
                },
                ChengKeBaoENum = xianzhong.ChengKeBaoENum.HasValue ? xianzhong.ChengKeBaoENum.Value : 0,
                CheSun = new XianZhongUnit
                {
                    BaoE = xianzhong.CheSunBaoE.HasValue ? xianzhong.CheSunBaoE.Value : 0,
                    BaoFei = xianzhong.CheSunBaoFei.HasValue ? xianzhong.CheSunBaoFei.Value : 0
                },

                DaoQiang = new XianZhongUnit
                {
                    BaoE = xianzhong.DaoQiangBaoE.HasValue ? xianzhong.DaoQiangBaoE.Value : 0,
                    BaoFei = xianzhong.DaoQiangBaoFei.HasValue ? xianzhong.DaoQiangBaoFei.Value : 0
                },
                ForceTotal = xianzhong.ForceTotal.HasValue ? xianzhong.ForceTotal.Value : 0,
                HuaHen = new XianZhongUnit
                {
                    BaoE = xianzhong.HuaHenBaoE.HasValue ? xianzhong.HuaHenBaoE.Value : 0,
                    BaoFei = xianzhong.HuaHenBaoFei.HasValue ? xianzhong.HuaHenBaoFei.Value : 0
                },
                SanZhe = new XianZhongUnit
                {
                    BaoE = xianzhong.SanZheBaoE.HasValue ? xianzhong.SanZheBaoE.Value : 0,
                    BaoFei = xianzhong.SanZheBaoFei.HasValue ? xianzhong.SanZheBaoFei.Value : 0
                },
                SheShui = new XianZhongUnit
                {
                    BaoE = xianzhong.SheShuiBaoE.HasValue ? xianzhong.SheShuiBaoE.Value : 0,
                    BaoFei = xianzhong.SheShuiBaoFei.HasValue ? xianzhong.SheShuiBaoFei.Value : 0
                },
                SiJi = new XianZhongUnit
                {
                    BaoE = xianzhong.SiJiBaoE.HasValue ? xianzhong.SiJiBaoE.Value : 0,
                    BaoFei = xianzhong.SiJiBaoFei.HasValue ? xianzhong.SiJiBaoFei.Value : 0,
                },
                TaxTotal = xianzhong.TaxTotal.HasValue ? xianzhong.TaxTotal.Value : 0,
                TeYue = new XianZhongUnit
                {
                    BaoE = xianzhong.TeYueBaoE.HasValue ? xianzhong.TeYueBaoE.Value : 0,
                    BaoFei = xianzhong.TeYueBaoFei.HasValue ? xianzhong.TeYueBaoFei.Value : 0
                },
                ZiRan = new XianZhongUnit
                {
                    BaoE = xianzhong.ZiRanBaoE.HasValue ? xianzhong.ZiRanBaoE.Value : 0,
                    BaoFei = xianzhong.ZiRanBaoFei.HasValue ? xianzhong.ZiRanBaoFei.Value : 0
                },

                //2.1.5版本修改 新增6个字段
                BuJiMianChengKe = new XianZhongUnit
                {
                    BaoE = (xianzhong.BuJiMianChengKe.HasValue ? xianzhong.BuJiMianChengKe.Value : 0) > 0 ? 1 : 0,
                    BaoFei = xianzhong.BuJiMianChengKe.HasValue ? xianzhong.BuJiMianChengKe.Value : 0
                },
                BuJiMianSiJi = new XianZhongUnit
                {
                    BaoE = (xianzhong.BuJiMianSiJi.HasValue ? xianzhong.BuJiMianSiJi.Value : 0) > 0 ? 1 : 0,
                    BaoFei = xianzhong.BuJiMianSiJi.HasValue ? xianzhong.BuJiMianSiJi.Value : 0
                },
                BuJiMianHuaHen = new XianZhongUnit
                {
                    BaoE = (xianzhong.BuJiMianHuaHen.HasValue ? xianzhong.BuJiMianHuaHen.Value : 0) > 0 ? 1 : 0,
                    BaoFei = xianzhong.BuJiMianHuaHen.HasValue ? xianzhong.BuJiMianHuaHen.Value : 0
                },
                BuJiMianSheShui = new XianZhongUnit
                {
                    BaoE = (xianzhong.BuJiMianSheShui.HasValue ? xianzhong.BuJiMianSheShui.Value : 0) > 0 ? 1 : 0,
                    BaoFei = xianzhong.BuJiMianSheShui.HasValue ? xianzhong.BuJiMianSheShui.Value : 0
                },
                BuJiMianZiRan = new XianZhongUnit
                {
                    BaoE = (xianzhong.BuJiMianZiRan.HasValue ? xianzhong.BuJiMianZiRan.Value : 0) > 0 ? 1 : 0,
                    BaoFei = xianzhong.BuJiMianZiRan.HasValue ? xianzhong.BuJiMianZiRan.Value : 0
                },
                BuJiMianJingShenSunShi = new XianZhongUnit
                {
                    BaoE = (xianzhong.BuJiMianJingShenSunShi.HasValue ? xianzhong.BuJiMianJingShenSunShi.Value : 0) > 0 ? 1 : 0,
                    BaoFei = xianzhong.BuJiMianJingShenSunShi.HasValue ? xianzhong.BuJiMianJingShenSunShi.Value : 0
                },

                SanFangTeYue = new XianZhongUnit
                {
                    BaoE = xianzhong.SanFangTeYueBaoE.HasValue ? xianzhong.SanFangTeYueBaoE.Value : 0,
                    BaoFei = xianzhong.SanFangTeYueBaoFei.HasValue ? xianzhong.SanFangTeYueBaoFei.Value : 0
                },
                JingShenSunShi = new XianZhongUnit
                {
                    BaoE = xianzhong.JingShenSunShiBaoE.HasValue ? xianzhong.JingShenSunShiBaoE.Value : 0,
                    BaoFei = xianzhong.JingShenSunShiBaoFei.HasValue ? xianzhong.JingShenSunShiBaoFei.Value : 0
                },
                HuoWuZeRen = new XianZhongUnit
                {
                    BaoE = xianzhong.HuoWuZeRenBaoE.HasValue ? xianzhong.HuoWuZeRenBaoE.Value : 0,
                    BaoFei = xianzhong.HuoWuZeRenBaoFei.HasValue ? xianzhong.HuoWuZeRenBaoFei.Value : 0
                },
                SheBeiSunShi = new XianZhongUnit
                {
                    BaoE = xianzhong.SheBeiSunShiBaoE.HasValue ? xianzhong.SheBeiSunShiBaoE.Value : 0,
                    BaoFei = xianzhong.SheBeiSunShiBaoFei.HasValue ? xianzhong.SheBeiSunShiBaoFei.Value : 0
                },
                BuJiMianSheBeiSunShi = new XianZhongUnit
                {
                    BaoE = xianzhong.BuJiMianSheBeiSunShiBaoE.HasValue ? xianzhong.BuJiMianSheBeiSunShiBaoE.Value : 0,
                    BaoFei = xianzhong.BuJiMianSheBeiSunShiBaoFei.HasValue ? xianzhong.BuJiMianSheBeiSunShiBaoFei.Value : 0
                },
                XiuLiChang = new XianZhongUnit
                {
                    BaoE = xianzhong.XiuLiChangBaoE.HasValue ? xianzhong.XiuLiChangBaoE.Value : 0,
                    BaoFei = xianzhong.XiuLiChangBaoFei.HasValue ? xianzhong.XiuLiChangBaoFei.Value : 0
                },
                FeiYongBuChang = new XianZhongUnit
                {
                    BaoE = xianzhong.FeiYongBuChangBaoE.HasValue ? xianzhong.FeiYongBuChangBaoE.Value : 0,
                    BaoFei = xianzhong.FeiYongBuChangBaoFei.HasValue ? xianzhong.FeiYongBuChangBaoFei.Value : 0
                },
                SanZheJieJiaRi = new XianZhongUnit
                {
                    BaoE = xianzhong.SanZheJieJiaRiBaoE.HasValue ? xianzhong.SanZheJieJiaRiBaoE.Value : 0,
                    BaoFei = xianzhong.SanZheJieJiaRiBaoFei.HasValue ? xianzhong.SanZheJieJiaRiBaoFei.Value : 0
                },

                FybcDays = xianzhong.FybcDays.HasValue ? xianzhong.FybcDays.Value : 0,
                //2.1.5修改 结束
                JiaYiTotal = xianzhong.JiaYiTotal ?? 0
            };
            #region 不计免重新赋值，针对人保3.5代系统不计免赔合并展示的问题
            if (xianzhong.Source == 2 && xianzhong.BuJiMianFuJian > 0)
            {
                //车损
                if (xianzhong.BuJiMianCheSun == -1)
                {
                    item.XianZhongInfo.BuJiMianCheSun.BaoE = 1;
                    item.XianZhongInfo.BuJiMianCheSun.BaoFei = 0;
                }
                //盗抢
                if (xianzhong.BuJiMianDaoQiang == -1)
                {
                    item.XianZhongInfo.BuJiMianDaoQiang.BaoE = 1;
                    item.XianZhongInfo.BuJiMianDaoQiang.BaoFei = 0;
                }
                //人员
                if (xianzhong.BuJiMianRenYuan == -1)
                {
                    item.XianZhongInfo.BuJiMianRenYuan.BaoE = 1;
                    item.XianZhongInfo.BuJiMianRenYuan.BaoFei = 0;
                }
                //三者
                if (xianzhong.BuJiMianSanZhe == -1)
                {
                    item.XianZhongInfo.BuJiMianSanZhe.BaoE = 1;
                    item.XianZhongInfo.BuJiMianSanZhe.BaoFei = 0;
                }
                //乘客
                if (xianzhong.BuJiMianChengKe == -1)
                {
                    item.XianZhongInfo.BuJiMianChengKe.BaoE = 1;
                    item.XianZhongInfo.BuJiMianChengKe.BaoFei = 0;
                }
                //司机
                if (xianzhong.BuJiMianSiJi == -1)
                {
                    item.XianZhongInfo.BuJiMianSiJi.BaoE = 1;
                    item.XianZhongInfo.BuJiMianSiJi.BaoFei = 0;
                }
                //划痕
                if (xianzhong.BuJiMianHuaHen == -1)
                {
                    item.XianZhongInfo.BuJiMianHuaHen.BaoE = 1;
                    item.XianZhongInfo.BuJiMianHuaHen.BaoFei = 0;
                }
                //涉水
                if (xianzhong.BuJiMianSheShui == -1)
                {
                    item.XianZhongInfo.BuJiMianSheShui.BaoE = 1;
                    item.XianZhongInfo.BuJiMianSheShui.BaoFei = 0;
                }
                //自燃
                if (xianzhong.BuJiMianZiRan == -1)
                {
                    item.XianZhongInfo.BuJiMianZiRan.BaoE = 1;
                    item.XianZhongInfo.BuJiMianZiRan.BaoFei = 0;
                }
                //精神损失
                if (xianzhong.BuJiMianJingShenSunShi == -1)
                {
                    item.XianZhongInfo.BuJiMianJingShenSunShi.BaoE = 1;
                    item.XianZhongInfo.BuJiMianJingShenSunShi.BaoFei = 0;
                }
            }
            #endregion
            #endregion

            #region 业务员信息 2017-09-06
            item.AgentDetail = new AgentViewModelByBJ
            {
                AgentId = xinxi.AgentId ?? 0
            };
            #endregion

            #region 优惠活动信息 2017-09-06
            if (Activitys != null)
            {
                item.ActivityCount = Activitys.Count;

                #region ActivityDetail
                List<PreActivity> list = new List<PreActivity>();
                PreActivity Activitydetail;
                foreach (var i in Activitys)
                {
                    Activitydetail = new PreActivity();
                    Activitydetail.ActivityName = i.activity_name;
                    Activitydetail.ActivityContent = i.activity_content;
                    list.Add(Activitydetail);
                }
                item.Activitys = list;
                #endregion
            }
            #endregion

            #region 出险信息

            item.ClaimInfo = new ClaimInfo
            {
                LossBizCount = xinxi.loss_biz_count,
                LossBizAmount = String.Format("{0:F}", xinxi.loss_biz_amount),
                LossForceCount = xinxi.loss_force_count,
                LossForceAmount = String.Format("{0:F}", xinxi.loss_force_amount)
            };
            #endregion


            if (savequote != null)
            {
                item.XianZhongInfo.XiuLiChangType = savequote.HcXiuLiChangType.HasValue ? savequote.HcXiuLiChangType.Value : -1;
            }

            if (claimDetail != null)
            {
                item.ClaimCount = claimDetail.Count;

                #region ClaimDetail
                List<ClaimDetailViewModel> list = new List<ClaimDetailViewModel>();
                ClaimDetailViewModel detail;
                foreach (var i in claimDetail)
                {
                    detail = new ClaimDetailViewModel();
                    detail.Buid = i.b_uid;
                    detail.CreateTime = i.create_time;
                    detail.StrCreateTime = i.create_time.HasValue ? i.create_time.Value.ToString("yyyy-MM-dd") : "";
                    detail.EndCaseTime = i.endcase_time;
                    detail.StrEndCaseTime = i.endcase_time.HasValue ? i.endcase_time.Value.ToString("yyyy-MM-dd") : "";
                    detail.Id = i.id;
                    detail.Liid = i.li_id;
                    detail.LossTime = i.loss_time;
                    detail.StrLossTime = i.loss_time.HasValue ? i.loss_time.Value.ToString("yyyy-MM-dd") : "";
                    detail.PayAmount = i.pay_amount;
                    detail.PayCompanyName = i.pay_company_name;
                    detail.PayCompanyNo = i.pay_company_no;
                    list.Add(detail);
                }
                item.ClaimDetail = list;
                #endregion
            }

            return item;
        }

    }
}
