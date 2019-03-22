using System;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces
{
    public interface IGetMonth
    {
        int GetMonthValue(DateTime dtbegin, DateTime dtend);
    }
}
