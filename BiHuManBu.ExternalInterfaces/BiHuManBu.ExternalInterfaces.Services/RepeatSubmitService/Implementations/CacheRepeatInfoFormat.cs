using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Utility;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.Implementations
{
    public class CacheRepeatInfoFormat : IRepeatInfoFormat
    {
        private readonly ICacheService _cacheService;

        public CacheRepeatInfoFormat(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        public RepeatInfoFormatModel FormatRepeatInfo(int quoteGroup,string identity)
        {
            RepeatInfoFormatModel model = new RepeatInfoFormatModel()
            {
                RepeatPerCompany = new Dictionary<int, int>()
            };
            var quoteCompany = UtiService.GetCompnayTransSource();
            int repeattype = 0;
            string bizmsg = "", forcemsg = "", doubmsg = "";
          

            //整理每一家的到期日期
            foreach (var quote in quoteCompany)
            {
                if ((quoteGroup & quote.Key) == quote.Key)
                {
                    var result = _cacheService.Get<bx_submit_info>(string.Format("{0}-{1}-{2}", identity, quote.Value, "submitinfo"));

                    repeattype = result.is_repeat_submit.Value | repeattype;

                    if (result.is_repeat_submit.Value == 1)
                    {
                        if(string.IsNullOrEmpty(forcemsg))forcemsg = result.quote_result;
                        model.RepeatPerCompany.Add(quote.Key, 1);
                    }
                    if (result.is_repeat_submit.Value == 2)
                    {
                        if (string.IsNullOrEmpty(bizmsg))bizmsg = result.quote_result;
                        model.RepeatPerCompany.Add(quote.Key,2);
                    }
                   
                    if (string.IsNullOrEmpty(doubmsg) && result.is_repeat_submit.Value == 3)
                    {
                        doubmsg = result.quote_result;
                        model.RepeatPerCompany.Add(quote.Key, 3);
                        //break;
                    }
                }
            }

            if (model.RepeatPerCompany.Any(x => x.Value == 3))
            {
                model.CompositeRepeatType = 0;
            }
            else
            {
                if (model.RepeatPerCompany.Any(x => x.Value == 1) && model.RepeatPerCompany.Any(x => x.Value == 2))
                {
                    model.CompositeRepeatType = 1; //组合而成
                }
                else
                {
                    model.CompositeRepeatType = 0;
                }
            }

            model.RepeatType = repeattype;
            if (repeattype == 3)
            {
                model.RepeatMsg = doubmsg;
            }
            else
            {
                model.RepeatMsg = string.Format("{0}{1} ", bizmsg, forcemsg);
            }

            return model;
        }
    }
}