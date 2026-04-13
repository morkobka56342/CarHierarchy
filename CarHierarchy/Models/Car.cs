using System.Xml.Serialization;

namespace CarHierarchy.Models
{
    [XmlInclude(typeof(PassengerCar))]
    [XmlInclude(typeof(Truck))]
    [XmlInclude(typeof(SportCar))]
    [XmlInclude(typeof(ElectricCar))]
    [XmlInclude(typeof(DieselTruck))]
    [XmlInclude(typeof(HybridCar))]
    public abstract class Car : ICar
    {
        public string Brand { get; set; } = "Unknown";
        public string Model { get; set; } = "Unknown";
        public int Year { get; set; } = 2020;
        public decimal Price { get; set; } = 0;

        protected Car(string brand, string model, int year, decimal price)
        {
            Brand = brand;
            Model = model;
            Year = year;
            Price = price;
        }

        public Car() { }
        public override string ToString()
        {
            return GetDescription();
        }

        public abstract string GetCarType();
        public abstract string GetDescription();
    }
}