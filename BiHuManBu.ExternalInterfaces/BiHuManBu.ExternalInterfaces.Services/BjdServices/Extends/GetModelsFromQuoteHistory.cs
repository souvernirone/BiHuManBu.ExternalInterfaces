using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Models.PartialModels;
using log4net;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public class GetModelsFromQuoteHistory : IGetModelsFromQuoteHistory
    {
        private readonly IQuoteHistoryRepository _quoteHistoryRepository;
        private ILog logErr = LogManager.GetLogger("ERROR");
        public GetModelsFromQuoteHistory(IQuoteHistoryRepository quoteHistoryRepository)
        {
            _quoteHistoryRepository = quoteHistoryRepository;
        }

        public Tuple<bx_savequote, bx_quotereq_carinfo, List<bx_quoteresult>, List<bx_submit_info>, string, List<int>> GetModels(long buid, long groupspan)
        {
            List<bx_quote_history> listhistory = _quoteHistoryRepository.GetByBuid(buid, groupspan);
            List<int> listquote = new List<int>();
            //未取到历史记录默认赋值为空
            if (!listhistory.Any())
            {
                return new Tuple<bx_savequote, bx_quotereq_carinfo, List<bx_quoteresult>, List<bx_submit_info>, string, List<int>>(null, null, null, null, "", listquote);
            }
            try
            {
                listquote = listhistory.Select(l => l.source.Value).ToList();//老的source值
                //取到历史记录开始循环
                bx_savequote savequote = new bx_savequote();
                bx_quotereq_carinfo quotereq = new bx_quotereq_carinfo();
                List<bx_quoteresult> listquoteresult = new List<bx_quoteresult>();
                bx_quoteresult quoteresult = new bx_quoteresult();
                List<bx_submit_info> listsubmitinfo = new List<bx_submit_info>();
                bx_submit_info submitinfo = new bx_submit_info();
                //险种信息
                string strsavequote = listhistory.FirstOrDefault().savequote;
                savequote = !string.IsNullOrEmpty(strsavequote)&& !strsavequote.Equals("null") ? strsavequote.FromJson<bx_savequote>() : null;
                //请求车辆信息
                string strquotereq = listhistory.FirstOrDefault().quotereq;
                quotereq = !string.IsNullOrEmpty(strquotereq)&& !strquotereq.Equals("null") ? strquotereq.FromJson<bx_quotereq_carinfo>() : null;
                //保费结果
                string strquoteresult = string.Empty;
                //报价结果
                string strsubmitinfo = string.Empty;
                foreach (var history in listhistory)
                {
                    //保费结果
                    strquoteresult = history.quoteresult;
                    if (!string.IsNullOrEmpty(strquoteresult) && !strquoteresult.Equals("null"))
                    {
                        quoteresult = new bx_quoteresult();
                        quoteresult = strquoteresult.FromJson<bx_quoteresult>();
                        if (quoteresult != null)
                        {
                            listquoteresult.Add(quoteresult);
                        }
                    }
                    //报价结果
                    strsubmitinfo = history.submitinfo;
                    if (!string.IsNullOrEmpty(strsubmitinfo)&&!strsubmitinfo.Equals("null"))
                    {
                        submitinfo = new bx_submit_info();
                        submitinfo = strsubmitinfo.FromJson<bx_submit_info>();
                        if (submitinfo != null)
                        {
                            listsubmitinfo.Add(submitinfo);
                        }
                    }
                }
                string createtime = listhistory.FirstOrDefault().updatetime.HasValue ? listhistory.FirstOrDefault().updatetime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
                return new Tuple<bx_savequote, bx_quotereq_carinfo, List<bx_quoteresult>, List<bx_submit_info>, string, List<int>>(savequote, quotereq, listquoteresult, listsubmitinfo, createtime, listquote);
            }
            catch (Exception ex)
            {
                logErr.Info(string.Format("报价历史解析出错，buid:{0};groupspan:{1};\n异常信息:{2}\n{3}", buid, groupspan, ex.StackTrace, ex.Message));
            }
            return new Tuple<bx_savequote, bx_quotereq_carinfo, List<bx_quoteresult>, List<bx_submit_info>, string, List<int>>(null, null, null, null, null, listquote);
        }
    }
}
