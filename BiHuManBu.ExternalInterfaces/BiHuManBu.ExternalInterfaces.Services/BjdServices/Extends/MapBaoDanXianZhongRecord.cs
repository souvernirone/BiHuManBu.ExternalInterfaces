using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public class MapBaoDanXianZhongRecord : IMapBaoDanXianZhongRecord
    {
        public bj_baodanxianzhong MapBaodanxianzhong(bj_baodanxinxi baodanxinxi, bx_quoteresult quoteresult, bx_savequote savequote, bx_submit_info submitInfo, List<bx_ywxdetail> jylist)
        {
            var baodanxianzhong = new bj_baodanxianzhong()
            {
                BaoDanXinXiId = baodanxinxi.Id,
                BizTotal = quoteresult.BizTotal.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.BizTotal.Value : System.Math.Round((double)quoteresult.BizTotal.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,
                BoLiBaoE = savequote.BoLi,
                BoLiBaoFei = quoteresult.BoLi.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.BoLi.Value : System.Math.Round((double)quoteresult.BoLi.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,
                BuJiMianCheSun =
                quoteresult.BuJiMianCheSun.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.BuJiMianCheSun.Value : System.Math.Round((double)quoteresult.BuJiMianCheSun.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,
                BuJiMianDaoQiang =
                quoteresult.BuJiMianDaoQiang.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.BuJiMianDaoQiang.Value : System.Math.Round((double)quoteresult.BuJiMianDaoQiang.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,
                BuJiMianFuJian =
                quoteresult.BuJiMianFuJian.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.BuJiMianFuJian.Value : System.Math.Round((double)quoteresult.BuJiMianFuJian.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,
                BuJiMianRenYuan =
                quoteresult.BuJiMianRenYuan.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.BuJiMianRenYuan.Value : System.Math.Round((double)quoteresult.BuJiMianRenYuan.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,
                BuJiMianSanZhe =
                quoteresult.BuJiMianSanZhe.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.BuJiMianSanZhe.Value : System.Math.Round((double)quoteresult.BuJiMianSanZhe.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                //2.1.5版本修改 新增6个字段
                BuJiMianChengKe =
                quoteresult.BuJiMianChengKe.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.BuJiMianChengKe.Value : System.Math.Round((double)quoteresult.BuJiMianChengKe.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                BuJiMianSiJi =
                  quoteresult.BuJiMianSiJi.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.BuJiMianSiJi.Value : System.Math.Round((double)quoteresult.BuJiMianSiJi.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                BuJiMianHuaHen =
                 quoteresult.BuJiMianHuaHen.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.BuJiMianHuaHen.Value : System.Math.Round((double)quoteresult.BuJiMianHuaHen.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                BuJiMianSheShui =
                  quoteresult.BuJiMianSheShui.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.BuJiMianSheShui.Value : System.Math.Round((double)quoteresult.BuJiMianSheShui.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                BuJiMianZiRan =
                  quoteresult.BuJiMianZiRan.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.BuJiMianZiRan.Value : System.Math.Round((double)quoteresult.BuJiMianZiRan.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                BuJiMianJingShenSunShi =
                  quoteresult.BuJiMianJingShenSunShi.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.BuJiMianJingShenSunShi.Value : System.Math.Round((double)quoteresult.BuJiMianJingShenSunShi.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                SanFangTeYueBaoE = savequote.HcSanFangTeYue.HasValue ? savequote.HcSanFangTeYue.Value : 0,
                SanFangTeYueBaoFei =
                  quoteresult.HcSanFangTeYue.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.HcSanFangTeYue.Value : System.Math.Round((double)quoteresult.HcSanFangTeYue.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                JingShenSunShiBaoE = savequote.HcJingShenSunShi.HasValue ? savequote.HcJingShenSunShi.Value : 0,
                JingShenSunShiBaoFei =
                 quoteresult.HcJingShenSunShi.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.HcJingShenSunShi.Value : System.Math.Round((double)quoteresult.HcJingShenSunShi.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                HuoWuZeRenBaoE = savequote.HcHuoWuZeRen.HasValue ? savequote.HcHuoWuZeRen.Value : 0,
                HuoWuZeRenBaoFei =
                 quoteresult.HcHuoWuZeRen.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.HcHuoWuZeRen.Value : System.Math.Round((double)quoteresult.HcHuoWuZeRen.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                //设备损失
                SheBeiSunShiBaoE = savequote.HcSheBeiSunshi.HasValue ? savequote.HcSheBeiSunshi.Value : 0,
                SheBeiSunShiBaoFei =
                quoteresult.HcSheBeiSunshi.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.HcSheBeiSunshi.Value : System.Math.Round((double)quoteresult.HcSheBeiSunshi.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                //不计免设备损失
                BuJiMianSheBeiSunShiBaoE = savequote.BuJiMianSheBeiSunshi.HasValue ? savequote.BuJiMianSheBeiSunshi.Value : 0,
                BuJiMianSheBeiSunShiBaoFei =
                  quoteresult.BuJiMianSheBeiSunshi.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.BuJiMianSheBeiSunshi.Value : System.Math.Round((double)quoteresult.BuJiMianSheBeiSunshi.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                XiuLiChangBaoE = savequote.HcXiuLiChang,
                XiuLiChangBaoFei =
                quoteresult.HcXiuLiChang.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.HcXiuLiChang.Value : System.Math.Round((double)quoteresult.HcXiuLiChang.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                FeiYongBuChangBaoE = savequote.HcFeiYongBuChang,
                FeiYongBuChangBaoFei =
                 quoteresult.HcFeiYongBuChang.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.HcFeiYongBuChang.Value : System.Math.Round((double)quoteresult.HcFeiYongBuChang.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                //2.1.5修改结束
                //费用补偿天数
                FybcDays = savequote.FeiYongBuChangDays.HasValue ? savequote.FeiYongBuChangDays.Value : 0,

                CheDengBaoE = savequote.CheDeng,
                CheDengBaoFei =
                 quoteresult.CheDeng.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.CheDeng.Value : System.Math.Round((double)quoteresult.CheDeng.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                ChengKeBaoE = savequote.ChengKe,
                ChengKeBaoFei =
                   quoteresult.ChengKe.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.ChengKe.Value : System.Math.Round((double)quoteresult.ChengKe.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                //CheSunBaoE = savequote.CheSun,//车损保额修改quoteresult
                CheSunBaoE = quoteresult.CheSunBE.HasValue ? quoteresult.CheSunBE.Value : 0,
                CheSunBaoFei =
                 quoteresult.CheSun.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.CheSun.Value : System.Math.Round((double)quoteresult.CheSun.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,
                //DaoQiangBaoE = savequote.DaoQiang,//盗抢保额修改quoteresult
                DaoQiangBaoE = quoteresult.DaoQiangBE.HasValue ? quoteresult.DaoQiangBE : 0,
                DaoQiangBaoFei =
                  quoteresult.DaoQiang.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.DaoQiang.Value : System.Math.Round((double)quoteresult.DaoQiang.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,
                ForceTotal =
                  quoteresult.ForceTotal.HasValue ? quoteresult.ForceTotal.Value : 0,

                HuaHenBaoE = savequote.HuaHen,
                HuaHenBaoFei =
                  quoteresult.HuaHen.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.HuaHen.Value : System.Math.Round((double)quoteresult.HuaHen.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,
                SanZheBaoE = savequote.SanZhe,
                SanZheBaoFei =
                quoteresult.SanZhe.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.SanZhe.Value : System.Math.Round((double)quoteresult.SanZhe.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,
                SheShuiBaoE = savequote.SheShui,
                SheShuiBaoFei =
                 quoteresult.SheShui.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.SheShui.Value : System.Math.Round((double)quoteresult.SheShui.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,
                SiJiBaoE = savequote.SiJi,
                SiJiBaoFei =
                  quoteresult.SiJi.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.SiJi.Value : System.Math.Round((double)quoteresult.SiJi.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,
                Source = submitInfo.source,
                TaxTotal = quoteresult.TaxTotal.HasValue ? quoteresult.TaxTotal.Value : 0,
                //ZiRanBaoE = savequote.ZiRan,//自燃保额修改quoteresult
                ZiRanBaoE = quoteresult.ZiRanBE.HasValue ? quoteresult.ZiRanBE.Value : 0,
                ZiRanBaoFei =
                 quoteresult.ZiRan.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.ZiRan.Value : System.Math.Round((double)quoteresult.ZiRan.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,

                SanZheJieJiaRiBaoE = savequote.SanZheJieJiaRi,
                SanZheJieJiaRiBaoFei =
                quoteresult.SanZheJieJiaRi.HasValue ? ((!quoteresult.NewRate.HasValue || quoteresult.NewRate == 0) ? quoteresult.SanZheJieJiaRi.Value : System.Math.Round((double)quoteresult.SanZheJieJiaRi.Value / (double)quoteresult.TotalRate * (double)quoteresult.NewRate, 2)) : 0,
                CreateTime = DateTime.Now,
                JiaoQiang = savequote.JiaoQiang
            };
            #region 不计免重新赋值，针对人保3.5代系统不计免赔合并展示的问题
            if (baodanxianzhong.Source == 2 && baodanxianzhong.BuJiMianFuJian > 0)
            {
                //车损
                if (savequote.BuJiMianCheSun > 0 && quoteresult.BuJiMianCheSun == 0)
                {
                    baodanxianzhong.BuJiMianCheSun = -1;
                }
                //盗抢
                if (savequote.BuJiMianDaoQiang > 0 && quoteresult.BuJiMianDaoQiang == 0)
                {
                    baodanxianzhong.BuJiMianDaoQiang = -1;
                }
                //人员
                if (savequote.BuJiMianRenYuan > 0 && quoteresult.BuJiMianRenYuan == 0)
                {
                    baodanxianzhong.BuJiMianRenYuan = -1;
                }
                //三者
                if (savequote.BuJiMianSanZhe > 0 && quoteresult.BuJiMianSanZhe == 0)
                {
                    baodanxianzhong.BuJiMianSanZhe = -1;
                }
                //乘客
                if (savequote.BuJiMianChengKe > 0 && quoteresult.BuJiMianChengKe == 0)
                {
                    baodanxianzhong.BuJiMianChengKe = -1;
                }
                //司机
                if (savequote.BuJiMianSiJi > 0 && quoteresult.BuJiMianSiJi == 0)
                {
                    baodanxianzhong.BuJiMianSiJi = -1;
                }
                //划痕
                if (savequote.BuJiMianHuaHen > 0 && quoteresult.BuJiMianHuaHen == 0)
                {
                    baodanxianzhong.BuJiMianHuaHen = -1;
                }
                //涉水
                if (savequote.BuJiMianSheShui > 0 && quoteresult.BuJiMianSheShui == 0)
                {
                    baodanxianzhong.BuJiMianSheShui = -1;
                }
                //自燃
                if (savequote.BuJiMianZiRan > 0 && quoteresult.BuJiMianZiRan == 0)
                {
                    baodanxianzhong.BuJiMianZiRan = -1;
                }
                //精神损失
                if (savequote.BuJiMianJingShenSunShi > 0 && quoteresult.BuJiMianJingShenSunShi == 0)
                {
                    baodanxianzhong.BuJiMianJingShenSunShi = -1;
                }
            }
            //设备损失在上文已赋值，此处无需处理
            //baodanxianzhong.BuJiMianSheBeiSunShiBaoE
            #endregion
            if (jylist != null && jylist.Any())
            {
                jylist = jylist.Where(w => w.source == baodanxianzhong.Source).ToList();
                if (jylist != null && jylist.Any())
                {
                    baodanxianzhong.JiaYiTotal = jylist.Sum(l => l.amount ?? 0);
                }
            }

            return baodanxianzhong;
        }
    }
}
