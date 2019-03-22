using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

namespace BiHuManBu.BusinessInterfaces.API
{
    public static class Bootstrapper
    {
        public static void Run(HttpConfiguration configuration)
        {

            SetAutofacWebAPIServices(configuration);
        }       
        private static void SetAutofacWebAPIServices(HttpConfiguration configuration)
        {
            var builder = new ContainerBuilder();

            #region controllers

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
          
            #endregion


            #region Service

          

            #endregion

            #region reposities

          
             
            #endregion

            #region commmon 

           
            #endregion
            // Register API controllers using assembly scanning.
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterType<DefaultCommandBus>().As<ICommandBus>().InstancePerApiRequest();
            //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerApiRequest();
            //builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerApiRequest();
            //builder.RegisterAssemblyTypes(typeof(CategoryRepository)
            //    .Assembly).Where(t => t.Name.EndsWith("Repository"))
            //.AsImplementedInterfaces().InstancePerApiRequest();
            //var services = Assembly.Load("EFMVC.Domain");
            //builder.RegisterAssemblyTypes(services)
            //.AsClosedTypesOf(typeof(ICommandHandler<>)).InstancePerApiRequest();
            //builder.RegisterAssemblyTypes(services)
            //.AsClosedTypesOf(typeof(IValidationHandler<>)).InstancePerApiRequest();
            var container = builder.Build();
            // Set the dependency resolver implementation.
            var resolver = new AutofacWebApiDependencyResolver(container);
            configuration.DependencyResolver = resolver;           
        }
    }
}