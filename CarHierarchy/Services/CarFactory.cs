using System.Reflection;
using CarHierarchyLib.Models;

namespace CarHierarchy.Services
{
    public class CarFactory : ICarFactory
    {
        private readonly Dictionary<string, Type> _pluginTypes = new Dictionary<string, Type>();

        public CarFactory(string pluginsPath = "Plugins")
        {
            RegisterAllCarsFromAssembly(typeof(Car).Assembly);

            if (Directory.Exists(pluginsPath))
            {
                foreach (var file in Directory.GetFiles(pluginsPath, "*.dll"))
                {
                    try
                    {
                        var assembly = Assembly.LoadFrom(Path.GetFullPath(file));
                        RegisterAllCarsFromAssembly(assembly);
                    }
                    catch { }
                }
            }
        }

        private void RegisterAllCarsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Car)) && !t.IsAbstract);

            foreach (var type in types)
            {
                try
                {
                    var instance = (Car)Activator.CreateInstance(type);
                    string typeName = instance.GetCarType();
                    if (!_pluginTypes.ContainsKey(typeName))
                    {
                        _pluginTypes.Add(typeName, type);
                    }
                }
                catch { }
            }
        }

        public Car CreateCar(string typeName)
        {
            if (_pluginTypes.TryGetValue(typeName, out var type))
            {
                return (Car)Activator.CreateInstance(type);
            }
            throw new ArgumentException($"Type {typeName} is not found.");
        }

        public IEnumerable<string> GetAvailableTypes() => _pluginTypes.Keys;

        public IEnumerable<Type> GetAllRegisteredTypes() => _pluginTypes.Values;
    }
}