using System;
using System.Globalization;
using Ninject;

namespace Data.API.CacheKey
{
    public class FeedbackKey : BaseCacheKey
    {
        public FeedbackKey()
            : base("Feedback")
        {
        }
    }
}
