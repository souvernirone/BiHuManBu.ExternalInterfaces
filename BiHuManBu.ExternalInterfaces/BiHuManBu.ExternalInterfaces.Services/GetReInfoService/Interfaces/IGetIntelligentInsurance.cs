using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System;
using System.Threading.Tasks;


namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces
{
    public interface IGetIntelligentInsurance
    {
        Task<Tuple<SaveQuoteViewModel, bool>> GetCenterInsurance(GetIntelligentReInfoRequest request);
    }
}
