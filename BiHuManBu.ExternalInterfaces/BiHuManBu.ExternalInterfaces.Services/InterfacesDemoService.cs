using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class InterfacesDemoService
    {
        public static GetReInfoViewModel GetReInfo(GetReInfoRequest request)
        {
            var viewModel = new GetReInfoViewModel();

            UserInfoViewModel UserInfo = new UserInfoViewModel();
            UserInfo.CarUsedType = 1;
            UserInfo.LicenseNo = "京FF1234";
            UserInfo.LicenseOwner = "姚毅";
            UserInfo.InsuredName = "姚毅";
            UserInfo.PostedName = "姚毅";
            UserInfo.IdType = 1;
            UserInfo.CredentislasNum = "110108196905241319";
            UserInfo.CityCode = 1;
            UserInfo.EngineNo = "266832";
            UserInfo.ModleName = "奥迪FV6461HBQWG多用途乘用车";
            UserInfo.CarVin = "LFV3B28R4D3065341";
            UserInfo.RegisterDate = "2014-01-26";
            UserInfo.ForceExpireDate = "2017-01-19";
            UserInfo.BusinessExpireDate = "2017-01-19";
            UserInfo.NextForceStartDate = "2017-01-20";
            UserInfo.NextBusinessStartDate = "2017-01-20";
            UserInfo.PurchasePrice = 433710;
            UserInfo.SeatCount = 5;
            UserInfo.FuelType = 1;
            UserInfo.ProofType = 0;
            UserInfo.LicenseColor = 0;
            UserInfo.ClauseType = 0;
            UserInfo.RunRegion = 1;
            UserInfo.InsuredIdCard = "110108196905241319";
            UserInfo.InsuredIdType = 1;
            UserInfo.InsuredMobile = "";
            UserInfo.HolderIdCard = "110108196905241319";
            UserInfo.HolderIdType = 1;
            UserInfo.HolderMobile = "";
            UserInfo.RateFactor1 = 0;
            UserInfo.RateFactor2 = 0;
            UserInfo.RateFactor3 = 0;
            UserInfo.RateFactor4 = 0;
            viewModel.UserInfo = UserInfo;

            SaveQuoteViewModel SaveQuote = new SaveQuoteViewModel();
            SaveQuote.Source = 0;
            SaveQuote.CheSun = 433710;
            SaveQuote.SanZhe = 500000;
            SaveQuote.DaoQiang = 342630.9;
            SaveQuote.SiJi = 0;
            SaveQuote.ChengKe = 0;
            SaveQuote.BoLi = 1;
            SaveQuote.HuaHen = 0;
            //SaveQuote.CheDeng = 0;
            SaveQuote.SheShui = 0;
            SaveQuote.ZiRan = 0;
            SaveQuote.BuJiMianChengKe = 0;
            SaveQuote.BuJiMianSiJi = 0;
            SaveQuote.BuJiMianHuaHen = 0;
            SaveQuote.BuJiMianSheShui = 0;
            SaveQuote.BuJiMianZiRan = 0;
            SaveQuote.BuJiMianJingShenSunShi = 0;

            SaveQuote.BuJiMianCheSun = 1;
            SaveQuote.BuJiMianSanZhe = 1;
            SaveQuote.BuJiMianDaoQiang = 1;
           // SaveQuote.BuJiMianRenYuan = 0;
            //SaveQuote.BuJiMianFuJia = 0;

            SaveQuote.HcSanFangTeYue = 0;
            SaveQuote.HcJingShenSunShi = 0;

            viewModel.SaveQuote = SaveQuote;

            viewModel.CustKey = request.CustKey;
            viewModel.BusinessStatus = 1;
            viewModel.StatusMessage = "续保成功";

            return viewModel;
        }

        public static GetPrecisePriceViewModel GetPrecisePrice(GetPrecisePriceRequest request)
        {
            var viewModel = new GetPrecisePriceViewModel();
            GetPrecisePriceOfUserInfoViewModel UserInfo = new GetPrecisePriceOfUserInfoViewModel();
            UserInfo.LicenseNo = "京FF1234";
            UserInfo.ForceExpireDate = "2017-01-19";
            UserInfo.BusinessExpireDate = "2017-01-19";
            UserInfo.BusinessStartDate = "2016-07-17";
            UserInfo.ForceStartDate = "";
            UserInfo.InsuredName = "姚毅";
            UserInfo.InsuredIdCard = "110108196905241319";
            UserInfo.InsuredIdType = 1;
            UserInfo.InsuredMobile = "13154582463";
            UserInfo.HolderName = "姚毅";
            UserInfo.HolderIdCard = "110108196905241319";
            UserInfo.HolderIdType = 1;
            UserInfo.HolderMobile = "13154582463";

            viewModel.UserInfo = UserInfo;

            PrecisePriceItemViewModel Item = new PrecisePriceItemViewModel();
            Item.BizRate = 0; ;
            Item.ForceRate = 0;
            Item.BizTotal = 8438.24;
            Item.ForceTotal = 0;
            Item.TaxTotal = 0;
            Item.Source = 1;
            Item.QuoteStatus = 1;
            Item.QuoteResult = "成功";

            XianZhongUnit CheSun = new XianZhongUnit();
            CheSun.BaoE = 358244;
            CheSun.BaoFei = 4234.46;
            Item.CheSun = CheSun;

            XianZhongUnit SanZhe = new XianZhongUnit();
            SanZhe.BaoE = 500000;
            SanZhe.BaoFei = 1063.52;
            Item.SanZhe = SanZhe;

            XianZhongUnit DaoQiang = new XianZhongUnit();
            DaoQiang.BaoE = 358244;
            DaoQiang.BaoFei = 1458.49;
            Item.DaoQiang = DaoQiang;

            XianZhongUnit SiJi = new XianZhongUnit();
            SiJi.BaoE = 0;
            SiJi.BaoFei = 0;
            Item.SiJi = SiJi;

            XianZhongUnit ChengKe = new XianZhongUnit();
            ChengKe.BaoE = 0;
            ChengKe.BaoFei = 0;
            Item.ChengKe = ChengKe;

            XianZhongUnit BoLi = new XianZhongUnit();
            BoLi.BaoE = 1;
            BoLi.BaoFei = 595.38;
            Item.BoLi = BoLi;

            XianZhongUnit HuaHen = new XianZhongUnit();
            HuaHen.BaoE = 0;
            HuaHen.BaoFei = 0;
            Item.HuaHen = HuaHen;

            XianZhongUnit SheShui = new XianZhongUnit();
            SheShui.BaoE = 0;
            SheShui.BaoFei = 0;
            Item.SheShui = SheShui;

            //XianZhongUnit CheDeng = new XianZhongUnit();
            //CheDeng.BaoE = 0;
            //CheDeng.BaoFei = 0;
            //Item.CheDeng = CheDeng;

            XianZhongUnit ZiRan = new XianZhongUnit();
            ZiRan.BaoE = 0;
            ZiRan.BaoFei = 0;
            Item.ZiRan = ZiRan;

            XianZhongUnit BuJiMianChengKe = new XianZhongUnit();
            BuJiMianChengKe.BaoE = 0;
            BuJiMianChengKe.BaoFei = 0;
            Item.BuJiMianChengKe = BuJiMianChengKe;

            XianZhongUnit BuJiMianSiJi = new XianZhongUnit();
            BuJiMianSiJi.BaoE = 0;
            BuJiMianSiJi.BaoFei = 0;
            Item.BuJiMianSiJi = BuJiMianSiJi;

            XianZhongUnit BuJiMianHuaHen = new XianZhongUnit();
            BuJiMianHuaHen.BaoE = 0;
            BuJiMianHuaHen.BaoFei = 0;
            Item.BuJiMianHuaHen = BuJiMianHuaHen;

            XianZhongUnit BuJiMianSheShui = new XianZhongUnit();
            BuJiMianSheShui.BaoE = 0;
            BuJiMianSheShui.BaoFei = 0;
            Item.BuJiMianSheShui = BuJiMianSheShui;

            XianZhongUnit BuJiMianZiRan = new XianZhongUnit();
            BuJiMianZiRan.BaoE = 0;
            BuJiMianZiRan.BaoFei = 0;
            Item.BuJiMianZiRan = BuJiMianZiRan;

            XianZhongUnit BuJiMianJingShenSunShi = new XianZhongUnit();
            BuJiMianJingShenSunShi.BaoE = 0;
            BuJiMianJingShenSunShi.BaoFei = 0;
            Item.BuJiMianJingShenSunShi = BuJiMianJingShenSunShi;

            XianZhongUnit BuJiMianCheSun = new XianZhongUnit();
            BuJiMianCheSun.BaoE = 1;
            BuJiMianCheSun.BaoFei = 635.16;
            Item.BuJiMianCheSun = BuJiMianCheSun;

            XianZhongUnit BuJiMianSanZhe = new XianZhongUnit();
            BuJiMianSanZhe.BaoE = 1;
            BuJiMianSanZhe.BaoFei = 159.53;
            Item.BuJiMianSanZhe = BuJiMianSanZhe;

            XianZhongUnit BuJiMianDaoQiang = new XianZhongUnit();
            BuJiMianDaoQiang.BaoE = 1;
            BuJiMianDaoQiang.BaoFei = 291.69;
            Item.BuJiMianDaoQiang = BuJiMianDaoQiang;

            //XianZhongUnit BuJiMianRenYuan = new XianZhongUnit();
            //BuJiMianRenYuan.BaoE = 0;
            //BuJiMianRenYuan.BaoFei = 0;
            //Item.BuJiMianRenYuan = BuJiMianRenYuan;

            //XianZhongUnit BuJiMianFuJia = new XianZhongUnit();
            //BuJiMianFuJia.BaoE = 0;
            //BuJiMianFuJia.BaoFei = 0;
            //Item.BuJiMianFuJia = BuJiMianFuJia;

            XianZhongUnit HcSheBeiSunshi = new XianZhongUnit();
            HcSheBeiSunshi.BaoE = 0;
            HcSheBeiSunshi.BaoFei = 0;
            Item.HcSheBeiSunshi = HcSheBeiSunshi;

            XianZhongUnit HcHuoWuZeRen = new XianZhongUnit();
            HcHuoWuZeRen.BaoE = 0;
            HcHuoWuZeRen.BaoFei = 0;
            Item.HcHuoWuZeRen = HcHuoWuZeRen;

            XianZhongUnit HcFeiYongBuChang = new XianZhongUnit();
            HcFeiYongBuChang.BaoE = 0;
            HcFeiYongBuChang.BaoFei = 0;
            Item.HcFeiYongBuChang = HcFeiYongBuChang;

            XianZhongUnit HcJingShenSunShi = new XianZhongUnit();
            HcJingShenSunShi.BaoE = 0;
            HcJingShenSunShi.BaoFei = 0;
            Item.HcJingShenSunShi = HcJingShenSunShi;

            XianZhongUnit HcSanFangTeYue = new XianZhongUnit();
            HcSanFangTeYue.BaoE = 0;
            HcSanFangTeYue.BaoFei = 0;
            Item.HcSanFangTeYue = HcSanFangTeYue;

            XianZhongUnit HcXiuLiChang = new XianZhongUnit();
            HcXiuLiChang.BaoE = 0;
            HcXiuLiChang.BaoFei = 0;
            Item.HcXiuLiChang = HcXiuLiChang;

            Item.RateFactor1 = 0;
            Item.RateFactor2 = decimal.Parse("0.85");
            Item.RateFactor3 = decimal.Parse("0.85");
            Item.RateFactor4 = 0;

            viewModel.Item = Item;

            QuoteResultCarInfoViewModel CarInfo = new QuoteResultCarInfoViewModel();

            viewModel.CarInfo = CarInfo;

            viewModel.CustKey = request.CustKey;
            viewModel.CheckCode = request.CheckCode;

            return viewModel;
        }
    }
}
