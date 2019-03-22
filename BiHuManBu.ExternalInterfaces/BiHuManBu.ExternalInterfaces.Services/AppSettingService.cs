using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class AppSettingService : CommonBehaviorService, IAppSettingService
    {
        private IAppSettingRepository _appSettingRepository;

        public AppSettingService(IAppSettingRepository appSettingRepository, IAgentRepository agentRepository, ICacheHelper cacheHelper)
            : base(agentRepository, cacheHelper)
        {
            _appSettingRepository = appSettingRepository;
        }

        public GetAppVersionResponse GetAppVersion(GetAppVersionRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new GetAppVersionResponse();

            //参数校验
            //bx_agent agentModel = GetAgent(request.Agent);
            //if (agentModel == null)
            //{
            //    response.Status = HttpStatusCode.Forbidden;
            //    return response;
            //}
            //if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
            //{
            //    response.Status = HttpStatusCode.Forbidden;
            //    return response;
            //}

            switch (request.RenewalType)
            {
                case 6: response.AppSetting = _appSettingRepository.FindByKey("IOSUpdate");
                    break;
                case 7: response.AppSetting = _appSettingRepository.FindByKey("AndroidUpdate");
                    break;
                default:
                    break;
            }
            if (response.AppSetting == null)
            {
                response.ErrCode = -1;
            }

            return response;
        }

    }
}
