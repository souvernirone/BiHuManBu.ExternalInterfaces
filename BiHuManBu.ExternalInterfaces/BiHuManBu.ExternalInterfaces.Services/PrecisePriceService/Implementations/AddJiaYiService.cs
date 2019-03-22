using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;
using log4net;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations
{
    public class AddJiaYiService : IAddJiaYiService
    {
        private readonly IYwxdetailRepository _ywxdetailRepository;
        private ILog logError = LogManager.GetLogger("ERROR");
        public AddJiaYiService(IYwxdetailRepository ywxdetailRepository)
        {
            _ywxdetailRepository = ywxdetailRepository;
        }

        public void AddJiaYi(long buid, string jiayi)
        {
            try
            {
                //执行删除操作
                _ywxdetailRepository.DelList(buid);
                if (string.IsNullOrWhiteSpace(jiayi))
                {
                    return;
                }
                //执行插入操作
                List<bx_ywxdetail> ywxlist = new List<bx_ywxdetail>();
                List<JiaYiModel> requestList = jiayi.FromJson<List<JiaYiModel>>();
                if (requestList != null && requestList.Any())
                {
                    foreach (var item in requestList)
                    {
                        ywxlist.Add(new bx_ywxdetail()
                        {
                            b_uid = buid,
                            source = SourceGroupAlgorithm.GetOldSource(item.Source),
                            amount = item.Amount,
                            count = item.Count,
                            brandcode = item.BrandCode,
                            brandname = item.BrandName,
                            classid = item.ClassId,
                            classname = item.ClassName,
                            code = item.Code,
                            name = item.Name,
                            createtime = DateTime.Now,
                            updatetime = DateTime.Now
                        });
                    }
                    _ywxdetailRepository.AddList(ywxlist);
                }
            }
            catch (Exception ex)
            {
                logError.Info("发生异常:" + ex.Source + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + ex.InnerException);
            }
        }
    }
}
