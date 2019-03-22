using System;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using log4net;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public class CreateActivity : ICreateActivity
    {
        private IPreferentialActivityRepository _preferentialActivityRepository;
        private IAgentRepository _agentRepository;
        private ILog logErr;

        public CreateActivity(IAgentRepository agentRepository,IPreferentialActivityRepository preferentialActivityRepository)
        {
            _agentRepository = agentRepository;
            _preferentialActivityRepository = preferentialActivityRepository;
            logErr = LogManager.GetLogger("ERROR");

        }

        public bx_preferential_activity AddActivity(CreateOrUpdateBjdInfoRequest request, int aciivityType)
        {
            var model = new bx_preferential_activity();
            try
            {
                if (!string.IsNullOrWhiteSpace(request.ActivityContent))
                {
                    bx_preferential_activity modelactivity = _preferentialActivityRepository.GetListByType(5,
                            request.ActivityContent);
                    if (modelactivity != null)
                    {
                        model = modelactivity;
                    }
                    else
                    {
                        var agentinfo = _agentRepository.GetAgent(request.ChildAgent);
                        model.top_agent_id = request.Agent;
                        model.agent_id = request.ChildAgent;
                        model.activity_type = 5;
                        model.activity_name = "";
                        model.activity_content = request.ActivityContent;
                        model.activity_status = 1;
                        model.create_time = DateTime.Now;
                        model.create_name = agentinfo == null ? "" : agentinfo.AgentName;
                        model.modify_time = DateTime.Now;
                        model.modify_name = agentinfo == null ? "" : agentinfo.AgentName;
                        _preferentialActivityRepository.AddActivity(model);
                       
                    }
                }
            }
            catch (Exception ex)
            {
                logErr.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
                return new bx_preferential_activity();
            }
            
            return model;
        }
    }
}
