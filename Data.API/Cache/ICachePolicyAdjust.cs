using System;
using System.Collections.Generic;
using Data.API.CacheKey;

namespace Data.API
{
    public interface ICachePolicyAdjust
    {
        CachePolicy Adjust(CachePolicy policy);
    }
}
