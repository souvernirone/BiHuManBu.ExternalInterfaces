using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using log4net;
using System.Collections.Generic;
using System.Linq;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public class SetPrecisePriceItemNew : ISetPrecisePriceItemNew
    {
        private IAgentConfigRepository _agentConfigRepository;
        private ILog logErr;
        public SetPrecisePriceItemNew(IAgentConfigRepository agentConfigService)
        {
            _agentConfigRepository = agentConfigService;
            logErr = LogManager.GetLogger("ERROR");
        }
        public List<long> FindSource(List<long>listquote01,int agent)
        {
            List<long> sourceLong = _agentConfigRepository.FindSource(agent);
            List<long> listsource = listquote01;
            
            if (listquote01.Any()) {
                foreach (var i in listquote01) {
                    sourceLong.Remove(i);
                }
            }
            if (sourceLong.Any()) {
                foreach (var i in sourceLong)
                {
                    listsource.Add(i);
                }
            }            
            return sourceLong;
        }
    }
}
