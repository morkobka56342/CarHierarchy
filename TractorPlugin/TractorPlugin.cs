using CarHierarchyLib.Models;

namespace TractorPlugin
{
    public class Tractor : Car
    {
        public string EngineType { get; set; } 
        public bool HasBucket { get; set; }  

        public Tractor() { } 

        public override string GetCarType() => "Tractor";

        public override string GetDescription() =>
            $"{Brand} {Model} ({EngineType}): {(HasBucket ? "With bucket" : "No bucket")}";
    }
}