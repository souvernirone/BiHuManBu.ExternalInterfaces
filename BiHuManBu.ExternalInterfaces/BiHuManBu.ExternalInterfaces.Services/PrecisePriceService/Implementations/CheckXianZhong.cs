

using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations
{
    public class CheckXianZhong : ICheckXianZhong
    {
        public CheckXianZhong() { }
        /// <summary>
        /// 险种校验
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string CheckRequestXianZhong(PostPrecisePriceRequest request)
        {
            string msg = string.Empty;
            if (request.QuoteGroup > 0)
            {
                if (request.QuoteGroup < request.SubmitGroup)
                {
                    msg = "参数SubmitGroup不能大于QuoteGroup";
                    return msg;
                }
                //if ((request.QuoteGroup & request.SubmitGroup)!= request.SubmitGroup)
                //{
                //    msg = "请核对SubmitGroup参数是否是QuoteGroup中的资源组合";
                //    return msg;
                //}
            }

            if (request.CheSun == 0 && request.BuJiMianCheSun == 1)
            {
                msg = "不选择车辆损失险,不能选择不计免车损";
                return msg;
            }
            if (request.SanZhe == 0 && request.BuJiMianSanZhe > 0)
            {
                msg = "不选择第三者责任险,不能选择不计免三者";
                return msg;
            }
            //if ((request.SiJi == 0 && request.ChengKe == 0) && request.BuJiMianRenYuan > 0)
            //{
            //    msg = "不选择司机座位险或者乘客座位险,不能选择不计免人员险";
            //    return msg;
            //}
            if (request.DaoQiang == 0 && request.BuJiMianDaoQiang > 0)
            {
                msg = "不选择盗抢险,不能选择不计免盗抢";
                return msg;
            }
            //2.1.5版本修改
            if (request.ChengKe == 0 && request.BuJiMianChengKe == 1)
            {
                msg = "不选择乘客座位险,不能选择不计免乘客";
                return msg;
            }
            if (request.SiJi == 0 && request.BuJiMianSiJi == 1)
            {
                msg = "不选择司机座位险,不能选择不计免司机";
                return msg;
            }
            if (request.HuaHen == 0 && request.BuJiMianHuaHen == 1)
            {
                msg = "不选择划痕险,不能选择不计免划痕";
                return msg;
            }
            if (request.SheShui == 0 && request.BuJiMianSheShui == 1)
            {
                msg = "不选择涉水险,不能选择不计免涉水";
                return msg;
            }
            if (request.ZiRan == 0 && request.BuJiMianZiRan == 1)
            {
                msg = "不选择自燃险,不能选择不计免自燃";
                return msg;
            }
            if (request.HcJingShenSunShi == 1 && request.SiJi <= 0 && request.ChengKe <= 0 && request.SanZhe <= 0)
            {
                msg = "投保精神损失险，三者、司机、乘客至少有一个要投保";
                return msg;
            }
            if (request.HcJingShenSunShi == 0 && request.BuJiMianJingShenSunShi == 1)
            {
                msg = "不选择精神损失险,不能选择不计免精神损失";
                return msg;
            }
            if (request.ForceTax == 0 && string.IsNullOrWhiteSpace(request.BizStartDate) && string.IsNullOrWhiteSpace(request.BizTimeStamp))
            {
                msg = "单商业，商业险开始日期BizStartDate不能为空";
                return msg;
            }
            if (request.SheBeiSunshi == 0 && request.BjmSheBeiSunshi == 1)
            {
                msg = "不选择新增设备损失险,不能选择不计免新增设备损失险";
                return msg;
            }
            //单交强
            if (request.ForceTax == 2)
            {
                if (request.CheSun > 0 || request.BuJiMianCheSun > 0 || request.DaoQiang > 0 || request.BuJiMianDaoQiang > 0 || request.BuJiMianSanZhe > 0 || request.BuJiMianSanZhe > 0 ||
                    request.SiJi > 0 || request.ChengKe > 0 ||
                        request.HuaHen > 0 || request.BoLi > 0 || request.ZiRan > 0 || request.SheShui > 0 || request.SanZhe > 0
                      )
                {
                    msg = "单交强，商业险不能选择险种";
                    return msg;
                }
            }
            //险种选择
            #region 划痕
            var huahenlist = new List<double>
            {
                2000,
                5000,
                10000,
                20000
            };
            if (request.HuaHen > 0)
            {
                if (!huahenlist.Any(x => x == request.HuaHen))
                {
                    msg = "划痕险请选择正确的保额";
                    return msg;
                }
            }
            #endregion
            #region 司机&乘客
            var sijilist = new List<double>
            {
                10000,
                20000,
                30000,
                40000,
                50000,
                100000,
                200000,
                250000,//国寿财用
                300000
            };
            if (request.SiJi > 0)
            {
                //if (!sijilist.Any(x => x == request.SiJi))
                //{
                //    msg = "车上人员责任险（司机）请选择正确的保额";
                //    return msg; 
                //}
                if (request.SiJi < 1000 || request.SiJi > 1000000)
                {
                    msg = "车上人员责任险（司机）请选择正确的保额";
                    return msg;
                }
            }
            if (request.ChengKe > 0)
            {
                //if (!sijilist.Any(x => x == request.ChengKe))
                //{
                //    msg = "车上人员责任险（乘客）请选择正确的保额";
                //    return msg;
                //}
                if (request.ChengKe < 1000 || request.ChengKe > 1000000)
                {
                    msg = "车上人员责任险（乘客）请选择正确的保额";
                    return msg;
                }
            }
            #endregion
            #region 三者
            if (request.SanZhe > 0)
            {
                var sanzelist = new List<double>
                {
                    50000,
                    100000,
                    150000,
                    200000,
                    300000,
                    500000,
                    1000000,
                    1500000,
                    2000000,//国寿财用
                    2500000,
                    3000000,
                    5000000,
                    8000000,
                    10000000,
                };
                if (request.SanZhe < 1000 || request.SanZhe > 10000000)
                {
                    msg = "三者险请选择正确的保额";
                    return msg;
                }
            }
            #endregion
            #region 修理厂险判断
            //人保以外的其他保司，国产的范围是0.1-0.3，进口的范围是0.15-0.6
            if (request.HcXiuLiChang > 0 && request.QuoteGroup != 4)
            {
                if (request.HcXiuLiChangType == 0)
                {
                    if (!string.IsNullOrWhiteSpace(request.CarVin) && !request.CarVin.StartsWith("L"))
                    {
                        //车架号国产是L开头
                        return "如果车架号非L开头，指定修理厂类型须为进口";                        
                    }
                    if (request.HcXiuLiChang > 0.3 || request.HcXiuLiChang < 0.1)
                    {
                        return "如果指定修理厂类型为国产，修理厂险的范围是0.1-0.3";
                    }
                }
                else if (request.HcXiuLiChangType == 1)
                {
                    if (!string.IsNullOrWhiteSpace(request.CarVin) && request.CarVin.StartsWith("L"))
                    {
                        //车架号进口不是L开头
			return "如果车架号L开头，指定修理厂类型须为国产";
                    }
                    if (request.HcXiuLiChang > 0.6 || request.HcXiuLiChang < 0.15)
                    {
                        return "如果指定修理厂类型为进口，修理厂险的范围是0.15-0.6";
                    }
                }
                else
                {
                    request.HcXiuLiChangType = -1;
                    request.HcXiuLiChang = 0;
                }
            }
            #endregion
            #region 车损和不计免规则
            if (request.HuaHen > 0 || request.BoLi > 0 || request.ZiRan > 0 || request.SheShui > 0 || request.HcSanFangTeYue == 1 || request.HcXiuLiChang > 0 || request.Fybc > 0 || request.SheBeiSunshi > 0)
            {
                if (request.CheSun == 0)
                {
                    return "选择了划痕、玻璃、自燃、涉水、三方特约、指定修理厂、费用补偿、设备损失等附加险，必须选择车辆损失险";
                }
            }
            #endregion
            #region 如果上精神损失、车上货物责任险（费改后暂不支持）、法定节假日责任限额翻倍，必须上三者
            if (request.HcJingShenSunShi > 0 || request.HcHuoWuZeRen > 0 || request.SanZheJieJiaRi > 0)
                if (request.SanZhe == 0)
                {
                    return "选择了精神损失险、车上货物责任险、法定节假日责任限额翻倍等附加险，必须选择三者险";
                }
            #endregion
            return msg;
        }
    }
}
