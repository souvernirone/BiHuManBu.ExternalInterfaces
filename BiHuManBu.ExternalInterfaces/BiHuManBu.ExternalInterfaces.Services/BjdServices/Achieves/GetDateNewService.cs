using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves
{
    public class GetDateNewService : IGetDateNewService
    {
        private ILog logErr = LogManager.GetLogger("ERROR");
        public GetDateNewService()
        {
        }
        public Tuple<string, string> GetDate(List<bx_quoteresult> listquoteresult)
        {
            string postBizStartDate = string.Empty;
            string postForceStartDate = string.Empty;
            if (listquoteresult.Any())
            {
                DateTime? postBize = listquoteresult.Max(l => l.BizStartDate);
                DateTime? postForce = listquoteresult.Max(l => l.ForceStartDate);
                postBizStartDate = postBize.HasValue ? postBize.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
                postForceStartDate = postForce.HasValue ? postForce.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
            }
            return new Tuple<string, string>(postBizStartDate, postForceStartDate);
        }
    }
}
