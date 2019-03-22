using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class JiaYiModelMapper
    {
        public static List<JiaYiModel> ConvertViewModel(this List<bx_ywxdetail> items, int quotestatus, out double total)
        {
            total = 0;
            List<JiaYiModel> models = new List<JiaYiModel>();
            JiaYiModel model = new JiaYiModel();
            if (items != null && items.Any())
            {
                foreach (var item in items)
                {
                    model = new JiaYiModel()
                    {
                        Amount = item.amount ?? 0,
                        BrandCode = item.brandcode,
                        BrandName = item.brandname,
                        ClassId = item.classid,
                        ClassName = item.classname,
                        Code = item.code,
                        Name = item.name,
                        Count = item.count ?? 1,
                        Source = SourceGroupAlgorithm.GetNewSource(item.source.Value),
                    };
                    //只有报价成功，才计算结果
                    if (quotestatus > 0)
                    {
                        model.JyTotal = (item.amount ?? 0) * (item.count ?? 1);
                        total += (item.amount ?? 0) * (item.count ?? 1);
                    }
                    models.Add(model);
                }
            }
            return models;
        }
    }
}
