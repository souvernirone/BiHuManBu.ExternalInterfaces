using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends;
using BiHuManBu.ExternalInterfaces.Services.BjdServices.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves
{
    public class CreateBjdInfoService : ICreateBjdInfoService
    {
        private IBaodanXianZhongRepository _baodanXianZhongRepository;
        private IBaodanxinxiRepository _baodanxinxiRepository;
        private IBxBjUnionRepository _bjxUnionRepository;
        private readonly IAgentRepository _agentRepository;
        private readonly IAddCrmStepsService _addCrmStepsService;

        private ILog logErr;
        private ILog logInfo;

        private ICreateActivity _createActivity;
        private IMapBaoDanXinXiRecord _mapBaoDanXinXiRecord;
        private IMapBaoDanXianZhongRecord _mapBaoDanXianZhongRecord;
        private readonly IUpdateBjdCheck _bjdCheck;

        private IYwxdetailRepository _ywxdetailRepository;

        public CreateBjdInfoService(ICreateActivity createActivity,
            IMapBaoDanXinXiRecord mapBaoDanXinXiRecord,IMapBaoDanXianZhongRecord mapBaoDanXianZhongRecord,IBxBjUnionRepository bxBjUnionRepository,
            IBaodanXianZhongRepository baodanXianZhongRepository, IBaodanxinxiRepository baodanxinxiRepository,IUpdateBjdCheck bjdCheck, IAgentRepository agentRepository, IAddCrmStepsService addCrmStepsService,
            IYwxdetailRepository ywxdetailRepository)
        {
            _createActivity = createActivity;
            _mapBaoDanXinXiRecord = mapBaoDanXinXiRecord;
            _mapBaoDanXianZhongRecord = mapBaoDanXianZhongRecord;
            _bjxUnionRepository = bxBjUnionRepository;
            _baodanXianZhongRepository = baodanXianZhongRepository;
            _baodanxinxiRepository = baodanxinxiRepository;
            _bjdCheck = bjdCheck;
            _agentRepository = agentRepository;
            _addCrmStepsService = addCrmStepsService;
            _ywxdetailRepository = ywxdetailRepository;
            logErr = LogManager.GetLogger("ERROR");
            logInfo = LogManager.GetLogger("INFO");

        }

        public long UpdateBjdInfo(CreateOrUpdateBjdInfoRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            //校验
            #region
            //bx_claim_detail
            long baodanxinxiid = 0;

            //20160905修改source1248=>0123，传入的新数据转换
            var checkResult = _bjdCheck.Valid(request);
            if (checkResult.State == 0)
            {
                logErr.Info(checkResult.Message);
                return checkResult.State; 
            }

            #endregion
            try
            {
                //新增
                if (request.BxId == 0)
                {
                    //单独写一个接口  实现
                    bx_preferential_activity model = _createActivity.AddActivity(request, 5);
                    //单独写一个接口  实现

                    var baodanxinxi = _mapBaoDanXinXiRecord.MapBaodanxinxi(request, checkResult.SubmitInfo, checkResult.Quoteresult,
                        checkResult.Savequote, checkResult.Userinfo,checkResult.ReqCarInfo, model);
                    var item = _baodanxinxiRepository.Add(baodanxinxi);

                    var agentinfo = _agentRepository.GetAgent(checkResult.Userinfo.Agent);

                    Task.Factory.StartNew(() =>
                    {
                        _addCrmStepsService.AddCrmSteps(request.ChildAgent, agentinfo == null ? "" : agentinfo.AgentName, "",
                            checkResult.Userinfo.LicenseNo, request.Source, request.BizRate, request.ForceRate,
                            model.id, request.Buid, item.Id, request.CityCode);
                    });

                    List<bx_ywxdetail> ywxList = _ywxdetailRepository.GetList(request.Buid);

                    var baodanxianzhong = _mapBaoDanXianZhongRecord.MapBaodanxianzhong(baodanxinxi, checkResult.Quoteresult, checkResult.Savequote,
                        checkResult.SubmitInfo,ywxList);
                    _baodanXianZhongRepository.Add(baodanxianzhong);

                    _bjxUnionRepository.Add(request.Buid, item.Id);
                    baodanxinxiid = item.Id;
                }
            }
            catch (Exception ex)
            {
                baodanxinxiid = -1;
                logErr.Info("创建报价单发生异常，请求串为：" + request.ToJson() + "/n错误信息：" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return baodanxinxiid;
        }

    }
}
