using CarHierarchyLib.Models;


namespace MotorcyclePlugin
{
    public class Motorcycle : Car
    {
        public bool HasSidecar { get; set; } 
        public string BikeType { get; set; } 

        public Motorcycle() { }

        public override string GetCarType() => "Motorcycle";

        public override string GetDescription() =>
            $"{Brand} {Model} ({BikeType}): {(HasSidecar ? "With sidecar" : "No sidecar")}";
    }
}