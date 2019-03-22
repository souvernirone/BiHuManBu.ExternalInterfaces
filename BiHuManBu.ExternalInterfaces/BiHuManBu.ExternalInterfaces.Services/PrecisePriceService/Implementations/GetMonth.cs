using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations
{
    public class GetMonth: IGetMonth
    {
        public GetMonth() { }
        public int GetMonthValue(DateTime dtbegin, DateTime dtend)
        {
            int Month = 0;
            //  DateTime dtbegin = Convert.ToDateTime(txtworkday.Text.ToString()); //起始时间  
            //  DateTime dtend = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));  //结束时间  
            if ((dtend.Year - dtbegin.Year) == 0)
            {
                if (dtend.Day - dtbegin.Day > 0)
                {
                    Month = dtend.Month - dtbegin.Month;
                }
                else
                {
                    Month = dtend.Month - dtbegin.Month - 1;
                    if (Month < 0)
                    {
                        Month = 0;
                    }
                }
            }
            if ((dtend.Year - dtbegin.Year) >= 1)
            {
                if (dtend.Month - dtbegin.Month < 0)
                {
                    if (dtend.Day - dtbegin.Day > 0)
                    {
                        Month = (dtend.Year - dtbegin.Year) * 12 + dtend.Month - dtbegin.Month;
                    }
                    else
                    {
                        Month = (dtend.Year - dtbegin.Year) * 12 + dtend.Month - dtbegin.Month - 1;
                    }
                }
                else if ((dtend.Month - dtbegin.Month == 0) && (dtend.Year - dtbegin.Year >= 1))
                {
                    if (dtend.Day - dtbegin.Day > 0)
                    {
                        Month = (dtend.Year - dtbegin.Year) * 12;
                    }
                    else
                    {
                        Month = (dtend.Year - dtbegin.Year) * 12 - 1;
                    }
                }
                else if ((dtend.Month - dtbegin.Month > 0) && (dtend.Year - dtbegin.Year >= 1))
                {
                    if (dtend.Day - dtbegin.Day > 0)
                    {
                        Month = (dtend.Year - dtbegin.Year) * 12 + dtend.Month - dtbegin.Month;
                    }
                    else
                    {
                        Month = (dtend.Year - dtbegin.Year) * 12 + dtend.Month - dtbegin.Month - 1;
                    }
                }
                else
                {
                    if (dtend.Day - dtbegin.Day > 0)
                    {
                        Month = (dtend.Year - dtbegin.Year) * 12 + dtend.Month - dtbegin.Month + 1;
                    }
                    else
                    {
                        Month = (dtend.Year - dtbegin.Year) * 12 + dtend.Month - dtbegin.Month + 1 - 1;
                    }
                }
            }
            return Month;
        }
    }
}
