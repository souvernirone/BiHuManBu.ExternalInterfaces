using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class ViolationViewModel
    {
        public string LossTime { get; set; }
        public string SanctionDate { get; set; }
        public string LossActionDesc { get; set; }
        public decimal Coeff { get; set; }
        public string PeccancyId { get; set; }
        public string ProcessingStatus { get; set; }
        public string ViolationRecordTypeName { get; set; }
        public string AcceptagencyCode { get; set; }
        public string LossAction { get; set; }

    }
}
