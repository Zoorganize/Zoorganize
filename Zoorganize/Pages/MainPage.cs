using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Zoorganize.Pages
{
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AnimalsPage animals = new AnimalsPage();
            animals.Dock = DockStyle.Fill;
            animals.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(animals);
            animals.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BuildingsPage building = new BuildingsPage();
            building.Dock = DockStyle.Fill;
            building.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(building);
            building.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            WorkersPage worker = new WorkersPage();
            worker.Dock = DockStyle.Fill;
            worker.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(worker);
            worker.Show();
        }
    }
}
