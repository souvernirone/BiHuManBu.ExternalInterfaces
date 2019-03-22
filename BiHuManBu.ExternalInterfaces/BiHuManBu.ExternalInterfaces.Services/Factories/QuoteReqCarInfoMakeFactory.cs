using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.Factories
{
    public class QuoteReqCarInfoMakeFactory
    {
        public static void Save(long buid, int lastnewcar, IQuoteReqCarinfoRepository repository)
        {
            var newquotereq = new bx_quotereq_carinfo
            {
                b_uid = buid,
                is_lastnewcar = lastnewcar,
                //is_public=request.IsPublic,
                create_time = DateTime.Now,
                update_time = DateTime.Now
            };
            repository.Add(newquotereq);
        }

        public static void Update(bx_quotereq_carinfo quotereq, int lastnewcar, IQuoteReqCarinfoRepository repository)
        {
            quotereq.is_lastnewcar = lastnewcar;
            //quotereq.is_public = request.IsPublic;
            quotereq.update_time = DateTime.Now;
            repository.Update(quotereq);
        }

        public static void QuoteAdd(PostPrecisePriceRequest request, long buid, int publictype, IQuoteReqCarinfoRepository repository)
        {
            bx_quotereq_carinfo newquotereq = new bx_quotereq_carinfo
            {
                is_loans = request.IsLoans,
                b_uid = buid,
                car_type = request.CarType,
                car_used_type = request.CarUsedType,
                seat_count = request.SeatCount,
                is_newcar = request.IsNewCar,
                car_ton_count = request.TonCount,
                create_time = DateTime.Now,
                update_time = DateTime.Now
            };
            //经办人、送修码、归属人等设置，重写source值
            //if (!string.IsNullOrWhiteSpace(request.ConfigCode))
            //{
            //    //将用户传的configcode的source改为旧值。
            //    var configcodemodel = request.ConfigCode.FromJson<Dictionary<string, Dictionary<string, string>>>();
            //    var newconfigcodemodel = new Dictionary<string, Dictionary<string, string>>();
            //    foreach (var item in configcodemodel)
            //    {
            //        newconfigcodemodel.Add(SourceGroupAlgorithm.GetOldSource(long.Parse(item.Key)).ToString(), item.Value);
            //    }
            //    request.ConfigCode = newconfigcodemodel.ToJson();
            //}
            newquotereq.account_relation = request.ConfigCode;

            //浮动价格
            if (request.NegotiatePrice >= 0)
            {
                newquotereq.co_real_value = request.NegotiatePrice;
            }
            else
            {
                newquotereq.co_real_value = 0;
            }
            //车型选择source
            if (request.VehicleSource > 0)
            {
                newquotereq.moldcode_company = request.VehicleSource;
            }
            //过户车日期逻辑
            if (!string.IsNullOrWhiteSpace(request.TransferDate))
            {
                newquotereq.transfer_date = DateTime.Parse(request.TransferDate);
            }
            else
            {
                newquotereq.transfer_date = null;
            }
            if (!string.IsNullOrWhiteSpace(request.BizStartDate))
            {
                newquotereq.biz_start_date = DateTime.Parse(request.BizStartDate);
            }
            else
            {
                newquotereq.biz_start_date = null;
            }
            if (!string.IsNullOrWhiteSpace(request.ForceStartDate))
            {
                newquotereq.force_start_date = DateTime.Parse(request.ForceStartDate);
            }
            else
            {
                newquotereq.force_start_date = null;
            }
            //精友编码
            if (!string.IsNullOrWhiteSpace(request.AutoMoldCode))
            {
                newquotereq.auto_model_code = request.AutoMoldCode.Trim();
                newquotereq.auto_model_code_source = 2;
            }
            else
            {
                //20181119，解决续保车补充到其他保司的精友码，中心查到carmold的车型信息跟平安保司不匹配
                //如果是平安，则把数据库的精友码删掉
                if (request.QuoteGroup == 2) {
                    newquotereq.auto_model_code = null;
                    newquotereq.auto_model_code_source = null;
                }
            }
            if (request.AutoMoldCodeSource > 0)
            {
                newquotereq.auto_model_code_source = request.AutoMoldCodeSource;
            }
            //平安备注
            if (!string.IsNullOrWhiteSpace(request.Remark))
            {
                newquotereq.pa_remark = request.Remark;
            }
            else
            {
                newquotereq.pa_remark = string.Empty;
            }
            //时间戳支持
            if (!string.IsNullOrWhiteSpace(request.BizTimeStamp))
            {
                newquotereq.biz_start_date =
                    DateTime.Parse(request.BizTimeStamp.UnixTimeToDateTime().ToString("yyyy-MM-dd HH") +
                                   ":00:00");
            }
            //else
            //{
            //    newquotereq.biz_start_date = null;
            //}
            if (!string.IsNullOrWhiteSpace(request.ForceTimeStamp))
            {
                newquotereq.force_start_date =
                    DateTime.Parse(request.ForceTimeStamp.UnixTimeToDateTime().ToString("yyyy-MM-dd HH") +
                                   ":00:00");
            }
            //else
            //{
            //    newquotereq.force_start_date = null;
            //}
            //商业险短时起保
            if (!string.IsNullOrWhiteSpace(request.BizShortEndDate))
            {
                newquotereq.biz_end_date =
                    DateTime.Parse(request.BizShortEndDate.UnixTimeToDateTime().ToString("yyyy-MM-dd HH") +
                                   ":00:00");
                newquotereq.IsShortQuote = 1;
            }
            else
            {
                newquotereq.biz_end_date = null;
                newquotereq.IsShortQuote = 0;
            }
            //if (request.IsPublic > 0)
            //{
            //    newquotereq.is_public = request.IsPublic;
            //}
            if (request.CarUsedType == 6 || request.CarUsedType == 7 || request.CarUsedType == 20)
            {
            }
            else
            {
                if (publictype > 0)
                {
                    newquotereq.is_public = publictype;
                }
            }
            if (request.ClauseType > 0)
            {
                newquotereq.clause_type = request.ClauseType;
            }
            if (request.FuelType > 0)
            {
                newquotereq.fuel_type = request.FuelType;
            }
            if (request.ProofType > 0)
            {
                newquotereq.proof_type = request.ProofType;
            }
            if (request.RunMiles > 0)
            {
                newquotereq.run_miles = request.RunMiles;
            }
            if (request.RateFactor4 > 0)
            {
                newquotereq.special_rate = request.RateFactor4;
            }
            if (request.LicenseColor > 0)
            {
                newquotereq.license_color = request.LicenseColor;
            }
            if (request.SeatCount > 0)
            {
                newquotereq.seat_count = request.SeatCount;
            }
            if (request.ExhaustScale > 0)
            {
                newquotereq.exhaust_scale = request.ExhaustScale;
            }
            if (request.PurchasePrice > 0)
            {
                newquotereq.PriceT = request.PurchasePrice;
            }
            newquotereq.drivlicense_cartype_name = request.DriveLicenseTypeName;
            newquotereq.drivlicense_cartype_value = request.DriveLicenseTypeValue;
            if (!string.IsNullOrWhiteSpace(request.OwnerOrg))
            {
                newquotereq.ownerOrg = request.OwnerOrg;
            }
            if (!string.IsNullOrWhiteSpace(request.PolicyOrg))
            {
                newquotereq.policyOrg = request.PolicyOrg;
            }
            if (!string.IsNullOrWhiteSpace(request.RenBaoRateSplit))
            {
                newquotereq.renBaoRateSplit = request.RenBaoRateSplit; //.ToJson();
            }
            //特殊折扣申请
            if (request.SpecialDiscount > 0)
            {
                newquotereq.special_discount_application = request.SpecialDiscount;
            }
            //是否修改座位数
            if (request.SeatUpdated != -1)
            {
                newquotereq.seatflag = request.SeatUpdated;
            }
            //太平洋实际折扣系数 20180725by gpj
            newquotereq.ActualDiscounts = request.ActualDiscounts;
            newquotereq.VehicleAlias = request.VehicleAlias;
            newquotereq.VehicleYear = request.VehicleYear;

            if (string.IsNullOrWhiteSpace(request.DiscountJson))
            {
                newquotereq.TrCausesWhy = "";
                newquotereq.SubmitRate = 0;
                newquotereq.ChannelRate = 0;
                newquotereq.actualdiscounts_ratio = "";
            }
            else
            {
                //前端传折扣json进来
                List<DiscountViewModel> dclist = new List<DiscountViewModel>();
                try
                {
                    //将折扣json转成对象
                    dclist = request.DiscountJson.FromJson<List<DiscountViewModel>>();
                }
                catch (Exception ex)
                {

                }
                if (dclist.Any())
                {
                    //拼接actualdiscounts_ratio的数组
                    Dictionary<int, decimal> dictionary = new Dictionary<int, decimal>();
                    foreach (var dc in dclist)
                    {
                        if (dc.Source == 1)
                        {//兼容之前逻辑，把太平洋单独字段存ActualDiscounts中
                            newquotereq.ActualDiscounts = dc.AD;
                        }
                        if (dc.Source == 2)
                        {//平安单独保存3个字段
                            newquotereq.TrCausesWhy = dc.TRCR;
                            newquotereq.SubmitRate = dc.SR;
                            newquotereq.ChannelRate = dc.CR;
                        }
                        //其他折扣率直接存json
                        dictionary.Add(SourceGroupAlgorithm.GetOldSource(dc.Source), dc.AD);
                    }
                    //只有当折扣率有值的时候才保存进来//不过此处肯定有值，除非参数有问题
                    if (dictionary.Any())
                    {
                        newquotereq.actualdiscounts_ratio = dictionary.ToJson();
                    }
                }
            }


            newquotereq.ActualSalesForceRatio = request.ActualSalesForceRatio;
            newquotereq.ActualSalesBizRatio = request.ActualSalesBizRatio;
            newquotereq.ActualDtaryForceRatio = request.ActualDtaryForceRatio;
            newquotereq.ActualDtaryBizRatio = request.ActualDtaryBizRatio;
            //是否暂存
            newquotereq.IsTempStorage = request.IsTempStorage;

            //平安底价报价
            newquotereq.IsPaFloorPrice = request.IsPaFloorPrice;
            newquotereq.DriverCard = request.DriverCard;
            newquotereq.DriverCardType = request.DriverCardType;
            repository.Add(newquotereq);
        }

        public static void QuoteUpdate(bx_quotereq_carinfo quotereq, PostPrecisePriceRequest request, int publictype, IQuoteReqCarinfoRepository repository)
        {
            //经办人、送修码、归属人等设置，重写source值
            //if (!string.IsNullOrWhiteSpace(request.ConfigCode))
            //{
            //    //将用户传的configcode的source改为旧值。
            //    var configcodemodel = request.ConfigCode.FromJson<Dictionary<string, Dictionary<string, string>>>();
            //    var newconfigcodemodel = new Dictionary<string, Dictionary<string, string>>();
            //    foreach (var item in configcodemodel) {
            //        newconfigcodemodel.Add(SourceGroupAlgorithm.GetOldSource(long.Parse(item.Key)).ToString(),item.Value);
            //    }
            //    request.ConfigCode = newconfigcodemodel.ToJson();
            //}
            quotereq.account_relation = request.ConfigCode;

            if (request.SeatCount > 0)
            {
                quotereq.seat_count = request.SeatCount;
            }
            if (request.TonCount > 0)
            {
                quotereq.car_ton_count = request.TonCount;
            }
            quotereq.is_loans = request.IsLoans;
            quotereq.car_type = request.CarType;
            quotereq.car_used_type = request.CarUsedType;
            quotereq.is_newcar = request.IsNewCar;
            quotereq.update_time = DateTime.Now;
            //浮动价格
            if (request.NegotiatePrice >= 0)
            {
                quotereq.co_real_value = request.NegotiatePrice;
            }
            else
            {
                quotereq.co_real_value = 0;
            }
            //车型选择source
            if (request.VehicleSource > 0)
            {
                quotereq.moldcode_company = request.VehicleSource;
            }
            //过户车日期逻辑
            if (!string.IsNullOrWhiteSpace(request.TransferDate))
            {
                quotereq.transfer_date = DateTime.Parse(request.TransferDate);
            }
            else
            {
                quotereq.transfer_date = null;
            }
            if (!string.IsNullOrWhiteSpace(request.BizStartDate))
            {
                quotereq.biz_start_date = DateTime.Parse(request.BizStartDate);
            }
            else
            {
                quotereq.biz_start_date = null;
            }
            if (!string.IsNullOrWhiteSpace(request.ForceStartDate))
            {
                quotereq.force_start_date = DateTime.Parse(request.ForceStartDate);
            }
            else
            {
                quotereq.force_start_date = null;
            }
            //精友编码
            if (!string.IsNullOrWhiteSpace(request.AutoMoldCode))
            {
                quotereq.auto_model_code = request.AutoMoldCode.Trim();
                quotereq.auto_model_code_source = 2;
            }
            else
            {
                //20181119，解决续保车补充到其他保司的精友码，中心查到carmold的车型信息跟平安保司不匹配
                //如果是平安，则把数据库的精友码删掉
                if (request.QuoteGroup == 2)
                {
                    quotereq.auto_model_code = null;
                    quotereq.auto_model_code_source = null;
                }
            }
            if (request.AutoMoldCodeSource > 0)
            {
                quotereq.auto_model_code_source = request.AutoMoldCodeSource;
            }
            //平安备注
            if (!string.IsNullOrWhiteSpace(request.Remark))
            {
                quotereq.pa_remark = request.Remark;
            }
            else
            {
                quotereq.pa_remark = string.Empty;
            }
            //时间戳支持
            if (!string.IsNullOrWhiteSpace(request.BizTimeStamp))
            {
                quotereq.biz_start_date =
                    DateTime.Parse(request.BizTimeStamp.UnixTimeToDateTime().ToString("yyyy-MM-dd HH") +
                                   ":00:00");
                ;
            }
            //else
            //{
            //    quotereq.biz_start_date = null;
            //}
            if (!string.IsNullOrWhiteSpace(request.ForceTimeStamp))
            {
                quotereq.force_start_date =
                    DateTime.Parse(request.ForceTimeStamp.UnixTimeToDateTime().ToString("yyyy-MM-dd HH") +
                                   ":00:00");
            }
            //else
            //{
            //    quotereq.force_start_date = null;
            //}
            //商业险短时起保
            if (!string.IsNullOrWhiteSpace(request.BizShortEndDate))
            {
                quotereq.biz_end_date =
                    DateTime.Parse(request.BizShortEndDate.UnixTimeToDateTime().ToString("yyyy-MM-dd HH") +
                                   ":00:00");
                quotereq.IsShortQuote = 1;
            }
            else
            {
                quotereq.biz_end_date = null;
                quotereq.IsShortQuote = 0;
            }
            //if (request.IsPublic > 0)
            //{
            //    quotereq.is_public = request.IsPublic;
            //}
            if (request.CarUsedType == 6 || request.CarUsedType == 7 || request.CarUsedType == 20)
            {
            }
            else
            {
                if (publictype > 0)
                {
                    quotereq.is_public = publictype;
                }
            }
            if (request.ClauseType > 0)
            {
                quotereq.clause_type = request.ClauseType;
            }
            if (request.FuelType > 0)
            {
                quotereq.fuel_type = request.FuelType;
            }
            if (request.ProofType > 0)
            {
                quotereq.proof_type = request.ProofType;
            }
            if (request.RunMiles > 0)
            {
                quotereq.run_miles = request.RunMiles;
            }
            if (request.RateFactor4 > 0)
            {
                quotereq.special_rate = request.RateFactor4;
            }
            if (request.LicenseColor > 0)
            {
                quotereq.license_color = request.LicenseColor;
            }
            if (request.SeatCount > 0)
            {
                quotereq.seat_count = request.SeatCount;
            }
            if (request.ExhaustScale > 0)
            {
                quotereq.exhaust_scale = request.ExhaustScale;
            }
            if (request.PurchasePrice > 0)
            {
                quotereq.PriceT = request.PurchasePrice;
            }
            quotereq.drivlicense_cartype_name = request.DriveLicenseTypeName;
            quotereq.drivlicense_cartype_value = request.DriveLicenseTypeValue;
            if (!string.IsNullOrWhiteSpace(request.OwnerOrg))
            {
                quotereq.ownerOrg = request.OwnerOrg;
            }
            if (!string.IsNullOrWhiteSpace(request.PolicyOrg))
            {
                quotereq.policyOrg = request.PolicyOrg;
            }
            if (!string.IsNullOrWhiteSpace(request.RenBaoRateSplit))
            {
                quotereq.renBaoRateSplit = request.RenBaoRateSplit; //.ToJson();
            }
            if (request.SpecialDiscount > 0)
            {
                quotereq.special_discount_application = request.SpecialDiscount;
            }
            //是否修改座位数
            if (request.SeatUpdated != -1)
            {
                quotereq.seatflag = request.SeatUpdated;
            }
            //太平洋实际折扣系数 20180725by gpj
            quotereq.ActualDiscounts = request.ActualDiscounts;
            quotereq.VehicleAlias = request.VehicleAlias;
            quotereq.VehicleYear = request.VehicleYear;

            if (string.IsNullOrWhiteSpace(request.DiscountJson))
            {
                quotereq.TrCausesWhy = "";
                quotereq.SubmitRate = 0;
                quotereq.ChannelRate = 0;
                quotereq.actualdiscounts_ratio = "";
            }
            else
            {
                //前端传折扣json进来
                List<DiscountViewModel> dclist = new List<DiscountViewModel>();
                try
                {
                    //将折扣json转成对象
                    dclist = request.DiscountJson.FromJson<List<DiscountViewModel>>();
                }
                catch (Exception ex)
                {

                }
                if (dclist.Any())
                {
                    //拼接actualdiscounts_ratio的数组
                    Dictionary<int, decimal> dictionary = new Dictionary<int, decimal>();
                    foreach (var dc in dclist)
                    {
                        if (dc.Source == 1)
                        {//兼容之前逻辑，把太平洋单独字段存ActualDiscounts中
                            quotereq.ActualDiscounts = dc.AD;
                        }
                        if (dc.Source == 2)
                        {//平安单独保存3个字段
                            quotereq.TrCausesWhy = dc.TRCR;
                            quotereq.SubmitRate = dc.SR;
                            quotereq.ChannelRate = dc.CR;
                        }
                        //其他折扣率直接存json
                        dictionary.Add(SourceGroupAlgorithm.GetOldSource(dc.Source), dc.AD);
                    }
                    //只有当折扣率有值的时候才保存进来//不过此处肯定有值，除非参数有问题
                    if (dictionary.Any())
                    {
                        quotereq.actualdiscounts_ratio = dictionary.ToJson();
                    }
                }
            }

            quotereq.ActualSalesForceRatio = request.ActualSalesForceRatio;
            quotereq.ActualSalesBizRatio = request.ActualSalesBizRatio;
            quotereq.ActualDtaryForceRatio = request.ActualDtaryForceRatio;
            quotereq.ActualDtaryBizRatio = request.ActualDtaryBizRatio;
            //是否暂存
            quotereq.IsTempStorage = request.IsTempStorage;
            //平安底价报价
            quotereq.IsPaFloorPrice = request.IsPaFloorPrice;
            quotereq.DriverCard = request.DriverCard;
            quotereq.DriverCardType = request.DriverCardType;
            repository.Update(quotereq);
        }
    }
}