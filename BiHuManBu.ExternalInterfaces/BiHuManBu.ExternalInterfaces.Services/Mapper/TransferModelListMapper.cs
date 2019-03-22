using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using System.Collections.Generic;
using System.Linq;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class TransferModelListMapper
    {
        public static List<TransferModelNew> ConvertToViewModel(this List<TransferModel> models)
        {
            List<TransferModelNew> newitems = new List<TransferModelNew>();
            if (models.Any())
            {
                TransferModelNew newitem;
                foreach (var item in models)
                {
                    newitem = new TransferModelNew();
                    newitem.CarVin = item.CarVin;
                    newitem.CompanyName = item.CompanyName;
                    newitem.EngineNo = item.EngineNo;
                    newitem.LicenseNo = item.LicenseNo;
                    newitem.RegisterDate = item.RegisterDate.ToString("yyyy-MM-dd");
                    newitem.Source = SourceGroupAlgorithm.GetNewSource(item.Source);
                    newitems.Add(newitem);
                }
            }

            return newitems;
        }
    }
}
