using CarHierarchyLib.Models;
using CarHierarchy.Services;
using CarHierarchy.Forms;
using PluginBase;

namespace CarHierarchy
{
    public partial class MainForm : Form
    {

        private List<Car> _cars;
        private XmlCarSerializer _serializer;
        private readonly ICarFactory _carFactory;
        private List<IDataTransformer> _transformers = new List<IDataTransformer>();

        public MainForm()
        {
            InitializeComponent();

            _cars = new List<Car>();
            _serializer = new XmlCarSerializer();
            _carFactory = new CarFactory();

            RefreshTransformerList(); 
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

                    var pluginTypes = assembly.GetTypes()
                        .Where(t => typeof(IDataTransformer).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var type in pluginTypes)
                    {
                        var instance = (IDataTransformer)Activator.CreateInstance(type);
                        _transformers.Add(instance);
                        comboBoxPlugins.Items.Add(instance.Name);
                    }
                }
                catch
                {
                }
            }

            if (comboBoxPlugins.Items.Count > 0)
                comboBoxPlugins.SelectedIndex = 0;
        }

        private void UpdateCarList()
        {
            listBoxDisplayCars.DataSource = null;
            listBoxDisplayCars.DataSource = _cars;
        }

        private void UpdateCarCount()
        {
            labelCarCount.Text = $"Total vehicles: {_cars.Count}";
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
                    _cars.Add(dialog.CreatedCar);
                    UpdateCarList();
                    UpdateCarCount();
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
                    _cars.Remove(selected);
                    UpdateCarList();
                    UpdateCarCount();
                    ClearPropertyGrid();
                }
            }
        }

        public void btnSaveChange_Click(object sender, EventArgs e)
        {
            UpdateCarList();
            UpdateCarCount();
            MessageBox.Show("Property changes applied!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            if (_cars == null || _cars.Count == 0) return;

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

                        string dataToSave = _serializer.SerializeToString(_cars, types);

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
                        _cars = _serializer.DeserializeFromString(fileContent, types);

                        UpdateCarList();
                        UpdateCarCount();

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
                string selectedPlugin = comboBoxPlugins.SelectedItem.ToString();

                if (selectedPlugin.Contains("JSON"))
                {
                    dialog.Filter = "JSON files (*.json)|*.json";
                    dialog.DefaultExt = "json";
                }
                else
                {
                    dialog.Filter = "XML files (*.xml)|*.xml";
                    dialog.DefaultExt = "xml";
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var factory = _carFactory as CarFactory;
                        if (factory != null)
                        {
                            factory.RegisterPluginFromFile(dialog.FileName);

                            string pluginsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
                            if (!Directory.Exists(pluginsDir))
                            {
                                Directory.CreateDirectory(pluginsDir);
                            }

                            string destFile = Path.Combine(pluginsDir, Path.GetFileName(dialog.FileName));

                            if (Path.GetFullPath(dialog.FileName) != Path.GetFullPath(destFile))
                            {
                                File.Copy(dialog.FileName, destFile, true);
                            }

                            RefreshTransformerList();
                       

                            MessageBox.Show("Plugin successfully added and registered!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding plugin: {ex.Message}", "Error",
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
                _cars = _cars.OrderBy(c => c.Brand).ThenBy(c => c.Model).ToList();

                UpdateCarList();
            }
        }
    }
}