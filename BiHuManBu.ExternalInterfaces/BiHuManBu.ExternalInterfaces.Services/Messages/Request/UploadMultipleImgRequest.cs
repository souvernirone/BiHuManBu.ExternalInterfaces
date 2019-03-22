using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class UploadMultipleImgRequest
    {
        [Required(ErrorMessage = "图片内容不能为空")]
        public List<BaseContect> ListBaseContect { get; set; }
        [Range(0, 10000000000)]
        public long BuId { get; set; }

        /// <summary>
        /// 默认平安
        /// </summary>
        private long _source = 2;

        /// <summary>
        /// 2017.11新增字段。渠道1,2,4,8...
        /// </summary>
        public long Source
        {
            get { return _source; }
            set { _source = value; }
        }

        #region 为了替换buid新增的
        public string LicenseNo { get; set; }
        public string CustKey { get; set; }
        /// <summary>
        ///     0小车，1大车，默认0
        /// </summary>
        public int RenewalCarType { get; set; }
        /// <summary>
        /// 顶级代理Id
        /// </summary>
        public int Agent { get; set; }
        /// <summary>
        /// 子集代理Id
        /// </summary>
        public int ChildAgent { get; set; }
        #endregion
    }
    public class BaseContect
    {
        [Required(ErrorMessage = "图片内容不能为空")]
        public string StrBase { get; set; }
        private string _imgType = string.Empty;
        [Required(ErrorMessage = "图片类型不能为空")]
        public string ImgType
        {
            get
            {
                return _imgType;
            }
            set
            {
                switch (value) {
                    case "T01_1":
                        _imgType = "T10";
                        break;
                    case "T01_2":
                        _imgType = "T11";
                        break;
                    case "T02_1":
                        _imgType = "B10";
                        break;
                    case "T02_2":
                        _imgType = "B11";
                        break;
                    case "T03_1":
                        _imgType = "C10";
                        break;
                    case "T03_2":
                        _imgType = "C11";
                        break;
                    case "T04_1":
                        _imgType = "C60";
                        break;
                    case "T04_2":
                        _imgType = "C61";
                        break;
                    default:
                        _imgType = value;
                        break;
                }
            }
        }
        /// <summary>
        /// 0未上传 1已上传
        /// </summary>
        [Range(0, 1)]
        public int IsUpload { get; set; }
    }
}
