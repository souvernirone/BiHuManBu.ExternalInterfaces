using BiHuManBu.ExternalInterfaces.Services.GetCenterValueService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System.Configuration;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Implementations
{
    public class ReWriteUserInfo : IReWriteUserInfo
    {
        private readonly IGetCenterValueService _getCenterValue;
        public ReWriteUserInfo(IGetCenterValueService getCenterValue)
        {
            _getCenterValue = getCenterValue;
        }

        public UserInfoViewModel ReWriteUserInfoService(UserInfoViewModel userinfo, int topAgentId)
        {
            string getvalue = _getCenterValue.GetValue("S_S_RenewalTask_version", "", "pa_unfilter_agents");
            UserInfoViewModel viewmodel = userinfo;
            getvalue = GetString(getvalue);
            if (!string.IsNullOrEmpty(getvalue) && getvalue.Contains("," + topAgentId + ","))
            {
                viewmodel.LicenseOwner = GetRelationInfo(viewmodel.LicenseOwner);//车主
                viewmodel.InsuredName = GetRelationInfo(viewmodel.InsuredName);//被保
                viewmodel.PostedName = GetRelationInfo(viewmodel.PostedName);//投保
                viewmodel.HolderName = GetRelationInfo(viewmodel.HolderName);//投保

                viewmodel.CredentislasNum = GetRelationInfo(viewmodel.CredentislasNum);//车主
                viewmodel.InsuredIdCard = GetRelationInfo(viewmodel.InsuredIdCard);//被保
                viewmodel.HolderIdCard = GetRelationInfo(viewmodel.HolderIdCard);//投保

                viewmodel.IdType = string.IsNullOrEmpty(viewmodel.CredentislasNum) ? 0 : viewmodel.IdType;
                viewmodel.InsuredIdType= string.IsNullOrEmpty(viewmodel.InsuredIdCard) ? 0 : viewmodel.InsuredIdType;
                viewmodel.HolderIdType = string.IsNullOrEmpty(viewmodel.HolderIdCard) ? 0 : viewmodel.HolderIdType;

                viewmodel.InsuredMobile = GetRelationInfo(viewmodel.InsuredMobile);//被保
                viewmodel.HolderMobile = GetRelationInfo(viewmodel.HolderMobile);//投保
            }
            //新华特殊配置,如果车架号发动机号带星，则返回空
            string xinhua=ConfigurationManager.AppSettings["SpecialXinHua"];
            if (!string.IsNullOrEmpty(xinhua)) {
                if (xinhua.Equals(topAgentId.ToString())) {
                    viewmodel.CarVin = GetRelationInfo(viewmodel.CarVin);
                    viewmodel.EngineNo = GetRelationInfo(viewmodel.EngineNo);
                }
            }
            return viewmodel;
        }

        /// <summary>
        /// 信息包含星号，就返回空
        /// </summary>
        /// <param name="strInfo"></param>
        /// <returns></returns>
        private string GetRelationInfo(string strInfo)
        {
            if (!string.IsNullOrEmpty(strInfo) && strInfo.Contains("*"))
            {
                return "";
            }
            return strInfo;
        }

        /// <summary>
        /// 截前后字符
        /// </summary>
        /// <param name="val">原字符串</param>
        /// <param name="c">要截取的字符</param>
        /// <returns></returns>
        private string GetString(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return "";
            }
            return val.Replace('[', ',').Replace(']', ',');
        }
    }
}
