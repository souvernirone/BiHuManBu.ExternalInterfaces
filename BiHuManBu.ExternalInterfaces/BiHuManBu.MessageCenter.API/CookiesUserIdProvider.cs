using System;
using Microsoft.AspNet.SignalR;

namespace BiHuManBu.MessageCenter.API
{
    public class CookiesUserIdProvider:IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException();
            }

            Cookie cookie;

            if (request.Cookies.TryGetValue("agentid", out cookie))
            {
                return cookie.Value;
            }
            else
            {
                return null;
            }
        }
    }
}