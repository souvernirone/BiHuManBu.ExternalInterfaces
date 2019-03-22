using BiHuManBu.ExternalInterfaces.Infrastructure.CacheKeyFactory;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class RecordNewCarService : IRecordNewCarService
    {
        private readonly ICarRecordRepository _carRecordRepository;
        private readonly IMessageCenter _messageCenter;
        private readonly ILog logError = LogManager.GetLogger("ERROR");
        public RecordNewCarService(ICarRecordRepository carRecordRepository, IMessageCenter messageCenter)
        {
            _carRecordRepository = carRecordRepository;
            _messageCenter = messageCenter;
        }

        public async Task<BaseViewModel> RecordNewCar(RecordNewCarRequest request)
        {
            int source = SourceGroupAlgorithm.GetOldSource(request.Source);
            bx_car_record model = _carRecordRepository.GetModel(request.CarVin, request.EngineNo);
            if (model != null && model.Id > 0)
            {
                if (model.RecordStatus > 0)
                {
                    //如果1分钟之内提交过，则不允许再请求
                    return new BaseViewModel() { BusinessStatus = -10001, StatusMessage = "该车已提交过备案，请勿重复提交！" };
                }
                if (model.UpdateTime.Value.AddSeconds(30) > DateTime.Now)
                {
                    //如果30s之内提交过，则不允许再请求
                    return new BaseViewModel() { BusinessStatus = -10001, StatusMessage = "刚提交过新车备案，请稍后再试！" };
                }
                //此处else省略是因为下面可以通用，只修改updatetime
            }
            #region 初始化字段，存数据库值
            else
            {
                model = new bx_car_record();
                model.CreateTime = DateTime.Now;
            }
            model.CarVin = request.CarVin;
            model.EngineNo = request.EngineNo;
            model.DriveLicenseCarTypeName = request.DriveLicenseCarTypeName;
            model.DriveLicenseCarTypeValue = request.DriveLicenseCarTypeValue;
            model.LicenseOwner = request.LicenseOwner;
            model.LicenseOwnerIdType = request.LicenseOwnerIdType;
            model.LicenseOwnerIdTypeValue = request.LicenseOwnerIdTypeValue;
            model.LicenseOwnerIdNo = request.LicenseOwnerIdNo;
            model.TonCountflag = request.TonCountflag;
            model.CarTonCount = request.CarTonCount;
            model.CarLotEquQuality = request.CarLotEquQuality;
            model.VehicleExhaust = request.VehicleExhaust;
            model.FuelType = request.FuelType;
            model.FuelTypeValue = request.FuelTypeValue;
            model.ProofType = request.ProofType;
            model.ProofTypeValue = request.ProofTypeValue;
            model.ProofNo = request.ProofNo;
            model.ProofTime = !string.IsNullOrWhiteSpace(request.ProofTime) ? DateTime.Parse(request.ProofTime) : DateTime.MinValue;
            model.CarPrice = request.CarPrice;
            model.VehicleYear = request.VehicleYear;
            model.VehicleName = request.VehicleName;
            model.ModelCode = request.ModelCode;
            model.RecordStatus = 0;//初始化时是0
            model.UpdateTime = DateTime.Now;
            model.Source = source;
            int record = _carRecordRepository.AddUpdateCarRecord(model);
            #endregion
            if (record == 0)
            {
                return new BaseViewModel() { BusinessStatus = 0, StatusMessage = "请求失败，数据未保存成功" };
            }
            #region 给中心发消息
            string xuBaoCacheKey = CommonCacheKeyFactory.CreateKeyWithLicense(request.CarVin);
            var xuBaoKey = string.Format("{0}-CarRecord", xuBaoCacheKey);
            CacheProvider.Remove(xuBaoKey);
            var msgBody = new
            {
                Source = source,//哪家渠道
                ChannelId = request.ChannelId,
                CarVin = request.CarVin,//车架号
                EngineNo = request.EngineNo,//发动机号
                NotifyCacheKey = xuBaoCacheKey,
                RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            //发送续保信息
            var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(), ConfigurationManager.AppSettings["MessageCenter"], ConfigurationManager.AppSettings["BxCarRecord"]);
            try
            {
                var cacheKey = CacheProvider.Get<string>(xuBaoKey);
                if (cacheKey == null)
                {
                    for (int i = 0; i < 60; i++)
                    {
                        cacheKey = CacheProvider.Get<string>(xuBaoKey);
                        if (!string.IsNullOrWhiteSpace(cacheKey))
                        {
                            break;
                        }
                        else
                        {
                            await Task.Delay(TimeSpan.FromSeconds(1));
                        }
                    }
                }
                if (cacheKey == null)
                {
                    return new BaseViewModel() { BusinessStatus = -1, StatusMessage = "请求失败，数据与保司交互超时。" };
                }
                else
                {
                    WaBaseResponse response = new WaBaseResponse();
                    response = cacheKey.FromJson<WaBaseResponse>();
                    if (response.ErrCode == 1)
                    {
                        return new BaseViewModel() { BusinessStatus = 1, StatusMessage = "请求成功" };
                    }
                    else
                    {
                        logError.Info(string.Format("新车备案请求调用中心失败:请求参数为：{0}，返回值：{1}", request.ToJson(), cacheKey));
                        return new BaseViewModel() { BusinessStatus = 0, StatusMessage = response.ErrMsg };
                    }
                }
            }
            catch (Exception ex)
            {
                logError.Info("新车备案请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                return new BaseViewModel() { BusinessStatus = -10003, StatusMessage = "请求发生异常！" };
            }
            #endregion
        }

        public async Task<GetRecordNewCarViewModel> GetRecordNewCar(GetRecordNewCarRequest request)
        {
            GetRecordNewCarViewModel viewModel = new GetRecordNewCarViewModel();
            try
            {
                bx_car_record model = _carRecordRepository.GetModel(request.CarVin, request.EngineNo);
                if (model != null && model.Id > 0)
                {
                    //1成功2失败。如果失败，可以继续修改
                    if (model.RecordStatus.HasValue)
                    {
                        if (model.RecordStatus.Value == 1)
                            viewModel.HasRecord = 1;
                        else
                        {
                            viewModel.HasRecord = 2;
                        }
                    }
                    else
                    {
                        viewModel.HasRecord = 2;
                    }
                    //返回实体item对象
                    RecordNewCarModel item = new RecordNewCarModel
                    {
                        CarLotEquQuality = model.CarLotEquQuality,
                        CarPrice = model.CarPrice??0,
                        CarTonCount = model.CarTonCount,
                        CarVin = model.CarVin,
                        DriveLicenseCarTypeName = model.DriveLicenseCarTypeName,
                        DriveLicenseCarTypeValue = model.DriveLicenseCarTypeValue,
                        EngineNo = model.EngineNo,
                        FuelType = model.FuelType,
                        FuelTypeValue = model.FuelTypeValue,
                        LicenseOwner = model.LicenseOwner,
                        LicenseOwnerIdNo = model.LicenseOwnerIdNo,
                        LicenseOwnerIdType = model.LicenseOwnerIdType,
                        LicenseOwnerIdTypeValue = model.LicenseOwnerIdTypeValue,
                        ModelCode = model.ModelCode,
                        ProofNo = model.ProofNo,
                        ProofTime = model.ProofTime.HasValue ? model.ProofTime.Value.ToString("yyyy-MM-dd") : "",
                        ProofType = model.ProofType,
                        ProofTypeValue = model.ProofTypeValue,
                        TonCountflag = model.TonCountflag,
                        VehicleExhaust = model.VehicleExhaust??0,
                        VehicleName = model.VehicleName,
                        VehicleYear = model.VehicleYear
                    };
                    viewModel.RecordNewCarModel = item;
                }
                else
                {
                    viewModel.HasRecord = 0;
                    viewModel.RecordNewCarModel = new RecordNewCarModel()
                    {
                        CarVin = "",
                        EngineNo = "",
                        DriveLicenseCarTypeName = "",
                        DriveLicenseCarTypeValue = "",
                        LicenseOwner = "",
                        LicenseOwnerIdType = 0,
                        LicenseOwnerIdTypeValue = "",
                        LicenseOwnerIdNo = "",
                        TonCountflag = 0,
                        CarTonCount = "",
                        CarLotEquQuality = "",
                        VehicleExhaust = 0,
                        FuelType = "",
                        FuelTypeValue = "",
                        ProofType = "",
                        ProofTypeValue = "",
                        ProofNo = "",
                        ProofTime = "",
                        CarPrice = 0,
                        VehicleYear = "",
                        VehicleName = "",
                        ModelCode = ""

                    };
                }
                viewModel.BusinessStatus = 1;
                viewModel.StatusMessage = "获取信息成功";
            }
            catch (Exception ex)
            {
                logError.Info("新车备案请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                return new GetRecordNewCarViewModel() { BusinessStatus = -10003, StatusMessage = "请求发生异常！", RecordNewCarModel = new RecordNewCarModel(), HasRecord = 0 };
            }
            return viewModel;
        }
    }
}
