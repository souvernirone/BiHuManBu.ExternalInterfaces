using System.Collections.Generic;
using System.Globalization;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class SaveQuoteMapper
    {
        public static SaveQuoteViewModel ConvetToViewModel(this bx_car_renewal savequote)
        {
            SaveQuoteViewModel model = new SaveQuoteViewModel();
            if (savequote != null)
            {
                model.BoLi = savequote.BoLi ?? 0;
                //model.CheDeng = savequote.CheDeng ?? 0;
                model.CheSun = savequote.CheSun ?? 0;
                model.ChengKe = savequote.ChengKe ?? 0;
                model.DaoQiang = savequote.DaoQiang ?? 0;
                model.HuaHen = savequote.HuaHen ?? 0;
                model.SanZhe = savequote.SanZhe ?? 0;
                model.SheShui = savequote.SheShui ?? 0;
                model.SiJi = savequote.SiJi ?? 0;
                model.BuJiMianCheSun = savequote.BuJiMianCheSun ?? 0;
                model.BuJiMianDaoQiang = savequote.BuJiMianDaoQiang ?? 0;
                //model.BuJiMianFuJia = savequote.BuJiMianFuJia ?? 0;
                //model.BuJiMianRenYuan = savequote.BuJiMianRenYuan ?? 0;
                model.BuJiMianSanZhe = savequote.BuJiMianSanZhe ?? 0;
                model.ZiRan = savequote.ZiRan ?? 0;
                model.Source = savequote.LastYearSource.Value;
                //2.1.5修改 新增8个字段
                model.BuJiMianChengKe = savequote.BuJiMianChengKe ?? 0;
                model.BuJiMianSiJi = savequote.BuJiMianSiJi ?? 0;
                model.BuJiMianHuaHen = savequote.BuJiMianHuaHen ?? 0;
                model.BuJiMianSheShui = savequote.BuJiMianSheShui ?? 0;
                model.BuJiMianZiRan = savequote.BuJiMianZiRan ?? 0;
                model.BuJiMianJingShenSunShi = savequote.BuJiMianJingShenSunShi ?? 0;
                model.HcSanFangTeYue = savequote.SanFangTeYue ?? 0;
                model.HcJingShenSunShi = savequote.JingShenSunShi ?? 0;
                model.HcXiuLiChang = (savequote.XiuLiChang ?? 0).ToString(CultureInfo.InvariantCulture);
                model.HcXiuLiChangType = (savequote.XiuLiChangType ?? -1).ToString();
                model.Fybc = (savequote.FeiYongBuChang ?? 0).ToString(CultureInfo.InvariantCulture);
                model.FybcDays = (savequote.FeiYongBuChangDays ?? 0).ToString();
                model.SheBeiSunShi = (savequote.SheBeiSunShi ?? 0).ToString(CultureInfo.InvariantCulture);
                model.BjmSheBeiSunShi = (savequote.BuJiMianSheBeiSunshi ?? 0).ToString(CultureInfo.InvariantCulture);
                List<SheBei> sheBeis = new List<SheBei>();
                if (!string.IsNullOrWhiteSpace(savequote.SheBeiSunShiConfig))
                {
                    if (!savequote.SheBeiSunShiConfig.ToUpper().Equals("NULL"))
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
                }
                model.SheBeis = sheBeis;
                model.SanZheJieJiaRi = (savequote.SanZheJieJiaRi ?? 0).ToString();
            }
            return model;
        }

        public static AppSaveQuoteViewModel AppConvetToViewModel(this bx_car_renewal savequote)
        {
            AppSaveQuoteViewModel model = new AppSaveQuoteViewModel();
            if (savequote != null)
            {
                model.BoLi = savequote.BoLi ?? 0;
                //model.CheDeng = savequote.CheDeng ?? 0;
                model.CheSun = savequote.CheSun ?? 0;
                model.ChengKe = savequote.ChengKe ?? 0;
                model.DaoQiang = savequote.DaoQiang ?? 0;
                model.HuaHen = savequote.HuaHen ?? 0;
                model.SanZhe = savequote.SanZhe ?? 0;
                model.SheShui = savequote.SheShui ?? 0;
                model.SiJi = savequote.SiJi ?? 0;
                model.BuJiMianCheSun = savequote.BuJiMianCheSun ?? 0;
                model.BuJiMianDaoQiang = savequote.BuJiMianDaoQiang ?? 0;
                //model.BuJiMianFuJia = savequote.BuJiMianFuJia ?? 0;
                //model.BuJiMianRenYuan = savequote.BuJiMianRenYuan ?? 0;
                model.BuJiMianSanZhe = savequote.BuJiMianSanZhe ?? 0;
                model.ZiRan = savequote.ZiRan ?? 0;
                model.Source = savequote.LastYearSource.Value;
                if (savequote.LastYearSource.HasValue)
                {
                    model.SourceName = savequote.LastYearSource.Value.ToEnumDescriptionString(typeof(EnumSource));
                    //Enum.GetName(typeof(EnumSource), savequote.LastYearSource.Value);
                }
                else
                {
                    model.SourceName = "";
                }
                //2.1.5修改 新增8个字段
                model.BuJiMianChengKe = savequote.BuJiMianChengKe ?? 0;
                model.BuJiMianSiJi = savequote.BuJiMianSiJi ?? 0;
                model.BuJiMianHuaHen = savequote.BuJiMianHuaHen ?? 0;
                model.BuJiMianSheShui = savequote.BuJiMianSheShui ?? 0;
                model.BuJiMianZiRan = savequote.BuJiMianZiRan ?? 0;
                model.BuJiMianJingShenSunShi = savequote.BuJiMianJingShenSunShi ?? 0;
                model.HcSanFangTeYue = savequote.SanFangTeYue ?? 0;
                model.HcJingShenSunShi = savequote.JingShenSunShi ?? 0;
            }
            return model;
        }
    }
}