using CarHierarchyLib.Models;
using System.Collections.Generic;

namespace CarHierarchy.Services
{

    public interface ICarFactory
    {
        Car CreateCar(string typeName);
        IEnumerable<string> GetAvailableTypes();
    }
}