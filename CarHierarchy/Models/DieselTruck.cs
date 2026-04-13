namespace CarHierarchy.Models
{
    public class DieselTruck : Truck
    {
        public double EngineVolume { get; set; } 
        public double FuelTankCapacity { get; set; } 
        public double FuelConsumption { get; set; }

        public DieselTruck(string brand, string model, int year, decimal price,
                          double loadCapacity, int axleCount, double cargoVolume,
                          double engineVolume, double fuelTankCapacity, double fuelConsumption)
            : base(brand, model, year, price, loadCapacity, axleCount, cargoVolume)
        {
            EngineVolume = engineVolume;
            FuelTankCapacity = fuelTankCapacity;
            FuelConsumption = fuelConsumption;
        }

        public DieselTruck() { }
        public override string GetCarType() => "Diesel Truck";

        public override string GetDescription()
            => $"{Brand} {Model}: {EngineVolume}L engine, {FuelTankCapacity}L tank, consumption {FuelConsumption} L/100km";
    }
}