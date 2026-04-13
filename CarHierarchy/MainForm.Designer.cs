namespace CarHierarchy
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            propertyGrid = new PropertyGrid();
            listBoxDisplayCars = new ListBox();
            btnAddCar = new Button();
            btnDelete = new Button();
            btnSaveChange = new Button();
            btnSave = new Button();
            btnLoad = new Button();
            labelCarCount = new Label();
            SuspendLayout();
            // 
            // propertyGrid
            // 
            propertyGrid.Location = new Point(834, 12);
            propertyGrid.Name = "propertyGrid";
            propertyGrid.Size = new Size(303, 423);
            propertyGrid.TabIndex = 0;
            // 
            // listBoxDisplayCars
            // 
            listBoxDisplayCars.FormattingEnabled = true;
            listBoxDisplayCars.Location = new Point(12, 12);
            listBoxDisplayCars.Name = "listBoxDisplayCars";
            listBoxDisplayCars.Size = new Size(816, 424);
            listBoxDisplayCars.TabIndex = 1;
            listBoxDisplayCars.SelectedIndexChanged += listBoxDisplayCars_SelectedIndexChanged;
            // 
            // btnAddCar
            // 
            btnAddCar.Location = new Point(12, 442);
            btnAddCar.Name = "btnAddCar";
            btnAddCar.Size = new Size(185, 29);
            btnAddCar.TabIndex = 2;
            btnAddCar.Text = "Add Car";
            btnAddCar.UseVisualStyleBackColor = true;
            btnAddCar.Click += btnAddCar_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(12, 477);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(185, 29);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Delete Car";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnSaveChange
            // 
            btnSaveChange.Location = new Point(12, 512);
            btnSaveChange.Name = "btnSaveChange";
            btnSaveChange.Size = new Size(185, 29);
            btnSaveChange.TabIndex = 4;
            btnSaveChange.Text = "Save Changes";
            btnSaveChange.UseVisualStyleBackColor = true;
            btnSaveChange.Click += btnSaveChange_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(213, 442);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(615, 42);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save File";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(213, 499);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(615, 42);
            btnLoad.TabIndex = 6;
            btnLoad.Text = "Load File";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // labelCarCount
            // 
            labelCarCount.AutoSize = true;
            labelCarCount.Location = new Point(968, 518);
            labelCarCount.Name = "labelCarCount";
            labelCarCount.Size = new Size(50, 20);
            labelCarCount.TabIndex = 7;
            labelCarCount.Text = "label1";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1149, 547);
            Controls.Add(labelCarCount);
            Controls.Add(btnLoad);
            Controls.Add(btnSave);
            Controls.Add(btnSaveChange);
            Controls.Add(btnDelete);
            Controls.Add(btnAddCar);
            Controls.Add(listBoxDisplayCars);
            Controls.Add(propertyGrid);
            Name = "MainForm";
            Text = "CarHierarchy";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PropertyGrid propertyGrid; //for dispaly properties
        private ListBox listBoxDisplayCars; //for display cars list
        private Button btnAddCar; //for add car
        private Button btnDelete; //for delete car
        private Button btnSaveChange; //for save changes after change properies
        private Button btnSave; //for save xml
        private Button btnLoad; //for load xml
        private Label labelCarCount; //for display car count
    }
}
