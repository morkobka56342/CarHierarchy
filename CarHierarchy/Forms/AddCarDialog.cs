using System;
using System.Linq;
using System.Windows.Forms;
using CarHierarchyLib.Models;
using CarHierarchy.Services;

namespace CarHierarchy.Forms
{
    public partial class AddCarDialog : Form
    {
        private readonly ICarFactory _carFactory;
        public Car CreatedCar { get; private set; }

        public AddCarDialog(ICarFactory carFactory)
        {
            InitializeComponent();
            _carFactory = carFactory;

            comboBoxType.Items.Clear();
            comboBoxType.Items.AddRange(_carFactory.GetAvailableTypes().ToArray());

            comboBoxType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxType.SelectedIndex = 0;

            comboBoxType.SelectedIndexChanged += (s, e) => UpdatePropertyGrid();
            btnOK.Click += btnOK_Click;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            UpdatePropertyGrid();
        }

        private void UpdatePropertyGrid()
        {
            try
            {
                string selectedType = comboBoxType.SelectedItem?.ToString();
                if (!string.IsNullOrEmpty(selectedType))
                {
                    CreatedCar = _carFactory.CreateCar(selectedType);
                    propertyGridCar.SelectedObject = CreatedCar;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing type: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (CreatedCar == null || string.IsNullOrWhiteSpace(CreatedCar.Brand))
            {
                MessageBox.Show("Please fill in the vehicle details in the property grid.",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}