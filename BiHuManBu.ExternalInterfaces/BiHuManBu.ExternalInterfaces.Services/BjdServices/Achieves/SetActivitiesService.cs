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
    public class SetActivitiesService : ISetActivitiesService
    {
        private IPreferentialActivityRepository _preferentialActivityRepository;
        private ILog logErr;
        public SetActivitiesService(IPreferentialActivityRepository preferentialActivityRepository)
        {
            _preferentialActivityRepository = preferentialActivityRepository;
            logErr = LogManager.GetLogger("ERROR");
        }
        public MyBaoJiaViewModel SetActivities(MyBaoJiaViewModel my, GetMyBjdDetailRequest request)
        {
            my.Activity = new List<PreActivity>();
            if (!string.IsNullOrWhiteSpace(request.Activity))
            {
                var preActivity = _preferentialActivityRepository.GetActivityByIds(request.Activity.Trim());
                var activity = preActivity.Select(
                    model => new PreActivity
                    {
                        ActivityName = model.activity_name,
                        ActivityContent = model.activity_content
                    }).ToList();
                my.Activity = activity;
            }
            return my;
        }
    }
}
