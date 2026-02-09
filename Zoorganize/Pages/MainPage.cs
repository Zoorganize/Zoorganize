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
        private readonly StaffFunctions staffFunctions;
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

            
            animalFunctions = new AnimalFunctions(context, null);  
            staffFunctions = new StaffFunctions(animalFunctions, context);
            animalFunctions.SetKeeperFunctions(staffFunctions); 
            roomFunctions = new RoomFunctions(context);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AnimalsPage animals = new AnimalsPage(this.animalFunctions, this.roomFunctions, this.staffFunctions);
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
            WorkersPage worker = new WorkersPage(this.staffFunctions, this.animalFunctions);
            worker.Dock = DockStyle.Fill;
            worker.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(worker);
            worker.Show();
        }
    }
}
