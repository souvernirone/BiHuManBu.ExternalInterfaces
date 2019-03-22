using System.Collections.Generic;
using System.Linq;
using BiHuManBu.ExternalInterfaces.Infrastructure.Helpers;
using BiHuManBu.ExternalInterfaces.Services.ViewModels;

namespace BiHuManBu.ExternalInterfaces.Services.Mapper
{
    public static class AgentSourceMapper
    {
        public static List<Source> ConverToViewModel(this List<int> list)
        {
            List<Source> model = new List<Source>();
            if (list!=null)
            {
                Source source;
                foreach (var item in list)
                {
                    source=new Source();
                    source.Id = item;
                    source.Name = item.ToEnumDescriptionString(typeof(EnumSource));
                    model.Add(source);
                }

            }
            return model;
        }
    }
}
