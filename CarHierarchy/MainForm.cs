using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CarHierarchy.Models;
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
            // Pass the factory to the dialog via constructor
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
            else
            {
                MessageBox.Show("Please select a vehicle to delete!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                // settings for save
                dialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                dialog.DefaultExt = "xml";
                dialog.FileName = "cars.xml";
                dialog.Title = "Save Vehicle List";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _serializer.Serialize(dialog.FileName, _cars);
                        MessageBox.Show($"Successfully saved {_cars.Count} vehicles!\nFile: {dialog.FileName}",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                dialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                dialog.Title = "Load Vehicle List";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _cars = _serializer.Deserialize(dialog.FileName);
                        UpdateCarList();
                        UpdateCarCount();
                        ClearPropertyGrid();
                        MessageBox.Show($"Successfully loaded {_cars.Count} vehicles!",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Load error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}