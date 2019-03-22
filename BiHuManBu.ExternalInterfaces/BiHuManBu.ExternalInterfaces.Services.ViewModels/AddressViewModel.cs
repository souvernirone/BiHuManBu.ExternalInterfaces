using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiHuManBu.ExternalInterfaces.Models;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class AddressViewModel:BaseViewModel
    {
        public AddressModel Address { get; set; }
    }

    public class AddressesViewModel : BaseViewModel
    {
        public List<AddressModel> Addresses { get; set; }
    }

    public class AddressModel
    {
        public int id { get; set; }
        public Nullable<int> userid { get; set; }
        public string Name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public Nullable<int> provinceId { get; set; }
        public Nullable<int> cityId { get; set; }
        public Nullable<int> areaId { get; set; }
        public Nullable<int> agentId { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> createtime { get; set; }
        public Nullable<System.DateTime> updatetime { get; set; }
        public string provinceName { get; set; }
        public string cityName { get; set; }
        public string areaName { get; set; }
    }
}
