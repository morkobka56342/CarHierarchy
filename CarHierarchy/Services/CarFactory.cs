using System;
using System.Collections.Generic;
using System.Linq;
using CarHierarchy.Models;

namespace CarHierarchy.Services
{
    public class CarFactory : ICarFactory
    {
     
        private readonly Dictionary<string, Func<Car>> _creators;

        public CarFactory()
        {
            _creators = new Dictionary<string, Func<Car>>
            {
                { "Passenger Car", () => new PassengerCar() },
                { "Truck", () => new Truck() },
                { "Sport Car", () => new SportCar() },
                { "Electric Car", () => new ElectricCar() },
                { "Diesel Truck", () => new DieselTruck() },
                { "Hybrid Car", () => new HybridCar() }
            };
        }

        public Car CreateCar(string typeName)
        {
            if (_creators.TryGetValue(typeName, out var creator))
            {
                return creator();
            }
            throw new ArgumentException($"Type {typeName} is not registered in the factory.");
        }

        public IEnumerable<string> GetAvailableTypes() => _creators.Keys;
    }
}