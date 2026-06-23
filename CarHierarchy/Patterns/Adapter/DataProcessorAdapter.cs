using PluginBase;
using System;

namespace CarHierarchy.Patterns.Adapter
{

    public class DataProcessorAdapter : IDataTransformer
    {
        private readonly IDataProcessorPlugin _adaptee;

        public DataProcessorAdapter(IDataProcessorPlugin adaptee)
        {
            _adaptee = adaptee ?? throw new ArgumentNullException(nameof(adaptee));
        }
        public string Name => $"[Adapted] {_adaptee.ProcessorName}";

        public string TransformBeforeSave(string xmlData)
        {
            return _adaptee.ProcessBeforeSave(xmlData);
        }
    }
}