namespace CarHierarchy.Forms
{
    partial class AddCarDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            btnOK = new Button();
            btnCancel = new Button();
            propertyGridCar = new PropertyGrid();
            comboBoxType = new ComboBox();
            SuspendLayout();

            // btnOK
            btnOK.Location = new Point(99, 395);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(94, 29);
            btnOK.TabIndex = 11;
            btnOK.Text = "OK";

            // btnCancel
            btnCancel.Location = new Point(241, 395);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 12;
            btnCancel.Text = "Cancel";

            // propertyGridCar
            propertyGridCar.Location = new Point(12, 73);
            propertyGridCar.Name = "propertyGridCar";
            propertyGridCar.Size = new Size(434, 300);
            propertyGridCar.TabIndex = 13;

            // comboBoxType
            comboBoxType.Location = new Point(19, 11);
            comboBoxType.Name = "comboBoxType";
            comboBoxType.Size = new Size(427, 28);
            comboBoxType.TabIndex = 14;

            // AddCarDialog
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(458, 449);
            Controls.Add(comboBoxType);
            Controls.Add(propertyGridCar);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddCarDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add Car";
            ResumeLayout(false);
        }

        #endregion

        private Button btnOK;
        private Button btnCancel;
        private PropertyGrid propertyGridCar;
        private ComboBox comboBoxType;
    }
}