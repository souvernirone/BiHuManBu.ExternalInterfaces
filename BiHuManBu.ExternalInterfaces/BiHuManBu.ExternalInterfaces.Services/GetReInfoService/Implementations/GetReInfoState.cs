using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Implementations
{
    public class GetReInfoState: IGetReInfoState
    {
        private IUserInfoRepository _userInfoRepository;
        public GetReInfoState(IUserInfoRepository userInfoRepository) {
            _userInfoRepository = userInfoRepository;
        }

        public bool GetState(long buid)
        {
            var userinfo = _userInfoRepository.FindByBuid(buid);
            bool isContinue = true;
            if (userinfo.NeedEngineNo == 1)
            {//需要完善行驶证信息
                isContinue = false;
            }
            if (userinfo.NeedEngineNo == 0 && userinfo.RenewalStatus != 1)
            {  //获取车辆信息成功，但获取险种失败
                isContinue = false;
            }
            if ((userinfo.NeedEngineNo == 0 && userinfo.LastYearSource > -1) || userinfo.RenewalStatus == 1)
            {  //续保成功
                isContinue = false;
            }
            return isContinue;
        }
    }
}
