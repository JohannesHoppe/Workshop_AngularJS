using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

namespace AngularDemo
{
    public class IocConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            // Registers all ASP.NET MVC controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // Registers all Web API controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // all types which are decorated with [RegisterInstancePerRequest]
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.GetCustomAttributes(typeof(RegisterInstancePerRequestAttribute), false).Any())
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            var container = builder.Build();

            // Set the dependency resolver for MVC
            var mvcResolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(mvcResolver);

            // Set the dependency resolver for Web API.
            var webApiResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;
        }
    }
}