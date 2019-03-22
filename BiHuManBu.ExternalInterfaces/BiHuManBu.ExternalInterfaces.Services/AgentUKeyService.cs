using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using BiHuManBu.ExternalInterfaces.Infrastructure;
using BiHuManBu.ExternalInterfaces.Infrastructure.Caches;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.PartialModels.bx_agent;
using BiHuManBu.ExternalInterfaces.Models.ReportModel;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using log4net;
using ServiceStack.Text;
using System.Text.RegularExpressions;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class AgentUKeyService : CommonBehaviorService, IAgentUKeyService
    {
        private ICacheHelper _cacheHelper;
        private ILog logInfo;
        private ILog logError;
        private IAgentRepository _agentRepository;
        private IAgentUKeyRepository _iagentAgentUKeyRepository;
        private IAgentConfigRepository _agentConfig;
        private static readonly string _baoxianCenter =
            System.Configuration.ConfigurationManager.AppSettings["baoxianCenterApi"];

        public AgentUKeyService(ICacheHelper cacheHelper, IAgentRepository agentRepository, IAgentUKeyRepository iagentAgentUKeyRepository, IAgentConfigRepository agentConfig)
            : base(agentRepository, cacheHelper)
        {
            _cacheHelper = cacheHelper;
            logInfo = LogManager.GetLogger("INFO");
            logError = LogManager.GetLogger("ERROR");
            _agentRepository = agentRepository;
            _iagentAgentUKeyRepository = iagentAgentUKeyRepository;
            _agentConfig = agentConfig;
        }

        public UKeyListResponse GetUKeyList(GetUKeyListRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            UKeyListResponse response = new UKeyListResponse();
            try
            {
                IBxAgent agentModel = GetAgentModelFactory(request.Agent);
                if (!agentModel.AgentCanUse())
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
                if (!ValidateReqest(pairs, agentModel.SecretKey, request.SecCode))
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
                var list = _iagentAgentUKeyRepository.GetList(request.Agent);
                if (list.Any())
                {
                    //如果source传值的话，只取一条记录
                    if (request.Source > 0)
                    {
                        request.Source = SourceGroupAlgorithm.GetOldSource(request.Source);
                        list = list.Where(i => i.Source == request.Source).ToList();
                        if (list.Count > 0)
                        {
                            list = list.GetRange(0, 1);
                        }
                        else
                        {
                            response.ErrCode = -1;
                            response.ErrMsg = "无记录";
                            return response;
                        }
                    }
                    //else
                    //{
                    //此处省略了list=list；
                    //}
                    //转换source值
                    var newList = new List<CityUKeyModel>();
                    var model = new CityUKeyModel();
                    foreach (var item in list)
                    {
                        model = new CityUKeyModel();
                        model = item;
                        model.Source = SourceGroupAlgorithm.GetNewSource((int)item.Source);
                        newList.Add(model);
                    }
                    //获取信息成功
                    response.Status = HttpStatusCode.OK;
                    response.CityUKeyList = newList;
                }
                else
                {
                    response.ErrCode = -1;
                    response.ErrMsg = "无记录";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response = new UKeyListResponse();
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("获取代理人ukey请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException + ",返回对象信息：" + request.ToJson());
            }
            return response;
        }
        public BaseResponse EditAgentUKey(EditAgentUKeyRequest request, IEnumerable<KeyValuePair<string, string>> pairs)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                IBxAgent agentModel = GetAgentModelFactory(request.Agent);

                if (!agentModel.AgentCanUse())
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
                if (request.ReqSource == 1)
                {//对外的接口才做这一层校验
                    var validateUrl = string.Format("UserCode={0}&UkeyId={1}&OldPassWord={2}&NewPassWord={3}&Agent={4}", request.UserCode, request.UkeyId, request.OldPassWord, request.NewPassWord, request.Agent);
                    if (!ValidatePostReqest(validateUrl, agentModel.SecretKey, request.SecCode))
                    {
                        response.Status = HttpStatusCode.Forbidden;
                        return response;
                    }
                }
                //查询ukey信息
                var ukeyModel = _iagentAgentUKeyRepository.GetModel(request.UkeyId);
                if (ukeyModel == null)
                {
                    response.ErrCode = -1;
                    response.ErrMsg = "未查到Ukey信息";
                    return response;
                }
                //请求中心修改密码接口
                string strUrl = string.Format("{0}/api/ChangePassWord/ChangePwd", _baoxianCenter);
                string oldPwd = request.ReqSource == 1 ? request.OldPassWord : string.Empty;
                var objPost = new
                {
                    UserCode = request.UserCode,
                    UkeyId = request.UkeyId,
                    OldPassWord = oldPwd,
                    NewPassWord = request.NewPassWord
                };
                StringBuilder postData = new StringBuilder();
                postData.Append("UserCode=").Append(request.UserCode)
                    .Append("&UkeyId=").Append(request.UkeyId)
                    .Append("&OldPassWord=").Append(oldPwd)
                    .Append("&NewPassWord=").Append(request.NewPassWord)
                    .Append("&ReqSource=").Append(request.ReqSource);
                string result = string.Empty;
                int i = HttpWebAsk.Post(strUrl, postData.ToString(), out result);
                logInfo.Info("调用中心修改密码接口Url：" + strUrl + ";请求参数为：" + objPost.ToJson() + ";返回结果为：" + result);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    var ret = result.FromJson<UKeyEdit>();
                    if (ret.ErrCode == 0)
                    {//修改成功
                        response.Status = HttpStatusCode.OK;
                        //如果修改成功，则保存用户名
                        if (string.IsNullOrEmpty(ukeyModel.InsuranceUserName))
                        {
                            ukeyModel.InsuranceUserName = request.UserCode;
                            int u = _iagentAgentUKeyRepository.UpdateModel(ukeyModel);
                            logInfo.Info(string.Format("修改保司密码成功，数据库修改{0}条记录。", u));
                        }
                    }
                    else
                    {
                        //修改失败
                        response.ErrCode = -1;
                        response.ErrMsg = ret.ErrMsg;
                    }
                }
            }
            catch (Exception ex)
            {
                response = new BaseResponse();
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("修改保司密码请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException + ",返回对象信息：" + request.ToJson());
            }
            return response;
        }
        /// <summary>
        /// 修改代理人备份密码 2017-10-20 zky/运营后台
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse EditBackupPwd(EditBackupPwdRequest request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                IBxAgent agentModel = GetAgentModelFactory(request.Agent);
                if (!agentModel.AgentCanUse())
                {
                    response.Status = HttpStatusCode.Forbidden;
                    return response;
                }
                if (request.ReqSource == 1)
                {//对外的接口才做这一层校验
                    var validateUrl = string.Format("Agent={0}&UkeyId={1}&PassWord1={2}&PassWord2={3}&PassWord3={4}", request.Agent, request.UkeyId, request.PwdOne, request.PwdTwo, request.PwdThree);
                    if (!ValidatePostReqest(validateUrl, agentModel.SecretKey, request.SecCode))
                    {
                        response.Status = HttpStatusCode.Forbidden;
                        return response;
                    }
                }

                //查询ukey信息
                var ukeyModel = _iagentAgentUKeyRepository.GetModel(request.UkeyId);
                if (ukeyModel == null)
                {
                    response.ErrCode = -1;
                    response.ErrMsg = "未查到Ukey信息";
                    return response;
                }

                #region 20170927  L： 密码复杂度正则表达式   校验时候用  暂时做个储备
                Regex Num = new Regex("(?=.*[0-9])"); //数字
                Regex Cn = new Regex("[\u4e00-\u9fa5]+");//中文
                Regex Up = new Regex("(?=.*[A-Z])");//大写字母
                Regex Low = new Regex("(?=.*[a-z])");//小写字母
                Regex Len = new Regex(".{8,20}");//8到20字符
                Regex Special = new Regex("[`~!@#$%^&*()+=|{}':;',\\[\\].<>?/~！@#￥%……&*（）——+|{}【】‘；：”“’。，、？]");//特殊字符
                #endregion

                #region 20170928  L： 密码难度规则校验
                var bl = false;
                //判断密码的复杂度
                string[] lstStrs = new[] { request.PwdOne, request.PwdTwo, request.PwdThree };

                foreach (var item in lstStrs)
                {
                    //判断长度和密码难度  是否符合两种
                    int checkItem = (Num.IsMatch(item) ? 1 : 0) + (Up.IsMatch(item) ? 1 : 0) +
                     (Low.IsMatch(item) ? 1 : 0) + (Special.IsMatch(item) ? 1 : 0);
                    if (checkItem < 2)
                    {
                        bl = true;
                        break;
                    }

                    //判断是否含有picc  和  长度校验  和  中文字符校验 
                    if (item.ToLower().Contains("picc") || !Len.IsMatch(item) || Cn.IsMatch(item))
                    {
                        bl = true;
                        break;
                    }
                }

                //判断密码难度  或者  四个密码是否有相同
                if (bl || lstStrs.Distinct().Count() < lstStrs.Length)
                {
                    response.ErrCode = -1;
                    response.ErrMsg = "密码格式信息不对，请参照密码格式提示输入！";
                    return response;
                }

                #endregion

                //调用中心修改备用密码接口
                string url = string.Format("{0}/api/ChangePassWord/ChangeHistoryPwd", _baoxianCenter);
                var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                                    {
                                         {"UkeyId",request.UkeyId.ToString()},
                                         {"PassWord1", request.PwdOne},
                                         {"PassWord2", request.PwdTwo},
                                         {"PassWord3", request.PwdThree}
                                    });
                var handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip };
                using (var http = new HttpClient(handler))
                {
                    var ResultReturn = http.PostAsync(url, content).Result.Content.ReadAsStringAsync().Result;
                    var result = ResultReturn.FromJson<BaseViewModel>();
                    if (result.BusinessStatus == 200)
                    {
                        response.Status = HttpStatusCode.OK;
                        //保存备用密码
                        ukeyModel.backup_pwd_one = request.PwdOne;
                        ukeyModel.backup_pwd_two = request.PwdTwo;
                        ukeyModel.backup_pwd_three = request.PwdThree;
                        _iagentAgentUKeyRepository.UpdateModel(ukeyModel);
                    }
                    else
                    {
                        //修改失败
                        response.ErrCode = -1;
                        response.ErrMsg = result.StatusMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                response = new BaseResponse();
                response.Status = HttpStatusCode.ExpectationFailed;
                logError.Info("修改保司备份密码请求发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException + ",返回对象信息：" + request.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 根据UKId获取城市Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int GetAgentCityCodeByUKId( int Id)
        {
            var model = _iagentAgentUKeyRepository.GetAgentUKeyModel(Id);
            return model == null ? 0 : model.city_id.HasValue ? model.city_id.Value : 0;
        }
    }
}
