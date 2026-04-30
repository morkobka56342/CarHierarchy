using PluginBase;
using System.Xml.Linq;
using System.Xml;
using Newtonsoft.Json;

namespace SortPlugin
{
    public class AlphabeticalSortPlugin : IDataTransformer
    {
        public string Name => "XML to JSON with Sort";

        // sorts the XML structure by the "Brand" element value
        private string SortXml(string xmlData)
        {
            try
            {
                var doc = XDocument.Parse(xmlData);
                var root = doc.Root;
                if (root != null)
                {
                    var sorted = root.Elements()
                        .OrderBy(e => e.Element("Brand")?.Value ?? string.Empty)
                        .ToList();
                    root.ReplaceAll(sorted);
                }
                return doc.ToString();
            }
            catch {
                // if parsing fails, return original data     
                return xmlData;
            }
        }

        public string TransformBeforeSave(string xmlData)
        {
            // sort
            string sortedXml = SortXml(xmlData);
            // initialize a new XmlDocument and load the sorted XML string into it
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sortedXml);
            // convert the XML document into a JSON
            return JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
        }

    }
}