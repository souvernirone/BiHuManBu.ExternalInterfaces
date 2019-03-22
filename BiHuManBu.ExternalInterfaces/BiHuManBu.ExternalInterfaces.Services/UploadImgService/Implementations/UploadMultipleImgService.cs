using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.UploadImg;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.UploadImgService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.UploadImgService.Implementations
{
    public class UploadMultipleImgService : IUploadMultipleImgService
    {
        private readonly IUploadImgValidate _uploadImgValidate;
        private readonly IUpdateImgTimes _updateImgTimes;
        private readonly IPictureService _pictureService;
        private readonly string _imageUpload = System.Configuration.ConfigurationManager.AppSettings["ImageUpload"];
        private readonly string _uploadImgUrl = System.Configuration.ConfigurationManager.AppSettings["ImageServer"];
        private readonly int _upImgCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["UpImgCount"]);
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        public UploadMultipleImgService(IUploadImgValidate uploadImgValidate, IUpdateImgTimes updateImgTimes, IPictureService pictureService)
        {
            _uploadImgValidate = uploadImgValidate;
            _updateImgTimes = updateImgTimes;
            _pictureService = pictureService;
        }

        public UploadMultipleViewModel UploadMultipleImg(UploadMultipleImgRequest request)
        {
            UploadMultipleViewModel viewModel = new UploadMultipleViewModel();
            try
            {
                //验证
                var validate = _uploadImgValidate.Validate(request);
                if (validate.BusinessStatus != 1)
                {
                    //参数校验失败，返回对应结果
                    viewModel.BusinessStatus = validate.BusinessStatus;
                    viewModel.StatusMessage = validate.StatusMessage;
                    return viewModel;
                }

                //上传逻辑
                bool isOk = true;
                List<UrlAndType> listImg = new List<UrlAndType>();
                List<UploadMultipleFileResult> listResult = new List<UploadMultipleFileResult>();
                //上传的图片
                List<BaseContect> dic = request.ListBaseContect;
                foreach (var item in dic)
                {
                    string upImgUrl = string.Empty;
                    //获取图片路径
                    if (item.IsUpload == 1)
                    {//无需上传
                        upImgUrl = item.StrBase;
                    }
                    else if (item.IsUpload == 0)
                    {//需要上传
                        string base64 = item.StrBase.Substring(item.StrBase.IndexOf(',') + 1);
                        byte[] data = Convert.FromBase64String(base64);
                        var versions = new Dictionary<string, string>();
                        versions.Add("_large", "maxwidth=600&maxheight=400&format=jpg");
                        var fileUploadModel = new FileUploadModel
                        {
                            FileName = "xxxxx.jpg",
                            VersionKey = versions
                        };
                        //上传图片至服务器 
                        var dw = new DynamicWebService();
                        object[] postArg = new object[2];
                        postArg[0] = fileUploadModel.ToJson();
                        postArg[1] = data;
                        var ret = dw.InvokeWebservice(
                            _imageUpload + "/fileuploadcenter.asmx", "BiHuManBu.ServerCenter.FileUploadCenter",
                            "FileUploadCenter", "ImageUpload", postArg);

                        UploadMultipleFileResult itemResult = ret.ToString().FromJson<UploadMultipleFileResult>();
                        //itemResult.Index = item.ImgId;//item.Key;
                        itemResult.ImgType = item.ImgType;
                        listResult.Add(itemResult);

                        if (itemResult.ResultCode == 0)
                        {
                            upImgUrl = itemResult.FilePath;
                        }
                        else
                        {
                            isOk = false;
                        }
                    }
                    //将图片Url保存到List中，存库
                    UrlAndType imgUrl = new UrlAndType
                    {
                        Url = upImgUrl.Contains(_uploadImgUrl) ? upImgUrl : _uploadImgUrl + upImgUrl,
                        Type = item.ImgType
                    };
                    listImg.Add(imgUrl);
                }
                viewModel.ListResult = listResult;
                if (!isOk)
                {
                    viewModel.BusinessStatus = -10002;
                    viewModel.StatusMessage = "某个图片上传失败，具体内容查看ListResult";
                }
                else
                {
                    var redisKey = "UpImgTimes_" + request.BuId;
                    var times = CacheProvider.Get<int?>(redisKey);
                    int alreadyTimes = times.HasValue ? times.Value : 0;
                    //修改redis中的上传次数
                    if (_upImgCount != 0)
                    {//如果=0，没有上传限制
                        _updateImgTimes.UpdateTimes(request.BuId, ++alreadyTimes);
                    }
                    AddMultipleInput input = new AddMultipleInput
                    {
                        BuId = request.BuId,
                        UrlList = listImg,
                        Source = request.Source
                    };
                    logInfo.Info("请求图片请求：" + input.ToJson());
                    var upImg = _pictureService.AddMultiple(input);
                    var re = new UploadMultipleViewModel()
                    {
                        BusinessStatus = upImg.BusinessStatus,
                        StatusMessage = upImg.StatusMessage,
                        UrlList = listImg
                    };
                    return re;
                }
            }
            catch (Exception ex)
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "图片上传异常";
                logError.Info("获取报价单详情接口发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return null;
        }


    }
}
