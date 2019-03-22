using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Repository.DbOper;
using BiHuManBu.ExternalInterfaces.Services.IndependentService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.SubmitInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.IndependentService.Implementations
{
    public class PostIndependentSubmitService : IPostIndependentSubmitService
    {
        private readonly IValidateService _validateService;
        private readonly IUserInfoValidateService _userInfoValidateService;
        private readonly IRemoveHeBaoKey _removeHeBaoKey;
        private readonly IMessageCenter _messageCenter;
        private readonly IRepository<bx_anxin_delivery> _anxindeliveryRepository;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly ISubmitInfoRepository _submitInfoRepository;
        public PostIndependentSubmitService(IValidateService validateService, IUserInfoValidateService userInfoValidateService,
            IRemoveHeBaoKey removeHeBaoKey, IMessageCenter messageCenter, IRepository<bx_anxin_delivery> anxindeliveryRepository,
            IUserInfoRepository userInfoRepository, ISubmitInfoRepository submitInfoRepository)
        {
            _validateService = validateService;
            _userInfoValidateService = userInfoValidateService;
            _removeHeBaoKey = removeHeBaoKey;
            _messageCenter = messageCenter;
            _anxindeliveryRepository = anxindeliveryRepository;
            _userInfoRepository = userInfoRepository;
            _submitInfoRepository = submitInfoRepository;
        }

        public async Task<BaseResponse> PostIndependentSubmit(PostIndependentSubmitRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            GetFloatingInfoResponse response = new GetFloatingInfoResponse();
            //校验：1基础校验
            BaseResponse baseResponse = _validateService.Validate(request, pairs);
            if (baseResponse.Status == HttpStatusCode.Forbidden)
            {
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            //校验：2报价基础信息
            UserInfoValidateRequest validateRequest = new UserInfoValidateRequest()
            {
                LicenseNo = request.LicenseNo,
                CustKey = request.CustKey,
                ChildAgent = request.ChildAgent == 0 ? request.Agent : request.ChildAgent,
                RenewalCarType = request.RenewalCarType
            };
            //校验2
            var validateResult = _userInfoValidateService.UserInfoValidate(validateRequest);
            if (validateResult.Item1.Status == HttpStatusCode.NotAcceptable)
            {
                response.Status = HttpStatusCode.NotAcceptable;
                return response;
            }
            //插库操作
            try
            {
                bx_userinfo userinfo = validateResult.Item2;
                userinfo.Source = (userinfo.Source.Value | request.Source);

                #region 车主信息
                if (!string.IsNullOrWhiteSpace(request.Mobile))
                {
                    userinfo.Mobile = request.Mobile;
                }

                if (!string.IsNullOrWhiteSpace(request.CarOwnersName))
                {
                    userinfo.LicenseOwner = request.CarOwnersName;
                }
                if (!string.IsNullOrWhiteSpace(request.IdCard))
                {
                    userinfo.IdCard = request.IdCard;
                }

                if (request.OwnerIdCardType >= 0)
                {
                    userinfo.OwnerIdCardType = request.OwnerIdCardType;
                }
                #endregion

                #region 被保险人信息
                if (!string.IsNullOrWhiteSpace(request.InsuredName))
                {
                    userinfo.InsuredName = request.InsuredName.Trim();
                }
                if (!string.IsNullOrWhiteSpace(request.InsuredIdCard))
                {
                    userinfo.InsuredIdCard = request.InsuredIdCard.ToUpper();
                    //if (request.InsuredIdCard.IsValidIdCard())
                    //{
                    //    request.InsuredIdType = 1;
                    //}
                }
                if (!string.IsNullOrWhiteSpace(request.InsuredEmail))
                {
                    userinfo.InsuredEmail = request.InsuredEmail;
                }
                if (!string.IsNullOrWhiteSpace(request.InsuredMobile))
                {
                    userinfo.InsuredMobile = request.InsuredMobile.Trim();
                }
                if (request.InsuredIdType >= 0)
                {
                    userinfo.InsuredIdType = request.InsuredIdType;
                }
                userinfo.InsuredAddress = request.InsuredAddress;
                //userinfo.InsuredCertiStartdate = request.InsuredCertiStartdate;
                //userinfo.InsuredCertiEnddate = request.InsuredCertiEnddate;
                //userinfo.InsuredSex = request.InsuredSex;
                //userinfo.InsuredBirthday = request.InsuredBirthday;
                //userinfo.InsuredIssuer = request.InsuredAuthority;
                //userinfo.InsuredNation = request.InsuredNation;

                #endregion
                #region 投保人信息
                if (!string.IsNullOrWhiteSpace(request.HolderEmail))
                {
                    userinfo.HolderEmail = request.HolderEmail;
                }
                if (!string.IsNullOrWhiteSpace(request.HolderName))
                {
                    userinfo.HolderName = request.HolderName.Trim();
                }
                if (!string.IsNullOrWhiteSpace(request.HolderIdCard))
                {
                    userinfo.HolderIdCard = request.HolderIdCard.ToUpper();
                    //if (request.HolderIdCard.IsValidIdCard())
                    //{
                    //    request.HolderIdType = 1;
                    //}
                }
                if (!string.IsNullOrWhiteSpace(request.HolderMobile))
                {
                    userinfo.HolderMobile = request.HolderMobile.Trim();
                }
                if (request.HolderIdType >= 0)
                {
                    userinfo.HolderIdType = request.HolderIdType;
                }
                userinfo.HolderAddress = request.HolderAddress;
                //userinfo.HolderCertiStartdate = request.HolderCertiStartdate;
                //userinfo.HolderCertiEnddate = request.HolderCertiEnddate;
                //userinfo.HolderSex = request.HolderSex;
                //userinfo.HolderBirthday = request.HolderBirthday;
                //userinfo.HolderIssuer = request.HolderAuthority;
                //userinfo.HolderNation = request.HolderNation;

                #endregion

                _userInfoRepository.Update(userinfo);
                bx_submit_info submitinfo = _submitInfoRepository.GetSubmitInfo(validateResult.Item2.Id, SourceGroupAlgorithm.GetOldSource(request.Source));
                bx_anxin_delivery oldData = _anxindeliveryRepository.Search(l => l.b_uid == validateResult.Item2.Id && l.status == 1).FirstOrDefault();
                if (oldData != null && oldData.id != 0)
                {
                    oldData.status = 0;
                    oldData.updatetime = DateTime.Now;
                    _anxindeliveryRepository.Update(oldData);
                }
                //先删除，再插入
                bx_anxin_delivery model = new bx_anxin_delivery()
                {
                    b_uid = validateResult.Item2.Id,
                    signincnm = request.SignerName,//签收人姓名
                    signintel = request.SignerTel,//签收人手机号
                    sendorderaddr = request.SingerAddress,//签收人地址
                    zipcde = request.ZipCode,//邮政编码
                    sy_plytyp = request.BizPolicyType,//商业保单形式
                    sy_invtype = request.BizElcInvoice,//商业电子发票形式
                    sy_appno = submitinfo != null ? submitinfo.biz_tno : "",//request.BizNo,//商业投保单号
                    jq_invtype = request.ForceElcInvoice,//交强电子发票类型
                    jq_plytyp = request.ForcePolicyType,//交强投保单形式
                    jq_appno = submitinfo != null ? submitinfo.force_tno : "",//request.ForceNO,//交强投保单号
                    appvalidateno = request.ProductNo,//产品代码
                    status = 1,//删除状态标示 0标识删除
                    createtime = DateTime.Now,
                    updatetime = DateTime.Now
                };
                _anxindeliveryRepository.Insert(model);
            }
            catch (Exception ex)
            { }
            //实现
            //清理缓存
            string baojiaCacheKey = string.Empty;
            try
            {
                PostSubmitInfoRequest newrequest = new PostSubmitInfoRequest()
                {
                    LicenseNo = request.LicenseNo,
                    ChildAgent = request.ChildAgent == 0 ? request.Agent : request.ChildAgent,
                    Agent = request.Agent,
                    RenewalCarType = request.RenewalCarType,
                    CustKey = request.CustKey,
                    Source = request.Source
                };
                baojiaCacheKey = _removeHeBaoKey.RemoveHeBao(newrequest);
            }
            catch (RedisOperateException exception)
            {
                response.Status = HttpStatusCode.ExpectationFailed;
                response.ErrMsg = exception.Message;
                return response;
            }
            //通知中心
            var msgBody = new
            {
                BUid = validateResult.Item2.Id,
                Source = SourceGroupAlgorithm.GetOldSource(request.Source),
                RedisKey = baojiaCacheKey,
                //20180509新增
                PayFinishUrl = request.PayFinishUrl,
                PayCancelUrl = request.PayCancelUrl,
                BgRetUrl = string.IsNullOrWhiteSpace(request.BgRetUrl) ? "http://buc.91bihu.com/api/PayOut/GetAXPayBack" : request.BgRetUrl,
                PayErrorUrl = request.PayErrorUrl,
                Attach = request.Attach
            };
            //发送安心核保消息
            try
            {
                var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(),
                    ConfigurationManager.AppSettings["MessageCenter"],
                    ConfigurationManager.AppSettings["BxAnXinHeBao"]);
            }
            catch (MessageException exception)
            {
                response.Status = HttpStatusCode.ExpectationFailed;
                response.ErrMsg = exception.Message;
                return response;
            }

            return response;
        }
    }
}
