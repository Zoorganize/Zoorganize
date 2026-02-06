using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Zoorganize.Pages
{
    //ersezen durch deine Klasse
    class Worker
    {
        public string Name { get; set; }
        public string Birthday { get; set; }
        public int Wage { get; set; }
        public string Notes { get; set; }
    }
    public partial class WorkersPage : Form
    {
        List<Worker> worker = new List<Worker>();
        public WorkersPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainPage mainPage = new MainPage();
            mainPage.Dock = DockStyle.Fill;
            mainPage.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(mainPage);
            mainPage.Show();
        }

        private void AddWorker_Click(object sender, EventArgs e)
        {
            using (var dlg = new WorkerForm())
            {
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.ShowInTaskbar = false;
                dlg.StartPosition = FormStartPosition.CenterParent;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    worker.Add(dlg.worker);
                    workerOverview.Controls.Clear();

                    foreach (var worker in worker)
                    {
                        Button workersButton = new Button
                        {
                            Text = worker.Name,
                            AutoSize = true,
                            Tag = worker
                        };

                        workersButton.Click += workersButton_Click;

                        workerOverview.Controls.Add(workersButton);
                    }
                }
            }
        }

        private void workersButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show($"Name: {((sender as Button)?.Tag is Worker worker ? worker.Name : "Unknown")}\n" +
                $"Birthday: {((sender as Button)?.Tag is Worker worker1 ? worker1.Birthday : "Unknown")}\n" +
                $"Wage: {((sender as Button)?.Tag is Worker worker2 ? worker2.Wage.ToString() : "Unknown")}\n" +
                $"Notes: {((sender as Button)?.Tag is Worker worker3 ? worker3.Notes : "Unknown")}");
        }


        //ergänzen mit den anderen attributen
        class WorkerForm : Form
        {
            TextBox name = new TextBox();
            TextBox birthday = new TextBox();
            TextBox wage = new TextBox();
            TextBox notes = new TextBox();
            Button submit = new Button();
            public Worker worker { get; private set; }
            public WorkerForm()
            {
                Text = "Arbeiter Hinzufügen";
                Width = 300;
                Height = 300;
                name.Text = "Name";
                name.Left = 20;
                name.Top = 20;
                birthday.Left = 20;
                birthday.Top = 60;
                wage.Left = 20;
                wage.Top = 100;
                notes.Left = 20;
                notes.Top = 140;
                birthday.Text = "Birthday";
                wage.Text = "Wage";
                notes.Text = "Notes";
                submit.Text = "Submit";
                submit.Left = 20;
                submit.Top = 200;
                submit.Click += Submit_Click;
                Controls.Add(name);
                Controls.Add(birthday);
                Controls.Add(wage);
                Controls.Add(notes);
                Controls.Add(submit);
            }
            private void Submit_Click(object? sender, EventArgs e)
            {
                Worker newWorker = new Worker
                {
                    Name = name.Text,
                    Birthday = birthday.Text,
                    Wage = int.Parse(wage.Text),
                    Notes = notes.Text
                };

                worker = newWorker;
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
