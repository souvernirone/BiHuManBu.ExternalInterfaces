using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Models.IRepository
{
    public interface ICityQuoteDayRepository
    {
        int GetDaysNum(int cityId);
        List<bx_cityquoteday> GetList(int agentId);
        bx_cityquoteday Get(int cityCode);
    }
}
