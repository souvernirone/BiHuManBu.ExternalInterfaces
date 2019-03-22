using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public  static class RepeatSubmitMapper
    {
        public static GetRepeatSubmitViewModel RepeatSubmitConvertToViewModel(this GetRepeatSubmitResponse response)
        {
            GetRepeatSubmitViewModel viewModel = new GetRepeatSubmitViewModel();
            viewModel.RepeatInfo = new GetRepeatSubmitInfo
            {
                BusinessExpireDate = response.BusinessExpireDate,
                ForceExpireDate = response.ForceExpireDate,
                RepeatSubmitMessage = response.RepeatSubmitMessage,
                RepeatSubmitResult = response.RepeatSubmitResult,
                CompositeRepeatType = response.CompositeRepeatType,
                RepeatSubmitPerComp = response.RepeatSubmitPerComp
            };

            return viewModel;

        }
    }
}
