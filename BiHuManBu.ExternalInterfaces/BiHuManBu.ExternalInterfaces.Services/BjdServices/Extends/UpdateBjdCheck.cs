using System;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public class UpdateBjdCheck:IUpdateBjdCheck
    {
        private ISaveQuoteRepository _saveQuoteRepository;
        private ISubmitInfoRepository _submitInfoRepository;
        private IQuoteResultRepository _quoteResultRepository;
        private IUserInfoRepository _userInfoRepository;
        private IQuoteReqCarinfoRepository _quoteReqCarinfoRepository;

        public UpdateBjdCheck(ISaveQuoteRepository saveQuoteRepository, ISubmitInfoRepository submitInfoRepository,
            IQuoteResultRepository quoteResultRepository, IUserInfoRepository userInfoRepository,
            IQuoteReqCarinfoRepository quoteReqCarinfoRepository)
        {
            _saveQuoteRepository = saveQuoteRepository;
            _submitInfoRepository = submitInfoRepository;
            _quoteResultRepository = quoteResultRepository;
            _userInfoRepository = userInfoRepository;
            _quoteReqCarinfoRepository = quoteReqCarinfoRepository;
        }
        public UpdateBjdCheckMessage Valid(CreateOrUpdateBjdInfoRequest request)
        {
            UpdateBjdCheckMessage message = new UpdateBjdCheckMessage();

            message.State = 1;

            try
            {
                message.SubmitInfo = _submitInfoRepository.GetSubmitInfo(request.Buid, SourceGroupAlgorithm.GetOldSource(request.Source));
                if (message.SubmitInfo == null)
                {
                    message.Message = string.Format("Buid为{0}Source为{1}的记录submitinfo为空", request.Buid, request.Source);
                    message.State = 0;
                    return message;
                }

                message.Quoteresult = _quoteResultRepository.GetQuoteResultByBuid(request.Buid, SourceGroupAlgorithm.GetOldSource(request.Source));
                if (message.Quoteresult == null)
                {
                    message.Message = string.Format("Buid为{0}Source为{1}的记录quoteinfo为空", request.Buid, request.Source);
                    message.State = 0;
                    return message;
                }

                message.ReqCarInfo = _quoteReqCarinfoRepository.Find(request.Buid);
                if (message.ReqCarInfo == null)
                {
                    message.Message = string.Format("Buid为{0}Source为{1}的记录quotereqcarinfo为空", request.Buid, request.Source);
                    message.State = 0;
                    return message;
                }

                message.Savequote = _saveQuoteRepository.GetSavequoteByBuid(request.Buid);
                if (message.Savequote == null)
                {
                    message.Message = string.Format("Buid为{0}Source为{1}的记录saveinfo为空", request.Buid, request.Source);
                    message.State = 0;
                    return message;
                }

                message.Userinfo = _userInfoRepository.FindByBuid(request.Buid);
                if (message.Userinfo == null)
                {
                    message.Message = string.Format("Buid为{0}Source为{1}的记录userinfo为空", request.Buid, request.Source);
                    message.State = 0;
                    return message;
                }
            }
            catch (Exception ex)
            {
                message.Message = string.Format("程序出险异常，错误信息为：{0}", ex.Message);
                message.State = -1;
                return message;
            }
               

            return message;
        }
    }
}
