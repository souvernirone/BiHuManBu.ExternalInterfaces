using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System;
using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations;
using ServiceStack.Text;
using System.Text.RegularExpressions;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations
{
    public class CheckRequestPostPrecisePrice : ICheckRequestPostPrecisePrice
    {
        private readonly ICheckReqInsurance _checkReqInsurance;
        private readonly ICheckReqBaseInfo _checkReqBaseInfo;
        public CheckRequestPostPrecisePrice(ICheckReqInsurance checkReqInsurance, ICheckReqBaseInfo checkReqBaseInfo)
        {
            _checkReqInsurance = checkReqInsurance;
            _checkReqBaseInfo = checkReqBaseInfo;
        }

        /// <summary>
        /// 请求报价，校验请求参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseViewModel CheckRequest(PostPrecisePriceRequest request)
        {
            BaseViewModel viewModel = new BaseViewModel();

            //基础信息、车辆信息等校验
            viewModel = _checkReqBaseInfo.CheckBaseInfo(request);
            if (viewModel.BusinessStatus == -10000)
            {
                return viewModel;
            }
            //险种相关校验
            viewModel = _checkReqInsurance.CheckInsurance(request);
            if (viewModel.BusinessStatus == -10000)
            {
                return viewModel;
            }

            return viewModel;
        }

    }
}
