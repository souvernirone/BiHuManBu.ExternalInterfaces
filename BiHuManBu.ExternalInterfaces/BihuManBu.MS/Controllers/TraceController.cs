using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BihuManBu.MS.Messages.Request;

namespace BihuManBu.MS.Controllers
{
    public class TraceController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        public async Task<HttpResponseMessage> Trace([FromBody]TraceRequest request)
        {
           
            return new HttpResponseMessage();
        } 
    }
}
