using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class ContiunedPeriodMapper
    {
        public static ContinedPeriod ConvertToViewModel(this bx_cityquoteday item)
        {
            ContinedPeriod model = new ContinedPeriod();
            model.BizDays = item.bizquotedays ?? 0;
            model.ForceDays = item.quotedays ?? 0;
            model.CityCode = item.cityid ?? 0;
            return model;
        }
        public static ContinedPeriodNew ConvertToViewModelNew(this bx_cityquoteday item)
        {
            ContinedPeriodNew model = new ContinedPeriodNew();
            model.BizDays = item.bizquotedays ?? 0;
            model.ForceDays = item.quotedays ?? 0;
            model.CityCode = item.cityid ?? 0;
            model.CityName = item.cityname ?? "";
            return model;
        }
        public static ContinuedPeriodViewModel ConverToList(this List<bx_cityquoteday> items)
        {
            ContinuedPeriodViewModel lists = new ContinuedPeriodViewModel();
            lists.Items = new List<ContinedPeriod>();
            foreach (bx_cityquoteday item in items)
            {
                lists.Items.Add(item.ConvertToViewModel());
            }

            return lists;
        }
        public static ContinuedPeriodViewModel ConverToListAddCityName(this List<bx_cityquoteday> items)
        {
            ContinuedPeriodViewModel lists = new ContinuedPeriodViewModel();
            lists.ItemsAddCityName = new List<ContinedPeriodNew>();
            foreach (bx_cityquoteday item in items)
            {
                lists.ItemsAddCityName.Add(item.ConvertToViewModelNew());
            }

            return lists;
        }
    }
}
