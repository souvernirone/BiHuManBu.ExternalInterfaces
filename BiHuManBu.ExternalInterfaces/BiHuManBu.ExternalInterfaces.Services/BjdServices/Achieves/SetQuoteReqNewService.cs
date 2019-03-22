using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;
using System.Collections.Generic;
using System.Linq;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves
{
    public class SetQuoteReqNewService : ISetQuoteReqNewService
    {
        private ILog logErr = LogManager.GetLogger("ERROR");
        public SetQuoteReqNewService()
        {
        }
        public MyBaoJiaViewModel SetQuoteReq(MyBaoJiaViewModel my, List<long> listquote1, bx_quotereq_carinfo quotereqcarinfo, ref string postBizStartDate, ref string postForceStartDate)
        {
            var quotereqCarInfo = quotereqcarinfo;
            my.IsNewCar = 2;//默认赋值旧车2
            //公车私车
            if (quotereqCarInfo != null)
            {
                //报价失败，取req表起保时间
                if (listquote1.Any())
                {
                    postForceStartDate = quotereqCarInfo.force_start_date.HasValue
                        ? quotereqCarInfo.force_start_date.Value.ToString("yyyy-MM-dd HH:mm") : "";
                    postBizStartDate = quotereqCarInfo.biz_start_date.HasValue
                        ? quotereqCarInfo.biz_start_date.Value.ToString("yyyy-MM-dd HH:mm") : "";
                }
                my.IsPublic = quotereqCarInfo.is_public.HasValue ? quotereqCarInfo.is_public.Value : 0;
                if (quotereqCarInfo.is_newcar.HasValue)
                {
                    if (quotereqCarInfo.is_newcar.Value != 0)
                    {
                        my.IsNewCar = quotereqCarInfo.is_newcar.Value;
                    }
                }
                my.AutoMoldCode = quotereqCarInfo.auto_model_code;
                my.CoRealValue = quotereqCarInfo.co_real_value.HasValue ? quotereqCarInfo.co_real_value.Value : 0;
                my.CarUsedType = quotereqCarInfo.car_used_type;
                my.SeatCount = quotereqCarInfo.seat_count;

                //报价返回请求值
                my.ReqInfo = new QuoteReqCarInfoViewModel()
                {
                    AutoMoldCode = quotereqCarInfo.auto_model_code ?? "",
                    IsNewCar = quotereqCarInfo.is_newcar ?? 2,
                    NegotiatePrice = quotereqCarInfo.co_real_value ?? 0,
                    IsPublic = quotereqCarInfo.is_public ?? 0,
                    CarUsedType = quotereqCarInfo.car_used_type ?? 0,
                    AutoMoldCodeSource = quotereqCarInfo.auto_model_code_source ?? -1,
                    DriveLicenseTypeName = quotereqCarInfo.drivlicense_cartype_name ?? "",
                    DriveLicenseTypeValue = quotereqCarInfo.drivlicense_cartype_value ?? "",
                    SeatUpdated = (quotereqCarInfo.seatflag ?? -1).ToString(),
                    RequestActualDiscounts=(quotereqCarInfo.ActualDiscounts??0).ToString(),
                    RequestIsPaFloorPrice = (quotereqCarInfo.IsPaFloorPrice ?? 0).ToString(),
                    DriverCard= quotereqCarInfo.DriverCard ?? "",
                    DriverCardType = quotereqCarInfo.DriverCardType ?? "",
                };
                //addby20180915继续拼装viewModel.ReqInfo对象，这是新增的折扣率的对象
                if (quotereqCarInfo != null)
                {
                    List<DiscountViewModel> dclist = new List<DiscountViewModel>();
                    DiscountViewModel dc;
                    Dictionary<int, decimal> dictionary = new Dictionary<int, decimal>();
                    dictionary = quotereqCarInfo.actualdiscounts_ratio.FromJson<Dictionary<int, decimal>>();
                    if (dictionary != null)
                    {
                        foreach (var item in dictionary)
                        {
                            dc = new DiscountViewModel()
                            {
                                Source = SourceGroupAlgorithm.GetNewSource(item.Key),
                                AD = item.Value,
                                CR = 0,
                                SR = 0,
                                TRCR = ""
                            };
                            //平安的特殊处理，取单独的字段
                            if (item.Key == 0)
                            {
                                dc.CR = quotereqCarInfo.ChannelRate ?? 0;
                                dc.SR = quotereqCarInfo.SubmitRate ?? 0;
                                dc.TRCR = quotereqCarInfo.TrCausesWhy ?? "";
                            }
                            dclist.Add(dc);
                        }
                    }
                    my.ReqInfo.RequestDiscount = dclist;
                }
            }
            else
            {
                my.IsPublic = 0;
                my.CoRealValue = 0;
                my.ReqInfo = new QuoteReqCarInfoViewModel()
                {
                    AutoMoldCode = "",
                    IsNewCar = 2,
                    NegotiatePrice = 0,
                    IsPublic = 0,
                    CarUsedType = 0,
                    AutoMoldCodeSource = -1,
                    DriveLicenseTypeName = "",
                    DriveLicenseTypeValue = "",
                    SeatUpdated = "-1",
                    RequestActualDiscounts = "0",
                    RequestIsPaFloorPrice="0",
                    DriverCard="",
                    DriverCardType="",
                    RequestDiscount = new List<DiscountViewModel>()
                };
            }
            return my;
        }
    }
}
