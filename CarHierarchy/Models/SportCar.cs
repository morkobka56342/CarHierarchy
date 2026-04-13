namespace CarHierarchy.Models
{
    public class SportCar : Car
    {
        public int Horsepower { get; set; } 
        public double Acceleration { get; set; } 
        public string DriveType { get; set; } = "RWD"; 

        public SportCar(string brand, string model, int year, decimal price,
                       int horsepower, double acceleration, string driveType)
            : base(brand, model, year, price)
        {
            Horsepower = horsepower;
            Acceleration = acceleration;
            DriveType = driveType;
        }

        public SportCar() { }
        public override string GetCarType() => "Sportcar";

        public override string GetDescription()
            => $"{Brand} {Model}: {Horsepower} hp, 0-100 in {Acceleration} sec, drive: {DriveType}";
    }
}