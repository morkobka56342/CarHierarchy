using CarHierarchy.Services;
using System;

namespace CarHierarchy.Patterns.Singleton
{

    public sealed class SerializerSingleton
    {
        private static readonly Lazy<XmlCarSerializer> _instance =
            new(() => new XmlCarSerializer());

        public static XmlCarSerializer Instance => _instance.Value;

        private SerializerSingleton() { }
    }
}