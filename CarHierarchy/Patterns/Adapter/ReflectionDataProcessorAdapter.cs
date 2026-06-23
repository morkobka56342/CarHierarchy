using PluginBase;

namespace CarHierarchy.Patterns.Adapter
{

    public class ReflectionDataProcessorAdapter : IDataTransformer
    {
        private readonly string _name;
        private readonly Func<string, string> _transformFunc;

        public ReflectionDataProcessorAdapter(string name, Func<string, string> transformFunc)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _transformFunc = transformFunc ?? throw new ArgumentNullException(nameof(transformFunc));
        }

        public string Name => $"[Adapted] {_name}";

        public string TransformBeforeSave(string xmlData)
        {
            return _transformFunc(xmlData);
        }
    }
}