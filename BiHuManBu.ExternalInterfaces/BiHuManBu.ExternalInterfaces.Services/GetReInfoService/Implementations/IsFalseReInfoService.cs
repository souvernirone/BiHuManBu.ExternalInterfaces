using BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces;
using log4net;
using System;
using System.Configuration;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Implementations
{
    public class IsFalseReInfoService: IIsFalseReInfoService
    {
        private static readonly string _reInfoChance = ConfigurationManager.AppSettings["ReInfoChance"];
        private readonly ILog logError = LogManager.GetLogger("ERROR");
        public IsFalseReInfoService() { }

        public bool IsFalseReInfo(int topAgentId)
        {
            //顶级是广州人财保
            int intrandom = new Random().Next(0, 99);
            logError.Info("随机数：" + intrandom);
            return intrandom > int.Parse(_reInfoChance);//如果随机数在后台设定范围内，返回true
        }
    }
}
