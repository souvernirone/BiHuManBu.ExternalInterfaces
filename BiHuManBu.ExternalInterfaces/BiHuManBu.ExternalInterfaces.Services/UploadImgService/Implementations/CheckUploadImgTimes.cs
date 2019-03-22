using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.UploadImgService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.UploadImgService.Implementations
{
    public class CheckUploadImgTimes : ICheckUploadImgTimes
    {
        private static readonly int UpImgCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["UpImgCount"]);

        public BaseViewModel ValidateTimes(UploadMultipleImgRequest request)
        {
            BaseViewModel viewModel = new BaseViewModel();
            viewModel.BusinessStatus = 1;
            //判断次数
            if (UpImgCount != 0)
            {//如果=0，没有上传限制
                int alreadyTimes = 0;//已经上传的次数
                if (!CheckTimes(request.BuId, out alreadyTimes))
                {
                    return BaseViewModel.GetBaseViewModel(-10013, "上传次数过多，请直接去保险系统修改！");
                }
            }
            return viewModel;
        }

        /// <summary>
        /// 检查上传次数
        /// 可以继续上传返回true，否则返回false
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        private bool CheckTimes(long buid, out int alreadyTimes)
        {
            alreadyTimes = 0;
            var redisKey = "UpImgTimes_" + buid;
            var times = CacheProvider.Get<int?>(redisKey);
            if (!times.HasValue)
            {
                return true;
            }
            alreadyTimes = times.Value;
            //如果配置是0，不做限制
            //if (_upImgCount == 0)
            //{
            //    return true;
            //}
            if (alreadyTimes >= UpImgCount)
            {
                return false;
            }
            return true;
        }
    }
}
