namespace CarHierarchy.Models
{
    public class Truck : Car
    {
        public double LoadCapacity { get; set; } 
        public int AxleCount { get; set; } = 2;
        public double CargoVolume { get; set; } 

        public Truck(string brand, string model, int year, decimal price,
                    double loadCapacity, int axleCount, double cargoVolume)
            : base(brand, model, year, price)
        {
            LoadCapacity = loadCapacity;
            AxleCount = axleCount;
            CargoVolume = cargoVolume;
        }

        public Truck() { }
        public override string GetCarType() => "Truck";

        public override string GetDescription()
            => $"{Brand} {Model}: load capacity {LoadCapacity}t, {AxleCount} axle count, cargo volume  {CargoVolume}m³";
    }
}