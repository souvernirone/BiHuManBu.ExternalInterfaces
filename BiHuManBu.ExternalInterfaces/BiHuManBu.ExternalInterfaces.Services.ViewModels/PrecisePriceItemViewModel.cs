using System;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    /// <summary>
    /// 对外的报价单接口
    /// 备注：由于被继承，如减少其属性，需增加继承对象的属性，除非该属性确实废弃不用了
    /// </summary>
    public class PrecisePriceItemViewModel : XianZhong
    {
        public double BizRate { get; set; }
        public double ForceRate { get; set; }
        public double BizTotal { get; set; }
        public double ForceTotal { get; set; }
        public double TaxTotal { get; set; }
        public long Source { get; set; }
        public int QuoteStatus { get; set; }
        public string QuoteResult { get; set; }
        /// <summary>
        /// 错误码 201803.by.gpj
        /// </summary>
        public string QuoteErrorCode { get; set; }
        /// <summary>
        /// 错误信息 201803.by.gpj
        /// </summary>
        public string QuoteErrorResult { get; set; }
        public string RepeatSubmitResult { get; set; }
        public decimal RateFactor1 { get; set; }
        public decimal RateFactor2 { get; set; }
        public decimal RateFactor3 { get; set; }
        public decimal RateFactor4 { get; set; }
        public string TotalRate { get; set; }
        public List<SheBei> SheBeis { get; set; }
        /// <summary>
        /// 平安评分
        /// </summary>
        public string PingAnScore { get; set; }
        public ChannelInfo Channel { get; set; }
        /// <summary>
        /// 预期赔付率
        /// </summary>
        public string ExpectedLossRate { get; set; }
        /// <summary>
        /// 人太平等系统版本类型
        /// </summary>
        public string VersionType { get; set; }
        /// <summary>
        /// 是否是人保三代系统
        /// </summary>
        public string IsRB3Version { get; set; }
        public ValidateCar ValidateCar { get; set; }
        public string BizCpicScore { get; set; }
        public string TotalCpicScore { get; set; }
        public string TotalEcompensationRate { get; set; }
        /// <summary>
        /// 太平洋实际折扣系数
        /// </summary>
        public string ActualDiscounts { get; set; }

        /// <summary>
        /// 平安底价报价 1是0否
        /// </summary>
        public string IsPaFloorPrice { get; set; }
        public List<JiaYiModel> JiaYi { get; set; }
        public string JiaYiTotal { get; set;}
    }

    /// <summary>
    /// 对内的报价单接口，我的报价单专用
    /// </summary>
    public class MyPrecisePriceItemViewModel : PrecisePriceItemViewModel
    {
        public int SubmitStatus { get; set; }
        public string SubmitResult { get; set; }
        /// <summary>
        /// 0:单商业 ，1：商业+交强车船，2：单交强+车船
        /// </summary>
        public int JiaoQiang { get; set; }
        /// <summary>
        /// 商业险投保单号
        /// </summary>
        public string BizTno { get; set; }
        /// <summary>
        /// 交强险投保单号
        /// </summary>
        public string ForceTno { get; set; }
        /// <summary>
        /// 商业系统费率
        /// </summary>
        public decimal BizSysRate { get; set; }
        /// <summary>
        /// 交强系统费率
        /// </summary>
        public decimal ForceSysRate { get; set; }
        /// <summary>
        /// 优惠费率
        /// </summary>
        public decimal BenefitRate { get; set; }
        /// <summary>
        /// 报价结果增加使用性质
        /// </summary>
        public int CarUsedType { get; set; }
        /// <summary>
        /// 精算口径
        /// </summary>
        public string JingSuanKouJing { get; set; }
        /// <summary>
        /// 安心报价核保流程,支付预定单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// quoteresultcarinfo里面的座位数，详情页下方展示
        /// </summary>
        public int SeatCount { get; set; }
    }

    /// <summary>
    /// 带buid的对内使用的报价单
    /// </summary>
    public class PrecisePriceItemViewModelWithBuid : PrecisePriceItemViewModel
    {
        public Int64 BuId { get; set; }
        /// <summary>
        /// quoteresultcarinfo里面的座位数，详情页下方展示
        /// </summary>
        public int SeatCount { get; set; }
        /// <summary>
        /// 0:单商业 ，1：商业+交强车船，2：单交强+车船
        /// </summary>
        public int JiaoQiang { get; set; }
    }

    public class XianZhong
    {
        public XianZhongUnit SanZheJieJiaRi { get; set; }
        public XianZhongUnit CheSun { get; set; }
        public XianZhongUnit SanZhe { get; set; }
        public XianZhongUnit DaoQiang { get; set; }
        public XianZhongUnit SiJi { get; set; }
        public XianZhongUnit ChengKe { get; set; }
        /// <summary>
        /// 2进口1国产0不投保
        /// </summary>
        public XianZhongUnit BoLi { get; set; }
        public XianZhongUnit HuaHen { get; set; }
        public XianZhongUnit SheShui { get; set; }
        public XianZhongUnit ZiRan { get; set; }
        public XianZhongUnit TeYue { get; set; }
        public XianZhongUnit BuJiMianCheSun { get; set; }
        public XianZhongUnit BuJiMianSanZhe { get; set; }
        public XianZhongUnit BuJiMianDaoQiang { get; set; }
        /// <summary>
        ///保司新系统返回不计免总额 
        /// </summary>
        public XianZhongUnit BuJiMianFuJia { get; set; }
        #region 2.1.5修改 增加6个字段
        public XianZhongUnit BuJiMianChengKe { get; set; }
        public XianZhongUnit BuJiMianSiJi { get; set; }
        public XianZhongUnit BuJiMianHuaHen { get; set; }
        public XianZhongUnit BuJiMianSheShui { get; set; }
        public XianZhongUnit BuJiMianZiRan { get; set; }
        public XianZhongUnit BuJiMianJingShenSunShi { get; set; }
        #endregion
        public XianZhongUnit HcSheBeiSunshi { get; set; }
        public XianZhongUnit HcHuoWuZeRen { get; set; }
        public XianZhongUnit HcFeiYongBuChang { get; set; }
        public XianZhongUnit HcJingShenSunShi { get; set; }
        public XianZhongUnit HcSanFangTeYue { get; set; }
        public XianZhongUnit HcXiuLiChang { get; set; }
        public XianZhongUnit Fybc { get; set; }
        public XianZhongUnit FybcDays { get; set; }
        /// <summary>
        /// 设备损失
        /// </summary>
        public XianZhongUnit SheBeiSunShi { get; set; }
        public XianZhongUnit BjmSheBeiSunShi { get; set; }
        public XianZhongUnit CheDeng { get; set; }
        public XianZhongUnit BuJiMianRenYuan { get; set; }
        /// <summary>
        /// 修理厂：国产、进口
        /// </summary>
        public string HcXiuLiChangType { get; set; }
    }

    /// <summary>
    /// 驾意险模型
    /// </summary>
    public class JiaYiModel
    {
        /// <summary>
        /// 新的source值
        /// </summary>
        public long Source { get; set; }
        /// <summary>
        /// 一级code
        /// </summary>
        public string ClassId { get; set; }
        /// <summary>
        /// 一级name
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 二级code
        /// </summary>
        public string BrandCode { get; set; }
        /// <summary>
        /// 二级name
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// 三级code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 三级name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 单价保费
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 单个险种驾意险总额
        /// </summary>
        public double JyTotal { get; set; }
    }
}