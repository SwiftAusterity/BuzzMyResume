using System;
using Ninject;

namespace Data.API.CacheKey
{
    public abstract class BaseCacheKey
    {
        [Inject]
        public static IKernel Kernel { get; set; }

        public string Prefix { get; set; }
        public string SubKey { get; set; }

        public BaseCacheKey(string prefix)
        {
            Prefix = prefix;
        }

        public virtual string Key
        {
            get
            {
                return String.IsNullOrEmpty(SubKey)
                            ?  Prefix 
                            : Prefix + "." + SubKey;
            }
        }

        public virtual string PolicyKey
        {
            get
            {
                return (String.IsNullOrEmpty(SubKey)
                            ? Prefix 
                            : Prefix + "/" + SubKey) + "/";
            }
        }

        public virtual CachePolicy DefaultPolicy
        {
            get
            {
                var policy =  new CachePolicy();
                policy.AbsoluteSeconds = 10;    // every ten second should pepper the server nicely
                return policy;
            }
        }

        public virtual CachePolicy Policy
        {
            get
            {
                // lookup in the config the policy for the official key-and-parameters given
                var policyKey = PolicyKey;
                var defaultPolicy = DefaultPolicy;
                var policyRepo = Kernel.Get<ICachePolicyRepository>();
                return policyRepo.ComputePolicy(policyKey, defaultPolicy); // lookup the policy via the provider
            }
        }
    }
}
