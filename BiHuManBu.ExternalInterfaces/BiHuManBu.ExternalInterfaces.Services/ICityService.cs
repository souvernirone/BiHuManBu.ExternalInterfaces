using System.Collections.Generic;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public interface ICityService
    {
        Task<GetCityListResponse> GetCityList(GetCityListRequest request, IEnumerable<KeyValuePair<string,string>> pairs);

        List<bx_city> FindList();

        Task<GetContinedPeriodResponse> GetContinuedPeriod(GetCityContinuedPeriodRequest request,IEnumerable<KeyValuePair<string,string>> pairs);
    }
}
