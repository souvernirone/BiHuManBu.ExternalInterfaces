using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure.Utility;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Repository.DbOper;
using BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.RepeatSubmitService.Implementations
{
    public class DbRepeatInfoFormat : IRepeatInfoFormat
    {
        private IRepository<bx_submit_info> _repository;

        public DbRepeatInfoFormat(IRepository<bx_submit_info> repository)
        {
            _repository = repository;
        }
        public RepeatInfoFormatModel FormatRepeatInfo(int quoteGroup, string identity)
        {
            RepeatInfoFormatModel model = new RepeatInfoFormatModel()
            {
                RepeatPerCompany = new Dictionary<int, int>()
            };
            var quoteCompany = UtiService.GetCompnayTransSource();
            int repeattype = 0;
            string bizmsg = "", forcemsg = "", doubmsg = "";
            var buid = Convert.ToInt64(identity);
            var dbItems = _repository.Search(x => x.b_uid ==buid).ToList();

            //整理每一家的到期日期
            foreach (var quote in quoteCompany)
            {
                if ((quoteGroup & quote.Key) == quote.Key)
                {
                    var result = dbItems.FirstOrDefault(x => x.source == quote.Value);

                    repeattype = result.is_repeat_submit.Value | repeattype;

                    if (result.is_repeat_submit.Value == 1)
                    {
                        if (string.IsNullOrEmpty(forcemsg)) forcemsg = result.quote_result;
                        model.RepeatPerCompany.Add(quote.Key, 1);
                    }
                    if (result.is_repeat_submit.Value == 2)
                    {
                        if (string.IsNullOrEmpty(bizmsg)) bizmsg = result.quote_result;
                        model.RepeatPerCompany.Add(quote.Key, 2);
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