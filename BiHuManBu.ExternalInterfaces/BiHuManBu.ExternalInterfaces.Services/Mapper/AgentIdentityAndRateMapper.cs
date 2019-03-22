using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Services.Messages.Response;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class AgentIdentityAndRateMapper
    {
        public static AgentIdentityAndRateViewModel ConvertToViewModel(this GetAgentIdentityAndRateResponse response)
        {
            var model = new AgentIdentityAndRateViewModel();
            //model.Item = new AgentIdentityAndRateViewModel.AgentIdentityAndRate
            //{
            //    BizRate = response.BizRate,
            //    ForceRate = response.ForceRate,
            //    IsAgent = response.IsAgent,
            //    TaxRate = response.TaxRate
            //};
            model.Item = new AgentIdentityAndRateViewModel.AgentIdentityAndRate();
            model.Item.IsAgent = response.IsAgent;
            if (response.IsAgent == 1)
            {
                if (response.AgentRate != null)
                {
                    model.Item.AgentRate = new AgentIdentityAndRateViewModel.Rate
                    {
                        BizRate = response.AgentRate.BizRate,
                        ForceRate = response.AgentRate.ForceRate,
                        TaxRate = response.AgentRate.TaxRate
                    };
                }
                else
                {
                    model.Item.AgentRate = new AgentIdentityAndRateViewModel.Rate();
                }

                model.Item.ZhiKeRate = new List<AgentIdentityAndRateViewModel.Rate>();
            }
            else
            {
                model.Item.AgentRate = new AgentIdentityAndRateViewModel.Rate();
                model.Item.ZhiKeRate = new List<AgentIdentityAndRateViewModel.Rate>();
                if (response.ZhiKeRate != null)
                {
                    for (int i = 0; i < response.ZhiKeRate.Count; i++)
                    {
                        model.Item.ZhiKeRate.Add(new AgentIdentityAndRateViewModel.Rate
                        {
                            BizRate = response.ZhiKeRate[i].BizRate,
                            Source = response.ZhiKeRate[i].Source
                        });
                    }
                }
                else
                {
                    model.Item.ZhiKeRate = new List<AgentIdentityAndRateViewModel.Rate>();
                }

            }
            return model;
        }
    }
}
