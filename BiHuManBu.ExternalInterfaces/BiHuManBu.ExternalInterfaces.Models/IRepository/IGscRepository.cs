﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiHuManBu.ExternalInterfaces.Models
{
    public  interface IGscRepository
    {
        bx_gsc_userinfo FindUserInfoByGsc(string usercode, string pwd);
    }
}