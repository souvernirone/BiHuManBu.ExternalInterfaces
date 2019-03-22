using System;
using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ValidateService.Achieves;
using System.Net;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.Mapper;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models.IRepository;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves
{
    public class SetAgentService : ISetAgentService
    {
        private IAgentRepository _agentRepository;
        private ILog logErr;
        public SetAgentService(IAgentRepository agentRepository)
        {
            _agentRepository = agentRepository;
            logErr = LogManager.GetLogger("ERROR");
        }
        public MyBaoJiaViewModel SetAgent(MyBaoJiaViewModel my, bx_userinfo userinfo, GetMyBjdDetailRequest request)
        {
            var curAgent = _agentRepository.GetAgent(int.Parse(userinfo.Agent));
            my.CurAgentName = curAgent != null ? curAgent.AgentName : string.Empty;
            my.CurAgentMobile = curAgent != null ? curAgent.Mobile : string.Empty;
            //初始化不显示费率 0为显示
            my.IsShowCalc = 1;
            if (request.ChildAgent != 0)
            {
                bx_agent bxAgent = _agentRepository.GetAgent(request.ChildAgent);
                my.IsShowCalc = bxAgent != null ? (bxAgent.IsShow.HasValue ? bxAgent.IsShow.Value : 1) : 1;
            }

            return my;
        }
    }
}
