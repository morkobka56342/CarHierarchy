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
                //only .dll
                foreach (var file in Directory.GetFiles(pluginsPath, "*.dll"))
                {
                    try
                    {
                        //load to process
                        var assembly = Assembly.LoadFrom(Path.GetFullPath(file));
                        RegisterAllCarsFromAssembly(assembly);
                    }
                    catch {
                        //skip      
                          }
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
                    // find constructor and create new object
                    var instance = (Car)Activator.CreateInstance(type);
                    string typeName = instance.GetCarType();
                    // dublicates
                    if (!_pluginTypes.ContainsKey(typeName))
                    {
                        _pluginTypes.Add(typeName, type);
                    }
                }
                catch { 
                        // skip
                    }
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

        public void RegisterPluginFromFile(string filePath)
        {
            try
            {
                var assembly = Assembly.LoadFrom(Path.GetFullPath(filePath));
                RegisterAllCarsFromAssembly(assembly);
            }
            catch (Exception ex)
            {
                throw new Exception($"Can not upload file: {ex.Message}");
            }
        }

        public IEnumerable<string> GetAvailableTypes() => _pluginTypes.Keys;

        public IEnumerable<Type> GetAllRegisteredTypes() => _pluginTypes.Values;
    }
}