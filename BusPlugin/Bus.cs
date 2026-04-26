using CarHierarchyLib.Models;

namespace BusPlugin
{
    public class Bus : Car
    {
        public int PassengerCapacity { get; set; }
        public bool HasArticulatedSection { get; set; } 

        public Bus() { } 

        public override string GetCarType() => "Bus";

        public override string GetDescription() =>
            $"{Brand} {Model}: {PassengerCapacity} seats, Articulated: {HasArticulatedSection}";
    }
}