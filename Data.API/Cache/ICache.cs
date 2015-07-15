using System;
using System.Collections.Generic;
using Data.API.CacheKey;

namespace Data.API
{
    public interface ICache
    {
        T Get<T>(string key);
        T Enroll<T>(BaseCacheKey key, Func<bool, T> filler);
        T Enroll<T>(BaseCacheKey key, Func<FreshnessRequest, T> filler);

        IList<TElement> GetCollection<TCollectionKey, TElement, TElementKey>(TCollectionKey collectionKey)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey;
        IList<TElement> EnrollCollection<TCollectionKey, TElement, TElementKey>(
                TCollectionKey key,
                Func<bool, IList<TElement>> filler,
                Func<TCollectionKey, TElement, TElementKey> keyProjection)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey;
        IList<TElement> EnrollCollection<TCollectionKey, TElement, TElementKey>(
                TCollectionKey key,
                Func<FreshnessRequest, IList<TElement>> filler,
                Func<TCollectionKey, TElement, TElementKey> keyProjection)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey;

        void Clear();
        void Clear(BaseCacheKey key);
    }
}
