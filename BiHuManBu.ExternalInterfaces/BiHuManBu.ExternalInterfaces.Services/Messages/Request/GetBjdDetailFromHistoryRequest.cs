using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class GetBjdDetailFromHistoryRequest : BaseRequest
    {
        [Range(1, 2100000000)]
        public long Buid { get; set; }
                
        /// <summary>
        /// 子经纪人Id，App验证+PC端crm调用
        /// </summary>
        public int ChildAgent { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public long GroupSpan { get; set; }
    }
}
