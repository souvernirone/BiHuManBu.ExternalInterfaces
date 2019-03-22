
using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class CarOrderMapper
    {
        public static List<CarOrderModel> ConvertToViewModel(this List<CarOrderModel> list)
        {
            var newlist = new List<CarOrderModel>();
            CarOrderModel model;
            foreach (var item in list)
            {
                model = new CarOrderModel();
                model = item;
                if (item.source.HasValue)
                    model.source = SourceGroupAlgorithm.GetNewSource((int)item.source.Value);
                newlist.Add(model);
            }
            return newlist;
        }

        public static CarOrderModel ConvertToViewModel(this CarOrderModel item)
        {
            if (item.source.HasValue)
                item.source = SourceGroupAlgorithm.GetNewSource((int)item.source.Value);
            return item;
        }
    }
}
