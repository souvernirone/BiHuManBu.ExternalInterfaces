﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    [Serializable]
    public class MessageResult
    {
        //<>0:失败
        public int ResultCode { get; set; }
        public string Message { get; set; }

    }
}
