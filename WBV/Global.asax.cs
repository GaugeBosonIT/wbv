using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WBV.DataAccess;
using Ninject;
using WBV.Interfaces;
using WBV.Models;
using WBV.Services;

namespace WBV
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

                        routes.MapRoute(
            "api", // Route name
            "api/{controller}/{action}/{id}", // URL with parameters
            new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                    );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );



        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterDependencyResolver();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        private void RegisterDependencyResolver()
        {
            HttpContextBase context = new HttpContextWrapper(HttpContext.Current);
            var kernel = new StandardKernel();
            kernel.Bind<IData>().To<DataService>();
            kernel.Bind<ISession>().To<SessionService>()
                                   .WithConstructorArgument("context", ninjectContext => context);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    } 




}