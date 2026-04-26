namespace CarHierarchyLib.Models
{
    public interface ICar
    {
        string Brand { get; set; }
        string Model { get; set; }
        int Year { get; set; }
        decimal Price { get; set; }
    }
}