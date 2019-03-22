using System;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Services.UploadImgService.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.UploadImgService.Implementations
{
    public class UpdateImgTimes:IUpdateImgTimes
    {
        public void UpdateTimes(long buid, int times)
        {
            var date = DateTime.Now;
            var dateStr = date.ToShortDateString() + " 23:59:59";
            var maxTody = DateTime.Parse(dateStr);
            var seconds = (maxTody - date).TotalSeconds;

            var redisKey = "UpImgTimes_" + buid;
            //这里应该是先看看redis中有没有，有的话使用times更新数据，没有的话直接存入1
            var value = CacheProvider.Get<int?>(redisKey);
            if (!value.HasValue)
            {
                CacheProvider.Set(redisKey, 1, (long)seconds);
            }
            else
            {
                CacheProvider.Set(redisKey, times, (long)seconds);
            }
        }
    }
}
