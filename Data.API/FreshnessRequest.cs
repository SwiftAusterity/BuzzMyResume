using Infuz.Utilities;

namespace Data.API
{
    public enum FreshnessRequest : byte
    {
        Peek = 1,
        AsyncBackfill = 2,
        Normal = 4,
        Committed = 8
    }

    public static partial class EnumHelper
    {
        public static bool TryParse(this string value, out FreshnessRequest desiredFreshness)
        {
            return value.TryParse(out desiredFreshness, FreshnessRequest.Normal);
        }
    }
}
