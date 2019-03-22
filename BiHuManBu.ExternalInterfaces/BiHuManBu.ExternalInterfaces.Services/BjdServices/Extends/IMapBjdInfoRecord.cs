using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.BjdServices.Extends
{
    public interface IMapBjdInfoRecord
    {
        BaojiaItemViewModel ConvertToViewModel(GetBjdItemResponse bjdItemResponse);
    }
}
