using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Zoorganize.Pages
{
    public enum BuildingType
    {
        Enclosure,
        Visitor,
        Worker
    }
    
    public class Building
    {
        public string Name { get; set; }
        public BuildingType Type { get; set; }
        public Building(string name, BuildingType type)
        {
            Name = name;
            Type = type;
        }
    }
    public partial class BuildingsPage : Form
    {
        private List<Building> buildings = new List<Building>();
        public BuildingsPage()
        {
            InitializeComponent();
            showBuildings.Size = new Size(400, 400);
        }

        //Zurück zum main menü
        private void button1_Click(object sender, EventArgs e)
        {
            MainPage mainPage = new MainPage();
            mainPage.Dock = DockStyle.Fill;
            mainPage.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(mainPage);
            mainPage.Show();

        }

        //fügt ein Gebäude hinzu, öffnet die Liste der Gebäude in dem Panel die zu dem erstellten gehören
        private void button2_Click(object sender, EventArgs e)
        {
            using (AddBuildingForm form = new AddBuildingForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    buildings.Add(form.CreatedBuilding);
                    showBuildings.Controls.Clear();
                    foreach (var building in buildings)
                    {
                        Button buildingButton = new Button
                        {
                            Text = building.Name,
                            AutoSize = true,
                            Tag = building
                        };
                        buildingButton.Click += BuildingButton_Click;
                        if (building.Type == form.CreatedBuilding.Type)
                        {
                            showBuildings.Controls.Add(buildingButton);
                        }
                    }

                }

            }
        }
        //Zeigt alle Gehege an
        private void penButton_Click(object sender, EventArgs e)
        {
            if (buildings.Count == 0)
            {
                MessageBox.Show("Keine Gebäude vorhanden.");
                return;
            }
            showBuildings.Controls.Clear();
            foreach (var building in buildings)
            {
                if (building.Type == BuildingType.Enclosure)
                {
                    Button buildingButton = new Button
                    {
                        Text = building.Name,
                        AutoSize = true,
                        Tag = building
                    };
                    buildingButton.Click += BuildingButton_Click;
                    showBuildings.Controls.Add(buildingButton);
                }
            }
        }
        //Zeigt alle Besucher Gebäude an
        private void visitorButton_Click(object sender, EventArgs e)
        {
            if (buildings.Count == 0)
            {
                MessageBox.Show("Keine Gehege vorhanden.");
                return;
            }
            showBuildings.Controls.Clear();
            foreach (var building in buildings)
            {
                if (building.Type == BuildingType.Visitor)
                {
                    Button buildingButton = new Button
                    {
                        Text = building.Name,
                        AutoSize = true,
                        Tag = building
                    };
                    buildingButton.Click += BuildingButton_Click;
                    showBuildings.Controls.Add(buildingButton);
                }
            }
        }
        //Zeigt alle Mitarbeiter Gebäude an
        private void workerButton_Click(object sender, EventArgs e)
        {
            if (buildings.Count == 0)
            {
                MessageBox.Show("Keine Gehege vorhanden.");
                return;
            }
            showBuildings.Controls.Clear();
            foreach (var building in buildings)
            {
                if (building.Type == BuildingType.Worker)
                {
                    Button buildingButton = new Button
                    {
                        Text = building.Name,
                        AutoSize = true,
                        Tag = building
                    };
                    buildingButton.Click += BuildingButton_Click;
                    showBuildings.Controls.Add(buildingButton);
                }
            }
        }

        //Button funktion zum anzeigen
        private void BuildingButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Building building = btn.Tag as Building;
            MessageBox.Show($"Gebäude: {building.Name}\nTyp: {building.Type}");
        }

        //Form für die Eingabe der neuen Gebäude Informationen
        public partial class AddBuildingForm : Form
        {
            public Building CreatedBuilding { get; private set; }
            TextBox txtName;
            RadioButton rbEnclosure;
            RadioButton rbVisitor;
            RadioButton rbWorker;
            Button submit;

            public AddBuildingForm()
            {
                rbEnclosure = new RadioButton();
                rbEnclosure.Checked = true; // default selection
                rbVisitor = new RadioButton();
                rbWorker = new RadioButton();
                txtName = new TextBox();
                submit = new Button();

                rbEnclosure.Text = "Gehege";
                rbVisitor.Text = "Besucher Gebäude";
                rbWorker.Text = "Mitarbeiter Gebäude";
                txtName.PlaceholderText = "Name des Gebäudes";
                submit.Text = "Gebäude hinzufügen";
                submit.AutoSize = true;
                rbEnclosure.AutoSize = true;
                rbVisitor.AutoSize = true;
                rbWorker.AutoSize = true;

                txtName.Location = new Point(20, 20);
                rbEnclosure.Location = new Point(20, 50);
                rbVisitor.Location = new Point(20, 80);
                rbWorker.Location = new Point(20, 110);
                submit.Location = new Point(20, 150);

                submit.Click += btnSubmit_Click;
                Controls.Add(txtName);
                Controls.Add(rbEnclosure);
                Controls.Add(rbVisitor);
                Controls.Add(rbWorker);
                Controls.Add(submit);
            }

            //Fügt das erstellte object in die Liste der Gebäude ein, abhängig von der Auswahl des Typs
            private void btnSubmit_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Please enter a name.");
                    return;
                }

                BuildingType type;

                if (rbEnclosure.Checked)
                    type = BuildingType.Enclosure;
                else if (rbVisitor.Checked)
                    type = BuildingType.Visitor;
                else
                    type = BuildingType.Worker;

                CreatedBuilding = new Building(txtName.Text, type);

                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }

}
