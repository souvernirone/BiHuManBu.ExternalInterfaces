using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.UploadImgService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.UploadImgService.Implementations
{
    public class UploadImgValidate : IUploadImgValidate
    {
        private readonly ICheckUploadImgTimes _checkUploadImgTimes;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly ISubmitInfoRepository _submitInfoRepository;
        public UploadImgValidate(ICheckUploadImgTimes checkUploadImgTimes, IUserInfoRepository userInfoRepository, ISubmitInfoRepository submitInfoRepository)
        {
            _checkUploadImgTimes = checkUploadImgTimes;
            _userInfoRepository = userInfoRepository;
            _submitInfoRepository = submitInfoRepository;
        }

        public BaseViewModel Validate(UploadMultipleImgRequest request)
        {
            BaseViewModel viewModel = new BaseViewModel();
            #region 参数验证
            bx_userinfo userinfo = new bx_userinfo();
            if (request.BuId > 0)
            {
                //userinfo是否有值，buid是否正确
                userinfo = _userInfoRepository.FindByBuid(request.BuId);
            }
            //modifybygpj20181205修改，不开放前端传buid了
            else
            {
                //如果前端传子集代理Id，则用子集代理Id来查数据
                request.Agent = request.ChildAgent > 0 ? request.ChildAgent : request.Agent;
                //参数有效性校验
                if (string.IsNullOrWhiteSpace(request.LicenseNo) || string.IsNullOrWhiteSpace(request.CustKey) || request.Agent < 1)
                {
                    return BaseViewModel.GetBaseViewModel(-10000, "车牌号、CustKey、Agent不允许为空");
                }
                //根据车牌、代理、custkey来取数据库userinfo
                userinfo = _userInfoRepository.FindByOpenIdAndLicense(request.CustKey, request.LicenseNo, request.Agent.ToString(), request.RenewalCarType);
            }
            //如果userinfo没记录，直接提示错误参数
            if (userinfo == null || userinfo.Id == 0)
            {
                return BaseViewModel.GetBaseViewModel(-10000, "参数错误，无法拿到请求的车辆信息");
            }
            //给buid赋值
            if (request.BuId == 0)
            {
                request.BuId = userinfo.Id;
            }
            int source = SourceGroupAlgorithm.GetOldSource(request.Source);
            //submitinfo是否有值
            bx_submit_info submitInfo = _submitInfoRepository.GetSubmitInfo(request.BuId, source);
            if (submitInfo == null)
            {
                return BaseViewModel.GetBaseViewModel(-10000, "未取到核保信息，请稍后再试");
            }
            //是否需要上传图片
            if (request.Source == 2)
            {
                if (string.IsNullOrEmpty(submitInfo.documentGroupId))
                {
                    return BaseViewModel.GetBaseViewModel(-10000, "请确认是否需要上传类型范围内的证件资料");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(submitInfo.biz_tno) && string.IsNullOrEmpty(submitInfo.force_tno))
                {
                    return BaseViewModel.GetBaseViewModel(-10000, "请确认是否需要上传类型范围内的证件资料");
                }
            }
            //上传的图片最少1张
            var dic = request.ListBaseContect;//JsonConvert.DeserializeObject<Dictionary<int, string>>(request.ListBaseContect);
            if (dic.Count == 0)
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "输入参数错误，ListBaseContect为空";
                return viewModel;
            }
            //不能超过11张上传图片//以前是8张的限制
            else if (dic.Count > 11)
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "最多11张图片";
                return viewModel;
            }
            #endregion
            //校验上传次数
            var timesValidate = _checkUploadImgTimes.ValidateTimes(request);
            if (timesValidate.BusinessStatus == -10013)
            {
                viewModel.BusinessStatus = -10013;
                viewModel.StatusMessage = timesValidate.StatusMessage;
                return viewModel;
            }
            return BaseViewModel.GetBaseViewModel(1, "校验成功");
        }
    }
}
