using System;
using System.Collections.Generic;
using Data.API;
using Data.API.CacheKey;

namespace Data.Cached
{
    public class NullCacheShim : ICache
    {
        public T Get<T>(string key)
        {
            return default(T);
        }

        public T Enroll<T>(BaseCacheKey key, Func<bool, T> filler)
        {
            T value = filler(true);
            return value;
        }

        public T Enroll<T>(BaseCacheKey key, Func<FreshnessRequest, T> filler)
        {
            T value = filler(FreshnessRequest.Normal);
            return value;
        }

        public void Clear()
        {
        }

        public void Clear(BaseCacheKey key)
        {
        }

        public IList<TElement> GetCollection<TCollectionKey, TElement, TElementKey>(TCollectionKey collectionKey)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey
        {
            return null;
        }

        public IList<TElement> EnrollCollection<TCollectionKey, TElement, TElementKey>(
                TCollectionKey key,
                Func<bool, IList<TElement>> filler,
                Func<TCollectionKey, TElement, TElementKey> keyProjection)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey
        {
            var list = filler(true);
            return MemoizeCollection(key, list, keyProjection);
        }

        public IList<TElement> EnrollCollection<TCollectionKey, TElement, TElementKey>(
                TCollectionKey key,
                Func<FreshnessRequest, IList<TElement>> filler,
                Func<TCollectionKey, TElement, TElementKey> keyProjection)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey
        {
            var list = filler(FreshnessRequest.Normal);
            return MemoizeCollection(key, list, keyProjection);
        }

        private IList<TElement> MemoizeCollection<TCollectionKey, TElement, TElementKey>(
                        TCollectionKey key,
                        IList<TElement> list,
                        Func<TCollectionKey, TElement, TElementKey> keyProjection)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey
        {
            if (list != null)
            {
                var keys = new List<TElementKey>(list.Count);

                foreach (var element in list)
                {
                    var elementKey = keyProjection(key, element);
                    keys.Add(elementKey);
                }
            }

            return list;
        }
    }
}
