using CarHierarchyLib.Models;
using CarHierarchy.Services;
using CarHierarchy.Forms;

namespace CarHierarchy
{
    public partial class MainForm : Form
    {
 
        private List<Car> _cars;
        private XmlCarSerializer _serializer;
        private readonly ICarFactory _carFactory;

        public MainForm()
        {
            InitializeComponent();

            _cars = new List<Car>();
            _serializer = new XmlCarSerializer();
            _carFactory = new CarFactory();

            UpdateCarList();
            UpdateCarCount();
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
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                dialog.DefaultExt = "xml";
                dialog.FileName = "cars.xml";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var types = ((CarFactory)_carFactory).GetAllRegisteredTypes();
                        _serializer.Serialize(dialog.FileName, _cars, types);

                        MessageBox.Show("Saved successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Save error: {ex.Message}");
                    }
                }
            }
        }

        public void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var types = ((CarFactory)_carFactory).GetAllRegisteredTypes();
                        _cars = _serializer.Deserialize(dialog.FileName, types);

                        UpdateCarList();
                        UpdateCarCount();
                        ClearPropertyGrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Load error: {ex.Message}");
                    }
                }
            }
        }
    }
}