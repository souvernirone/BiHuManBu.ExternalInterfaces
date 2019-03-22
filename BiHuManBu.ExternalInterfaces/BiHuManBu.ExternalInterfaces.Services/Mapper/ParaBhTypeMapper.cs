using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class ParaBhTypeMapper
    {
        public static List<ParaBhType> ConvertToViewModel(this List<bx_para_bhtype> list)
        {
            var newList = new List<ParaBhType>();
            ParaBhType paraBhType;
            foreach (var item in list)
            {
                paraBhType = new ParaBhType();
                paraBhType.Id = item.id;
                paraBhType.BhId = item.bh_id.HasValue ? item.bh_id.Value : 0;
                paraBhType.BhName = item.bh_name;
                paraBhType.BhType = item.bh_type.HasValue ? item.bh_type.Value : 0;
                paraBhType.IsSupport = item.is_support.HasValue ? item.is_support.Value : 0;
                newList.Add(paraBhType);
            }
            return newList;
        }
    }
}
