using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using System;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves
{
    public class SetDateService : ISetDateService
    {
        private ILastInfoRepository _lastInfoRepository;
        private ILog logErr;
        public SetDateService(ILastInfoRepository lastInfoRepository)
        {
            _lastInfoRepository = lastInfoRepository;
            logErr = LogManager.GetLogger("ERROR");
        }
        public MyBaoJiaViewModel SetDate(MyBaoJiaViewModel my, bx_userinfo userinfo, string postBizStartDate, string postForceStartDate)
        {
            #region 格式化并赋值商业交强起保时间
            if (!string.IsNullOrWhiteSpace(postForceStartDate))
            {
                var st = DateTime.Parse(postForceStartDate);
                if (st.Date == DateTime.MinValue.Date)
                {
                    postForceStartDate = "";
                }
            }
            if (!string.IsNullOrWhiteSpace(postBizStartDate))
            {
                var st = DateTime.Parse(postBizStartDate);
                if (st.Date == DateTime.MinValue.Date)
                {
                    postBizStartDate = "";
                }
            }
            my.ForceStartDate = postForceStartDate;//crm前端不用，app、微信在用
            my.BizStartDate = postBizStartDate;//crm前端不用，app、微信在用
            my.PostStartDate = new PostStartDateTime()
            {//crm前端在用
                BusinessStartDate = postBizStartDate,
                ForceStartDate = postForceStartDate
            };
            #endregion
            #region 赋值商业交强结束时间

            var lastInfo = _lastInfoRepository.GetEndDateAndClaim(userinfo.Id);
            if (lastInfo != null)
            {
                my.LastBusinessEndDdate = string.IsNullOrEmpty(lastInfo.LastBusinessEndDdate) ? string.Empty : lastInfo.LastBusinessEndDdate;
                my.LastEndDate = string.IsNullOrEmpty(lastInfo.LastForceEndDdate) ? string.Empty : lastInfo.LastForceEndDdate;
                if (!string.IsNullOrWhiteSpace(my.LastBusinessEndDdate))
                {
                    var st = DateTime.Parse(my.LastBusinessEndDdate);
                    if (st.Date == DateTime.MinValue.Date)
                    {
                        my.LastBusinessEndDdate = string.Empty;
                    }
                }
                if (!string.IsNullOrWhiteSpace(my.LastEndDate))
                {
                    var st = DateTime.Parse(my.LastEndDate);
                    if (st.Date == DateTime.MinValue.Date)
                    {
                        my.LastEndDate = string.Empty;
                    }
                }
                //addbygpj20181106新增商业交强出险次数，从lastinfo取。
                my.ForceCliamCount = lastInfo.ForceClaimCount;
                my.BizClaimCount = lastInfo.BizClaimCount;
            }
            else
            {
                my.LastBusinessEndDdate = string.Empty;
                my.LastEndDate = string.Empty;
            }
            #endregion
            return my;
        }
    }
}
