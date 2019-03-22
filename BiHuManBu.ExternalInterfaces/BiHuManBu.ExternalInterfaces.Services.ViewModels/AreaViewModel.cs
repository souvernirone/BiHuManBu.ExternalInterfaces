using System;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class AreaViewModel
    {
        public List<bx_area> Areas { get; set; }
    }
    public partial class bx_area
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Pid { get; set; }
    }
}
