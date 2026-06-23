namespace CarHierarchy.Patterns.Adapter
{
    public interface IDataProcessorPlugin
    {
        string ProcessorName { get; }
        bool IsEnabled { get; set; }
        string ProcessBeforeSave(string xmlData);
        string ProcessAfterLoad(string xmlData);
    }
}