using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.CacheServices;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class BjdService : CommonBehaviorService, IBjdService
    {
        #region
        private ISaveQuoteRepository _saveQuoteRepository;
        private ISubmitInfoRepository _submitInfoRepository;
        private IBaodanXianZhongRepository _baodanXianZhongRepository;
        private IBaodanxinxiRepository _baodanxinxiRepository;
        private IQuoteResultRepository _quoteResultRepository;
        private IQuoteReqCarinfoRepository _quoteReqCarinfoRepository;
        private IQuoteResultCarinfoRepository _quoteResultCarinfoRepository;
        private IUserInfoRepository _userInfoRepository;
        private IBxBjUnionRepository _bjxUnionRepository;
        private ILastInfoRepository _lastInfoRepository;
        private IAgentRepository _agentRepository;
        private ICarInfoRepository _carInfoRepository;
        private IAgentConfigRepository _agentConfigRepository;
        private IOrderRepository _orderRepository;
        private ICarRenewalRepository _carRenewalRepository;
        private IUserinfoRenewalInfoRepository _userinfoRenewalInfoRepository;
        private IConsumerReviewRepository _consumerReviewRepository;
        private IBxBjUnionRepository _bxBjUnionRepository;
        private IUserClaimRepository _userClaimRepository;
        private ITransferRecordRepository _transferRecordRepository;
        private IPreferentialActivityRepository _preferentialActivityRepository;
        private IGscRenewalRepository _gscRenewalRepository;
        private IImagesRepository _imagesRepository;
        private IHebaoDianweiRepository _hebaoDianweiRepository;
        private ICacheHelper _cacheHelper;
        private ILog logErr;
        private ILog logInfo;
        private ICarInsuranceCache _carInsuranceCache;
        private IPictureRepository _pictureRepository;
        private IGetMyBjdDetailService _getMyBjdDetailService;
        private IGetBjdDetailFromHistoryService _getBjdDetailFromHistoryService;


        public BjdService(ISaveQuoteRepository saveQuoteRepository,
            ISubmitInfoRepository submitInfoRepository,
            IBaodanXianZhongRepository baodanXianZhongRepository, IBaodanxinxiRepository baodanxinxiRepository, IQuoteResultRepository quoteResultRepository,
            IUserInfoRepository userInfoRepository, IQuoteReqCarinfoRepository quoteReqCarinfoRepository,
            IBxBjUnionRepository bjUnionRepository, ILastInfoRepository lastInfoRepository, IAgentRepository agentRepository, ICarInfoRepository carInfoRepository,
            IAgentConfigRepository agentConfigRepository, IQuoteResultCarinfoRepository quoteResultCarinfoRepository,
            IOrderRepository orderRepository, ICarRenewalRepository carRenewalRepository,
            IUserinfoRenewalInfoRepository userinfoRenewalInfoRepository, IBxBjUnionRepository bxBjUnionRepository,
            IUserClaimRepository userClaimRepository, ITransferRecordRepository transferRecordRepository,
            ICacheHelper cacheHelper, IConsumerReviewRepository consumerReviewRepository,
            IPreferentialActivityRepository preferentialActivityRepository, IGscRenewalRepository gscRenewalRepository, ICarInsuranceCache carInsuranceCache,
            IHebaoDianweiRepository hebaoDianweiRepository, IImagesRepository imagesRepository, IPictureRepository pictureRepository, IGetMyBjdDetailService getMyBjdDetailService,
            IGetBjdDetailFromHistoryService getBjdDetailFromHistoryService)
            : base(agentRepository, cacheHelper)
        {
            _getMyBjdDetailService = getMyBjdDetailService;
            _saveQuoteRepository = saveQuoteRepository;
            _quoteResultRepository = quoteResultRepository;
            _submitInfoRepository = submitInfoRepository;
            _baodanXianZhongRepository = baodanXianZhongRepository;
            _baodanxinxiRepository = baodanxinxiRepository;
            _userInfoRepository = userInfoRepository;
            _bjxUnionRepository = bjUnionRepository;
            _lastInfoRepository = lastInfoRepository;
            _agentRepository = agentRepository;
            _carInfoRepository = carInfoRepository;
            _agentConfigRepository = agentConfigRepository;
            _quoteReqCarinfoRepository = quoteReqCarinfoRepository;
            _quoteResultCarinfoRepository = quoteResultCarinfoRepository;
            _orderRepository = orderRepository;
            _carRenewalRepository = carRenewalRepository;
            _userinfoRenewalInfoRepository = userinfoRenewalInfoRepository;
            _bxBjUnionRepository = bxBjUnionRepository;
            _userClaimRepository = userClaimRepository;
            _transferRecordRepository = transferRecordRepository;
            _consumerReviewRepository = consumerReviewRepository;
            _preferentialActivityRepository = preferentialActivityRepository;
            _gscRenewalRepository = gscRenewalRepository;
            _imagesRepository = imagesRepository;
            _hebaoDianweiRepository = hebaoDianweiRepository;
            logErr = LogManager.GetLogger("ERROR");
            logInfo = LogManager.GetLogger("INFO");
            _carInsuranceCache = carInsuranceCache;
            _pictureRepository = pictureRepository;
            _getBjdDetailFromHistoryService = getBjdDetailFromHistoryService;
        }

        #endregion

        public long UpdateBjdInfo(CreateOrUpdateBjdInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            //校验
            #region
            //bx_claim_detail
            long count = 0;

            //20160905修改source1248=>0123，传入的新数据转换
            var submitinfo = _submitInfoRepository.GetSubmitInfo(request.Buid, SourceGroupAlgorithm.GetOldSource(request.Source));
            if (submitinfo == null)
            {
                logInfo.Info(string.Format("Buid为{0}Source为{1}的记录submitinfo为空", request.Buid, request.Source));
                return 0;
            }

            var quoteinfo = _quoteResultRepository.GetQuoteResultByBuid(request.Buid, SourceGroupAlgorithm.GetOldSource(request.Source));
            if (quoteinfo == null)
            {
                logInfo.Info(string.Format("Buid为{0}Source为{1}的记录quoteinfo为空", request.Buid, request.Source));
                return 0;
            }


            var saveinfo = _saveQuoteRepository.GetSavequoteByBuid(request.Buid);
            if (saveinfo == null)
            {
                logInfo.Info(string.Format("Buid为{0}Source为{1}的记录saveinfo为空", request.Buid, request.Source));
                return 0;
            }

            var userinfo = _userInfoRepository.FindByBuid(request.Buid);
            if (userinfo == null)
            {
                logInfo.Info(string.Format("Buid为{0}Source为{1}的记录userinfo为空", request.Buid, request.Source));
                return 0;
            }
            #endregion

            var lastinfo = _lastInfoRepository.GetByBuid(request.Buid);
            var agentinfo = _agentRepository.GetAgent(userinfo.Agent);
            var quotecarinfo = _quoteResultCarinfoRepository.Find(request.Buid, SourceGroupAlgorithm.GetOldSource(request.Source));
            var listClaim = _userClaimRepository.FindList(request.Buid);
            #region 20171103  L：商业出险次数、商业出险金额、交强出险次数、交强出险金额
            int loss_biz_count = listClaim.Where(n => n.pay_type == 0).ToList().Count;  //商业出险次数
            double? loss_biz_amount = listClaim.Where(n => n.pay_type == 0).ToList().Sum(n => n.pay_amount);  //商业出险金额
            int loss_force_count = listClaim.Where(n => n.pay_type == 1).ToList().Count;  //交强出险次数
            double? loss_force_amount = listClaim.Where(n => n.pay_type == 1).ToList().Sum(n => n.pay_amount); //交强出险金额
            #endregion

            try
            {
                //新增
                if (request.BxId == 0)
                {
                    //单独写一个接口  实现

                    bx_preferential_activity model = new bx_preferential_activity();
                    if (!string.IsNullOrWhiteSpace(request.ActivityContent))
                    {
                        bx_preferential_activity modelactivity = _preferentialActivityRepository.GetListByType(5,
                            request.ActivityContent);
                        if (modelactivity != null)
                        {
                            model = modelactivity;
                        }
                        else
                        {
                            model.top_agent_id = request.Agent;
                            model.agent_id = request.ChildAgent;
                            model.activity_type = 5;
                            model.activity_name = "";
                            model.activity_content = request.ActivityContent;
                            model.activity_status = 1;
                            model.create_time = DateTime.Now;
                            model.create_name = agentinfo == null ? "" : agentinfo.AgentName;
                            model.modify_time = DateTime.Now;
                            model.modify_name = agentinfo == null ? "" : agentinfo.AgentName;
                            _preferentialActivityRepository.AddActivity(model);
                        }
                    }

                    //单独写一个接口  实现

                    var baodanxinxi = new bj_baodanxinxi
                    {
                        //BEGIN 2017-09-06新增字段   20170909L 当前后期修改中去掉了代理人信息（电话等）
                        AgentId = request.ChildAgent == 0 ? int.Parse(userinfo.Agent) : request.ChildAgent,
                        //END

                        //BEGIN 2017-09-09新增字段
                        //activity_content = request.ActivityContent,
                        activity_ids = model.id.ToString(),
                        //END


                        // AgentId = int.Parse(userinfo.Agent),
                        BizEndDate = lastinfo == null ? DateTime.MinValue : Convert.ToDateTime(lastinfo.last_business_end_date), //submitinfo.biz_end_time,
                        BizNum = submitinfo.biz_tno,
                        BizPrice = quoteinfo.BizTotal.HasValue ? quoteinfo.BizTotal.Value : 0,
                        BizRate = double.Parse(submitinfo.biz_rate.HasValue ? submitinfo.biz_rate.Value.ToString() : "0"),
                        BizStartDate = submitinfo.biz_start_time,
                        CarBrandModel = userinfo.MoldName,
                        CarEngineNo = userinfo.EngineNo,
                        CarLicense = userinfo.LicenseNo,
                        CarOwner = userinfo.LicenseOwner,
                        CarRegisterDate = string.IsNullOrEmpty(userinfo.RegisterDate) ? DateTime.MinValue : DateTime.Parse(userinfo.RegisterDate),
                        CarVIN = userinfo.CarVIN,
                        ChannelId = submitinfo.channel_id,
                        CompanyId = submitinfo.source,
                        CreateTime = DateTime.Now,
                        ForceEndDate = lastinfo == null ? DateTime.MinValue : Convert.ToDateTime(lastinfo.last_end_date),//submitinfo.force_end_time,
                        ForceStartDate = submitinfo.force_start_time,
                        ForceNum = submitinfo.force_tno,
                        ForcePrice = (quoteinfo.ForceTotal.HasValue ? quoteinfo.ForceTotal.Value : 0),
                        ForceRate =
                            double.Parse(submitinfo.force_rate.HasValue ? submitinfo.force_rate.Value.ToString() : "0"),
                        InsuredName = userinfo.InsuredName,
                        InsureIdNum = userinfo.InsuredIdCard,
                        ManualBizRate = request.BizRate,
                        ManualTaxRate = request.TaxRate,
                        ManualForceRate = request.ForceRate,
                        ObjectId = int.Parse(userinfo.Agent),
                        ObjectType = 1,
                        SubmitStatus = submitinfo.submit_status,
                        TaxPrice = quoteinfo.TaxTotal.HasValue ? quoteinfo.TaxTotal.Value : 0,

                        //新增的4个费率
                        NonClaimRate = (double)(quoteinfo.RateFactor1.HasValue ? quoteinfo.RateFactor1 : 0),
                        MultiDiscountRate = (double)(quoteinfo.RateFactor2.HasValue ? quoteinfo.RateFactor2 : 0),
                        AvgMileRate = (double)(quoteinfo.RateFactor3.HasValue ? quoteinfo.RateFactor3 : 0),
                        RiskRate = (double)(quoteinfo.RateFactor4.HasValue ? quoteinfo.RateFactor4 : 0),
                        //总费率系数
                        TotalRate = quoteinfo.TotalRate.HasValue ? quoteinfo.TotalRate.Value.ToString() : "0",
                        //20170221新增增值税
                        AddValueTaxRate = request.AddValueTaxRate,
                        //载客//+载质量
                        CarSeated = quotecarinfo != null ? (quotecarinfo.seat_count.HasValue ? quotecarinfo.seat_count.Value.ToString() : "0") : "0",
                        //+ "/" + (quotecarinfo.car_equ_quality.HasValue ? quotecarinfo.car_equ_quality.Value.ToString() : "0")
                        VehicleInfo = VehicleInfoMapper.VehicleInfoMethod(quotecarinfo),
                        JqVehicleClaimType = quotecarinfo != null ? quotecarinfo.JqVehicleClaimType : "",
                        SyVehicleClaimType = quotecarinfo != null ? quotecarinfo.SyVehicleClaimType : "",
                        loss_biz_count = loss_biz_count,
                        loss_biz_amount = loss_biz_amount.HasValue ? loss_biz_amount.Value : 0,
                        loss_force_count = loss_force_count,
                        loss_force_amount = loss_force_amount.HasValue ? loss_force_amount.Value : 0
                    };
                    var item = _baodanxinxiRepository.Add(baodanxinxi);

                    Task.Factory.StartNew(() =>
                    {
                        AddCrmSteps(request.ChildAgent, agentinfo == null ? "" : agentinfo.AgentName, "",
                            userinfo.LicenseNo, request.Source, request.BizRate, request.ForceRate,
                            model.id, request.Buid, item.Id, request.CityCode);
                    });

                    var baodanxianzhong = new bj_baodanxianzhong()
                    {
                        BaoDanXinXiId = item.Id,
                        BizTotal = quoteinfo.BizTotal.HasValue ? quoteinfo.BizTotal.Value : 0,
                        BoLiBaoE = saveinfo.BoLi,
                        BoLiBaoFei = quoteinfo.BoLi.HasValue ? quoteinfo.BoLi.Value : 0,
                        BuJiMianCheSun =
                            quoteinfo.BuJiMianCheSun.HasValue ? quoteinfo.BuJiMianCheSun.Value : 0,
                        BuJiMianDaoQiang =
                            quoteinfo.BuJiMianDaoQiang.HasValue ? quoteinfo.BuJiMianDaoQiang.Value : 0,
                        BuJiMianFuJian =
                            quoteinfo.BuJiMianFuJian.HasValue ? quoteinfo.BuJiMianFuJian.Value : 0,
                        BuJiMianRenYuan =
                            quoteinfo.BuJiMianRenYuan.HasValue ? quoteinfo.BuJiMianRenYuan.Value : 0,
                        BuJiMianSanZhe =
                            quoteinfo.BuJiMianSanZhe.HasValue ? quoteinfo.BuJiMianSanZhe.Value : 0,

                        //2.1.5版本修改 新增6个字段
                        BuJiMianChengKe =
                        quoteinfo.BuJiMianChengKe.HasValue ? quoteinfo.BuJiMianChengKe.Value : 0,
                        BuJiMianSiJi =
                            quoteinfo.BuJiMianSiJi.HasValue ? quoteinfo.BuJiMianSiJi.Value : 0,
                        BuJiMianHuaHen =
                        quoteinfo.BuJiMianHuaHen.HasValue ? quoteinfo.BuJiMianHuaHen.Value : 0,
                        BuJiMianSheShui =
                            quoteinfo.BuJiMianSheShui.HasValue ? quoteinfo.BuJiMianSheShui.Value : 0,
                        BuJiMianZiRan =
                        quoteinfo.BuJiMianZiRan.HasValue ? quoteinfo.BuJiMianZiRan.Value : 0,
                        BuJiMianJingShenSunShi =
                            quoteinfo.BuJiMianJingShenSunShi.HasValue ? quoteinfo.BuJiMianJingShenSunShi.Value : 0,

                        SanFangTeYueBaoE = saveinfo.HcSanFangTeYue.HasValue ? saveinfo.HcSanFangTeYue.Value : 0,
                        SanFangTeYueBaoFei = quoteinfo.HcSanFangTeYue.HasValue ? quoteinfo.HcSanFangTeYue.Value : 0,

                        JingShenSunShiBaoE = saveinfo.HcJingShenSunShi.HasValue ? saveinfo.HcJingShenSunShi.Value : 0,
                        JingShenSunShiBaoFei = quoteinfo.HcJingShenSunShi.HasValue ? quoteinfo.HcJingShenSunShi.Value : 0,

                        HuoWuZeRenBaoE = saveinfo.HcHuoWuZeRen.HasValue ? saveinfo.HcHuoWuZeRen.Value : 0,
                        HuoWuZeRenBaoFei = quoteinfo.HcHuoWuZeRen.HasValue ? quoteinfo.HcHuoWuZeRen.Value : 0,

                        //设备损失
                        SheBeiSunShiBaoE = saveinfo.HcSheBeiSunshi.HasValue ? saveinfo.HcSheBeiSunshi.Value : 0,
                        SheBeiSunShiBaoFei = quoteinfo.HcSheBeiSunshi.HasValue ? quoteinfo.HcSheBeiSunshi.Value : 0,
                        //不计免设备损失
                        BuJiMianSheBeiSunShiBaoE = saveinfo.BuJiMianSheBeiSunshi.HasValue ? saveinfo.BuJiMianSheBeiSunshi.Value : 0,
                        BuJiMianSheBeiSunShiBaoFei = quoteinfo.BuJiMianSheBeiSunshi.HasValue ? quoteinfo.BuJiMianSheBeiSunshi.Value : 0,

                        XiuLiChangBaoE = saveinfo.HcXiuLiChang,
                        XiuLiChangBaoFei = quoteinfo.HcXiuLiChang.HasValue ? quoteinfo.HcXiuLiChang.Value : 0,

                        FeiYongBuChangBaoE = saveinfo.HcFeiYongBuChang,
                        FeiYongBuChangBaoFei = quoteinfo.HcFeiYongBuChang.HasValue ? quoteinfo.HcFeiYongBuChang.Value : 0,
                        //2.1.5修改结束
                        //费用补偿天数
                        FybcDays = saveinfo.FeiYongBuChangDays.HasValue ? saveinfo.FeiYongBuChangDays.Value : 0,

                        CheDengBaoE = saveinfo.CheDeng,
                        CheDengBaoFei = quoteinfo.CheDeng.HasValue ? quoteinfo.CheDeng.Value : 0,
                        ChengKeBaoE = saveinfo.ChengKe,
                        ChengKeBaoFei = quoteinfo.ChengKe.HasValue ? quoteinfo.ChengKe.Value : 0,
                        //CheSunBaoE = saveinfo.CheSun,//车损保额修改quoteresult
                        CheSunBaoE = quoteinfo.CheSunBE.HasValue ? quoteinfo.CheSunBE.Value : 0,
                        CheSunBaoFei = quoteinfo.CheSun.HasValue ? quoteinfo.CheSun.Value : 0,
                        //DaoQiangBaoE = saveinfo.DaoQiang,//盗抢保额修改quoteresult
                        DaoQiangBaoE = quoteinfo.DaoQiangBE.HasValue ? quoteinfo.DaoQiangBE : 0,
                        DaoQiangBaoFei = quoteinfo.DaoQiang.HasValue ? quoteinfo.DaoQiang.Value : 0,
                        ForceTotal = quoteinfo.ForceTotal.HasValue ? quoteinfo.ForceTotal.Value : 0,
                        HuaHenBaoE = saveinfo.HuaHen,
                        HuaHenBaoFei = quoteinfo.HuaHen.HasValue ? quoteinfo.HuaHen.Value : 0,
                        SanZheBaoE = saveinfo.SanZhe,
                        SanZheBaoFei = quoteinfo.SanZhe.HasValue ? quoteinfo.SanZhe.Value : 0,
                        SheShuiBaoE = saveinfo.SheShui,
                        SheShuiBaoFei = quoteinfo.SheShui.HasValue ? quoteinfo.SheShui.Value : 0,
                        SiJiBaoE = saveinfo.SiJi,
                        SiJiBaoFei = quoteinfo.SiJi.HasValue ? quoteinfo.SiJi.Value : 0,
                        Source = submitinfo.source,
                        TaxTotal = quoteinfo.TaxTotal.HasValue ? quoteinfo.TaxTotal.Value : 0,
                        //ZiRanBaoE = saveinfo.ZiRan,//自燃保额修改quoteresult
                        ZiRanBaoE = quoteinfo.ZiRanBE.HasValue ? quoteinfo.ZiRanBE.Value : 0,
                        ZiRanBaoFei = quoteinfo.ZiRan.HasValue ? quoteinfo.ZiRan.Value : 0,
                        CreateTime = DateTime.Now
                    };
                    _baodanXianZhongRepository.Add(baodanxianzhong);

                    _bjxUnionRepository.Add(request.Buid, item.Id);
                    count = item.Id;
                }
                else
                {
                    var bjinfo = _baodanxinxiRepository.Find(request.BxId);
                    var bxinfo = _baodanXianZhongRepository.Find(request.BxId);

                    //////BEGIN 2017-09-06新增字段//////
                    bjinfo.AgentId = request.ChildAgent == 0 ? int.Parse(userinfo.Agent) : request.ChildAgent;
                    bjinfo.activity_ids = "";
                    //////END 2017-09-06新增字段//////

                    bjinfo.BizEndDate = submitinfo.biz_end_time;
                    bjinfo.BizNum = submitinfo.biz_tno;
                    bjinfo.BizPrice = quoteinfo.BizTotal.HasValue ? quoteinfo.BizTotal.Value : 0;
                    bjinfo.BizRate = double.Parse(submitinfo.biz_rate.HasValue ? submitinfo.biz_rate.Value.ToString() : "0");
                    bjinfo.BizStartDate = submitinfo.biz_start_time;
                    bjinfo.CarBrandModel = userinfo.MoldName;
                    bjinfo.CarEngineNo = userinfo.EngineNo;
                    bjinfo.CarLicense = userinfo.LicenseNo;
                    bjinfo.CarOwner = userinfo.LicenseOwner;
                    if (string.IsNullOrEmpty(userinfo.RegisterDate))
                    {
                        bjinfo.CarRegisterDate = DateTime.Parse(userinfo.RegisterDate);
                    }
                    bjinfo.CarVIN = userinfo.CarVIN;
                    bjinfo.ChannelId = submitinfo.channel_id;
                    bjinfo.CompanyId = submitinfo.source.HasValue ? (int)SourceGroupAlgorithm.GetNewSource(submitinfo.source.Value) : 0;
                    bjinfo.CreateTime = DateTime.Now;
                    bjinfo.ForceEndDate = submitinfo.force_end_time;
                    bjinfo.ForceStartDate = submitinfo.force_start_time;
                    bjinfo.ForceNum = submitinfo.force_tno;
                    bjinfo.ForcePrice = quoteinfo.ForceTotal.HasValue ? quoteinfo.ForceTotal.Value : 0;
                    bjinfo.ForceRate =
                        double.Parse(submitinfo.force_rate.HasValue ? submitinfo.force_rate.Value.ToString() : "0");
                    bjinfo.InsuredName = userinfo.InsuredName;
                    bjinfo.InsureIdNum = userinfo.InsuredIdCard;
                    bjinfo.ManualBizRate = request.BizRate;
                    bjinfo.ManualTaxRate = request.TaxRate;
                    bjinfo.ManualForceRate = request.ForceRate;
                    bjinfo.ObjectId = int.Parse(userinfo.Agent);
                    bjinfo.ObjectType = 1;
                    bjinfo.SubmitStatus = submitinfo.submit_status;
                    bjinfo.TaxPrice = quoteinfo.TaxTotal.HasValue ? quoteinfo.TaxTotal.Value : 0;

                    //新增的4个费率
                    bjinfo.NonClaimRate = (double)(quoteinfo.RateFactor1.HasValue ? quoteinfo.RateFactor1 : 0);
                    bjinfo.MultiDiscountRate = (double)(quoteinfo.RateFactor2.HasValue ? quoteinfo.RateFactor2 : 0);
                    bjinfo.AvgMileRate = (double)(quoteinfo.RateFactor3.HasValue ? quoteinfo.RateFactor3 : 0);
                    bjinfo.RiskRate = (double)(quoteinfo.RateFactor4.HasValue ? quoteinfo.RateFactor4 : 0);
                    //总费率系数
                    bjinfo.TotalRate = quoteinfo.TotalRate.HasValue ? quoteinfo.TotalRate.Value.ToString() : "0";
                    //20170221新增增值税
                    bjinfo.AddValueTaxRate = request.AddValueTaxRate;
                    //载客+载质量
                    bjinfo.CarSeated = quotecarinfo != null ? (quotecarinfo.seat_count.HasValue
                        ? quotecarinfo.seat_count.Value.ToString()
                        : "0") : "0";// + "/" +(quotecarinfo.car_equ_quality.HasValue? quotecarinfo.car_equ_quality.Value.ToString(): "0");

                    bjinfo.VehicleInfo = VehicleInfoMapper.VehicleInfoMethod(quotecarinfo);
                    bjinfo.JqVehicleClaimType = quotecarinfo != null ? quotecarinfo.JqVehicleClaimType : "";
                    bjinfo.SyVehicleClaimType = quotecarinfo != null ? quotecarinfo.SyVehicleClaimType : "";

                    _baodanxinxiRepository.Update(bjinfo);

                    bxinfo.BizTotal = quoteinfo.BizTotal.HasValue ? quoteinfo.BizTotal.Value : 0;
                    bxinfo.BoLiBaoE = saveinfo.BoLi;
                    bxinfo.BoLiBaoFei = quoteinfo.BoLi.HasValue ? quoteinfo.BoLi.Value : 0;
                    bxinfo.BuJiMianCheSun =
                        quoteinfo.BuJiMianCheSun.HasValue ? quoteinfo.BuJiMianCheSun.Value : 0;
                    bxinfo.BuJiMianDaoQiang =
                        quoteinfo.BuJiMianDaoQiang.HasValue ? quoteinfo.BuJiMianDaoQiang.Value : 0;
                    bxinfo.BuJiMianFuJian =
                        quoteinfo.BuJiMianFuJian.HasValue ? quoteinfo.BuJiMianFuJian.Value : 0;
                    bxinfo.BuJiMianRenYuan =
                        quoteinfo.BuJiMianRenYuan.HasValue ? quoteinfo.BuJiMianRenYuan.Value : 0;
                    bxinfo.BuJiMianSanZhe =
                        quoteinfo.BuJiMianSanZhe.HasValue ? quoteinfo.BuJiMianSanZhe.Value : 0;
                    bxinfo.CheDengBaoE = saveinfo.CheDeng;
                    bxinfo.CheDengBaoFei = quoteinfo.CheDeng.HasValue ? quoteinfo.CheDeng.Value : 0;
                    bxinfo.ChengKeBaoE = saveinfo.ChengKe;
                    bxinfo.ChengKeBaoFei = quoteinfo.ChengKe.HasValue ? quoteinfo.ChengKe.Value : 0;
                    bxinfo.CheSunBaoE = saveinfo.CheSun;
                    bxinfo.CheSunBaoFei = quoteinfo.CheSun.HasValue ? quoteinfo.CheSun.Value : 0;
                    bxinfo.DaoQiangBaoE = saveinfo.DaoQiang;
                    bxinfo.DaoQiangBaoFei = quoteinfo.DaoQiang.HasValue ? quoteinfo.DaoQiang.Value : 0;
                    bxinfo.ForceTotal = quoteinfo.ForceTotal.HasValue ? quoteinfo.ForceTotal.Value : 0;
                    bxinfo.HuaHenBaoE = saveinfo.HuaHen;
                    bxinfo.HuaHenBaoFei = quoteinfo.HuaHen.HasValue ? quoteinfo.HuaHen.Value : 0;
                    bxinfo.SanZheBaoE = saveinfo.SanZhe;
                    bxinfo.SanZheBaoFei = quoteinfo.SanZhe.HasValue ? quoteinfo.SanZhe.Value : 0;
                    bxinfo.SheShuiBaoE = saveinfo.SheShui;
                    bxinfo.SheShuiBaoFei = quoteinfo.SheShui.HasValue ? quoteinfo.SheShui.Value : 0;
                    bxinfo.SiJiBaoE = saveinfo.SiJi;
                    bxinfo.SiJiBaoFei = quoteinfo.SiJi.HasValue ? quoteinfo.SiJi.Value : 0;
                    //20160905修改source0123=>1248，数据库里返回的老数据转换
                    bxinfo.Source = submitinfo.source.HasValue ? (int)SourceGroupAlgorithm.GetNewSource(submitinfo.source.Value) : 0;
                    bxinfo.TaxTotal = quoteinfo.TaxTotal.HasValue ? quoteinfo.TaxTotal.Value : 0;
                    bxinfo.ZiRanBaoE = saveinfo.ZiRan;
                    bxinfo.ZiRanBaoFei = quoteinfo.ZiRan.HasValue ? quoteinfo.ZiRan.Value : 0;

                    //2.1.5版本修改 新增6个字段
                    bxinfo.BuJiMianChengKe = quoteinfo.BuJiMianChengKe.HasValue ? quoteinfo.BuJiMianChengKe.Value : 0;
                    bxinfo.BuJiMianSiJi = quoteinfo.BuJiMianSiJi.HasValue ? quoteinfo.BuJiMianSiJi.Value : 0;
                    bxinfo.BuJiMianHuaHen = quoteinfo.BuJiMianHuaHen.HasValue ? quoteinfo.BuJiMianHuaHen.Value : 0;
                    bxinfo.BuJiMianSheShui = quoteinfo.BuJiMianSheShui.HasValue ? quoteinfo.BuJiMianSheShui.Value : 0;
                    bxinfo.BuJiMianZiRan = quoteinfo.BuJiMianZiRan.HasValue ? quoteinfo.BuJiMianZiRan.Value : 0;
                    bxinfo.BuJiMianJingShenSunShi = quoteinfo.BuJiMianJingShenSunShi.HasValue ? quoteinfo.BuJiMianJingShenSunShi.Value : 0;
                    bxinfo.SanFangTeYueBaoE = saveinfo.HcSanFangTeYue.HasValue ? saveinfo.HcSanFangTeYue.Value : 0;
                    bxinfo.SanFangTeYueBaoFei = quoteinfo.HcSanFangTeYue.HasValue ? quoteinfo.HcSanFangTeYue.Value : 0;
                    bxinfo.JingShenSunShiBaoE = saveinfo.HcJingShenSunShi.HasValue ? saveinfo.HcJingShenSunShi.Value : 0;
                    bxinfo.JingShenSunShiBaoFei = quoteinfo.HcJingShenSunShi.HasValue ? quoteinfo.HcJingShenSunShi.Value : 0;
                    bxinfo.HuoWuZeRenBaoE = saveinfo.HcHuoWuZeRen.HasValue ? saveinfo.HcHuoWuZeRen.Value : 0;
                    bxinfo.HuoWuZeRenBaoFei = quoteinfo.HcHuoWuZeRen.HasValue ? quoteinfo.HcHuoWuZeRen.Value : 0;
                    //设备损失
                    bxinfo.SheBeiSunShiBaoE = saveinfo.HcSheBeiSunshi.HasValue ? saveinfo.HcSheBeiSunshi.Value : 0;
                    bxinfo.SheBeiSunShiBaoFei = quoteinfo.HcSheBeiSunshi.HasValue ? quoteinfo.HcSheBeiSunshi.Value : 0;
                    //不计免设备损失
                    bxinfo.BuJiMianSheBeiSunShiBaoE = saveinfo.BuJiMianSheBeiSunshi.HasValue ? saveinfo.BuJiMianSheBeiSunshi.Value : 0;
                    bxinfo.BuJiMianSheBeiSunShiBaoFei = quoteinfo.BuJiMianSheBeiSunshi.HasValue ? quoteinfo.BuJiMianSheBeiSunshi.Value : 0;
                    bxinfo.XiuLiChangBaoE = saveinfo.HcXiuLiChang;
                    bxinfo.XiuLiChangBaoFei = quoteinfo.HcXiuLiChang.HasValue ? quoteinfo.HcXiuLiChang.Value : 0;
                    bxinfo.FeiYongBuChangBaoE = saveinfo.HcFeiYongBuChang;
                    bxinfo.FeiYongBuChangBaoFei = quoteinfo.HcFeiYongBuChang.HasValue ? quoteinfo.HcFeiYongBuChang.Value : 0;
                    //2.1.5修改结束
                    //费用补偿天数
                    bxinfo.FybcDays = saveinfo.FeiYongBuChangDays.HasValue ? saveinfo.FeiYongBuChangDays.Value : 0;
                    bxinfo.CreateTime = DateTime.Now;

                    _baodanXianZhongRepository.Update(bxinfo);
                    count = request.BxId;
                }
            }
            catch (Exception ex)
            {
                logErr.Info("创建报价单发生异常，请求串为：" + request.ToJson() + "/n错误信息：" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }

            return count;
        }

        #region 新增步骤操作抵用方法
        private static string GetSourceName(long newSource)
        {
            if (newSource == 2147483648)
            {
                return "恒邦车险";
            }
            if (newSource == 4294967296)
            {
                return "中铁车险";
            }
            if (newSource == 8589934592)
            {
                return "美亚车险";
            }
            if (newSource == 17179869184)
            {
                return "富邦车险";
            }
            return ToEnumDescription(newSource, typeof(EnumSourceNew));
        }

        private static String ToEnumDescription(long value, Type enumType)
        {
            NameValueCollection nvc = GetNvcFromEnumValue(enumType);
            return nvc[value.ToString()];
        }

        private static NameValueCollection GetNvcFromEnumValue(Type enumType)
        {
            var nvc = new NameValueCollection();
            Type typeDescription = typeof(DescriptionAttribute);
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            string strValue = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        var aa = (DescriptionAttribute)arr[0];
                        strText = aa.Description;
                    }
                    else
                    {
                        strText = "";
                    }
                    nvc.Add(strValue, strText);
                }
            }
            return nvc;
        }

        /// <summary>
        /// 新增分享报价单步骤
        /// </summary>
        /// <param name="agentId">代理人ID</param>
        /// <param name="agentName">代理人姓名</param>
        /// <param name="mobile">手机号</param>
        /// <param name="licenseNo">车牌号</param>
        /// <param name="newSource">来源</param>
        /// <param name="bizRate">商业险折扣率</param>
        /// <param name="forceRate">交强险折扣率</param>
        /// <param name="activityId">优惠活动ID</param>
        /// <param name="buid">用户ID</param>
        /// <param name="bxid">报价单信息ID</param>
        private void AddCrmSteps(int agentId, string agentName, string mobile, string licenseNo, long newSource, double bizRate, double forceRate, int activityId, long buid, long bxid, int cityCode)
        {
            try
            {

                var crmTimeLineForSmsViewModel = new CrmTimeLineForSmsViewModel
                {
                    agent_id = agentId,
                    agent_name = agentName,
                    content = "",
                    sent_mobile = mobile,
                    sent_type = 0,
                    license_no = licenseNo,
                    Source = newSource,
                    sourceName = GetSourceName(newSource),
                    BizRate = bizRate,
                    ForceRate = forceRate,
                    ActivityId = activityId,
                    Bxid = bxid,
                    CityId = cityCode
                };

                string host = ConfigurationManager.AppSettings["SystemCrmUrl"];
                string url = string.Format("{0}/api/ConsumerDetail/AddCrmSteps", host);
                string postDataNoSecCode =
                    string.Format(
                        "JsonContent={0}&AgentId={1}&Type=11&BUid={2}",
                        crmTimeLineForSmsViewModel.ToJson(), agentId, buid);
                string res = String.Empty;
                string secCode = postDataNoSecCode.GetMd5();
                string postData = postDataNoSecCode + "&SecCode=" + secCode;
                //记录请求
                logInfo.Info("续保调用分配接口：Url:" + url + "，请求：" + postData);
                using (var client = new HttpClient(new HttpClientHandler()))
                {
                    HttpContent content = new StringContent(postData);
                    var typeHeader = new MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = "UTF-8" };
                    content.Headers.ContentType = typeHeader;
                    var response = client.PostAsync(url, content).Result;
                }
            }
            catch (Exception ex)
            {
                logErr.Info("续保请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
        }

        #endregion


        public GetBjdItemResponse GetBjdInfo(GetBjdItemRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new GetBjdItemResponse();
            try
            {
                var baodanxinxi = _baodanxinxiRepository.Find(request.Bxid);
                var baodanxianzhong = _baodanXianZhongRepository.Find(request.Bxid);

                var preActivity = new List<bx_preferential_activity>();
                if (baodanxinxi != null && !string.IsNullOrEmpty(baodanxinxi.activity_ids))
                {
                    preActivity = _preferentialActivityRepository.GetActivityByIds(baodanxinxi.activity_ids);
                }

                bx_bj_union union = _bxBjUnionRepository.Find(request.Bxid);
                if (union != null)
                {
                    response.ClaimDetail = _userClaimRepository.FindList(union.b_uid);
                    response.Savequote = _saveQuoteRepository.GetSavequoteByBuid(union.b_uid);
                }
                response.Activitys = preActivity;
                response.Baodanxinxi = baodanxinxi;
                response.Baodanxianzhong = baodanxianzhong;
            }
            catch (Exception ex)
            {
                logErr.Info("获取分享报价单单发生异常，请求串为：" + request.ToJson() + "/n错误信息：" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);

            }


            return response;
        }

        /// <summary>
        /// 获取我的报价单接口
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public List<MyBaoJiaViewModel> GetMyList(GetMyBjdListRequest request, IEnumerable<KeyValuePair<string, string>> pairs, out int totalCount)
        {
            var list = new List<MyBaoJiaViewModel>();
            //查询条件
            var sbWhere = new StringBuilder();
            //获取当前代理人Id，因为有直客和代理人区分，所以不直接传代理ID
            int curAgentId = request.ChildAgent;
            //_agentRepository.GetAgentId(request.OpenId, request.TopParentAgent.ToString());
            if (curAgentId != 0)
            {
                //如果当前用户为代理人
                if (request.IsOnlyMine.HasValue)
                {
                    if (request.IsOnlyMine.Value == 0)
                    {
                        //查自己及下级的所有数据
                        sbWhere.Append(" AND Agent in (")
                            .Append(_agentRepository.GetSonsIdToString(curAgentId, true))
                            .Append(") ");
                    }
                    else
                    {
                        sbWhere.Append(" AND Agent ='")
                            .Append(curAgentId)
                            .Append("' ");
                    }
                }
                else
                {
                    //只查自己的数据
                    sbWhere.Append(" AND Agent ='")
                        .Append(curAgentId)
                        .Append("' ");
                }
            }
            else
            {
                //如果当前用户为直客
                sbWhere.Append(" AND OpenId ='")
                        .Append(request.OpenId)
                        .Append("' ");
            }
            if (!string.IsNullOrWhiteSpace(request.LicenseNo))
            {
                //20170310已于产品沟通，由于查车主太慢，故将其舍去
                //sbWhere.Append(string.Format(" AND ( LicenseNo = '{0}' OR LicenseOwner = '{0}' ) ", request.LicenseNo.Trim().ToUpper()));
                sbWhere.Append(string.Format(" AND LicenseNo like '{0}%' ", request.LicenseNo.Trim().ToUpper()));
            }
            //获取我的报价单列表
            List<bx_userinfo> userinfo = _userInfoRepository.GetMyBjdList(false, sbWhere.ToString(), request.PageSize, request.CurPage, out totalCount);

            if (userinfo.Count > 0)
            {
                MyBaoJiaViewModel my;
                List<bx_quoteresult_carinfo> quoteresultCarinfo;
                List<MyPrecisePriceItemViewModel> listquoteTotal;

                //根据顶级代理人获取代理资源
                List<long> sourceInt = _agentConfigRepository.FindSource(request.TopParentAgent);
                List<long> sourceList;

                //报价单资源对象
                bx_savequote sq;
                List<bx_quoteresult> qrList;
                List<bx_submit_info> siList;


                foreach (var item in userinfo)
                {
                    #region 基础属性
                    my = new MyBaoJiaViewModel();

                    //判断在该报价单在预约单中是否有记录
                    CarOrderStatusModel bxCarOrder = _orderRepository.GetOrderStatus(item.Id);
                    if (bxCarOrder != null)
                    {
                        my.HasOrder = 1;
                        my.OrderId = bxCarOrder.Id;
                        my.OrderStatus = bxCarOrder.OrderStatus.HasValue ? bxCarOrder.OrderStatus.Value : 0;
                    }
                    else
                    {
                        my.HasOrder = 0;
                        my.OrderId = 0;
                        my.OrderStatus = 0;
                    }

                    //20160905修改source，去掉source
                    //取车辆信息直接根据buid来取
                    //if (item.Source.HasValue)
                    //{
                    quoteresultCarinfo = _quoteResultCarinfoRepository.FindList(item.Id);//, item.Source.Value
                    var firstQuoteResult = quoteresultCarinfo.FirstOrDefault();
                    my.CarUsedType = firstQuoteResult != null ? firstQuoteResult.car_used_type : null;
                    my.SeatCount = firstQuoteResult != null ? firstQuoteResult.seat_count : null;
                    my.CarInfos = new List<DiffCarInfo>();
                    foreach (var car in quoteresultCarinfo)
                    {
                        my.CarInfos.Add(new DiffCarInfo
                        {
                            Source = car.source.HasValue ? SourceGroupAlgorithm.GetNewSource(car.source.Value) : 0,
                            AutoMoldCode = string.IsNullOrWhiteSpace(car.auto_model_code) ? string.Empty : car.auto_model_code,
                            CarSeat = car.seat_count.HasValue ? car.seat_count.Value : 0,
                            VehicleInfo = VehicleInfoMapper.VehicleInfoMethod(car)//,
                            //Risk = car.risk,
                            //XinZhuanXu = car.IsZhuanXubao,
                            //SyVehicleClaimType = car.SyVehicleClaimType,
                            //JqVehicleClaimType = car.JqVehicleClaimType,
                            //VehicleStyle = car.VehicleStyle
                        });
                    }

                    //}
                    if (!string.IsNullOrEmpty(item.LicenseNo))
                    {
                        my.LicenseNo = item.LicenseNo;
                        var carInfo = _carInfoRepository.Find(item.LicenseNo);
                        my.PurchasePrice = carInfo != null
                            ? (carInfo.purchase_price.HasValue ? carInfo.purchase_price.Value.ToString() : "0")
                            : "0";
                    }
                    else
                    {
                        my.LicenseNo = string.Empty;
                        my.PurchasePrice = "0";
                    }
                    my.QuoteGroup = item.IsSingleSubmit.HasValue ? item.IsSingleSubmit.Value : 0;//报价类型
                    //my.HasBaojia = item.QuoteStatus > -1 ? 1 : 0;//是否报过价
                    my.HasBaojia = my.QuoteGroup == 0 ? 0 : 1;
                    my.SubmitGroup = item.Source.HasValue ? (item.Source.Value > 0 ? item.Source.Value : 0) : 0;//核保类型

                    my.MoldName = !string.IsNullOrEmpty(item.MoldName) ? item.MoldName : string.Empty;
                    my.UserName = !string.IsNullOrEmpty(item.UserName) ? item.UserName : string.Empty;
                    my.LicenseOwner = !string.IsNullOrEmpty(item.LicenseOwner) ? item.LicenseOwner : string.Empty;
                    my.Buid = item.Id;
                    my.RegisterDate = !string.IsNullOrEmpty(item.RegisterDate) ? item.RegisterDate : string.Empty;
                    //InsuredName和PostedName是一个字段
                    my.InsuredName = !string.IsNullOrEmpty(item.InsuredName) ? item.InsuredName : string.Empty;
                    //被保险人电话
                    my.InsuredMobile = !string.IsNullOrEmpty(item.InsuredMobile) ? item.InsuredMobile : string.Empty;
                    //被保险人地址
                    my.InsuredAddress = !string.IsNullOrEmpty(item.InsuredAddress) ? item.InsuredAddress : string.Empty;
                    //被保险人证件类型
                    my.InsuredIdType = item.InsuredIdType.HasValue ? item.InsuredIdType.Value : 0;
                    my.PostedName = !string.IsNullOrEmpty(item.InsuredName) ? item.InsuredName : string.Empty;
                    my.IdType = item.OwnerIdCardType;//_quoteResultCarinfoRepository.GetOwnerIdType(item.Id);
                    my.IdCard = !string.IsNullOrEmpty(item.IdCard) ? item.IdCard : string.Empty;
                    //my.IdType = item.InsuredIdType.HasValue ? item.InsuredIdType.Value : 0;
                    //被保险人证件号
                    my.CredentislasNum = !string.IsNullOrEmpty(item.IdCard) ? item.IdCard : string.Empty;
                    my.InsuredIdCard = !string.IsNullOrEmpty(item.InsuredIdCard) ? item.InsuredIdCard : string.Empty;
                    my.CityCode = !string.IsNullOrEmpty(item.CityCode) ? item.CityCode : string.Empty;
                    my.EngineNo = !string.IsNullOrEmpty(item.EngineNo) ? item.EngineNo : string.Empty;
                    my.CarVin = !string.IsNullOrEmpty(item.CarVIN) ? item.CarVIN : string.Empty;
                    //获取电子保单的邮箱地址
                    //my.Email = !string.IsNullOrEmpty(item.Email) ? item.Email : string.Empty;
                    my.Email = !string.IsNullOrEmpty(item.InsuredEmail) ? item.InsuredEmail : string.Empty;
                    my.Email = !string.IsNullOrEmpty(my.Email) ? my.Email : item.Email;
                    //取商业险和交强险开始时间
                    var quoteresult = _quoteResultRepository.GetStartDate(item.Id);//,userinfo.Source.Value
                    my.ForceStartDate = quoteresult != null ? (quoteresult.ForceStartDate.HasValue
                            ? quoteresult.ForceStartDate.Value.ToString("yyyy-MM-dd")
                            : string.Empty)
                        : string.Empty;
                    my.BizStartDate = quoteresult != null
                        ? (quoteresult.BizStartDate.HasValue
                            ? quoteresult.BizStartDate.Value.ToString("yyyy-MM-dd")
                            : string.Empty)
                        : string.Empty;
                    if (!string.IsNullOrWhiteSpace(my.ForceStartDate))
                    {
                        var st = DateTime.Parse(my.ForceStartDate);
                        if (st.Date == DateTime.MinValue.Date)
                        {
                            my.ForceStartDate = string.Empty;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(my.BizStartDate))
                    {
                        var st = DateTime.Parse(my.BizStartDate);
                        if (st.Date == DateTime.MinValue.Date)
                        {
                            my.BizStartDate = string.Empty;
                        }
                    }

                    var lastInfo = _lastInfoRepository.GetEndDate(item.Id);
                    if (lastInfo != null)
                    {
                        //取商业险和交强险开始时间
                        my.LastBusinessEndDdate = string.IsNullOrEmpty(lastInfo.LastBusinessEndDdate) ? string.Empty : lastInfo.LastBusinessEndDdate;
                        my.LastEndDate = string.IsNullOrEmpty(lastInfo.LastForceEndDdate) ? string.Empty : lastInfo.LastForceEndDdate;
                        if (!string.IsNullOrWhiteSpace(my.LastBusinessEndDdate))
                        {
                            var st = DateTime.Parse(my.LastBusinessEndDdate);
                            my.LastBusinessEndDdate = st.Date == DateTime.MinValue.Date ? string.Empty : st.ToString("yyyy-MM-dd");
                        }
                        if (!string.IsNullOrWhiteSpace(my.LastEndDate))
                        {
                            var st = DateTime.Parse(my.LastEndDate);
                            my.LastEndDate = st.Date == DateTime.MinValue.Date ? string.Empty : st.ToString("yyyy-MM-dd");
                        }
                    }
                    else
                    {
                        my.LastBusinessEndDdate = string.Empty;
                        my.LastEndDate = string.Empty;
                    }
                    #endregion

                    if (item.IsSingleSubmit.HasValue)
                    {
                        sourceList = new List<long>();
                        sourceList = SourceGroupAlgorithm.GetSource(sourceInt, item.IsSingleSubmit.Value);

                        #region List<MyPrecisePriceItemViewModel>对象

                        sq = new bx_savequote();
                        sq = _saveQuoteRepository.GetSavequoteByBuid(item.Id);
                        qrList = new List<bx_quoteresult>();
                        qrList = _quoteResultRepository.GetQuoteResultList(item.Id);
                        siList = new List<bx_submit_info>();
                        siList = _submitInfoRepository.GetSubmitInfoList(item.Id);
                        listquoteTotal = new List<MyPrecisePriceItemViewModel>();
                        if (sourceList.Any())
                        {
                            foreach (int itk in sourceList)
                            {
                                //20160905修改source1248=>0123，传入的新数据转换
                                var oit = SourceGroupAlgorithm.GetOldSource(itk);//获取到旧的source值
                                var model = PrecisePriceMapper.ConvertToViewModelNew(oit, sq,
                                    qrList.FirstOrDefault(i => i.Source == oit),
                                    siList.FirstOrDefault(i => i.source == oit), item.QuoteStatus ?? -1, new List<AgentConfigNameModel>(), item.CarVIN,new List<bx_ywxdetail>());
                                listquoteTotal.Add(model);
                            }
                        }
                        #endregion

                        my.PrecisePriceItem = listquoteTotal;
                    }
                    else
                    {
                        my.PrecisePriceItem = new List<MyPrecisePriceItemViewModel>();
                    }
                    list.Add(my);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取我的报价单接口
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public MyBaoJiaViewModel GetMyBjdDetail(GetMyBjdDetailRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            return _getMyBjdDetailService.GetMyBjdDetail(request, pairs);
        }

        /// <summary>
        /// 从历史获取报价单详情接口
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public MyBaoJiaViewModel GetBjdDetailFromHistory(GetBjdDetailFromHistoryRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            return _getBjdDetailFromHistoryService.GetMyBjdDetail(request, pairs);
        }

        #region 取上传图片表
        /// <summary>
        /// 上传图片模型转换
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="buid"></param>
        /// <returns></returns>
        private List<UrlAndType> GetUrlAndType(bx_picture picture, long buid)
        {
            try
            {
                string uploadImgUrl = ConfigurationManager.AppSettings["ImageServer"];
                var imgList = picture.picsJson.FromJson<List<UrlAndType>>();
                imgList.ForEach(i => i.Url = (i.Url.Contains(uploadImgUrl) ? i.Url : uploadImgUrl + i.Url));
                return imgList;
            }
            catch
            {
                logInfo.Info("bx_picture中picsJson的格式不正确，buid为" + buid);
            }
            return new List<UrlAndType>();
        }

        /// <summary>
        /// 拼装ImgUrl模型
        /// </summary>
        /// <param name="picList"></param>
        /// <param name="oldSource"></param>
        /// <param name="newSource"></param>
        /// <param name="buid"></param>
        /// <returns></returns>
        private ImgUrl GetUrlList(List<bx_picture> picList, int oldSource, long newSource, long buid)
        {
            //取对应bx_picture记录
            List<UrlAndType> urls = GetUrlAndType(picList.FirstOrDefault(w => w.source == oldSource), buid);
            //如果库里有记录
            if (urls.Any())
            {
                ImgUrl imgModel = new ImgUrl()
                {
                    Urls = urls,
                    Source = newSource
                };
            }
            return null;
        }
        #endregion

        /// <summary>
        /// 获取我的续保单接口
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public AppGetReInfoResponse GetReInfoDetail(GetReInfoDetailRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new AppGetReInfoResponse();
            //bhToken校验
            //if (!AppTokenValidateReqest(request.BhToken, request.ChildAgent.Value))
            //{
            //    response.BusinessStatus = -300;
            //    response.BusinessMessage = "登录信息已过期，请重新登录";
            //    return response;
            //}
            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            //参数校验
            if (!agentModel.AgentCanUse())
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            if (!AppValidateReqest(pairs, request.SecCode))
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }

            try
            {
                //取bx_userinfo对象
                var userinfo = new bx_userinfo();
                if (request.Buid.HasValue && request.Buid.Value != 0)
                {
                    //如果传buid过来，重新赋值request
                    userinfo = _userInfoRepository.FindByBuid(request.Buid.Value);
                    if (userinfo != null)
                    {
                        request.LicenseNo = userinfo.LicenseNo;
                        request.ChildAgent = int.Parse(userinfo.Agent);
                    }
                }
                else
                {
                    if (request.ChildAgent.HasValue)
                    {
                        //根据OpenId、车牌号、代理人Id找userinfo对象
                        userinfo = _userInfoRepository.FindByAgentLicense(request.LicenseNo, request.ChildAgent.Value.ToString());
                    }
                }

                if (userinfo == null)
                {
                    response.Status = HttpStatusCode.NoContent;
                    return response;
                }
                else
                {
                    response.Buid = userinfo.Id;
                    //UserInfo
                    response.UserInfo = userinfo;
                    //增加当前userinfo的代理人返回
                    response.Agent = int.Parse(userinfo.Agent);
                    bx_agent topAgent = _agentRepository.GetAgent(response.Agent);//需要获取顶级代理
                    List<bx_transferrecord> listRecord = _transferRecordRepository.FindByBuid(userinfo.Id);
                    if (listRecord.Any())
                    {
                        if (listRecord[0].StepType == 6)
                        {//取最后一条记录是预分配的，则显示"未分配"
                            response.AgentName = "未分配";
                            response.IsDistrib = 0;
                        }
                        else
                        {
                            response.AgentName = topAgent != null ? topAgent.AgentName : "";
                            response.IsDistrib = 1;
                        }
                    }
                    else
                    {
                        response.AgentName = "未分配";
                        response.IsDistrib = 0;
                    }
                    //WorkOrder
                    response.WorkOrder = _userinfoRenewalInfoRepository.FindByBuid(userinfo.Id);
                    //WorkOrderDetailList
                    if (response.WorkOrder != null)
                    {
                        response.DetailList = _consumerReviewRepository.FindDetails(userinfo.Id);
                    }
                    //获取SaAgent
                    //取bx_transferrecord里最后一条sa记录
                    bx_transferrecord transferrecord;
                    transferrecord = _transferRecordRepository.FindFirstSaByBuid(userinfo.Id);
                    if (transferrecord != null)
                    {
                        response.SaAgent = transferrecord.FromAgentId;
                        bx_agent createAgent = _agentRepository.GetAgent(transferrecord.FromAgentId);
                        response.SaAgentName = createAgent != null ? createAgent.AgentName : "";
                    }

                    response.Status = HttpStatusCode.OK;

                    //int intstatus = _carInsuranceCache.GetReInfoStatus(userinfo.LicenseNo);

                    //if (intstatus == 4)
                    //{
                    if (!GetReInfoState(userinfo))
                    {
                        if (userinfo.NeedEngineNo == 1)
                        {
                            //需要完善行驶证信息
                            response.BusinessStatus = 2;
                            //response.BusinessMessage = "需要完善行驶证信息（车辆信息和险种都没有获取到）";
                        }
                        else if (userinfo.NeedEngineNo == 0 && userinfo.RenewalStatus != 1)
                        {
                            //获取车辆信息成功，但获取险种失败
                            response.BusinessStatus = 3;
                            //response.BusinessMessage = "获取车辆信息成功(车架号，发动机号，品牌型号及初登日期)，险种获取失败";
                        }
                        else if ((userinfo.NeedEngineNo == 0 && userinfo.LastYearSource > -1) || userinfo.RenewalStatus == 1)
                        {
                            //续保成功
                            response.BusinessStatus = 1;
                            //response.BusinessMessage = "续保成功";
                        }
                        ////从其他渠道获取投保信息
                        //List<bx_gsc_renewal> listOther = _gscRenewalRepository.FindListByBuid(userinfo.Id);
                        //bool isJiao = false;
                        //if (listOther.Any())
                        //{
                        //    //1商业
                        //    bx_gsc_renewal list1 = listOther.OrderByDescending(o => o.Enddate).
                        //        FirstOrDefault(i => i.IsCommerce == 1);
                        //    if (list1 != null)
                        //    {
                        //        if (!string.IsNullOrWhiteSpace(list1.Enddate))
                        //        {
                        //            if (DateTime.Parse(list1.Enddate) > response.SaveQuote.LastBizEndDate)
                        //            {
                        //                response.SaveQuote.LastBizEndDate = DateTime.Parse(list1.Enddate);
                        //                isJiao = true;
                        //            }
                        //        }
                        //    }
                        //    //0交强
                        //    bx_gsc_renewal list2 = listOther.OrderByDescending(o => o.Enddate).
                        //        FirstOrDefault(i => i.IsCommerce == 0);
                        //    if (list2 != null)
                        //    {
                        //        if (!string.IsNullOrWhiteSpace(list2.Enddate))
                        //        {
                        //            if (DateTime.Parse(list2.Enddate) > response.SaveQuote.LastForceEndDate)
                        //            {
                        //                response.SaveQuote.LastForceEndDate = DateTime.Parse(list2.Enddate);
                        //                isJiao = true;
                        //            }
                        //        }
                        //    }
                        //}
                        //if (isJiao)
                        //{
                        //    response.BusinessStatus = 8;
                        //}
                    }
                    else
                    {
                        if (userinfo.LastYearSource == -1)
                        {
                            response.BusinessStatus = -10002;
                            response.BusinessMessage = "获取续保信息失败";
                        }
                    }
                    //}
                    //else
                    //{
                    //    response.BusinessStatus = intstatus;
                    //}

                    if (!string.IsNullOrEmpty(userinfo.LicenseNo) && response.BusinessStatus != 2)
                    {
                        //savequote
                        //2016-11-15  针对  修改车架号报价的情况
                        var renewalFlag = _quoteReqCarinfoRepository.Find(userinfo.Id);
                        var licenseno = userinfo.LicenseNo;
                        if (renewalFlag.is_lastnewcar == 2)
                        {
                            licenseno = userinfo.CarVIN;
                        }
                        if (response.BusinessStatus == 1)
                        {
                            response.SaveQuote = _carRenewalRepository.FindCarRenewal(userinfo.Id);//_carRenewalRepository.FindByLicenseno(licenseno);
                        }
                        response.CarInfo = _carInfoRepository.Find(licenseno);
                    }

                    if (response.BusinessStatus == 1)
                    {
                        response.BusinessMessage = "续保成功";
                    }
                    else if (response.BusinessStatus == 2)
                    {
                        response.BusinessMessage = "需要完善行驶证信息（车辆信息和险种都没有获取到）";
                    }
                    else if (response.BusinessStatus == 3)
                    {
                        response.BusinessMessage = "获取车辆信息成功(车架号，发动机号，品牌型号及初登日期)，险种获取失败";
                    }
                    else if (response.BusinessStatus == -10002)
                    {
                        response.BusinessStatus = 0;
                        response.BusinessMessage = "获取续保信息失败";
                    }
                    else if (response.BusinessStatus == 8)
                    {
                        response.BusinessMessage = "该车是续保期外的车或者是投保我司对接外的其他保险公司的车辆，这种情况，只能返回该车的投保日期(ForceExpireDate,BusinessExpireDate),险种取不到，不再返回";
                    }

                    return response;

                }
            }
            catch (Exception ex)
            {
                response = new AppGetReInfoResponse { Status = HttpStatusCode.ExpectationFailed };
                logErr.Info("APP续保详情接口请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
        }

        public ReInfoListViewModel GetReInfoList(GetReInfoListRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var model = new ReInfoListViewModel();
            //bhToken校验
            if (!AppTokenValidateReqest(request.BhToken, request.ChildAgent))
            {
                model.BusinessStatus = -300;
                model.StatusMessage = "登录信息已过期，请重新登录";
                return model;
            }
            var list = new List<ReInfo>();
            int totalCount = 0;

            IBxAgent agentModel = GetAgentModelFactory(request.Agent);
            //参数校验
            if (!agentModel.AgentCanUse())
            {
                model.BusinessStatus = -10001;
                model.StatusMessage = "参数校验错误，请检查您的校验码";
                return model;
            }
            if (!AppValidateReqest(pairs, request.SecCode))
            {
                model.BusinessStatus = -10001;
                model.StatusMessage = "参数校验错误，请检查您的校验码";
                return model;
            }

            try
            {
                bool isAgent = false;
                var sonself = new List<bx_agent>();
                //当前根据openid获取当前经纪人 
                var curAgent = _agentRepository.GetAgent(request.ChildAgent);
                if (curAgent != null)
                {
                    //代理人
                    if (request.IsOnlyMine.HasValue)
                    {
                        if (request.IsOnlyMine.Value == 0)
                        {
                            sonself = _agentRepository.GetSonsAgent(curAgent.Id).ToList();
                        }
                    }
                    isAgent = true;
                    sonself.Add(curAgent);
                }

                List<bx_userinfo> userinfo = _userInfoRepository.FindReInfoList(isAgent, sonself,
                    curAgent.OpenId, request.LicenseNo, request.PageSize, request.CurPage, out totalCount);
                //续保总数
                model.TotalCount = totalCount;
                if (totalCount < 0)
                {
                    model.BusinessStatus = 0;
                    model.StatusMessage = "没有续保记录";
                    return model;
                }
                if (userinfo.Count > 0)
                {
                    ReInfo re;
                    bx_transferrecord transferrecord;
                    bx_userinfo_renewal_info bxWorkOrder;
                    List<bx_transferrecord> listRecord;
                    foreach (var item in userinfo)
                    {
                        if (string.IsNullOrWhiteSpace(item.LicenseNo)) continue;

                        re = new ReInfo();
                        re.Buid = item.Id;
                        //获取当前记录的agent
                        re.Agent = item.Agent;
                        //创建时间
                        if (item.CreateTime != null)
                            re.CreateTime = item.UpdateTime.HasValue ? item.UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        //获取意向
                        bxWorkOrder = _userinfoRenewalInfoRepository.FindByBuid(item.Id);
                        re.Intention_View = bxWorkOrder != null ? bxWorkOrder.client_intention : null;

                        listRecord = new List<bx_transferrecord>();
                        listRecord = _transferRecordRepository.FindByBuid(item.Id);
                        //第六步是分配管理员，如果最后的记录到这一步，就说明未分配
                        if (listRecord.Any())
                        {
                            re.IsDistrib = listRecord[0].StepType == 6 ? 0 : 1;
                        }
                        else
                        {
                            re.IsDistrib = 0;
                        }
                        //获取SaAgent
                        //取bx_transferrecord里最后一条sa记录
                        transferrecord = new bx_transferrecord();
                        transferrecord = _transferRecordRepository.FindFirstSaByBuid(item.Id);
                        re.SaAgent = transferrecord != null ? transferrecord.FromAgentId : 0;

                        //配置基础信息
                        re.UserInfo =
                            item.ConvertToViewModel(_carRenewalRepository.FindByLicenseno(item.LicenseNo),
                                _carInfoRepository.Find(item.LicenseNo),
                                _lastInfoRepository.GetByBuid(item.Id));

                        //续保判断返回信息
                        if (item.NeedEngineNo == 1)
                        {
                            //需要完善行驶证信息
                            re.BusinessStatus = 2;
                            re.StatusMessage = "需要完善行驶证信息（车辆信息和险种都没有获取到）";

                            re.UserInfo.BusinessExpireDate = "";
                            re.UserInfo.ForceExpireDate = "";
                            re.UserInfo.NextBusinessStartDate = "";
                            re.UserInfo.NextForceStartDate = "";
                        }
                        else if (item.NeedEngineNo == 0 && item.RenewalStatus != 1)
                        {
                            //获取车辆信息成功，但获取险种失败
                            re.BusinessStatus = 3;
                            re.StatusMessage = "获取车辆信息成功(车架号，发动机号，品牌型号及初登日期)，险种获取失败";

                            re.UserInfo.BusinessExpireDate = "";
                            re.UserInfo.ForceExpireDate = "";
                            re.UserInfo.NextBusinessStartDate = "";
                            re.UserInfo.NextForceStartDate = "";

                        }
                        else if ((item.NeedEngineNo == 0 && item.LastYearSource > -1) || item.RenewalStatus == 1)
                        {
                            //续保成功
                            re.BusinessStatus = 1;
                            re.StatusMessage = "续保成功";
                        }
                        else
                        {
                            re.BusinessStatus = 0;
                            re.StatusMessage = "获取续保信息失败";

                            re.UserInfo.BusinessExpireDate = "";
                            re.UserInfo.ForceExpireDate = "";
                            re.UserInfo.NextBusinessStartDate = "";
                            re.UserInfo.NextForceStartDate = "";
                        }
                        list.Add(re);
                    }
                    model.BusinessStatus = 1;
                    model.ReInfoList = list;
                }
            }
            catch (Exception ex)
            {
                model.BusinessStatus = -10003;
                model.StatusMessage = "服务发生异常";
                logErr.Info("APP续保列表接口请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return model;
        }

        public BjdCountInfoViewModel BjdCountInfo(BjdCountInfoRequest request,
            IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var model = new BjdCountInfoViewModel();
            var sonself = new List<bx_agent>();
            bool isAgent = false;

            //当前根据openid获取当前经纪人 
            var curAgent = _agentRepository.GetAgentByTopParentAgent(request.OpenId, request.Agent);
            if (curAgent != null)
            {//代理人
                if (request.IsOnlyMine == 0)
                {
                    sonself = _agentRepository.GetSonsAgent(curAgent.Id).ToList();
                }
                isAgent = true;
                sonself.Add(curAgent);
            }

            model.BusinessStatus = 1;
            model.CountBaoJia = _userInfoRepository.CountBaojia(isAgent, sonself, request.OpenId);
            model.CountYuYue = _orderRepository.CountYuYue(isAgent, sonself, request.OpenId, request.Agent);
            model.CountChuDan = _orderRepository.CountChuDan(isAgent, sonself, request.OpenId, request.Agent);
            return model;
        }

        /// <summary>
        /// 续保和报价综合列表（2016.11新版）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public MyListViewModel GetMyList(GetMyListRequest request)
        {
            //保证必要参数如agentId等，之前已做过验证
            var viewModel = new MyListViewModel();
            //赋值要查询的agent集合
            var sonself = new List<bx_agent>();
            //声明where查询条件
            var sbWhere = new StringBuilder();
            if (request.IsOnlyMine == 0)
            {//查询自己及子集所有代理
                //sonself = _agentRepository.GetSonsAgent(request.ChildAgent).ToList();
                //查自己及下级的所有数据
                string agentids = _agentRepository.GetSonsIdToString(request.ChildAgent, true);
                if (!string.IsNullOrEmpty(agentids))
                {
                    sbWhere.Append(" AND ui.Agent in (")
                        .Append(agentids)
                        .Append(") ");
                }
            }
            else
            {
                sbWhere.Append(" AND ui.Agent ='")
                    .Append(request.ChildAgent)
                    .Append("' ");

            }
            //sonself.Add(_agentRepository.GetAgent(request.ChildAgent));

            int totalCount = 0;
            List<UserInfoModel> userinfo = _userInfoRepository.FindMyList(true, sbWhere.ToString(), request.CustKey, request.LicenseNo, request.PageSize, request.CurPage, request.OrderBy, out totalCount);
            if (userinfo.Count > 0)
            {
                var myInfoList = new List<MyInfo>();
                MyInfo myInfo;
                List<PrecisePriceInfo> PrecisePrice;//声明每条记录的报价列表
                List<long> sourceInt = _agentConfigRepository.FindSource(request.Agent); //获取代理下面的渠道Id
                List<long> sourceList; //声明每条记录用到的渠道列表
                //bx_car_renewal bxCarRenewal;
                bx_consumer_review bxConsumerReview;
                //报价单资源对象
                bx_savequote sq;
                List<bx_quoteresult> qrList;
                List<bx_submit_info> siList;

                foreach (var item in userinfo)
                {
                    myInfo = new MyInfo();
                    bxConsumerReview = new bx_consumer_review();
                    bxConsumerReview =
                        _consumerReviewRepository.FindDetails(item.Id)
                            .OrderByDescending(o => o.create_time)
                            .FirstOrDefault();
                    if (bxConsumerReview != null)
                    {
                        myInfo.VisitedStatus = bxConsumerReview.result_status.HasValue ? bxConsumerReview.result_status.Value : 0;
                    }
                    myInfo.Buid = item.Id;
                    myInfo.LicenseNo = item.LicenseNo;
                    myInfo.MoldName = item.MoldName;
                    //先获取更新时间，如果没有更新时间，则获取创建时间
                    myInfo.CreateTime = item.UpdateTime.HasValue
                        ? item.UpdateTime.Value.ToString()
                        : (item.CreateTime.HasValue ? item.CreateTime.Value.ToString() : DateTime.Now.ToString());
                    myInfo.ItemChildAgent = item.Agent;
                    myInfo.ItemCustKey = item.OpenId;
                    myInfo.CityCode = item.CityCode;
                    myInfo.RenewalStatus = item.RenewalStatus;
                    myInfo.IsPrecisePrice = item.QuoteStatus > -1 ? 1 : 0; //是否已报价 1已报0未报
                    PrecisePrice = new List<PrecisePriceInfo>();
                    if (item.QuoteStatus > -1)
                    {
                        //报价
                        //获取报价渠道组合值
                        if (!item.IsSingleSubmit.HasValue) continue;
                        sourceList = new List<long>();

                        // 获取到记录用到的渠道列表
                        sourceList = SourceGroupAlgorithm.GetSource(sourceInt, item.IsSingleSubmit.Value);
                        #region List<MyPrecisePriceItemViewModel>对象
                        sq = new bx_savequote();
                        sq = _saveQuoteRepository.GetSavequoteByBuid(item.Id);
                        qrList = new List<bx_quoteresult>();
                        qrList = _quoteResultRepository.GetQuoteResultList(item.Id);
                        siList = new List<bx_submit_info>();
                        siList = _submitInfoRepository.GetSubmitInfoList(item.Id);
                        if (sourceList.Any())
                        {
                            foreach (int it in sourceList)
                            {
                                var oit = SourceGroupAlgorithm.GetOldSource(it);//获取到旧的source值
                                var ppInfo = MyListMapper.ConvertToPrecisePriceInfo(oit,
                                    qrList.FirstOrDefault(i => i.Source == oit),
                                    siList.FirstOrDefault(i => i.source == oit));
                                PrecisePrice.Add(ppInfo);
                            }
                        }
                        #endregion

                    }
                    myInfo.PrecisePrice = PrecisePrice;
                    //续保
                    //bxCarRenewal = _carRenewalRepository.FindByLicenseno(item.LicenseNo);
                    //if (bxCarRenewal == null) continue;

                    myInfo.BusinessExpireDate = string.Empty;
                    myInfo.ForceExpireDate = string.Empty;
                    //获取续保成功才可以取到期日期
                    if ((item.NeedEngineNo == 0 && item.LastYearSource > -1) || item.RenewalStatus == 1)
                    {
                        //商业到期日期
                        myInfo.BusinessExpireDate = item.LastBizEndDate.HasValue
                            ? item.LastBizEndDate.Value.ToString("yyyy-MM-dd")
                            : "";
                        if (!string.IsNullOrEmpty(myInfo.BusinessExpireDate))
                        {
                            var st = DateTime.Parse(myInfo.BusinessExpireDate);
                            if (st.Date == DateTime.MinValue.Date)
                            {
                                myInfo.BusinessExpireDate = string.Empty;
                            }
                        }
                        //交强险到期日期
                        myInfo.ForceExpireDate = item.LastForceEndDate.HasValue
                            ? item.LastForceEndDate.Value.ToString("yyyy-MM-dd")
                            : "";
                        if (!string.IsNullOrEmpty(myInfo.ForceExpireDate))
                        {
                            var st = DateTime.Parse(myInfo.ForceExpireDate);
                            if (st.Date == DateTime.MinValue.Date)
                            {
                                myInfo.ForceExpireDate = string.Empty;
                            }
                            if (!string.IsNullOrEmpty(myInfo.ForceExpireDate))
                            {
                                //到期天数
                                TimeSpan ts = DateTime.Parse(myInfo.ForceExpireDate) - DateTime.Now;
                                myInfo.ExpireDateNum = ts.Days;
                            }
                        }
                    }
                    myInfoList.Add(myInfo);
                }
                viewModel.BusinessStatus = 1;
                viewModel.TotalCount = totalCount;
                viewModel.MyInfoList = myInfoList;
            }
            else
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "获取信息失败";
            }
            return viewModel;
        }

        private bool GetReInfoState(bx_userinfo userinfo)
        {
            bool isContinue;
            isContinue = true;
            if (userinfo.NeedEngineNo == 1)
            {
                //需要完善行驶证信息
                isContinue = false;
            }
            if (userinfo.NeedEngineNo == 0 && userinfo.RenewalStatus != 1)
            {  //获取车辆信息成功，但获取险种失败
                isContinue = false;
            }
            if ((userinfo.NeedEngineNo == 0 && userinfo.LastYearSource > -1) || userinfo.RenewalStatus == 1)
            {  //续保成功
                isContinue = false;
            }
            return isContinue;
        }
    }
}
