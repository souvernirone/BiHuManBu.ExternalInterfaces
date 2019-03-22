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
    public class SetClaimsService : ISetClaimsService
    {
        private IUserClaimRepository _userClaimRepository;
        private ILog logErr;
        public SetClaimsService(IUserClaimRepository userClaimRepository)
        {
            _userClaimRepository = userClaimRepository;
            logErr = LogManager.GetLogger("ERROR");
        }
        public MyBaoJiaViewModel SetClaims(MyBaoJiaViewModel my, GetMyBjdDetailRequest request, bx_userinfo userinfo)
        {
            List<bx_claim_detail> claimlist = _userClaimRepository.FindList(userinfo.Id);
            //此处不再给ClaimCount赋值，因为平安拿不到信息。总计从lastinfo取
            if (request.IsNewClaim == 1)
            {
                my.ClaimItem = claimlist.ConvertToNewDetailList();
                my.ClaimDetail = new List<ClaimDetailViewModel>();
            }
            else
            {
                my.ClaimItem = new List<UserClaimDetailViewModel>();
                my.ClaimDetail = claimlist.ConvertToNewList();
            }
            return my;
        }
    }
}
