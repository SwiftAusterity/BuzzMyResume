using Data.API.CacheKey;

namespace Data.API
{
    public interface ICachePolicyRepository
    {
        CachePolicy ComputePolicy(string policyKey, CachePolicy defaultPolicy);
    }
}
