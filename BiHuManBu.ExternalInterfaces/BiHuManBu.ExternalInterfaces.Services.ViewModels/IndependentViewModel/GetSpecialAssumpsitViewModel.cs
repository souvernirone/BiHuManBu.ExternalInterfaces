using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels.IndependentViewModel
{
    public class GetSpecialAssumpsitViewModel:BaseViewModel
    {
        public List<SpecialVO> SpecialContents { get; set; }
    }

    /// <summary>
    /// 特别约定信息
    /// </summary>
    public class SpecialVO
    {
        /// <summary>
        /// 特别约定代码 
        /// </summary>
        public string CSpecNo { get; set; }

        /// <summary>
        /// 特别约定内容 
        /// </summary>
        public string CSysSpecContent { get; set; }

        /// <summary>
        /// 1交强 2商业
        /// </summary>
        public int Type { get; set; }
    }
}
