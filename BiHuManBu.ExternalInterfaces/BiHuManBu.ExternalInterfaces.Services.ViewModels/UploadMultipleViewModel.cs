using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{

    public class UploadMultipleViewModel : BaseViewModel
    {
        private List<UploadMultipleFileResult> _listResult = new List<UploadMultipleFileResult>();

        /// <summary>
        /// 每个图片的上传结果
        /// </summary>
        public List<UploadMultipleFileResult> ListResult
        {
            get { return _listResult; }
            set { _listResult = value; }
        }

        private List<UrlAndType> _urlList=new List<UrlAndType>();
        public List<UrlAndType> UrlList
        {
            get { return _urlList; }
            set { _urlList = value; }
        }
    }

    public class UploadMultipleFileResult
    {
        public string FileName { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
        public bool IsValid { get; set; }
        public string FilePath { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// 图片的标识
        /// </summary>
        //public int Index { get; set; }
        public string ImgType { get; set; }
    }
}