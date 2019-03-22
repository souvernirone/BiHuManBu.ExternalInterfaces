using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Request
{
    public class AddMultipleRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [Range(1, long.MaxValue, ErrorMessage = "BuId不正确")]
        public long BuId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "ImgList不能为空")]
        public string ImgList { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    //public class UrlAndType
    //{
    //    public string Url { get; set; }
    //    public string Type { get; set; }
    //}

    public class AddMultipleInput
    {
        public long Source { get; set; }
        public long BuId { get; set; }

        public List<UrlAndType> UrlList
        {
            get { return urlList; }
            set { urlList = value; }
        }

        private List<UrlAndType> urlList = new List<UrlAndType>();
    }
}
