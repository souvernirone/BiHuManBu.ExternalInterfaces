using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class IsUploadImgViewModel:BaseViewModel
    {
        private List<IsUploadImg> _isUpload = new List<IsUploadImg>();
        /// <summary>
        /// 新的用户上传的图片
        /// </summary>
        public List<IsUploadImg> IsUploadImg { get { return _isUpload; } set { _isUpload = value; } }
    }
}
