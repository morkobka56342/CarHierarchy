using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CarHierarchyLib.Models;

namespace CarHierarchy.Services
{
    public class XmlCarSerializer
    {

        public string SerializeToString(List<Car> cars, IEnumerable<Type> knownTypes)
        {
            var serializer = new XmlSerializer(typeof(List<Car>), knownTypes.ToArray());

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, cars);
                return writer.ToString();
            }
        }

        public List<Car> DeserializeFromString(string xmlData, IEnumerable<Type> knownTypes)
        {
            if (string.IsNullOrWhiteSpace(xmlData)) return new List<Car>();

            try
            {
                var serializer = new XmlSerializer(typeof(List<Car>), knownTypes.ToArray());

                using (var reader = new StringReader(xmlData))
                {
                    return (List<Car>)serializer.Deserialize(reader) ?? new List<Car>();
                }
            }
            catch
            {
                return new List<Car>();
            }
        }
    }
}