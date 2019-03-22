using BaoXian.Model.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class GetAccidentListViewModel:BaseViewModel
    {
        /// <summary>
        ///太保驾意险明细
        /// </summary>
        public List<TpyYwxProductInfoModel> TpyYwxProductInfo { get; set; }

        /// <summary>
        /// 平安驾意险
        /// </summary>
        public List<PingAnJyxInfoModel> PingAnJyxInfo { get; set; }
        public int CityCode { get; set; }
    }
}
