using System;
using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class DriverLicenseTypeViewModel : BaseViewModel
    {
        public List<DriverLicenseType> Items { get; set; } 
    }

    public  class DriverLicenseType
    {
        public  string TypeName { get; set; }
        public  string TypeValue { get; set; }
    }
}
