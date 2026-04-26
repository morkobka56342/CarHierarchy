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
        public void Serialize(string filePath, List<Car> cars, IEnumerable<Type> knownTypes)
        {
            var serializer = new XmlSerializer(typeof(List<Car>), knownTypes.ToArray());
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, cars);
            }
        }

        public List<Car> Deserialize(string filePath, IEnumerable<Type> knownTypes)
        {
            if (!File.Exists(filePath)) return new List<Car>();

            try
            {
                var serializer = new XmlSerializer(typeof(List<Car>), knownTypes.ToArray());
                using (var reader = new StreamReader(filePath))
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