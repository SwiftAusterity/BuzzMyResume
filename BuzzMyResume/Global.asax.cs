using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Microsoft.ApplicationBlocks.ExceptionManagement;

using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;

using BuzzMyResume.Ninject;
using BuzzMyResume.Routing;
using BuzzMyResume.ViewEngine;

namespace BuzzMyResume
{
    public class MvcApplication : NinjectHttpApplication
    {
        protected override IKernel CreateKernel()
        {
            var modules = new INinjectModule[]
                                    {
                                        new SiteModule()
                                    };
            return new StandardKernel(modules);
        }

        protected override void OnApplicationStarted()
        {
            ViewEngines.Engines.Add(new WebFormViewEngineWithMyPartialLocations(Kernel));
            RegisterRoutes(RouteTable.Routes);

            //TODO: We might want to suck up all the known synonyms here and get the cache filled, since we'd be doing it the first request anyways.
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // handled by ExceptionHandler defined in web.config
            try
            {
                Exception ex = Server.GetLastError();

                while (ex != null && ex.InnerException != null && ex is HttpUnhandledException)
                    ex = ex.InnerException;

                if (ex != null && !(ex is ThreadAbortException))
                    ExceptionManager.Publish(ex);

                Server.ClearError();
            }
            catch
            {
                // Avoid Infinte loop
            }
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            // Turns off the unnecessary file exists check
            //routes.RouteExistingFiles = true;

            // Ignore text, html, files.
            routes.IgnoreRoute("{file}.txt",
                new { categoryId = Kernel.Get<AllowedTextFilesConstraint>() });
            routes.IgnoreRoute("{file}.htm");
            routes.IgnoreRoute("{file}.html");

            // Ignore axd files such as assest, image, sitemap etc
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Ignore the assets directory which contains images, js, css & html
            routes.IgnoreRoute("Assets/{*pathInfo}");

#if(DEBUG)
            routes.MapRoute("DebuggingHook"
                          , "{foo}/{bar}/{whatever}/{yeah}/{right}/{ok}/{then}/{sharks}"
                          , new
                          {
                              controller = "Tweets",
                              action = "Diagnostic",
                              foo = String.Empty,
                              bar = String.Empty,
                              whatever = String.Empty,
                              yeah = String.Empty,
                              right = String.Empty,
                              ok = String.Empty,
                              then = String.Empty,
                              sharks = String.Empty
                          }
                          , new
                          {
                              foo = new DiagnosticRouteConstraint()
                          })
          ;
#endif
            routes.MapRoute(
                "IAmError",
                "error",
                new { controller = "Static", staticPage = "error", action = "ShowStaticContent" });

            routes.MapRoute(
                "WheresWaldo",
                "missing",
                new { controller = "Static", staticPage = "missing", action = "ShowStaticContent" });


            routes.MapRoute(
                "Static", // Route name
                "{staticPage}", // URL with parameters
                new { controller = "Static", action = "ShowStaticContent" },
                new { staticPage = @"(error)|(missing)|(faq)|(about)|(thankyou)|(suggestionthankyou)" });

        }
    }
}