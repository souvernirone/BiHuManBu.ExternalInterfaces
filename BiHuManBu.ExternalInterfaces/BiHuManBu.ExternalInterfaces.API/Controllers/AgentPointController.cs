using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using bx_agentpoint = BiHuManBu.ExternalInterfaces.Models.bx_agentpoint;


namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class AgentPointController : ApiController
    {
        //
        // GET: /AgentPoint/

        public IAgentPointService _AgentPointService;

        public AgentPointController(IAgentPointService agentPointService)
        {
            _AgentPointService = agentPointService;
        }

        public HttpResponseMessage GetAgentpoints(int agentId)
        {
            AgentPointsViewModel viewModel = new AgentPointsViewModel();
            try
            {
                viewModel.agentpoints = _AgentPointService.GetAgentpoints(agentId);
                if (viewModel.agentpoints.Count > 0)
                {
                    viewModel.BusinessStatus = 1;
                }
                else
                {
                    viewModel.BusinessStatus = -10000;
                    viewModel.StatusMessage = "没有自提点数据";
                }
            }
            catch (Exception)
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "查询失败";
            }
            return viewModel.ResponseToJson();
        }

        public HttpResponseMessage GetAgentpoint(int agentpointId)
        {
            AgentPointViewModel viewModel = new AgentPointViewModel();
            try
            {
                viewModel.agentpoint = _AgentPointService.GetAgentpoint(agentpointId);

                if (viewModel.agentpoint != null)
                {
                    viewModel.BusinessStatus = 1;
                }
                else
                {
                    viewModel.BusinessStatus = -10000;
                    viewModel.StatusMessage = "没有自提点数据";
                }
            }
            catch (Exception)
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "查询失败";
            }
            return viewModel.ResponseToJson();
        }


        public HttpResponseMessage GetWorkPoint(int agentId)
        {
            AgentPointViewModel viewModel = new AgentPointViewModel();
            try
            {
                viewModel.agentpoint = _AgentPointService.GetWorkAgentpoint(agentId);

                if (viewModel.agentpoint != null)
                {
                    viewModel.BusinessStatus = 1;
                }
                else
                {
                    viewModel.BusinessStatus = -10000;
                    viewModel.StatusMessage = "没有自提点数据";
                }
            }
            catch (Exception)
            {
                viewModel.BusinessStatus = -10002;
                viewModel.StatusMessage = "查询失败";
            }
            return viewModel.ResponseToJson();
        }

    }
}
