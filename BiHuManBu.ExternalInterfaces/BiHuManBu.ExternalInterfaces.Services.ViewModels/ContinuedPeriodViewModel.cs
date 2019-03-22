using System;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class ContinuedPeriodViewModel:BaseViewModel
    {
        public List<ContinedPeriod> Items { get; set; }
        public List<ContinedPeriodNew> ItemsAddCityName { get; set; }
    }

    public class ContinedPeriod
    {
        public int CityCode { get; set; }
        public int ForceDays { get; set; }
        public int BizDays { get; set; }
    }
    public class ContinedPeriodNew
    {
        public int CityCode { get; set; }
        public string CityName { get; set; }
        public int ForceDays { get; set; }
        public int BizDays { get; set; }
    }
}
