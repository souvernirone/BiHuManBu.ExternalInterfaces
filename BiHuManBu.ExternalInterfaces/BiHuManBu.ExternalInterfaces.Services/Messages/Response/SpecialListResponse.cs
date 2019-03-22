using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class SpecialListResponse : BaseResponse
    {
        public List<SpecialOptionViewModel> SpecialOptions { get; set; }
        public int BusinessStatus { get; set; }
        public string BusinessMessage { get; set; }
        /// <summary>
        /// 缓存Key
        /// </summary>
        public string Key { get; set; }
    }
}
