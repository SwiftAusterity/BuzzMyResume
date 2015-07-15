using System;
using Microsoft.Data.Caching;
using Ninject;
using Ninject.Activation;

namespace Data.Ninject
{
    public class VelocityDataCacheProvider : Provider<DataCache>
    {
        [Inject]
        public DataCacheFactory VelocityDataCacheFactory
        { get; set; }

        protected override DataCache CreateInstance(IContext context)
        {
            return VelocityDataCacheFactory.GetCache("STLTweets");
        }
    }
}
