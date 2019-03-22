
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class GetParaBhTypeViewModel : BaseViewModel
    {
        public List<ParaBhType> ParaBhType { get; set; }
    }
    public class ParaBhType
    {
        public int Id { get; set; }
        public int BhId { get; set; }
        public string BhName { get; set; }
        public int BhType { get; set; }
        public int IsSupport { get; set; }
        
    }
}
