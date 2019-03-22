using System;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using log4net;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class UserService : IUserService
    {
        private IUserInfoRepository _userInfoRepository;
        private IUserRepository _userRepository;
        private ILog logInfo = LogManager.GetLogger("INFO");
        private ILog logError = LogManager.GetLogger("ERROR");
        private static object obj = new object();
        public UserService(IUserInfoRepository userInfoRepository, IUserRepository userRepository)
        {
            _userInfoRepository = userInfoRepository;
            _userRepository = userRepository;
        }

       /// <summary>
       /// 根据userid查找用户信息
       /// </summary>
       /// <param name="userid"></param>
       /// <returns></returns>
       public bx_userinfo Find(int userid)
       {
           return _userInfoRepository.Find(userid);
       }

       /// <summary>
       /// 根据openid和车牌号查找用户信息
       /// </summary>
       /// <param name="openId"></param>
       /// <param name="licenseno"></param>
       /// <returns></returns>
       public bx_userinfo FindByOpenIdAndLicense(string openId, string licenseno)
       {
           return _userInfoRepository.FindByOpenIdAndLicense(openId, licenseno);
       }

       public long Add(bx_userinfo userinfo)
       {
           return _userInfoRepository.Add(userinfo);
       }

       /// <summary>
       /// 更新用户信息
       /// </summary>
       /// <param name="userinfo"></param>
       /// <returns></returns>
       public int Update(bx_userinfo userinfo)
       {
           return _userInfoRepository.Update(userinfo);
       }


       public user FindUserByOpenId(string openid)
       {
           if (string.IsNullOrEmpty(openid))
           {
               return null;
           }
           //根据openid，mobile，获取userid
           var user = _userRepository.FindByOpenId(openid);

           return user;
       }

        public user AddUser(string openid, string mobile)
        {
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(mobile))
            {
                return null;
            }
            user user = null;
            lock (obj)
            {
                //根据openid，mobile，获取userid
                user = _userRepository.FindByOpenId(openid);
                //不存在openid用户
                if (user == null || user.UserId <= 0)
                {
                    user = _userRepository.FindByMobile(mobile);
                    //openid,手机不存在用户，实现注册
                    if (user == null || user.UserId <= 0)
                    {
                        user = new user()
                        {
                            Openid = openid,
                            Mobile = mobile,
                            CreateTime = DateTime.Now,
                            RegisterType = 12
                        };
                        user.UserId = _userRepository.Add(user);
                        logInfo.Info("AddUser新增用户记录:" + user.ToJson());
                    }
                    //else
                    //{
                    //    user.Openid = openid;
                    //    _userRepository.Update(user);
                    //}
                }
            }
            return user;
        }
    }
}
