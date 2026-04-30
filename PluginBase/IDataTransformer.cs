namespace PluginBase
{
    public interface IDataTransformer
    {
        string Name { get; }

        string TransformBeforeSave(string xmlData);

    }
}