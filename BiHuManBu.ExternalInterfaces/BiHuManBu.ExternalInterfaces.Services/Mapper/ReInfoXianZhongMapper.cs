using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using System;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class ReInfoXianZhongMapper
    {
        private static ILog logError = LogManager.GetLogger("ERROR");
        /// <summary>
        /// 续保返回保额+保费
        /// </summary>
        /// <param name="premiumModel"></param>
        /// <param name="carRenewal"></param>
        /// <returns></returns>
        public static XianZhong ConvetToViewModel(this bx_car_renewal_premium premiumModel, bx_car_renewal carRenewal)
        {
            XianZhong xianzhong = new XianZhong()
            {
                CheSun = new XianZhongUnit(),
                SanZhe = new XianZhongUnit(),
                DaoQiang = new XianZhongUnit(),
                SiJi = new XianZhongUnit(),
                ChengKe = new XianZhongUnit(),
                BoLi = new XianZhongUnit(),
                HuaHen = new XianZhongUnit(),
                BuJiMianCheSun = new XianZhongUnit(),
                BuJiMianSanZhe = new XianZhongUnit(),
                BuJiMianDaoQiang = new XianZhongUnit(),
                BuJiMianFuJia = new XianZhongUnit(),
                BuJiMianChengKe = new XianZhongUnit(),
                BuJiMianSiJi = new XianZhongUnit(),
                BuJiMianHuaHen = new XianZhongUnit(),
                BuJiMianSheShui = new XianZhongUnit(),
                BuJiMianZiRan = new XianZhongUnit(),
                BuJiMianJingShenSunShi = new XianZhongUnit(),
                SheShui = new XianZhongUnit(),
                ZiRan = new XianZhongUnit(),
                HcSheBeiSunshi = new XianZhongUnit(),
                HcHuoWuZeRen = new XianZhongUnit(),
                HcJingShenSunShi = new XianZhongUnit(),
                HcSanFangTeYue = new XianZhongUnit(),
                HcXiuLiChang = new XianZhongUnit(),
                Fybc = new XianZhongUnit(),
                FybcDays = new XianZhongUnit(),
                SheBeiSunShi = new XianZhongUnit(),
                BjmSheBeiSunShi = new XianZhongUnit(),
                HcXiuLiChangType = "-1"
            };
            if (premiumModel == null)
            {
                premiumModel = new bx_car_renewal_premium();
            }
            if (carRenewal == null)
            {
                carRenewal = new bx_car_renewal();
            }

            try
            {
                xianzhong = new XianZhong()
                {
                    CheSun = new XianZhongUnit
                    {
                        //BaoE = carRenewal.CheSun.HasValue ? carRenewal.CheSun.Value : 0,
                        BaoE = carRenewal.CheSun ?? 0,
                        BaoFei = premiumModel.CheSun
                    },
                    SanZhe = new XianZhongUnit
                    {
                        BaoE = carRenewal.SanZhe.HasValue ? carRenewal.SanZhe.Value : 0,
                        BaoFei = premiumModel.SanZhe
                    },
                    DaoQiang = new XianZhongUnit
                    {
                        BaoE = carRenewal.DaoQiang ?? 0,
                        BaoFei = premiumModel.DaoQiang
                    },
                    SiJi = new XianZhongUnit
                    {
                        BaoE = carRenewal.SiJi.HasValue ? carRenewal.SiJi.Value : 0,
                        BaoFei = premiumModel.SiJi
                    },
                    ChengKe = new XianZhongUnit
                    {
                        BaoE = carRenewal.ChengKe.HasValue ? carRenewal.ChengKe.Value : 0,
                        BaoFei = premiumModel.ChengKe
                    },
                    BoLi = new XianZhongUnit
                    {
                        BaoE = carRenewal.BoLi.HasValue ? carRenewal.BoLi.Value : 0,
                        BaoFei = premiumModel.BoLi
                    },
                    HuaHen = new XianZhongUnit
                    {
                        BaoE = carRenewal.HuaHen.HasValue ? carRenewal.HuaHen.Value : 0,
                        BaoFei = premiumModel.HuaHen
                    },


                    BuJiMianCheSun = new XianZhongUnit
                    {
                        BaoE = carRenewal.BuJiMianCheSun.HasValue ? carRenewal.BuJiMianCheSun.Value : 0,
                        BaoFei = premiumModel.BuJiMianCheSun
                    },
                    BuJiMianSanZhe = new XianZhongUnit
                    {
                        BaoE = carRenewal.BuJiMianSanZhe.HasValue ? carRenewal.BuJiMianSanZhe.Value : 0,
                        BaoFei = premiumModel.BuJiMianSanZhe
                    },
                    BuJiMianDaoQiang = new XianZhongUnit
                    {
                        BaoE = carRenewal.BuJiMianDaoQiang.HasValue ? carRenewal.BuJiMianDaoQiang.Value : 0,
                        BaoFei = premiumModel.BuJiMianDaoQiang
                    },
                    //BuJiMianRenYuan = new XianZhongUnit
                    //{
                    //    BaoE = carRenewal.BuJiMianRenYuan.HasValue ? carRenewal.BuJiMianRenYuan.Value : 0,
                    //    BaoFei = premiumModel.BuJiMianRenYuan.HasValue ? premiumModel.BuJiMianRenYuan.Value : 0
                    //},

                    BuJiMianFuJia = new XianZhongUnit
                    {
                        BaoE = carRenewal.BuJiMianFuJia ?? 0,
                        BaoFei = premiumModel.BuJiMianFuJia
                    },

                    //2.1.5版本 修改 增加6个字段
                    BuJiMianChengKe = new XianZhongUnit()
                    {
                        BaoE = carRenewal.BuJiMianChengKe.HasValue ? carRenewal.BuJiMianChengKe.Value : 0,
                        BaoFei = premiumModel.BuJiMianChengKe
                    },
                    BuJiMianSiJi = new XianZhongUnit()
                    {
                        BaoE = carRenewal.BuJiMianSiJi.HasValue ? carRenewal.BuJiMianSiJi.Value : 0,
                        BaoFei = premiumModel.BuJiMianSiJi
                    },
                    BuJiMianHuaHen = new XianZhongUnit()
                    {
                        BaoE = carRenewal.BuJiMianHuaHen.HasValue ? carRenewal.BuJiMianHuaHen.Value : 0,
                        BaoFei = premiumModel.BuJiMianHuaHen
                    },
                    BuJiMianSheShui = new XianZhongUnit()
                    {
                        BaoE = carRenewal.BuJiMianSheShui.HasValue ? carRenewal.BuJiMianSheShui.Value : 0,
                        BaoFei = premiumModel.BuJiMianSheShui
                    },
                    BuJiMianZiRan = new XianZhongUnit()
                    {
                        BaoE = carRenewal.BuJiMianZiRan.HasValue ? carRenewal.BuJiMianZiRan.Value : 0,
                        BaoFei = premiumModel.BuJiMianZiRan
                    },
                    BuJiMianJingShenSunShi = new XianZhongUnit()
                    {
                        BaoE = carRenewal.BuJiMianJingShenSunShi.HasValue ? carRenewal.BuJiMianJingShenSunShi.Value : 0,
                        BaoFei = premiumModel.BuJiMianJingShenSunShi
                    },
                    //2.1.5修改结束

                    SheShui = new XianZhongUnit
                    {
                        BaoE = carRenewal.SheShui.HasValue ? carRenewal.SheShui.Value : 0,
                        BaoFei = premiumModel.SheShui
                    },
                    //CheDeng = new XianZhongUnit
                    //{
                    //    BaoE = carRenewal.CheDeng.HasValue ? carRenewal.CheDeng.Value : 0,
                    //    BaoFei = premiumModel.CheDeng.HasValue ? premiumModel.CheDeng.Value : 0
                    //},
                    ZiRan = new XianZhongUnit
                    {
                        BaoE = carRenewal.ZiRan ?? 0,
                        BaoFei = premiumModel.ZiRan
                    },
                    HcSheBeiSunshi = new XianZhongUnit
                    {
                        BaoE = carRenewal.SheBeiSunShi ?? 0,
                        BaoFei = premiumModel.SheBeiSunShi
                    },
                    HcHuoWuZeRen = new XianZhongUnit
                    {
                        BaoE = carRenewal.HuoWuZeRen ?? 0,
                        BaoFei = premiumModel.HuoWuZeRen
                    },
                    //HcFeiYongBuChang = new XianZhongUnit
                    //{
                    //    BaoE = carRenewal.HcFeiYongBuChang.HasValue ? carRenewal.HcFeiYongBuChang.Value : 0,
                    //    BaoFei = premiumModel.HcFeiYongBuChang.HasValue ? premiumModel.HcFeiYongBuChang.Value : 0
                    //},
                    HcJingShenSunShi = new XianZhongUnit
                    {
                        BaoE = carRenewal.JingShenSunShi ?? 0,
                        BaoFei = premiumModel.JingShenSunShi
                    },
                    HcSanFangTeYue = new XianZhongUnit
                    {
                        BaoE = carRenewal.SanFangTeYue ?? 0,
                        BaoFei = premiumModel.SanFangTeYue
                    },
                    HcXiuLiChang = new XianZhongUnit
                    {
                        BaoE = carRenewal.XiuLiChang ?? 0,
                        BaoFei = premiumModel.XiuLiChang
                    },
                    Fybc = new XianZhongUnit
                    {
                        BaoE = carRenewal.FeiYongBuChang ?? 0,
                        BaoFei = premiumModel.FeiYongBuChang
                    },
                    FybcDays = new XianZhongUnit()
                    {
                        BaoE = carRenewal.FeiYongBuChangDays ?? 0,
                        BaoFei = carRenewal.FeiYongBuChangDays ?? 0
                    },
                    SheBeiSunShi = new XianZhongUnit
                    {
                        BaoE = carRenewal.SheBeiSunShi ?? 0,
                        BaoFei = premiumModel.SheBeiSunShi
                    },
                    BjmSheBeiSunShi = new XianZhongUnit
                    {
                        BaoE = carRenewal.BuJiMianSheBeiSunshi ?? 0,
                        BaoFei = premiumModel.BuJiMianSheBeiSunshi
                    },
                    HcXiuLiChangType = (carRenewal.XiuLiChangType ?? -1).ToString()
                };
            }
            catch (Exception ex)
            {
                logError.Info("模型转换发生异常" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return xianzhong;
        }

    }
}
