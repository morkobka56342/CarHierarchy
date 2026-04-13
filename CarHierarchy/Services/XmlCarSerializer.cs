using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using CarHierarchy.Models;

namespace CarHierarchy.Services
{
    public class XmlCarSerializer
    {
        private readonly XmlSerializer _serializer;

        public XmlCarSerializer()
        {
            _serializer = new XmlSerializer(typeof(List<Car>), new[]
            {
                typeof(PassengerCar),
                typeof(Truck),
                typeof(SportCar),
                typeof(ElectricCar),
                typeof(DieselTruck),
                typeof(HybridCar)
            });
        }

        public void Serialize(string filePath, List<Car> cars)
        {
            using (var writer = new StreamWriter(filePath))
            {
                _serializer.Serialize(writer, cars);
            }
        }

        public List<Car> Deserialize(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<Car>();

            using (var reader = new StreamReader(filePath))
            {
                return (List<Car>)_serializer.Deserialize(reader) ?? new List<Car>();
            }
            //fix
        }
    }
}