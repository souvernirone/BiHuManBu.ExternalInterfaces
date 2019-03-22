using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class UKeyListResponse:BaseResponse
    {
        public List<CityUKeyModel> CityUKeyList { get; set; }
    }

    /// <summary>
    /// 中心返回修改保司密码 值
    /// </summary>
    public class UKeyEdit
    {
        //public bool isSucceed { get; set; }
        /// <summary>
        /// 错误码 0成功，其他为失败
        /// </summary>
        public int ErrCode { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        public string ErrMsg { get; set; }
    }
}
