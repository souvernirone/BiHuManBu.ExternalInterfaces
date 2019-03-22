
using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetByBuidRequest:BaseRequest
    {
        [Range(1,1000000000)]
        public long Buid { get; set; }
        [Range(1, 10000000)]
        public int ChildAgent { get; set; }
        //默认是平安，兼容老接口
        private long _source = 2;
        /// <summary>
        /// 新的source值
        /// </summary>
        public long Source { get { return _source; } set { _source = value; } }
    }
}
