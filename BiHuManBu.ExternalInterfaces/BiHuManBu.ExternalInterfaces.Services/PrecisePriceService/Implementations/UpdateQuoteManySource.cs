using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Models.IRepository;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Interfaces;
using ServiceStack.Text;

namespace BiHuManBu.ExternalInterfaces.Services.PrecisePriceService.Implementations
{
    public class UpdateQuoteManySource : IUpdateQuoteManySource
    {
        private readonly IQuoteManySourceRepository _quoteManySourceRepository;

        public UpdateQuoteManySource(IQuoteManySourceRepository quoteManySourceRepository)
        {
            _quoteManySourceRepository = quoteManySourceRepository;
        }
        public void UpdateQuoteManySourceMethod(int childAgent, int topAgent, string multiChannels, List<MultiChannels> models)
        {
            if (!string.IsNullOrWhiteSpace(multiChannels))
            {
                models = multiChannels.FromJson<List<MultiChannels>>();
            }
            if (models.Any())
            {
                //取出已存bx_quote_many_source的记录
                var sources = models.Select(l => l.Source).ToList();
                string strSources = string.Join(",", sources);
                List<bx_quote_many_source> list = _quoteManySourceRepository.GetModels(childAgent, strSources);
                //开始循环传进来的报价渠道模型，进行更新
                foreach (var item in models)
                {
                    int oldSource = (int)item.Source;
                    //根据source和childagent查对应记录，有就更新，没有就插
                    bx_quote_many_source model = list.Where(l => l.child_agent == childAgent && l.source == oldSource).FirstOrDefault();
                    if (model == null)
                    {
                        model = new bx_quote_many_source();
                        //插入
                        model.channel_id = item.ChannelId;
                        model.source = oldSource;
                        model.child_agent = childAgent;
                        model.top_agent = topAgent;
                        _quoteManySourceRepository.Add(model);
                    }
                    else if (model.channel_id != item.ChannelId)
                    {
                        //执行更新
                        model.channel_id = item.ChannelId;
                        _quoteManySourceRepository.Update(model);
                    }
                }
            }
        }

    }
}
