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
    class FeedbackRepository : IFeedbackRepository
    {
        [Inject]
        public IFeedbackRepositoryBackingStore BackingStore { get; set; }

        [Inject]
        public ICache Cache { get; set; }

        public IEnumerable<IFeedback> GetAll()
        {
            var cacheKey = new FeedbackKey();

            var keyString = cacheKey.Key;
            var cachedResult = Cache.Get<IEnumerable<IFeedback>>(keyString);

            if (cachedResult != null)
                return cachedResult;

            return Cache.Enroll(cacheKey, (bool initialLoad) => BackingStore.GetAll());
        }

        public IFeedback Insert(string contact, string name, string body)
        {
            return BackingStore.Insert(contact, name, body);
        }

        public IEnumerable<IFeedback> GetByContact(String search)
        {
            return BackingStore.GetByContact(search);
        }

        public IEnumerable<IFeedback> GetByName(String search)
        {
            return BackingStore.GetByName(search);
        }

        public IEnumerable<IFeedback> GetByAny(String search)
        {
            return BackingStore.GetByAny(search);
        }
    }
}
