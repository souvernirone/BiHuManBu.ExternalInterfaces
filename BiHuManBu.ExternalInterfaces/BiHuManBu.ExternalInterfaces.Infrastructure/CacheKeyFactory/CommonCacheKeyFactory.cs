using System;

namespace BiHuManBu.ExternalInterfaces.Infrastructure.CacheKeyFactory
{
    public  class CommonCacheKeyFactory
    {

        /// <summary>
        /// 续保核保（车牌号和经纪人）生成 key
        /// </summary>
        /// <param name="licenseNo"></param>
        /// <param name="agent"></param>
        /// <param name="custKey"></param>
        /// <returns></returns>
        public static string CreateKeyWithLicenseAndAgentAndCustKey(string licenseNo, int agent,string custKey)
        {
            if (string.IsNullOrWhiteSpace(licenseNo) || agent == 0||string.IsNullOrWhiteSpace(custKey))
                throw new ArgumentNullException();

            var obj = new
            {
                LicenseNo = licenseNo,
                Agent = agent,
                CustKey=custKey
            };

            return MakeCacheKey(obj);
        }





        /// <summary>
        /// 续保核保（车牌号和经纪人）生成 key
        /// </summary>
        /// <param name="licenseNo"></param>
        /// <param name="agent"></param>
        /// <returns></returns>
        public static string CreateKeyWithLicenseAndAgent(string licenseNo, int agent)
        {
            if(string.IsNullOrWhiteSpace(licenseNo)||agent==0)
                throw  new ArgumentNullException();

            var obj= new
            {
                LicenseNo = licenseNo,
                Agent = agent
            };

            return MakeCacheKey(obj);
        }

        /// <summary>
        /// 根据车牌号生成key
        /// </summary>
        /// <param name="licenseNo"></param>
        /// <returns></returns>
        public static string CreateKeyWithLicense(string licenseNo)
        {
            if (string.IsNullOrWhiteSpace(licenseNo))
                throw new ArgumentNullException();
            var obj = new
            {
                LicenseNo = licenseNo
            };
            return MakeCacheKey(obj);
        }


        private static string MakeCacheKey(object obj)
        {
            var keyValues = CommonHelper.EachProperties(obj);
            var keyValuesString = CommonHelper.GetParasString(keyValues);
            var xubaoKey = keyValuesString.StringToHash().Replace("-","");

            return xubaoKey;
        }
    }
}
