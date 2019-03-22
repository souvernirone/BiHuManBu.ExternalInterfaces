using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class SpecialOptionViewModel
    { /// <summary>
        /// 编号
        /// </summary>
        public string Code { set; get; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { set; get; }

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsChecked { set; get; }

        /// <summary>
        /// 是否商业，0不区分,1商业，2交强
        /// </summary>
        public int IsCommerce { set; get; }
    }
}
