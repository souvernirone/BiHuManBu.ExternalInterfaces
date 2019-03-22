using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using System.Net;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves
{
    public class CheckRequestService : ICheckRequestService
    {
        private IValidateService _validateService;
        private ILog logErr;
        public CheckRequestService(IValidateService validateService)
        {
            _validateService = validateService;
            logErr = LogManager.GetLogger("ERROR");
        }
        public MyBaoJiaViewModel CheckRequest(bx_userinfo userInfo, GetMyBjdDetailRequest request)
        {
            var my = new MyBaoJiaViewModel();

            if (userInfo == null || userInfo.Id == 0)
            {
                my.BusinessStatus = -10000;
                my.StatusMessage = "无法获取数据";
                return my;
            }
            if (request.NewRate != null && !SourceGroupAlgorithm.ParseSource((int)userInfo.IsSingleSubmit).Contains(8))
            {
                my.BusinessStatus = 0;
                my.StatusMessage = "没有找到国寿财报价信息";
                return my;
            }
            BaseResponse baseResponse = _validateService.ValidateAgent(request, userInfo);
            if (baseResponse.Status == HttpStatusCode.Forbidden)
            {
                my.BusinessStatus = -10000;
                my.StatusMessage = "请求数据无权限";
            }
            else my.BusinessStatus = 1;

            return my;
        }
    }
}
