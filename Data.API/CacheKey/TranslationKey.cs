using System;
using System.Globalization;
using Ninject;

namespace Data.API.CacheKey
{
    public class TranslationKey : BaseCacheKey
    {
        public TranslationKey()
            : base("Translation")
        {
        }
    }
}
