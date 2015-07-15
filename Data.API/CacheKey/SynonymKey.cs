using System;
using System.Globalization;
using Ninject;

namespace Data.API.CacheKey
{
    public class SynonymKey : BaseCacheKey
    {
        public SynonymKey()
            : base("Synonym")
        {
        }
    }
}
