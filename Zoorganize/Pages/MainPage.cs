using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Zoorganize.Database;
using Zoorganize.Functions;

namespace Zoorganize.Pages
{
    public partial class MainPage : Form
    {
        //TODO
        string[] appointments = {"TEst line", "Test line2", "heres an appointment"};

        private readonly AppDbContext context;
        private readonly KeeperFunctions keeperFunctions;
        private readonly AnimalFunctions animalFunctions;
        private readonly RoomFunctions roomFunctions;

        public MainPage()
        {
            InitializeComponent();
            appointmentList.Multiline = true;
            appointmentList.ScrollBars = ScrollBars.Vertical;
            appointmentList.ReadOnly = true;
            appointmentList.Text = string.Join(Environment.NewLine, appointments);

            context = new AppDbContext();

            // Zeige den GENAUEN Pfad der verwendeten Datenbank
            var dbPath = context.Database.GetDbConnection().DataSource;
            var fullPath = System.IO.Path.GetFullPath(dbPath);
            MessageBox.Show($"Verwendete Datenbank:\n{fullPath}\n\nExistiert: {System.IO.File.Exists(fullPath)}");
            animalFunctions = new AnimalFunctions(context, null);  
            keeperFunctions = new KeeperFunctions(animalFunctions, context);
            animalFunctions.SetKeeperFunctions(keeperFunctions); 
            roomFunctions = new RoomFunctions(context);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AnimalsPage animals = new AnimalsPage(this.animalFunctions);
            animals.Dock = DockStyle.Fill;
            animals.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(animals);
            animals.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BuildingsPage building = new BuildingsPage(this.roomFunctions);
            building.Dock = DockStyle.Fill;
            building.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(building);
            building.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            WorkersPage worker = new WorkersPage(this.keeperFunctions);
            worker.Dock = DockStyle.Fill;
            worker.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(worker);
            worker.Show();
        }
    }
}
