using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations
{
    public class CheckShebei : ICheckShebei
    {
        private readonly IGetMonth _getMonth;
        public CheckShebei(IGetMonth getMonth) {
            _getMonth = getMonth;
        }
        public string CheckRequestShebei(PostPrecisePriceRequest request)
        {
            var isallempty = true;
            var registerDate = Convert.ToDateTime(request.RegisterDate);
            var startDte = DateTime.Now;
            var paChannel = (request.QuoteGroup & 2) == 2;
            if (!string.IsNullOrWhiteSpace(request.BizStartDate))
            {
                startDte = Convert.ToDateTime(request.BizStartDate);
            }
            if (!string.IsNullOrWhiteSpace(request.BizTimeStamp))
            {
                startDte = request.BizTimeStamp.UnixTimeToDateTime();
            }
            string msg = string.Empty;
            if (!paChannel)
            {
                if (string.IsNullOrWhiteSpace(request.DN1) && string.IsNullOrWhiteSpace(request.PD1) && request.DA1 == 0 &&
                    request.DD1 == 0 && request.DT1 == 0 && request.DQ1 == 0)
                {
                }
                else
                {
                    isallempty = false;
                    if (string.IsNullOrWhiteSpace(request.DN1) || string.IsNullOrWhiteSpace(request.PD1) ||
                        request.DA1 == 0 || request.DQ1 == 0)
                    {
                        return "设备1参数都是必填项目";
                    }
                    else
                    {
                        //折旧
                        DateTime dt1 = Convert.ToDateTime(request.PD1);
                        if (dt1 > DateTime.Now || dt1 < registerDate)
                        {
                            return "设备1购买日期不对，应该在初登日期和今天之间的日期范围";
                        }
                        var datePeriod = _getMonth.GetMonthValue(dt1, startDte);
                        var acturalPrice = request.DA1 / request.DQ1 - datePeriod * 6;
                        //if (acturalPrice < 0 || acturalPrice != (request.DD1 / request.DQ1))
                        //{
                        //    return "设备1的DD1金额不对，应该是" + acturalPrice * request.DQ1;
                        //}
                    }
                }
                if (string.IsNullOrWhiteSpace(request.DN2) && string.IsNullOrWhiteSpace(request.PD2) && request.DA2 == 0 &&
                    request.DD2 == 0 && request.DT2 == 0 && request.DQ2 == 0)
                {
                }
                else
                {
                    isallempty = false;
                    if (string.IsNullOrWhiteSpace(request.DN2) || string.IsNullOrWhiteSpace(request.PD2) ||
                        request.DA2 == 0 || request.DQ2 == 0)
                    {
                        return "设备2参数都是必填项目";
                    }
                    else
                    {
                        //折旧
                        DateTime dt1 = Convert.ToDateTime(request.PD2);
                        if (dt1 > DateTime.Now || dt1 < registerDate)
                        {
                            return "设备2购买日期不对，应该在初登日期和今天之间的日期范围";
                        }
                        var datePeriod = _getMonth.GetMonthValue(dt1, startDte);
                        var acturalPrice = request.DA2 / request.DQ2 - datePeriod * 6;
                        //if (acturalPrice < 0 || acturalPrice != (request.DD2 / request.DQ2))
                        //{
                        //    return "设备2的DD2金额不对，应该是" + acturalPrice * request.DQ2;
                        //}
                    }
                }
                if (string.IsNullOrWhiteSpace(request.DN3) && string.IsNullOrWhiteSpace(request.PD3) && request.DA3 == 0 &&
                    request.DD3 == 0 && request.DT3 == 0 && request.DQ3 == 0)
                {
                }
                else
                {
                    isallempty = false;
                    if (string.IsNullOrWhiteSpace(request.DN3) || string.IsNullOrWhiteSpace(request.PD3) ||
                        request.DA3 == 0 || request.DQ3 == 0)
                    {
                        return "设备3参数都是必填项目";
                    }
                    else
                    {
                        //折旧
                        DateTime dt1 = Convert.ToDateTime(request.PD3);
                        if (dt1 > DateTime.Now || dt1 < registerDate)
                        {
                            return "设备3购买日期不对，应该在初登日期和今天之间的日期范围";
                        }
                        var datePeriod = _getMonth.GetMonthValue(dt1, startDte);
                        var acturalPrice = request.DA3 / request.DQ3 - datePeriod * 6;
                        //if (acturalPrice < 0 || acturalPrice != (request.DD3 / request.DQ3))
                        //{
                        //    return "设备3的DD3金额不对，应该是" + acturalPrice * request.DQ3;
                        //}
                    }
                }
                if (string.IsNullOrWhiteSpace(request.DN4) && string.IsNullOrWhiteSpace(request.PD4) && request.DA4 == 0 &&
                    request.DD4 == 0 && request.DT4 == 0 && request.DQ4 == 0)
                {
                }
                else
                {
                    isallempty = false;
                    if (string.IsNullOrWhiteSpace(request.DN4) || string.IsNullOrWhiteSpace(request.PD4) ||
                        request.DA4 == 0 || request.DQ4 == 0)
                    {
                        return "设备4参数都是必填项目";
                    }
                    else
                    {
                        //折旧
                        DateTime dt1 = Convert.ToDateTime(request.PD4);
                        if (dt1 > DateTime.Now || dt1 < registerDate)
                        {
                            return "设备4购买日期不对，应该在初登日期和今天之间的日期范围";
                        }
                        var datePeriod = _getMonth.GetMonthValue(dt1, startDte);
                        var acturalPrice = request.DA4 / request.DQ4 - datePeriod * 6;
                        //if (acturalPrice < 0 || acturalPrice != (request.DD4/request.DQ4))
                        //{
                        //    return "设备4的DD4金额不对，应该是" + acturalPrice*request.DQ4;
                        //}
                    }
                }
                if (string.IsNullOrWhiteSpace(request.DN5) && string.IsNullOrWhiteSpace(request.PD5) && request.DA5 == 0 &&
                    request.DD5 == 0 && request.DT5 == 0 && request.DQ5 == 0)
                {
                }
                else
                {
                    isallempty = false;
                    if (string.IsNullOrWhiteSpace(request.DN5) || string.IsNullOrWhiteSpace(request.PD5) ||
                        request.DA5 == 0 || request.DQ5 == 0)
                    {
                        return "设备5参数都是必填项目";
                    }
                    else
                    {
                        //折旧
                        DateTime dt1 = Convert.ToDateTime(request.PD5);
                        if (dt1 > DateTime.Now || dt1 < registerDate)
                        {
                            return "设备5购买日期不对，应该在初登日期和今天之间的日期范围";
                        }
                        var datePeriod = _getMonth.GetMonthValue(dt1, startDte);
                        var acturalPrice = request.DA5 / request.DQ5 - datePeriod * 6;
                        //if (acturalPrice < 0 || acturalPrice != (request.DD5/request.DQ5))
                        //{
                        //    return "设备5的DD5金额不对，应该是" + acturalPrice*request.DQ5;
                        //}
                    }
                }
                if (string.IsNullOrWhiteSpace(request.DN6) && string.IsNullOrWhiteSpace(request.PD6) && request.DA6 == 0 &&
                    request.DD6 == 0 && request.DT6 == 0 && request.DQ6 == 0)
                {
                }
                else
                {
                    isallempty = false;
                    if (string.IsNullOrWhiteSpace(request.DN6) || string.IsNullOrWhiteSpace(request.PD6) ||
                        request.DA6 == 0 || request.DQ6 == 0)
                    {
                        return "设备6参数都是必填项目";
                    }
                    else
                    {
                        //折旧
                        DateTime dt1 = Convert.ToDateTime(request.PD6);
                        if (dt1 > DateTime.Now || dt1 < registerDate)
                        {
                            return "设备6购买日期不对，应该在初登日期和今天之间的日期范围";
                        }
                        var datePeriod = _getMonth.GetMonthValue(dt1, startDte);
                        var acturalPrice = request.DA6 / request.DQ6 - datePeriod * 6;
                        //if (acturalPrice < 0 || acturalPrice != (request.DD6 / request.DQ6))
                        //{
                        //    return "设备6的DD6金额不对，应该是" + acturalPrice * request.DQ6;
                        //}
                    }
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(request.DN1) && request.DA1 == 0 && request.DD1 == 0 && request.DT1 == 0 && request.DQ1 == 0)
                {
                }
                else
                {
                    isallempty = false;
                    if (string.IsNullOrWhiteSpace(request.DN1) || request.DA1 == 0 || request.DQ1 == 0)
                    {
                        return "设备1的名称，购买金额，数量参数都是必填项目";
                    }
                    else
                    {
                        ////折旧
                        //DateTime dt1 = Convert.ToDateTime(request.PD1);
                        //if (dt1 > DateTime.Now || dt1 < registerDate)
                        //{
                        //    return "设备1购买日期不对，应该在初登日期和今天之间的日期范围";
                        //}
                        //var datePeriod = GetMonth(dt1, startDte);
                        //var acturalPrice = request.DA1 / request.DQ1 - datePeriod * 6;
                        //if (acturalPrice < 0 || acturalPrice != (request.DD1 / request.DQ1))
                        //{
                        //    return "设备1的DD1金额不对，应该是" + acturalPrice * request.DQ1;
                        //}
                    }
                }
                if (string.IsNullOrWhiteSpace(request.DN2) && request.DA2 == 0 && request.DD2 == 0 && request.DT2 == 0 && request.DQ2 == 0)
                {
                }
                else
                {
                    isallempty = false;
                    if (string.IsNullOrWhiteSpace(request.DN2) || request.DA2 == 0 || request.DQ2 == 0)
                    {
                        return "设备2名称，购买金额，数量参数都是必填项目";
                    }
                    else
                    {
                        //折旧
                        //DateTime dt1 = Convert.ToDateTime(request.PD2);
                        //if (dt1 > DateTime.Now || dt1 < registerDate)
                        //{
                        //    return "设备2购买日期不对，应该在初登日期和今天之间的日期范围";
                        //}
                        //var datePeriod = GetMonth(dt1, startDte);
                        //var acturalPrice = request.DA2 / request.DQ2 - datePeriod * 6;
                        //if (acturalPrice < 0 || acturalPrice != (request.DD2 / request.DQ2))
                        //{
                        //    return "设备2的DD2金额不对，应该是" + acturalPrice * request.DQ2;
                        //}
                    }
                }
                if (string.IsNullOrWhiteSpace(request.DN3) && request.DA3 == 0 && request.DD3 == 0 && request.DT3 == 0 && request.DQ3 == 0)
                {
                }
                else
                {
                    isallempty = false;
                    if (string.IsNullOrWhiteSpace(request.DN3) || request.DA3 == 0 || request.DQ3 == 0)
                    {
                        return "设备3名称，购买金额，数量参数都是必填项目";
                    }
                    else
                    {
                        ////折旧
                        //DateTime dt1 = Convert.ToDateTime(request.PD3);
                        //if (dt1 > DateTime.Now || dt1 < registerDate)
                        //{
                        //    return "设备3购买日期不对，应该在初登日期和今天之间的日期范围";
                        //}
                        //var datePeriod = GetMonth(dt1, startDte);
                        //var acturalPrice = request.DA3 / request.DQ3 - datePeriod * 6;
                        //if (acturalPrice < 0 || acturalPrice != (request.DD3 / request.DQ3))
                        //{
                        //    return "设备3的DD3金额不对，应该是" + acturalPrice * request.DQ3;
                        //}
                    }
                }
                if (string.IsNullOrWhiteSpace(request.DN4) && request.DA4 == 0 && request.DD4 == 0 && request.DT4 == 0 && request.DQ4 == 0)
                {
                }
                else
                {
                    isallempty = false;
                    if (string.IsNullOrWhiteSpace(request.DN4) || request.DA4 == 0 || request.DQ4 == 0)
                    {
                        return "设备4名称，购买金额，数量参数都是必填项目";
                    }
                    else
                    {
                        ////折旧
                        //DateTime dt1 = Convert.ToDateTime(request.PD4);
                        //if (dt1 > DateTime.Now || dt1 < registerDate)
                        //{
                        //    return "设备4购买日期不对，应该在初登日期和今天之间的日期范围";
                        //}
                        //var datePeriod = GetMonth(dt1, startDte);
                        //var acturalPrice = request.DA4 / request.DQ4 - datePeriod * 6;
                        //if (acturalPrice < 0 || acturalPrice != (request.DD4/request.DQ4))
                        //{
                        //    return "设备4的DD4金额不对，应该是" + acturalPrice*request.DQ4;
                        //}
                    }
                }
                if (string.IsNullOrWhiteSpace(request.DN5) && request.DA5 == 0 && request.DD5 == 0 && request.DT5 == 0 && request.DQ5 == 0)
                {
                }
                else
                {
                    isallempty = false;
                    if (string.IsNullOrWhiteSpace(request.DN5) || request.DA5 == 0 || request.DQ5 == 0)
                    {
                        return "设备5名称，购买金额，数量参数都是必填项目";
                    }
                    else
                    {
                        ////折旧
                        //DateTime dt1 = Convert.ToDateTime(request.PD5);
                        //if (dt1 > DateTime.Now || dt1 < registerDate)
                        //{
                        //    return "设备5购买日期不对，应该在初登日期和今天之间的日期范围";
                        //}
                        //var datePeriod = GetMonth(dt1, startDte);
                        //var acturalPrice = request.DA5 / request.DQ5 - datePeriod * 6;
                        //if (acturalPrice < 0 || acturalPrice != (request.DD5/request.DQ5))
                        //{
                        //    return "设备5的DD5金额不对，应该是" + acturalPrice*request.DQ5;
                        //}
                    }
                }
                if (string.IsNullOrWhiteSpace(request.DN6) && request.DA6 == 0 && request.DD6 == 0 && request.DT6 == 0 && request.DQ6 == 0)
                {
                }
                else
                {
                    isallempty = false;
                    if (string.IsNullOrWhiteSpace(request.DN6) || request.DA6 == 0 || request.DQ6 == 0)
                    {
                        return "设备6名称，购买金额，数量参数都是必填项目";
                    }
                    else
                    {
                        ////折旧
                        //DateTime dt1 = Convert.ToDateTime(request.PD6);
                        //if (dt1 > DateTime.Now || dt1 < registerDate)
                        //{
                        //    return "设备6购买日期不对，应该在初登日期和今天之间的日期范围";
                        //}
                        //var datePeriod = GetMonth(dt1, startDte);
                        //var acturalPrice = request.DA6 / request.DQ6 - datePeriod * 6;
                        //if (acturalPrice < 0 || acturalPrice != (request.DD6 / request.DQ6))
                        //{
                        //    return "设备6的DD6金额不对，应该是" + acturalPrice * request.DQ6;
                        //}
                    }
                }
            }

            if (isallempty)
            {
                return "选择了投保新增设备险，必须要添加一类设备";
            }
            return string.Empty;
        }
    }
}
