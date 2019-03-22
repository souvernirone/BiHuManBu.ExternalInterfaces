using System;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class ViolationMapper
    {
        public static ViolationViewModel ConverToViewModel(this bx_violationlog detail)
        {
            var vm = new ViolationViewModel()
            {
                AcceptagencyCode = detail.acceptagencycode??string.Empty,
                Coeff = detail.coeff??0,
                LossAction = detail.lossAction??string.Empty,
                LossActionDesc=detail.lossActionDesc??string.Empty,
                LossTime = detail.lossTime ?? string.Empty,
                PeccancyId = detail.peccancyid??string.Empty,
                ProcessingStatus = detail.processingStatus??string.Empty,
                SanctionDate = detail.sanctionDate??string.Empty,
                ViolationRecordTypeName = detail.violationRecordTypeName??string.Empty
            };

            return vm;
        }

        public static List<ViolationViewModel> ConverToViewModelList(this List<bx_violationlog> details)
        {
            return details.Select(detail => detail.ConverToViewModel()).ToList();
        }
    }
}
