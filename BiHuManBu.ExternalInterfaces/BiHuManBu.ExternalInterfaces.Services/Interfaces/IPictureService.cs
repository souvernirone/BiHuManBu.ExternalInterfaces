using System.Collections.Generic;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Interfaces
{
    public interface IPictureService
    {
        BaseViewModel AddMultiple(AddMultipleInput input);

        List<UrlAndType> GetPictures(long buid,long source);
        int IsUploadImg(long buid, long source);
        List<IsUploadImg> IsUploadImg(long buid);
        /// <summary>
        /// 修改上传图片的状态
        /// </summary>
        /// <param name="buid"></param>
        /// <param name="source"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        int UpdateImgState(long buid);
    }
}
