using Microsoft.Data.Caching;
using Ninject.Modules;

namespace Data.Ninject
{
    public class STLTweetsDataModule : NinjectModule
    {
        public override void Load()
        {
            Bind<DataCache>().ToProvider<VelocityDataCacheProvider>().InSingletonScope();
        }
    }
}
