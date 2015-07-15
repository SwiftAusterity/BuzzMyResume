using System;
using Data.API;

namespace Data.Cached
{
    public class HttpCacheNoRefillShim : HttpCacheShim
    {
        protected override CacheAddParameters<T> BuildParameters<T>(CachePolicy policy, Func<FreshnessRequest, T> filler)
        {
            var parameters = base.BuildParameters<T>(policy, filler);
            parameters.NumberOfRefillsRemaining = 0;
            return parameters;
        }
    }
}
