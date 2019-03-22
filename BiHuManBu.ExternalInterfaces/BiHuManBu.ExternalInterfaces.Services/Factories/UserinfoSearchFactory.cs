using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;

namespace BiHuManBu.ExternalInterfaces.Services.Factories
{
    public class UserinfoSearchFactory
    {
        public static bx_userinfo Find(GetReInfoRequest request, IUserInfoRepository infoRepository)
        {
            bx_userinfo userinfo = null;
            switch (request.IsLastYearNewCar)
            {
                case 1:
                    userinfo = infoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo,request.Agent.ToString(),request.RenewalCarType);
                    break;
                case 2:
                    userinfo = infoRepository.FindByCarvin(request.CarVin,request.CustKey, request.Agent.ToString(), request.RenewalCarType);
                    break;
            }
            return userinfo;
        }

        public static bx_userinfo FindByQuoteRequest(PostPrecisePriceRequest request, IUserInfoRepository infoRepository)
        {
            bx_userinfo userinfo = null;
            bool isCarLicenseno = false;
            if (!string.IsNullOrWhiteSpace(request.LicenseNo))
            {
                isCarLicenseno = CommonHelper.IsValidLicenseno(request.LicenseNo);
            }
          
            if (isCarLicenseno)
            {
                userinfo = infoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo,
                    request.Agent.ToString(),request.RenewalCarType);
            }
            else
            {
                //modify20181124，由根据车架+发动机号查询改为只根据车架查询。
                //目前已支持单车架号续保，所以这里匹配报价数据只根据车架号查
                 userinfo = infoRepository.FindByCarvin(request.CarVin, request.CustKey,
                        request.Agent.ToString(),request.RenewalCarType);
            }
            
            return userinfo;
        }
    }
}
