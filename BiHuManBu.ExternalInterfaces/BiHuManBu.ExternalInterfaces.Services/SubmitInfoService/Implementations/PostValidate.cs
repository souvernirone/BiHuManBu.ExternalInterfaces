using System;
using System.Net;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.SubmitInfoService.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.SubmitInfoService.Implementations
{
    public class PostValidate : IPostValidate
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly ISubmitInfoRepository _submitInfoRepository;
        public PostValidate(IUserInfoRepository userInfoRepository, ISubmitInfoRepository submitInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
            _submitInfoRepository = submitInfoRepository;
        }

        public Tuple<BaseResponse, bx_userinfo, bx_submit_info> SubmitInfoValidate(PostSubmitInfoRequest request)
        {
            BaseResponse response = new BaseResponse();
            //校验：2userinfo存在记录
            bx_userinfo userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.ChildAgent.ToString(), request.RenewalCarType);
            if (userinfo == null)
            {
                response.Status = HttpStatusCode.NotAcceptable;
                return Tuple.Create<BaseResponse, bx_userinfo, bx_submit_info>(response, null, null);
            }
            //校验：3重新核保的source必须在之前报价+核保过（bx_userinfo的source和issinglesubmit包含这个值）
            //核保source集合
            //long submitinfosource = userinfo.Source ?? 0;
            //报价source集合
            long quotesource = userinfo.IsSingleSubmit ?? 0;
            //20181112把核保过的校验去掉，此处为沃云保双录报价做的处理
            if (quotesource == 0)
            {
                response.Status = HttpStatusCode.NotAcceptable;
                return Tuple.Create<BaseResponse, bx_userinfo, bx_submit_info>(response, null, null);
            }
            if ((quotesource & request.Source) != request.Source)
            {
                response.Status = HttpStatusCode.NotAcceptable;
                return Tuple.Create<BaseResponse, bx_userinfo, bx_submit_info>(response, null, null);
            }
            //校验：4是否存在核保记录
            bx_submit_info submitInfo = _submitInfoRepository.GetSubmitInfo(userinfo.Id,
                SourceGroupAlgorithm.GetOldSource(request.Source));
            if (submitInfo == null)
            {
                response.Status = HttpStatusCode.NotAcceptable;
            }
            return Tuple.Create<BaseResponse, bx_userinfo, bx_submit_info>(response, userinfo, submitInfo);
        }
    }
}
