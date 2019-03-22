using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class GetSpecialAssumpsitResponse:BaseResponse
    {
        public List<SpecialVO> SpecialContents { get; set; }
    }
}
