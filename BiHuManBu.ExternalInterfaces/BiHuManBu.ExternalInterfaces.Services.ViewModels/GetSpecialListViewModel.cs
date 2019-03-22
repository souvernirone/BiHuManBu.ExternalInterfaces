using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class GetSpecialListViewModel
    {
        public List<SpecialOptionViewModel> SpecialList { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public int BusinessStatus { get; set; }
        /// <summary>
        /// 自定义状态描述
        /// </summary>
        public string StatusMessage { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 缓存Key
        /// </summary>
        public string Key { get; set; }
    }
}
