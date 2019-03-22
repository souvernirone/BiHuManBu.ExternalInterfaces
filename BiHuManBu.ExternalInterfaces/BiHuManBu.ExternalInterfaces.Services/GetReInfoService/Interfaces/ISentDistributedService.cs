using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.GetReInfoService.Interfaces
{
    public interface ISentDistributedService
    {
        /// <summary>
        /// 发送分配请求
        /// 20180820.by.gpj修改.调用刘松年新版分配，不用userinfo的moldname了
        /// </summary>
        /// <param name="businessStatus"></param>
        /// <param name="moldName"></param>
        /// <param name="buid"></param>
        /// <param name="reqAgent"></param>
        /// <param name="reqChildAgent">此处无须担心request.ChildAgent=0，在方法里会相关的判断操作</param>
        /// <param name="uiAgent"></param>
        /// <param name="reqCityCode"></param>
        /// <param name="reqLicenseNo"></param>
        /// <param name="reqRenewalType"></param>
        /// <param name="uiRenewalType"></param>
        /// <param name="businessExpireDate"></param>
        /// <param name="forceExpireDate"></param>
        /// <param name="needCarMoldFilter"></param>
        /// <param name="cameraAgent"></param>
        /// <param name="cameraId"></param>
        /// <param name="isAdd"></param>
        /// <param name="repeatStatus"></param>
        /// <param name="roleType"></param>
        /// <param name="custKey"></param>
        /// <param name="cityCode"></param>
        void SentDistributed(int businessStatus, string moldName, long buid, int reqAgent, int reqChildAgent, string uiAgent, int reqCityCode, string reqLicenseNo, int reqRenewalType, int uiRenewalType, string businessExpireDate, string forceExpireDate, bool needCarMoldFilter, int cameraAgent, string cameraId, bool isAdd, int repeatStatus, int roleType, string custKey, int cityCode);
    }
}
