using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public class SetPrecisePriceItem : ISetPrecisePriceItem
    {
        private IAgentConfigRepository _agentConfigRepository;
        private ILog logErr;
        public SetPrecisePriceItem(IAgentConfigRepository agentConfigService)
        {
            _agentConfigRepository = agentConfigService;
            logErr = LogManager.GetLogger("ERROR");
        }
        public List<long> FindSource(bx_userinfo userInfo, GetMyBjdDetailRequest request)
        {
            List<long> sourceInt = _agentConfigRepository.FindSource(request.Agent);
            var sourceList = new List<long>();
            if (request.Source != -1)
            {
                sourceList.Add(request.Source);
            }
            else
            {
                if (userInfo.IsSingleSubmit.HasValue)
                {
                    // 获取到记录用到的渠道列表
                    sourceList = SourceGroupAlgorithm.GetSource(sourceInt, userInfo.IsSingleSubmit.Value);
                }
            }
            return sourceList;
        }
    }
}
