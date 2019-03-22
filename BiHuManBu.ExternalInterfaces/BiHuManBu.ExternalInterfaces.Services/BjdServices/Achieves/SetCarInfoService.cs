using System;
using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Achieves;
using System.Net;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models.IRepository;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves
{
    public class SetCarInfoService : ISetCarInfoService
    {
        private IQuoteResultCarinfoRepository _quoteResultCarinfoRepository;
        private ILog logErr;
        public SetCarInfoService(IQuoteResultCarinfoRepository quoteResultCarinfoRepository)
        {
            _quoteResultCarinfoRepository = quoteResultCarinfoRepository;
            logErr = LogManager.GetLogger("ERROR");
        }
        public MyBaoJiaViewModel SetCarInfo(MyBaoJiaViewModel my, bx_userinfo userInfo, out List<bx_quoteresult_carinfo> quoteresultCarinfo)
        {
            quoteresultCarinfo = _quoteResultCarinfoRepository.FindList(userInfo.Id);//, item.Source.Value
            my.CarInfos = new List<DiffCarInfo>();
            foreach (var car in quoteresultCarinfo)
            {
                my.CarInfos.Add(new DiffCarInfo
                {
                    Source = car.source.HasValue ? SourceGroupAlgorithm.GetNewSource(car.source.Value) : 0,
                    AutoMoldCode = string.IsNullOrWhiteSpace(car.auto_model_code) ? string.Empty : car.auto_model_code,
                    CarSeat = car.seat_count.HasValue ? car.seat_count.Value : 0,
                    VehicleInfo = VehicleInfoMapper.VehicleInfoMethod(car),
                    Risk = car.risk,
                    XinZhuanXu = car.IsZhuanXubao,
                    SyVehicleClaimType = car.SyVehicleClaimType,
                    JqVehicleClaimType = car.JqVehicleClaimType,
                    VehicleStyle = car.VehicleStyle,
                    VehicleAlias=car.VehicleAlias,
                    VehicleYear=car.VehicleYear
                });
            }

            return my;
        }
    }
}
