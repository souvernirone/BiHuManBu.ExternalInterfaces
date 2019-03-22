using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.Messages.Response
{
    public class AXGainFixSpecInfoResponse
    {
        /// <summary>
        /// 交强险信息
        /// </summary>
        public PackageJQVO PackageJQVO { get; set; }

        /// <summary>
        /// 商业险信息
        /// </summary>
        public PackageSYVO PackageSYVO { get; set; }
    }

    /// <summary>
    /// 交强险信息 , 用于特别约定接口
    /// </summary>
    public class PackageJQVO
    {
        /// <summary>
        /// 申请单号 ,必传 
        /// </summary>
        public string CAppNo { get; set; }


        /// <summary>
        /// 产品代码 (030001  交强险 033011  商业险)
        /// </summary>
        public string CProdNo { get; set; }

        /// <summary>
        /// 失败原因
        /// </summary>
        public string cerrRes { get; set; }

        /// <summary>
        /// 是否成功    1-是 0否
        /// </summary>
        public string flag { get; set; }

        /// <summary>
        /// 特别约定信息 
        /// </summary>
        public List<FixSpecVO> FixSpecList { get; set; }

    }
    /// <summary>
    ///  商业险信息 , 用于特别约定接口
    /// </summary>
    public class PackageSYVO
    {
        /// <summary>
        /// 申请单号 ,必传 
        /// </summary>
        public string CAppNo { get; set; }

        /// <summary>
        /// 产品代码 (030001  交强险 033011  商业险)
        /// </summary>
        public string CProdNo { get; set; }


        /// <summary>
        /// 失败原因
        /// </summary>
        public string cerrRes { get; set; }


        /// <summary>
        /// 是否成功    1-是 0否
        /// </summary>
        public string flag { get; set; }

        /// <summary>
        /// 特别约定信息 
        /// </summary>
        public List<FixSpecVO> FixSpecList { get; set; }
    }

    /// <summary>
    /// 特别约定信息
    /// </summary>
    public class FixSpecVO
    {
        /// <summary>
        /// 特别约定代码 
        /// </summary>
        public string cSpecNo { get; set; }

        /// <summary>
        /// 特别约定内容 
        /// </summary>
        public string cSysSpecContent { get; set; }
    }
}
