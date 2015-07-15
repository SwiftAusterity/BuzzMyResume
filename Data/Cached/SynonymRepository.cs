using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ninject;

using Data.API;
using Data.API.CacheKey;
using Data.API.Interfaces.DB;
using Data.API.Interfaces.DO;

namespace Data.Cached
{
    class SynonymRepository : ISynonymRepository
    {
        [Inject]
        public ISynonymRepositoryBackingStore BackingStore { get; set; }

        [Inject]
        public ICache Cache { get; set; }

        public IEnumerable<ISynonym> GetAll()
        {
            var cacheKey = new SynonymKey();

            var keyString = cacheKey.Key;
            var cachedResult = Cache.Get<IEnumerable<ISynonym>>(keyString);

            if (cachedResult != null)
                return cachedResult;

            return Cache.Enroll(cacheKey, (bool initialLoad) => BackingStore.GetAll());
        }

        public ISynonym Insert(string source, string result)
        {
            return BackingStore.Insert(source, result);
        }

        public IEnumerable<ISynonym> GetBySource(String source)
        {
            return BackingStore.GetBySource(source);
        }
    }
}
