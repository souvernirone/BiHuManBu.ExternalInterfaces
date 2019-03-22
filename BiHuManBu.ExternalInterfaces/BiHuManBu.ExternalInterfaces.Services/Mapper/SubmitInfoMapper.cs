using System.Text;
using System.Text.RegularExpressions;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class SubmitInfoMapper
    {
        /// <summary>
        /// 返回人太平系统的 核保信息描述
        /// </summary>
        /// <param name="submitInfo"></param>
        /// <returns></returns>
        public static SubmitInfoViewModel ConverToViewModelForDaiLi(this bx_submit_info submitInfo)
        {
            SubmitInfoViewModel vm = new SubmitInfoViewModel();
            if (submitInfo.source != null)
                vm.Item = new SubmitInfoDetail
                {
                    BizNo = string.IsNullOrWhiteSpace(submitInfo.biz_tno) ? string.Empty : submitInfo.biz_tno,
                    ForceNo = string.IsNullOrWhiteSpace(submitInfo.force_tno) ? string.Empty : submitInfo.force_tno,
                    SubmitStatus = submitInfo.submit_status.HasValue?submitInfo.submit_status.Value:0,
                    SubmitResult = string.IsNullOrEmpty(submitInfo.submit_result) ? string.Empty : submitInfo.submit_result,
                    Source = submitInfo.source.Value,
                    BizRate = (double) (submitInfo.biz_rate.HasValue?submitInfo.biz_rate.Value:0),
                    ForceRate = (double)(submitInfo.force_rate.HasValue?submitInfo.force_rate.Value:0),
                    ChannelId = submitInfo.channel_id.HasValue ? submitInfo.channel_id.Value.ToString() : "-1",
                    JingSuanKouJing = string.IsNullOrWhiteSpace(submitInfo.RbJSKJ) ? string.Empty : submitInfo.RbJSKJ,
                };
            
            return vm;
        }
        /// <summary>
        /// 返回核保信息的 定制化信息
        /// </summary>
        /// <param name="submitInfo"></param>
        /// <returns></returns>
        public static SubmitInfoViewModel ConverToViewModelForCustom(this bx_submit_info submitInfo)
        {
            SubmitInfoViewModel vm = new SubmitInfoViewModel();
            if (submitInfo.source != null)
                vm.Item = new SubmitInfoDetail
                {
                    BizNo = string.IsNullOrWhiteSpace(submitInfo.biz_tno) ? string.Empty : submitInfo.biz_tno,
                    ForceNo = string.IsNullOrWhiteSpace(submitInfo.force_tno) ? string.Empty : submitInfo.force_tno,
                    SubmitStatus = submitInfo.submit_status.HasValue?submitInfo.submit_status.Value:0,
                    SubmitResult = string.IsNullOrWhiteSpace(submitInfo.quote_result_toc) ? string.Empty : submitInfo.quote_result_toc,
                    Source = submitInfo.source.Value,
                    BizRate = (double)(submitInfo.biz_rate.HasValue ? submitInfo.biz_rate.Value : 0),
                    ForceRate = (double)(submitInfo.force_rate.HasValue ? submitInfo.force_rate.Value : 0),
                    ChannelId = submitInfo.channel_id.HasValue ? submitInfo.channel_id.Value.ToString():"-1",
                };
            return vm;
        }

        private static string FormatSubmitInfo(string submitResult)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(submitResult))
            {
                MatchCollection matches = Regex.Matches(submitResult, @"[\u4e00-\u9fa5!，、]+([[\d-]+])?",
                    RegexOptions.ExplicitCapture);

                foreach (Match m in matches)
                {
                    sb.Append(m.Value);
                }
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}