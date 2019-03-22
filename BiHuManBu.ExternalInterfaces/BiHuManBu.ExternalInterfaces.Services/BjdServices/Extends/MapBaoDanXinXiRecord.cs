using System;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public class MapBaoDanXinXiRecord : IMapBaoDanXinXiRecord
    {
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly IQuoteResultCarinfoRepository _quoteResultCarinfoRepository;
        private readonly ILastInfoRepository _lastInfoRepository;
        public MapBaoDanXinXiRecord(IUserClaimRepository userClaimRepository, IQuoteResultCarinfoRepository quoteResultCarinfoRepository, ILastInfoRepository lastInfoRepository)
        {
            _userClaimRepository = userClaimRepository;
            _quoteResultCarinfoRepository = quoteResultCarinfoRepository;
            _lastInfoRepository = lastInfoRepository;
        }

        public bj_baodanxinxi MapBaodanxinxi(CreateOrUpdateBjdInfoRequest request, bx_submit_info submitInfo,
            bx_quoteresult quoteresult, bx_savequote savequote, bx_userinfo userinfo, bx_quotereq_carinfo reqCarInfo, bx_preferential_activity activity)
        {
            var quotecarinfo = _quoteResultCarinfoRepository.Find(request.Buid, SourceGroupAlgorithm.GetOldSource(request.Source));
            var lastinfo = _lastInfoRepository.GetByBuid(request.Buid);

            var listClaim = _userClaimRepository.FindList(request.Buid);
            int lossBizCount = listClaim.Where(n => n.pay_type == 0).ToList().Count;  //商业出险次数
            double? lossBizAmount = listClaim.Where(n => n.pay_type == 0).ToList().Sum(n => n.pay_amount);  //商业出险金额
            int lossForceCount = listClaim.Where(n => n.pay_type == 1).ToList().Count;  //交强出险次数
            double? lossForceAmount = listClaim.Where(n => n.pay_type == 1).ToList().Sum(n => n.pay_amount); //交强出险金额

            var baodanxinxi = new bj_baodanxinxi
            {
                //BEGIN 2017-09-06新增字段   20170909L 当前后期修改中去掉了代理人信息（电话等）
                AgentId = request.ChildAgent == 0 ? int.Parse(userinfo.Agent) : request.ChildAgent,
                //END

                //BEGIN 2017-09-09新增字段
                //activity_content = request.ActivityContent,
                activity_ids = activity.id.ToString(),
                //END

                // AgentId = int.Parse(userinfo.Agent),
                BizEndDate = lastinfo == null ? DateTime.MinValue : Convert.ToDateTime(lastinfo.last_business_end_date), //submitinfo.biz_end_time,
                BizNum = submitInfo.biz_tno,
                BizPrice = quoteresult.BizTotal.HasValue ? quoteresult.BizTotal.Value : 0,
                BizRate = double.Parse(submitInfo.biz_rate.HasValue ? submitInfo.biz_rate.Value.ToString() : "0"),
                BizStartDate = quoteresult.BizStartDate.HasValue ? quoteresult.BizStartDate.Value : (reqCarInfo.biz_start_date.HasValue?reqCarInfo.biz_start_date.Value:DateTime.MinValue),//submitInfo.biz_start_time,
                CarBrandModel = userinfo.MoldName,
                CarEngineNo = userinfo.EngineNo,
                CarLicense = userinfo.LicenseNo,
                CarOwner = userinfo.LicenseOwner,
                CarRegisterDate = string.IsNullOrEmpty(userinfo.RegisterDate) ? DateTime.MinValue : DateTime.Parse(userinfo.RegisterDate),
                CarVIN = userinfo.CarVIN,
                ChannelId = submitInfo.channel_id,
                CompanyId = submitInfo.source,
                CreateTime = DateTime.Now,
                ForceEndDate = lastinfo == null ? DateTime.MinValue : Convert.ToDateTime(lastinfo.last_end_date),//submitinfo.force_end_time,
                ForceStartDate = quoteresult.ForceStartDate.HasValue ? quoteresult.ForceStartDate.Value : (reqCarInfo.force_start_date.HasValue ? reqCarInfo.force_start_date.Value : DateTime.MinValue),//submitInfo.force_start_time,
                ForceNum = submitInfo.force_tno,
                ForcePrice = (quoteresult.ForceTotal.HasValue ? quoteresult.ForceTotal.Value : 0),
                ForceRate =
                    double.Parse(submitInfo.force_rate.HasValue ? submitInfo.force_rate.Value.ToString() : "0"),
                InsuredName = userinfo.InsuredName,
                InsureIdNum = userinfo.InsuredIdCard,
                ManualBizRate = request.BizRate,
                ManualTaxRate = request.TaxRate,
                ManualForceRate = request.ForceRate,
                ObjectId = int.Parse(userinfo.Agent),
                ObjectType = 1,
                SubmitStatus = submitInfo.submit_status,
                TaxPrice = quoteresult.TaxTotal.HasValue ? quoteresult.TaxTotal.Value : 0,

                //新增的4个费率
                NonClaimRate = (double)(quoteresult.RateFactor1.HasValue ? quoteresult.RateFactor1 : 0),
                MultiDiscountRate = (double)(quoteresult.RateFactor2.HasValue ? quoteresult.RateFactor2 : 0),
                AvgMileRate = (double)(quoteresult.RateFactor3.HasValue ? quoteresult.RateFactor3 : 0),
                RiskRate = (double)(quoteresult.RateFactor4.HasValue ? quoteresult.RateFactor4 : 0),
                //总费率系数
                TotalRate = (quoteresult.NewRate.HasValue && quoteresult.NewRate != 0) ? quoteresult.NewRate.Value.ToString() : (quoteresult.TotalRate.HasValue ? quoteresult.TotalRate.Value.ToString() : "0"),
                //20170221新增增值税
                AddValueTaxRate = request.AddValueTaxRate,
                //载客//+载质量
                CarSeated = quotecarinfo != null ? (quotecarinfo.seat_count.HasValue ? quotecarinfo.seat_count.Value.ToString() : "0") : "0",
                //+ "/" + (quotecarinfo.car_equ_quality.HasValue ? quotecarinfo.car_equ_quality.Value.ToString() : "0")
                VehicleInfo = VehicleInfoMapper.VehicleInfoMethod(quotecarinfo),
                JqVehicleClaimType = quotecarinfo != null ? quotecarinfo.JqVehicleClaimType : "",
                SyVehicleClaimType = quotecarinfo != null ? quotecarinfo.SyVehicleClaimType : "",
                loss_biz_count = lossBizCount,
                loss_biz_amount = lossBizAmount.HasValue ? lossBizAmount.Value : 0,
                loss_force_count = lossForceCount,
                loss_force_amount = lossForceAmount.HasValue ? lossForceAmount.Value : 0
            };
            return baodanxinxi;
        }
    }
}
