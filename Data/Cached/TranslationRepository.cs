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
    class TranslationRepository : ITranslationRepository
    {
        [Inject]
        public ITranslationRepositoryBackingStore BackingStore { get; set; }

        [Inject]
        public ICache Cache { get; set; }

        public IEnumerable<ITranslation> GetAll()
        {
            var cacheKey = new TranslationKey();

            var keyString = cacheKey.Key;
            var cachedResult = Cache.Get<IEnumerable<ITranslation>>(keyString);

            if (cachedResult != null)
                return cachedResult;

            return Cache.Enroll(cacheKey, (bool initialLoad) => BackingStore.GetAll());
        }

        public ITranslation Insert(IEnumerable<String> unknowns, IEnumerable<String> rejected, Dictionary<String, String> results, String source)
        {
            return BackingStore.Insert(unknowns, rejected, results, source);
        }

        public ITranslation GetByID(long id)
        {
            return BackingStore.GetByID(id);
        }

        public IEnumerable<ITranslation> GetBySource(String search)
        {
            return BackingStore.GetBySource(search);
        }
    }
}
