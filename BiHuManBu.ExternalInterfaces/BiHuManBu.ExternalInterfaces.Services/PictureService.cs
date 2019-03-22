using System.Configuration;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.MessageCenter;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Models;
using System;
using log4net;
using ServiceStack.Text;
using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Repository;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class PictureService : CommonBehaviorService, IPictureService
    {
        /// <summary>
        /// 上传图片的类型
        /// </summary>
        private static readonly string[] targetType = { "T01", "T02", "T03", "T04" };

        private IPictureRepository _pictureRepository;
        private IUserInfoRepository _userInfoRepository;
        private IMessageCenter _messageCenter;
        private IAgentRepository _agentRepository;
        private ICacheHelper _cacheHelper;
        private ILog logInfo;
        private ILog logError;

        public PictureService(IAgentRepository agentRepository, ICacheHelper cacheHelper, IPictureRepository pictureRepository, IUserInfoRepository userInfoRepository, IMessageCenter messageCenter)
            : base(agentRepository, cacheHelper)
        {
            _pictureRepository = pictureRepository;
            _userInfoRepository = userInfoRepository;
            _messageCenter = messageCenter;
            logInfo = LogManager.GetLogger("INFO");
            logError = LogManager.GetLogger("ERROR");
        }

        public BaseViewModel AddMultiple(AddMultipleInput input)
        {
            BaseViewModel viewModel = new BaseViewModel();
            try
            {
                //soucr转换
                int sourceold = SourceGroupAlgorithm.GetOldSource(input.Source);
                //判断list是否4种类型都有
                List<UrlAndType> urlType = input.UrlList;
                //if (urlType.Count < targetType.Length)
                //{
                //    return BaseViewModel.GetBaseViewModel(-10000, "每种照片至少传一张才可提交");
                //}
                //去掉类型限制
                //var acutalType = urlType.Select(o => o.Type).Distinct().ToArray();
                //if (!CommonHelper.ArrayCompare2(targetType, acutalType))
                //{
                //    return BaseViewModel.GetBaseViewModel(-10000, "缺少图片类型");
                //}

                //

                //检查buid的合法性  
                if (!_userInfoRepository.IsExist(o => o.Id == input.BuId))
                {
                    return BaseViewModel.GetBaseViewModel(-10000, "buid不存在");
                }

                var picsJson = input.UrlList.ToJson();

                bx_picture pic = _pictureRepository.FirstOrDefault(o => o.b_uid == input.BuId && o.source == sourceold);
                if (pic == null)
                {
                    //不存在是插入
                    return AddPicture(input.BuId, picsJson, sourceold);
                }
                else if (pic.state == 4)
                {//status=4 图片上传中不允许修改
                    return BaseViewModel.GetBaseViewModel(-10002, "图片上传中不允许修改");
                }
                else
                {//biud=>bx_picture存在 状态  !=4  直接更新
                    pic.picsJson = picsJson;
                    pic.state = 3;
                    pic.update_time = DateTime.Now;
                    pic.source = sourceold;
                    return UpdatePicture(pic);
                }
            }
            catch (Exception ex)
            {
                viewModel.BusinessStatus = -100003;
                logError.Info("上传图片发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
            return viewModel;
        }

        private BaseViewModel UpdatePicture(bx_picture picture)
        {
            _pictureRepository.Update(picture);
            if (_pictureRepository.SaveChanges() > 0)
            {
                //调用中心的方法
                return AddSuccessSendMsg(picture.b_uid.Value, picture.source.Value);
            }
            else
            {
                return BaseViewModel.GetBaseViewModel(-10003, "添加失败");
            }
        }


        /// <summary>
        /// 添加照片
        /// </summary>
        /// <param name="buId"></param>
        /// <param name="picsJson"></param>
        /// <returns></returns>
        private BaseViewModel AddPicture(long buId, string picsJson, int source)
        {
            bx_picture picture = new bx_picture
            {
                b_uid = buId,
                picsJson = picsJson,
                state = 3,
                create_time = DateTime.Now,
                update_time = DateTime.Now,
                source = source
            };
            _pictureRepository.Insert(picture);
            if (_pictureRepository.SaveChanges() > 0)
            {
                //调用中心的方法
                return AddSuccessSendMsg(buId, source);
            }
            else
            {
                return BaseViewModel.GetBaseViewModel(-10002, "添加失败");
            }
        }

        /// <summary>
        /// 中心返回的消息对象
        /// </summary>
        private class ReMsg
        {
            /// <summary>
            /// 错误码
            /// </summary>
            public int ErrCode { get; set; }
            /// <summary>
            /// 错误描述
            /// </summary>
            public string ErrMsg { get; set; }
            /// <summary>
            /// 机器人接口版本号
            /// </summary>
            public string Version { get; set; }
            /// <summary>
            /// 版本类型
            /// </summary>
            public string VersionType { get; set; }
        }

        /// <summary>
        /// 发送上传图片消息
        /// </summary>
        /// <param name="buid"></param>
        /// <param name="ulImgKey"></param>
        private void SendMsg(long buid, string ulImgKey, int source)
        {
            CacheProvider.Remove(ulImgKey);
            //初始化ulImgKey缓存，标记1
            ReMsg reMsg = new ReMsg
            {
                ErrCode = 1,
                ErrMsg = string.Empty,
                Version = string.Empty,
                VersionType = string.Empty
            };
            CacheProvider.Set(ulImgKey, reMsg, 3600);//缓存1h //20171025改为redis保存对象
            var msgBody = new
            {
                UploadType = 1,//上传图片方式，0，base64 1，url地址(20171031新增字段)
                B_Uid = buid,
                IsCloseSms = 0,
                NotifyCacheKey = ulImgKey,
                RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Source = source
            };
            logInfo.Info("发送的消息体：" + msgBody.ToJson());
            //发送请求上传图片消息
            var msgbody = _messageCenter.SendToMessageCenter(msgBody.ToJson(), ConfigurationManager.AppSettings["MessageCenter"], ConfigurationManager.AppSettings["BxPicName"]);
            logInfo.Info("返回：" + msgbody.ToJson());
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        private ReMsg ReadData(string ulImgKey)
        {
            var cacheKey = CacheProvider.Get<ReMsg>(ulImgKey);
            logInfo.Info("消息首次返回：" + cacheKey.ToJson());
            if (cacheKey.ErrCode != 1 && cacheKey.ErrCode != -1)
            {
                return cacheKey;
            }
            else
            {
                System.Threading.Thread.Sleep(1000);
                for (int i = 0; i < 200; i++)
                {
                    cacheKey = new ReMsg();
                    cacheKey = CacheProvider.Get<ReMsg>(ulImgKey);
                    logInfo.Info("消息第" + (i + 1) + "次返回：" + cacheKey.ToJson());
                    if (cacheKey.ErrCode != 1 && cacheKey.ErrCode != -1)
                    {
                        return cacheKey;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 入库成功，调用中心的消息
        /// </summary>
        /// <param name="buid"></param>
        /// <returns></returns>
        private BaseViewModel AddSuccessSendMsg(long buid, int source)
        {
            //return BaseViewModel.GetBaseViewModel(1, "添加成功！");
            var ulImgKey = string.Format("{0}-upimg-{1}", buid, "key");
            SendMsg(buid, ulImgKey, source);
            ReMsg msg = ReadData(ulImgKey);//new ReMsg();//
            if (msg == null)
            {
                return BaseViewModel.GetBaseViewModel(-10002, "添加失败！");
            }
            if (msg.ErrCode == 0)
            {
                return BaseViewModel.GetBaseViewModel(1, "添加成功！");
            }
            else
            {
                return BaseViewModel.GetBaseViewModel(-10002, msg.ErrMsg);
            }
        }

        public List<UrlAndType> GetPictures(long buid, long source)
        {
            int oldsource = SourceGroupAlgorithm.GetOldSource(source);
            bx_picture picture = _pictureRepository.FirstOrDefault(o => o.b_uid.Value == buid && o.source == oldsource);
            if (picture == null)
            {
                return new List<UrlAndType>();
            }
            try
            {
                if (string.IsNullOrEmpty(picture.picsJson))
                    return new List<UrlAndType>();
                var imgs = picture.picsJson.FromJson<List<UrlAndType>>();
                string uploadImgUrl = ConfigurationManager.AppSettings["ImageServer"];
                imgs.ForEach(i => i.Url = (i.Url.Contains(uploadImgUrl) ? i.Url : uploadImgUrl + i.Url));
                return imgs;
            }
            catch
            {
                logInfo.Info("bx_picture中picsJson的格式不正确，buid为" + buid);
            }

            return null;
        }

        public int IsUploadImg(long buid, long source)
        {
            int oldsource = SourceGroupAlgorithm.GetOldSource(source);
            bx_picture picture = _pictureRepository.FirstOrDefault(o => o.b_uid.Value == buid && o.source == oldsource);
            if (picture == null)
                return 0;
            if (picture.state == 0)
                return 0;
            else
                return 1;
        }

        public List<IsUploadImg> IsUploadImg(long buid)
        {
            List<IsUploadImg> isUploadImg = new List<IsUploadImg>();
            bx_userinfo userinfo = _userInfoRepository.FindByBuid(buid);
            if (userinfo == null)
            {
                return new List<IsUploadImg>();
            }
            if (!userinfo.IsSingleSubmit.HasValue)
            {
                return new List<IsUploadImg>();
            }
            //获取图片bx_picture
            List<bx_picture> picList = _pictureRepository.GetAllList(o => o.b_uid == buid);
            //获取报了哪些保司
            List<long> sourceList = SourceGroupAlgorithm.ParseSource(userinfo.IsSingleSubmit.Value);
            if (sourceList.Any() && picList.Any())
            {
                foreach (int itk in sourceList)
                {
                    var oit = SourceGroupAlgorithm.GetOldSource(itk);//获取到旧的source值
                    //拼装上传的图片模型
                    bx_picture model = picList.FirstOrDefault(l => l.source == oit);
                    if (model != null && model.id != 0 && model.state == 5)
                    {
                        IsUploadImg newmodel = new IsUploadImg()
                        {
                            IsUpload = 1,//已上传
                            Source = itk
                        };
                        isUploadImg.Add(newmodel);
                    }
                    else
                    {
                        IsUploadImg newmodel = new IsUploadImg()
                        {
                            IsUpload = 0,//未上传
                            Source = itk
                        };
                        isUploadImg.Add(newmodel);
                    }
                }
            }
            return isUploadImg;
        }

        public int UpdateImgState(long buid)
        {
            return _pictureRepository.UpdateState(buid);
        }
    }
}
