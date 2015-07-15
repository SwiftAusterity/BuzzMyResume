using System;
using System.Collections.Generic;
using Microsoft.Data.Caching;
using Ninject;
using Data.API;
using Data.API.CacheKey;

namespace Data.Cached
{
    public class VelocityShim : ICache
    {
        [Inject]
        public DataCache Cache
        { get; set; }

        public T Get<T>(string key)
        {
            return (T)Cache.Get(key);
        }

        public T Enroll<T>(BaseCacheKey key, Func<bool, T> filler)
        {
            var thunk = FreshnessRequestThunk(filler);
            return Enroll(key, thunk);
        }

        public T Enroll<T>(BaseCacheKey key, Func<FreshnessRequest, T> filler)
        {
            var value = filler(FreshnessRequest.Normal);
            return MemoizeElement(key, value);
        }

        public IList<TElement> GetCollection<TCollectionKey, TElement, TElementKey>(TCollectionKey collectionKey)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey
        {
            var keys = Get<IList<TElementKey>>(collectionKey.Key);

            if (keys == null)
                return null;

            var elements = new List<TElement>(keys.Count);

            foreach (var key in keys)
            {
                var element = Get<TElement>(key.Key);

                if (element == null)
                    return null;

                elements.Add(element);
            }

            return elements;
        }

        public IList<TElement> EnrollCollection<TCollectionKey, TElement, TElementKey>(
                TCollectionKey key,
                Func<bool, IList<TElement>> filler,
                Func<TCollectionKey, TElement, TElementKey> keyProjection)
            where TCollectionKey : BaseCacheKey
            where TElementKey : BaseCacheKey
        {
            var thunk = FreshnessRequestThunk(filler);
            return EnrollCollection(key, thunk, keyProjection);
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

        private static Func<FreshnessRequest, T> FreshnessRequestThunk<T>(Func<bool, T> filler)
        {
            return (freshness) => filler(freshness != FreshnessRequest.AsyncBackfill);
        }

        private T MemoizeElement<T>(BaseCacheKey key, T value)
        {
            var policy = key.Policy;
            var keyString = key.Key;
            var timeoutSeconds = CachePolicy.Unused;

            if (policy.AbsoluteSeconds != CachePolicy.Unused)
                timeoutSeconds = policy.AbsoluteSeconds;

            if (policy.SlidingSeconds != CachePolicy.Unused && timeoutSeconds < policy.SlidingSeconds)
                timeoutSeconds = policy.SlidingSeconds;

            if (timeoutSeconds == CachePolicy.Unused)
                Cache.Add(keyString, value);
            else
                Cache.Add(keyString, value, TimeSpan.FromSeconds(policy.SlidingSeconds));

            // velocity doesn't support cache callbacks, so we have to deal with this stuff the hard way... setting timers and starting threads...
            return value;
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
                    MemoizeElement(elementKey, element);
                    keys.Add(elementKey);
                }
            }

            return list;
        }

        public void Clear()
        {
            Cache.ClearRegion("Root");
        }

        public void Clear(BaseCacheKey key)
        {
        }
    }
}
