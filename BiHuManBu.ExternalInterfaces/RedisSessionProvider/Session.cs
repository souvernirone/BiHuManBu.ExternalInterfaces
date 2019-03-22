using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RedisSessionProvider
{
   public class Session
   {
       private HttpContextBase _contextBase;
       public const string SessionName = "__SessionName__";

       public string SessionId { get; set; }

       public string NewGuid()
       {
           return SessionName + Guid.NewGuid();
       }

       public Session(HttpContextBase context)
       {
           var cookie = context.Request.Cookies.Get(SessionName);

           if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
           {
               SessionId = NewGuid();
               context.Response.Cookies.Add(new HttpCookie(SessionName,SessionId));
               context.Request.Cookies.Add(new HttpCookie(SessionName,SessionId));
           }
           else
           {
               SessionId = cookie.Value;
           }
       }

       public T Get<T>() where T : class, new()
       {
           return new SessionClient<T>().SessionGet(SessionId);
       }

       public bool IsLogin()
       {
           return new SessionClient<object>().SessionHasExists(SessionId);
       }

       public void Login<T>(T obj) where T : class, new()
       {
           new SessionClient<T>().SessionSet(SessionId,obj,new TimeSpan(0,20,0));
       }

       public void Quit()
       {
           new SessionClient<object>().SessionRemove(SessionId);
       }

       public void Postpone()
       {
           new SessionClient<object>().SessionExpireIn(SessionId,new TimeSpan(0,20,0));
       }
  
   }
}
