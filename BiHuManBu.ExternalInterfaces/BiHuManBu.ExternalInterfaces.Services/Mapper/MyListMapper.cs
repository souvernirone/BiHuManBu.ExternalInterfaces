using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public class MyListMapper
    {
        public static PrecisePriceInfo ConvertToPrecisePriceInfo(int source, bx_quoteresult quoteresult, bx_submit_info submitInfo)
        {
            var myInfo = new PrecisePriceInfo();
            if (quoteresult != null)
            {
                myInfo.BizTotal = quoteresult.BizTotal.HasValue ? quoteresult.BizTotal.Value : 0;
                double force = quoteresult.ForceTotal.HasValue ? quoteresult.ForceTotal.Value : 0;
                double tax = quoteresult.TaxTotal.HasValue ? quoteresult.TaxTotal.Value : 0;
                myInfo.ForceTotal = force + tax;
            }
            if (submitInfo != null)
            {
                myInfo.QuoteStatus = submitInfo.quote_status.HasValue ? submitInfo.quote_status.Value : 0;
                if (submitInfo.submit_status.HasValue)
                {
                    myInfo.SubmitStatus = submitInfo.submit_status.Value;
                    switch (myInfo.SubmitStatus)
                    {
                        case 0:
                            myInfo.SubmitResult = "核保失败";
                            break;
                        case 1:
                            myInfo.SubmitResult = "核保成功";
                            break;
                        case 2:
                            myInfo.SubmitResult = "未到期未核保";
                            break;
                        case 3:
                            myInfo.SubmitResult = "人工审核中";
                            break;
                        case 4:
                            myInfo.SubmitResult = "非意向未核保";
                            break;
                        case 5:
                            myInfo.SubmitResult = "报价失败未核保";
                            break;
                        default:
                            myInfo.SubmitResult = "";
                            break;
                    }
                }
            }
            myInfo.Source = SourceGroupAlgorithm.GetNewSource(source);
            return myInfo;
        }
    }
}
