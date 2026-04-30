using PluginBase;
using Newtonsoft.Json;
using System.Xml;

namespace JsonTransformerPlugin
{
    public class XmlToJsonPlugin : IDataTransformer
    {
        public string Name => "XML to JSON Converter";

        public string TransformBeforeSave(string xmlData)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData);
            return JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
        }
    }
}