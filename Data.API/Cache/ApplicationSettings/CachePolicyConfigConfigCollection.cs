using System.Configuration;

namespace Data.API
{
    public class CachePolicyConfigConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CachePolicyConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CachePolicyConfigurationElement)element).Key;
        }
    }
}
