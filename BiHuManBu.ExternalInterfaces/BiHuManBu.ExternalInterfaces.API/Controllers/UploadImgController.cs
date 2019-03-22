using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.UploadImg;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.UploadImgService.Interfaces;
using log4net;
using ServiceStack.Text;
using BiHuManBu.ExternalInterfaces.API.Filters;
using Newtonsoft.Json;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class UploadImgController : ApiController
    {
        private static readonly int _upImgCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["UpImgCount"]);
        private static readonly string _uploadImgUrl = System.Configuration.ConfigurationManager.AppSettings["ImageServer"];
        private IPictureService _pictureService;
        private ILog logError;
        private ILog logInfo;
        private IUploadMultipleImgService _uploadMultipleImgService;
        public UploadImgController(IPictureService pictureService, IUploadMultipleImgService uploadMultipleImgService)
        {
            _pictureService = pictureService;
            _uploadMultipleImgService = uploadMultipleImgService;
            logError = LogManager.GetLogger("ERROR");
            logInfo = LogManager.GetLogger("INFO");
        }

        string imageUpload = System.Configuration.ConfigurationManager.AppSettings["ImageUpload"];
        [HttpPost]
        public HttpResponseMessage Upload([FromUri]string baseContent, UploadBusinessType filetype)
        {
            string source = baseContent;
            string base64 = source.Substring(source.IndexOf(',') + 1);
            byte[] data = Convert.FromBase64String(base64);
            var fileUploadModel = new FileUploadModel();
            fileUploadModel.VersionKey = new Dictionary<string, string>();
            fileUploadModel.BusinessType = filetype;
            fileUploadModel.FileName = "xxxxx.jpg";
            //上传图片至服务器 
            var dw = new DynamicWebService();
            object[] postArg = new object[2];
            postArg[0] = fileUploadModel.ToJson();
            postArg[1] = data;
            var ret = dw.InvokeWebservice(
                imageUpload + "/fileuploadcenter.asmx", "BiHuManBu.ServerCenter.FileUploadCenter", "FileUploadCenter", "ImageUpload", postArg);
            var tt = ret.ToString().FromJson<UploadFileResult>();
            return tt.ResponseToJson();
        }


        [HttpPost]
        public HttpResponseMessage UploadImg([FromBody]UploadImgRequest request)
        {
            UploadFileResult viewModel = new UploadFileResult();
            if (!ModelState.IsValid)
            {
                viewModel.ResultCode = -100;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";"));
                viewModel.Message = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            try
            {
                string source = request.baseContent;
                string base64 = source.Substring(source.IndexOf(',') + 1);
                byte[] data = Convert.FromBase64String(base64);
                var versions = new Dictionary<string, string>();
                versions.Add("_small", "maxwidth=50&maxheight=50&format=jpg");
                versions.Add("_medium", "maxwidth=200&maxheight=200&format=jpg");
                versions.Add("_large", "maxwidth=800&maxheight=660&format=jpg");
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
                    imageUpload + "/fileuploadcenter.asmx", "BiHuManBu.ServerCenter.FileUploadCenter", "FileUploadCenter", "ImageUpload", postArg);
                viewModel = ret.ToString().FromJson<UploadFileResult>();
            }
            catch (Exception)
            {
                viewModel.ResultCode = -100;
                viewModel.Message = "图片上传异常";
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 上传多个图片 该版本的seccode加密只用buid和agent。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ModelVerify]
        [HttpPost]
        public HttpResponseMessage UploadMultipleImg([FromBody]UploadMultipleImgRequest request)
        {
            logInfo.Info("请求图片请求：" + request.ToJson());
            var result = _uploadMultipleImgService.UploadMultipleImg(request);
            return result.ResponseToJson();
        }

        /// <summary>
        /// 获取是否上传过照片的标识，单独请求报价核保后发起的请求 /PC /2017.12 /光鹏洁
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage IsUploadImg([FromUri]IsUploadImgRequest request)
        {
            logInfo.Info("获取图片上传状态请求：" + Request.RequestUri);
            IsUploadImgViewModel viewModel = new IsUploadImgViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10001;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";"));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            List<IsUploadImg> isUploadImg = _pictureService.IsUploadImg(request.Buid);
            viewModel.BusinessStatus = 1;
            viewModel.StatusMessage = "获取成功";
            viewModel.IsUploadImg = isUploadImg;
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 修改上传图片的状态为需要重新上传，报价时将此值改为0. /PC /2018.1.20 /光鹏洁
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage UpdateImgState([FromUri]IsUploadImgRequest request)
        {
            BaseViewModel viewModel = new BaseViewModel();
            if (!ModelState.IsValid)
            {
                viewModel.BusinessStatus = -10001;
                string msg = ModelState.Values.Where(a => a.Errors.Count == 1).Aggregate(string.Empty, (current, a) => current + (a.Errors[0].ErrorMessage + ";"));
                viewModel.StatusMessage = "输入参数错误，" + msg;
                return viewModel.ResponseToJson();
            }
            int picState = _pictureService.UpdateImgState(request.Buid);
            logInfo.Info(string.Format("修改图片上传状态请求：{0}；修改{1}", Request.RequestUri, picState == 1 ? "成功" : "失败"));
            viewModel.BusinessStatus = 1;
            viewModel.StatusMessage = "修改成功";
            return viewModel.ResponseToJson();
        }
    }
}