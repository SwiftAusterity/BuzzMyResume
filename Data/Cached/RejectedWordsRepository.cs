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
    class RejectedWordsRepository : IRejectedWordsRepository
    {
        [Inject]
        public IRejectedWordsRepositoryBackingStore BackingStore { get; set; }

        [Inject]
        public ICache Cache { get; set; }

        public IEnumerable<String> GetAll()
        {
            var cacheKey = new RejectedWordsKey();

            var keyString = cacheKey.Key;
            var cachedResult = Cache.Get<IEnumerable<String>>(keyString);

            if (cachedResult != null)
                return cachedResult;

            return Cache.Enroll(cacheKey, (bool initialLoad) => BackingStore.GetAll());
        }

        public String Insert(String word)
        {
            return BackingStore.Insert(word);
        }
    }
}
