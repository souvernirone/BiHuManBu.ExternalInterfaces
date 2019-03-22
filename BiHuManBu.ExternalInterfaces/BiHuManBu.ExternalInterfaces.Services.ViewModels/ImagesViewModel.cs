using System;
using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class ImagesViewModel : BaseViewModel
    {
        public List<bx_images> imageses { get; set; }
    }

    //public class bx_images
    //{
    //    public long id { get; set; }
    //    public Nullable<int> type { get; set; }
    //    public string image { get; set; }
    //    public Nullable<long> buid { get; set; }
    //}
}
