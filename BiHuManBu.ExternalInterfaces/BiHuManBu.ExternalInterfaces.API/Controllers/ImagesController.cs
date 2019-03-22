using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using BiHuManBu.ExternalInterfaces.API.Filters;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.API.Controllers
{
    public class ImagesController : ApiController
    {
        private IImagesService _imagesService;
        private IPictureService _pictureService;
        private ILog logInfo;
        public ImagesController(IImagesService imagesService, IPictureService pictureService)
        {
            _imagesService = imagesService;
            _pictureService = pictureService;
            logInfo = LogManager.GetLogger("INFO");
        }
        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Add([FromBody]GetImageRequest request)
        {
            BaseViewModel viewModel = new BaseViewModel();
            try
            {
                int count = _imagesService.Add(request);
                if (count == 0)
                {
                    viewModel.BusinessStatus = 1;
                }
                else
                {
                    viewModel.BusinessStatus = 10002;
                    viewModel.StatusMessage = "添加图片失败";
                }
            }
            catch (Exception)
            {
                viewModel.BusinessStatus = 10002;
                viewModel.StatusMessage = "添加图片异常";
            }
            return viewModel.ResponseToJson();
        }

        [HttpGet]
        public HttpResponseMessage GetImages(long buid)
        {
            ImagesViewModel viewModel = new ImagesViewModel();
            try
            {
                viewModel.imageses = _imagesService.GetImages(buid);
                if (viewModel.imageses != null)
                {
                    viewModel.BusinessStatus = 1;
                }
                else
                {
                    viewModel.BusinessStatus = -10000;
                    viewModel.StatusMessage = "没有照片数据";
                }
            }
            catch (Exception)
            {
                viewModel.BusinessStatus = 10002;
                viewModel.StatusMessage = "查询图片异常";
            }
            return viewModel.ResponseToJson();
        }


        /// <summary>
        /// 批量上传图片
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ModelVerify]
        [HttpPost]
        public HttpResponseMessage AddMultiple([FromBody]AddMultipleRequest request)
        {
            BaseViewModel viewModel = new BaseViewModel();
            if (request == null)
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "参数为空";
                return viewModel.ResponseToJson();
            }
            try
            {
                AddMultipleInput input = new AddMultipleInput
                {
                    BuId = request.BuId,
                    UrlList = request.ImgList.FromJson<List<UrlAndType>>()
                };
                return _pictureService.AddMultiple(input).ResponseToJson();
            }
            catch
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "ImgList格式错误，格式要求：[{\"Url\":\" / 1900 / 01 / 01 / xx.jpg\",\"Type\":\"T01\"},{\"Url\":\" / 1900 / 01 / 01 / xx1.jpg\",\"Type\":\"T01\"}]";
            }
            return viewModel.ResponseToJson();
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ModelVerify]
        [HttpGet]
        public HttpResponseMessage GetPictures([FromUri]GetByBuidRequest request)
        {
            logInfo.Info("获取图片请求：" + Request.RequestUri);
            GetPicturesViewModel viewModel = new GetPicturesViewModel();
            if (request == null)
            {
                viewModel.BusinessStatus = -10000;
                viewModel.StatusMessage = "参数为空";
                return viewModel.ResponseToJson();
            }
            try
            {
                viewModel.BusinessStatus = 1;
                viewModel.UrlList = _pictureService.GetPictures(request.Buid,request.Source);
                return viewModel.ResponseToJson();
            }
            catch
            {
                viewModel.BusinessStatus = -10003;
                viewModel.StatusMessage = "获取图片异常";
            }
            return viewModel.ResponseToJson();
        }
    }
}
