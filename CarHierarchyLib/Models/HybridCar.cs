namespace CarHierarchyLib.Models
{
    public class HybridCar : PassengerCar
    {
        public double FuelConsumption { get; set; } 
        public int ElectricRange { get; set; } 
        public string HybridType { get; set; } = "Parallel"; 

        public HybridCar(string brand, string model, int year, decimal price,
                        int doorCount, string bodyType, string fuelType,
                        double fuelConsumption, int electricRange, string hybridType)
            : base(brand, model, year, price, doorCount, bodyType, fuelType)
        {
            FuelConsumption = fuelConsumption;
            ElectricRange = electricRange;
            HybridType = hybridType;
        }

        public HybridCar() { }
        public override string GetCarType() => "Hybrid Car";

        public override string GetDescription()
            => $"{Brand} {Model}: Fuel consumption {FuelConsumption}l/100km, Electric range {ElectricRange}km, type: {HybridType}";
    }
}