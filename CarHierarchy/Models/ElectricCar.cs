namespace CarHierarchy.Models
{
    public class ElectricCar : PassengerCar
    {
        public int BatteryCapacity { get; set; } 
        public int Range { get; set; } 
        public int ChargePower { get; set; } 


        public ElectricCar(string brand, string model, int year, decimal price,
                          int doorCount, string bodyType, string fuelType,
                          int batteryCapacity, int range, int chargePower)
            : base(brand, model, year, price, doorCount, bodyType, fuelType)
        {
            BatteryCapacity = batteryCapacity;
            Range = range;
            ChargePower = chargePower;
        }

        public ElectricCar() { }
        public override string GetCarType() => "Electric Car";

        public override string GetDescription()
            => $"{Brand} {Model}: {BatteryCapacity} kWh battery, {Range} km range, {ChargePower} kW charging";
    }
}