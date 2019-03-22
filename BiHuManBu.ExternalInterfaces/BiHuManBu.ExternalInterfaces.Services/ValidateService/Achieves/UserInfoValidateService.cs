using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.ValidateService.Achieves
{
    public class UserInfoValidateService:IUserInfoValidateService
    {
        private readonly IUserInfoRepository _userInfoRepository;
        public UserInfoValidateService(IUserInfoRepository userInfoRepository) {
            _userInfoRepository = userInfoRepository;
        }
        public Tuple<BaseResponse, bx_userinfo> UserInfoValidate(UserInfoValidateRequest request)
        {
            BaseResponse response = new BaseResponse();
            //校验：2userinfo存在记录
            bx_userinfo userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.ChildAgent.ToString(), request.RenewalCarType);
            if (userinfo == null)
            {
                response.Status = HttpStatusCode.NotAcceptable;
                response.ErrMsg = "记录不存在，请检查参数，重新发起续保或报价。";
                return Tuple.Create<BaseResponse, bx_userinfo>(response, null);
            }
            //校验：3重新核保的source必须在之前报价+核保过（bx_userinfo的source和issinglesubmit包含这个值）
            //核保source集合
            //long submitinfosource = userinfo.Source ?? 0;
            //报价source集合
            //long quotesource = userinfo.IsSingleSubmit ?? 0;
            //if (submitinfosource == 0 || quotesource == 0)
            //{
            //    response.Status = HttpStatusCode.NotAcceptable;
            //    return Tuple.Create<BaseResponse, bx_userinfo>(response, null);
            //}
            return Tuple.Create<BaseResponse, bx_userinfo>(response, userinfo);
        }
    }
}
