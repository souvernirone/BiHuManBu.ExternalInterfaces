﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Services.ViewModels
{
    public class AddOrUpdateWorkOrderViewModel:BaseViewModel
    {
        public int WorkOrderId { get; set; }
        public int AdvAgentId { get; set; }
    }
}
