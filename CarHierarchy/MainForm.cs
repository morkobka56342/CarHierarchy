using CarHierarchyLib.Models;
using CarHierarchy.Services;
using CarHierarchy.Forms;
using PluginBase;
using CarHierarchy.Patterns.Adapter;
using CarHierarchy.Patterns.Observer;
using CarHierarchy.Patterns.Singleton;

namespace CarHierarchy
{
    public partial class MainForm : Form, ICarListObserver
    {
        private CarListSubject _carSubject;  
        private readonly ICarFactory _carFactory;
        private List<IDataTransformer> _transformers = new List<IDataTransformer>();

        public MainForm()
        {
            InitializeComponent();

    
            _carSubject = new CarListSubject();
            _carSubject.Subscribe(this); 

            _carFactory = new CarFactory();

            RefreshTransformerList();
            
        }

     
        public void OnCarsChanged(IReadOnlyList<Car> cars)
        {
            
            UpdateCarList();
            UpdateCarCount();
        }

        private void RefreshTransformerList()
        {
            _transformers.Clear();
            comboBoxPlugins.Items.Clear();
            comboBoxPlugins.Items.Add("None (Standard XML)");

            string pluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            if (!Directory.Exists(pluginsPath)) return;

            foreach (var file in Directory.GetFiles(pluginsPath, "*.dll"))
            {
                try
                {
                    var assembly = System.Reflection.Assembly.LoadFrom(Path.GetFullPath(file));
                    var types = assembly.GetTypes().Where(t => !t.IsInterface && !t.IsAbstract);

                    foreach (var type in types)
                    {
                        if (typeof(IDataTransformer).IsAssignableFrom(type))
                        {
                            var instance = (IDataTransformer)Activator.CreateInstance(type);
                            _transformers.Add(instance);
                            comboBoxPlugins.Items.Add(instance.Name);
                        }
                        else if (type.GetInterface("IDataProcessorPlugin") != null)
                        {
                            object foreignPlugin = Activator.CreateInstance(type);

                            var nameProp = type.GetProperty("ProcessorName");
                            var method = type.GetMethod("ProcessBeforeSave");

                            if (nameProp != null && method != null)
                            {
                                string name = nameProp.GetValue(foreignPlugin)?.ToString() ?? "Unknown Plugin";

                                Func<string, string> transformFunc = (xml) =>
                                    (string)method.Invoke(foreignPlugin, new object[] { xml });

                                var adapter = new ReflectionDataProcessorAdapter(name, transformFunc);
                                _transformers.Add(adapter);
                                comboBoxPlugins.Items.Add(adapter.Name);
                            }
                        }
                    }
                }
                catch { }
            }

            if (comboBoxPlugins.Items.Count > 0)
                comboBoxPlugins.SelectedIndex = 0;
        }

        private void UpdateCarList()
        {
            listBoxDisplayCars.DataSource = null;
            listBoxDisplayCars.DataSource = _carSubject.Cars.ToList();  // Берем список из Subject
        }

        private void UpdateCarCount()
        {
            labelCarCount.Text = $"Total vehicles: {_carSubject.Cars.Count}";
        }

        private void ClearPropertyGrid()
        {
            propertyGrid.SelectedObject = null;
        }

        public void listBoxDisplayCars_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxDisplayCars.SelectedItem is Car selected)
            {
                propertyGrid.SelectedObject = selected;
            }
            else
            {
                ClearPropertyGrid();
            }
        }

        public void btnAddCar_Click(object sender, EventArgs e)
        {
            using (var dialog = new AddCarDialog(_carFactory))
            {
                if (dialog.ShowDialog() == DialogResult.OK && dialog.CreatedCar != null)
                {
                    _carSubject.AddCar(dialog.CreatedCar);  
                    
                }
            }
        }

        public void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxDisplayCars.SelectedItem is Car selected)
            {
                DialogResult result = MessageBox.Show(
                    $"Delete {selected.Brand} {selected.Model}?",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _carSubject.RemoveCar(selected);  
                    ClearPropertyGrid();
                   
                }
            }
        }

        public void btnSaveChange_Click(object sender, EventArgs e)
        {
            
            _carSubject.NotifyChanged();
            MessageBox.Show("Property changes applied!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            if (_carSubject.Cars.Count == 0) return;

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                string selectedName = comboBoxPlugins.SelectedItem.ToString();
                bool isJsonOutput = selectedName.Contains("JSON");

                if (isJsonOutput)
                {
                    dialog.Filter = "JSON files (*.json)|*.json";
                    dialog.DefaultExt = "json";
                    dialog.FileName = "cars.json";
                }
                else
                {
                    dialog.Filter = "XML files (*.xml)|*.xml";
                    dialog.DefaultExt = "xml";
                    dialog.FileName = "cars.xml";
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var types = ((CarFactory)_carFactory).GetAllRegisteredTypes();

                      
                        var serializer = SerializerSingleton.Instance;
                        string dataToSave = serializer.SerializeToString(_carSubject.Cars.ToList(), types);

                        if (comboBoxPlugins.SelectedIndex > 0)
                        {
                            var activePlugin = _transformers.FirstOrDefault(p => p.Name == selectedName);
                            if (activePlugin != null)
                            {
                                dataToSave = activePlugin.TransformBeforeSave(dataToSave);
                            }
                        }

                        File.WriteAllText(dialog.FileName, dataToSave);

                        MessageBox.Show("File saved successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Save error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "XML files (*.xml)|*.xml";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var types = ((CarFactory)_carFactory).GetAllRegisteredTypes();

                        string fileContent = File.ReadAllText(dialog.FileName);

                   
                        var serializer = SerializerSingleton.Instance;
                        var loadedCars = serializer.DeserializeFromString(fileContent, types);

                        
                        _carSubject.SetCars(loadedCars);

                        comboBoxPlugins.Enabled = true;

                        MessageBox.Show("Data loaded successfully from XML!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: File must be a valid XML. {ex.Message}", "Load Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        public void btnAddPlugin_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "C# Plugins (*.dll)|*.dll";
                dialog.Title = "Select plugin file (DLL)";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string pluginsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
                        if (!Directory.Exists(pluginsDir)) Directory.CreateDirectory(pluginsDir);

                        string destFile = Path.Combine(pluginsDir, Path.GetFileName(dialog.FileName));

                        if (Path.GetFullPath(dialog.FileName) != Path.GetFullPath(destFile))
                        {
                            File.Copy(dialog.FileName, destFile, true);
                        }

                        var factory = _carFactory as CarFactory;
                        if (factory != null)
                        {
                            factory.RegisterPluginFromFile(destFile);
                            RefreshTransformerList();
                            MessageBox.Show("Plugin (DLL) successfully added!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading DLL: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void comboBoxPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPlugin = comboBoxPlugins.SelectedItem.ToString();

            if (selectedPlugin.Contains("Sort"))
            {
                _carSubject.SortByBrand();  
               
            }
        }

    
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _carSubject.Unsubscribe(this);  
            base.OnFormClosed(e);
        }
    }
}