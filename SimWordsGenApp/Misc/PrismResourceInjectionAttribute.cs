using System;

namespace Prism.Unity
{
    public class PrismResourceInjectionAttribute : Attribute
    {
        public readonly string ResourceKey;

        public PrismResourceInjectionAttribute(string resourceKey = null)
        {
            ResourceKey = resourceKey;
        }
    }

    public class PrismGenericResourceInjectionAttribute : Attribute
    {
        public readonly Type GenericType;
        public readonly string ResourceKey;

        public PrismGenericResourceInjectionAttribute(Type genericType, string resourceKey = null)
        {
            if (genericType.IsGenericType)
                GenericType = genericType;
            ResourceKey = resourceKey;
        }
    }

    public class PrismResourceKeyFormatAttribute : Attribute
    {
        public readonly string KeyFormat;

        public PrismResourceKeyFormatAttribute(string keyFormat)
        {
            KeyFormat = keyFormat;
        }
    }
}
