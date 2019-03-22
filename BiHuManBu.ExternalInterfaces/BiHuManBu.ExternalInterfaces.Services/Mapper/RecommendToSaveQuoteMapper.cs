using BiHuManBu.ExternalInterfaces.Services.Messages.RemoteMessage;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System.Globalization;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class RecommendToSaveQuoteMapper
    {
        public static SaveQuoteViewModel ConverToViewModel(this RecommendModel model)
        {
            SaveQuoteViewModel vm = new SaveQuoteViewModel();
            if (model != null)
            {
                vm.BoLi = model.BoLi ?? 0;
                //vm.CheDeng = model.CheDeng ?? 0;
                vm.CheSun = model.CheSun ?? 0;
                vm.ChengKe = model.ChengKe ?? 0;
                vm.DaoQiang = model.DaoQiang ?? 0;
                vm.HuaHen = model.HuaHen ?? 0;
                vm.SanZhe = model.SanZhe ?? 0;
                vm.SheShui = model.SheShui ?? 0;
                vm.SiJi = model.SiJi ?? 0;
                vm.BuJiMianCheSun = model.BuJiMianCheSun ?? 0;
                vm.BuJiMianDaoQiang = model.BuJiMianDaoQiang ?? 0;
                //vm.BuJiMianFuJia = model.BuJiMianFuJia ?? 0;
                //vm.BuJiMianRenYuan = model.BuJiMianRenYuan ?? 0;
                vm.BuJiMianSanZhe = model.BuJiMianSanZhe ?? 0;
                vm.ZiRan = model.ZiRan ?? 0;
                vm.Source = 0;// model.LastYearSource.Value;中心不提供
                //2.1.5修改 新增8个字段
                vm.BuJiMianChengKe = model.BuJiMianChengKe ?? 0;
                vm.BuJiMianSiJi = model.BuJiMianSiJi ?? 0;
                vm.BuJiMianHuaHen = model.BuJiMianHuaHen ?? 0;
                vm.BuJiMianSheShui = model.BuJiMianSheShui ?? 0;
                vm.BuJiMianZiRan = model.BuJiMianZiRan ?? 0;
                vm.BuJiMianJingShenSunShi = model.BuJiMianJingShenSunShi ?? 0;
                vm.HcSanFangTeYue = model.SanFangTeYue ?? 0;
                vm.HcJingShenSunShi = model.JingShenSunShi ?? 0;
                vm.HcXiuLiChang = (model.XiuLiChang ?? 0).ToString(CultureInfo.InvariantCulture);
                vm.HcXiuLiChangType = "-1";//(model.XiuLiChangType ?? -1).ToString();中心不提供
                vm.Fybc = (model.FeiYongBuChang ?? 0).ToString(CultureInfo.InvariantCulture);
                vm.FybcDays = "0"; //(model.FeiYongBuChangDays ?? 0).ToString();中心不提供
                vm.SheBeiSunShi = (model.SheBeiSunShi ?? 0).ToString(CultureInfo.InvariantCulture);
                vm.BjmSheBeiSunShi = (model.BuJiMianSheBeiSunshi ?? 0).ToString(CultureInfo.InvariantCulture);
                //List<SheBei> sheBeis = new List<SheBei>();中心不提供
                //vm.SheBeis = sheBeis;
                vm.SanZheJieJiaRi = (model.SanZheJieJiaRi ?? 0).ToString();
            }
            return vm;
        }
    }
}
