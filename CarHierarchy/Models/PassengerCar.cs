
namespace CarHierarchy.Models
{
    public class PassengerCar : Car
    {
        public int DoorCount { get; set; } = 4;
        public string BodyType { get; set; } = "Sedan"; 
        public string FuelType { get; set; } = "Petrol"; 

        public PassengerCar(string brand, string model, int year, decimal price,
                           int doorCount, string bodyType, string fuelType)
            : base(brand, model, year, price)
        {
            DoorCount = doorCount;
            BodyType = bodyType;
            FuelType = fuelType;
        }

        public PassengerCar() { }
        public override string GetCarType() => "Passenger Car";

        public override string GetDescription()
            => $"{Brand} {Model}: {BodyType}, {DoorCount} doors, fuel: {FuelType}";
    }
}