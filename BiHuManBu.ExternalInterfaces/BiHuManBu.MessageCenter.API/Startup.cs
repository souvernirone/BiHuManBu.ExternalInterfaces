using System.Web.Http;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Repository;
using BiHuManBu.ExternalInterfaces.Services;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.MessageCenter.API.Demo;
using BiHuManBu.MessageCenter.API.RealTimeConnection;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Practices.Unity;
using Owin;

namespace BiHuManBu.MessageCenter.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {


            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, }
            );
            app.UseWebApi(config);
           
          

            var container = new UnityContainer();
            
            //container.RegisterType<IClock, Clock>(new ContainerControlledLifetimeManager());
            //container.RegisterType<IMessageFormatter, MessageFormatter>();
            container.RegisterType<IDemoService, DemoService>();
            container.RegisterType<IMessageService, MessageService>();

            container.RegisterType<IMessageRepository, MessageRepository>();
            container.RegisterType<IAgentRepository, AgentRepository>();
            container.RegisterType<INoticexbRepository, NoticexbRepository>();
            container.RegisterType<IUserInfoRepository, UserInfoRepository>();

            container.RegisterType<TrackerConnection>();

            config.DependencyResolver = new Microsoft.Practices.Unity.WebApi.UnityDependencyResolver(container);


            //app.MapSignalR(new HubConfiguration()
            //{
            //    Resolver = new UnityDependencyResolver(container)
            //});

            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider),()=>new CookiesUserIdProvider());

            //app.UseCors(CorsOptions.AllowAll)
            //    .MapSignalR<TrackerConnection>("/tracker", new ConnectionConfiguration()
            //    {
            //        Resolver = new UnityDependencyResolver(container)
            //    })
            //    .MapSignalR();
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    EnableJSONP = true
                };
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}