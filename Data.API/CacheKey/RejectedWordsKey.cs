using System;
using System.Globalization;
using Ninject;

namespace Data.API.CacheKey
{
    public class RejectedWordsKey : BaseCacheKey
    {
        public RejectedWordsKey()
            : base("RejectedWords")
        {
        }
    }
}
