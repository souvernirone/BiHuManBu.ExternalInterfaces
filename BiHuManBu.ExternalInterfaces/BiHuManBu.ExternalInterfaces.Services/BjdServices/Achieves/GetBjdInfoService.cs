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

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Achieves
{
    public class GetBjdInfoService : IGetBjdInfoService
    {
        private readonly IPreferentialActivityRepository _preferentialActivityRepository;
        private readonly IBxBjUnionRepository _bxBjUnionRepository;
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly ISaveQuoteRepository _saveQuoteRepository;
        private readonly IMapBjdInfoRecord _mapBjdInfoRecord;
        private readonly IGetBjdCheck _getBjdCheck;
        private ILog logErr;
        public GetBjdInfoService(IBaodanxinxiRepository baodanxinxiRepository, IBaodanXianZhongRepository baodanXianZhongRepository, IPreferentialActivityRepository preferentialActivityRepository, IBxBjUnionRepository bxBjUnionRepository, IUserClaimRepository userClaimRepository, ISaveQuoteRepository saveQuoteRepository, IMapBjdInfoRecord mapBjdInfoRecord, IGetBjdCheck getBjdCheck)
        {
            _preferentialActivityRepository = preferentialActivityRepository;
            _bxBjUnionRepository = bxBjUnionRepository;
            _userClaimRepository = userClaimRepository;
            _saveQuoteRepository = saveQuoteRepository;
            _mapBjdInfoRecord = mapBjdInfoRecord;
            _getBjdCheck = getBjdCheck;
            logErr = LogManager.GetLogger("ERROR");
        }

        public BaojiaItemViewModel GetBjdInfo(GetBjdItemRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            var response = new BaojiaItemViewModel() {BusinessStatus = 0};
            
            try
            {
                var bjdCheck = _getBjdCheck.BjdCheckMessage(request.Bxid);
                if (bjdCheck.State == 0)
                {
                    response.BusinessStatus = 0;
                    return response;
                }

                var bjdItemResponse = new GetBjdItemResponse();
                bjdItemResponse.Baodanxinxi = bjdCheck.Baodanxinxi;
                bjdItemResponse.Baodanxianzhong = bjdCheck.Baodanxianzhong;

                if (bjdItemResponse.Baodanxinxi != null && !string.IsNullOrEmpty(bjdItemResponse.Baodanxinxi.activity_ids))
                {
                    bjdItemResponse.Activitys = _preferentialActivityRepository.GetActivityByIdsAsync(bjdItemResponse.Baodanxinxi.activity_ids).Result;
                }

                if (bjdCheck.BjUnion != null)
                {
                    bjdItemResponse.ClaimDetail = _userClaimRepository.FindListAsync(bjdCheck.BjUnion.b_uid).Result;
                    bjdItemResponse.Savequote = _saveQuoteRepository.GetSavequoteByBuidAsync(bjdCheck.BjUnion.b_uid).Result;
                }
                response.BusinessStatus = 1;
                
                response = _mapBjdInfoRecord.ConvertToViewModel(bjdItemResponse);
            }
            catch (Exception ex)
            {
                response.BusinessStatus = -1;
                logErr.Info("获取分享报价单单发生异常，请求串为：" + request.ToJson() + "/n错误信息：" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return response;
        }
    }
}
