using System.Diagnostics;
using System.Security.Principal;
using System.Web;
//using ViewModels;
using Infuz.API.SQL;
using Infuz.SQL;
using Ninject;
using Ninject.Modules;
using API = Data.API;
using Cached = Data.Cached;
using DB = Data.DB;

namespace BuzzMyResume.Ninject
{
    public class SiteModule : NinjectModule
    {
        public override void Load()
        {
            // use the once-per-request cache when you are running a debugger so we don't have background refills going.
            // we use TransientScope so each ICache filled is a new one, so each cache repository gets his
            // own personal cache to clear
            if (Debugger.IsAttached)
                Bind<API.ICache>().To<Cached.HttpCacheNoRefillShim>().InTransientScope();
            else
                Bind<API.ICache>().To<Cached.HttpCacheShim>().InTransientScope();

            API.CacheKey.BaseCacheKey.Kernel = Kernel;
            Bind<API.ICachePolicyRepository>().To<API.ApplicationSettingCachePolicyRepository>().InSingletonScope();

            Bind<ISqlServiceProvider>()
                .ToMethod(context => new SqlServiceProvider("")) //TODO: Conx string
                .InSingletonScope();

            // Set the Repo's that the caching repos fallback to
            //Bind<API.ILabelRepositoryBackingStore>().To<DB.LabelRepository>().InSingletonScope();

            Bind<IPrincipal>().ToMethod(ctx => GetUser()).InRequestScope();
        }

        private IPrincipal GetUser()
        {
            var result = Kernel.Get<HttpContextBase>().User;
            return result;
        }

        //private L2T.InMemoryCredentials SetupCredentialManager()
        //{
        //    var creds = new L2T.InMemoryCredentials
        //        {
        //            ConsumerKey = SiteSettings.TwitterConsumerKey,
        //            ConsumerSecret = SiteSettings.TwitterConsumerSecret
        //        };

        //    return creds;
        //}
    }
}