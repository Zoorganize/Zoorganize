using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Zoorganize.Pages
{
    //Das ersetzen durch deine Klassen
    class Animal
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public int Age { get; set; }
        public string Habitat { get; set; }
    }
    public partial class AnimalsPage : Form
    {
        //Platzhalter für liste von Animal[] 
        List<Animal> animals = new List<Animal>();
        public AnimalsPage()
        {
            InitializeComponent();
        }

        //Button zum zurückkehren zum Hauptmenü
        private void button1_Click(object sender, EventArgs e)
        {
            MainPage mainPage = new MainPage();
            mainPage.Dock = DockStyle.Fill;
            mainPage.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(mainPage);
            mainPage.Show();
        }

        //Button der eine neue  Instanz von Animal erstellt und diese in die Liste der Tiere hinzufügt
        //Dieser Button öffnet ein Fenster, wo Informationen über das Tier eingegeben werden können
        private void addAnimal_Click(object sender, EventArgs e)
        {
            using (var dlg = new AnimalForm())
            {
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.ShowInTaskbar = false;
                dlg.StartPosition = FormStartPosition.CenterParent;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    animals.Add(dlg.animal);
                    typeOverview.Controls.Clear();
                    var uniqueSpecies = animals.Select(a => a.Species).Distinct();

                    foreach (var species in uniqueSpecies)
                    {
                        Button speciesButton = new Button
                        {
                            Text = species,
                            AutoSize = true,
                            Tag = species
                        };

                        speciesButton.Click += SpeciesButton_Click;

                        typeOverview.Controls.Add(speciesButton);
                    }
                    
                }
            }
        }

        //Wenn auf eine Tierart geklickt wird, werden alle Tiere dieser Art in der Tierübersicht angezeigt
        private void SpeciesButton_Click(object? sender, EventArgs e)
        {
            animalOverview.Controls.Clear();
            foreach (var animal in animals)
            {
                if (animal.Species == (sender as Button)?.Tag as string)
                {
                    Button animalButton = new Button
                    {
                        Text = animal.Name,
                        AutoSize = true,
                        Tag = animal
                    };
                    animalButton.Click += AnimalButton_Click;
                    animalOverview.Controls.Add(animalButton);
                }
            }
        }

        //Wenn auf ein Tier geklickt wird, öffnet sich ein Fenster, welches Informationen über das Tier anzeigt
        private void AnimalButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show($"Name: {((sender as Button)?.Tag is Animal animal ? animal.Name : "Unknown")}\n" +
                            $"Species: {((sender as Button)?.Tag is Animal animal2 ? animal2.Species : "Unknown")}\n" +
                            $"Age: {((sender as Button)?.Tag is Animal animal3 ? animal3.Age.ToString() : "Unknown")}\n" +
                            $"Habitat: {((sender as Button)?.Tag is Animal animal4 ? animal4.Habitat : "Unknown")}");
        }


        //Das ist das Formular, welches geöffnet wird, hier müssen die Informationen über das Tier eingegeben werden, damit es zur Liste der Tiere hinzugefügt werden kann
        //Input ist noch nicht Optional
        class AnimalForm : Form
        {
            TextBox name = new TextBox();
            TextBox species = new TextBox();
            TextBox age = new TextBox();
            TextBox habitat = new TextBox();
            Button submit = new Button();
            public Animal animal { get; private set; }
            public AnimalForm()
            {
                Text = "Tier hinzufügen";
                Width = 300;
                Height = 300;
                name.Text = "Name";
                name.Left = 20;
                name.Top = 20;
                species.Left = 20;
                species.Top = 60;
                age.Left = 20;
                age.Top = 100;
                habitat.Left = 20;
                habitat.Top = 140;
                species.Text = "Species";
                age.Text = "Age";
                habitat.Text = "habitat";
                submit.Text = "Submit";
                submit.Left = 20;
                submit.Top = 200;
                submit.Click += Submit_Click;
                Controls.Add(name);
                Controls.Add(species);
                Controls.Add(age);
                Controls.Add(habitat);
                Controls.Add(submit);
            }
            private void Submit_Click(object? sender, EventArgs e)
            {
                //Hier würde die Logik zum Hinzufügen des Tieres zur Liste der Tiere implementiert werden
                try 
                {
                    int.Parse(age.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Bitte geben Sie eine gültige Zahl für das Alter ein.");
                    return;
                }
                Animal newAnimal = new Animal
                {
                    Name = name.Text,
                    Species = species.Text,
                    Age = int.Parse(age.Text),
                    Habitat = habitat.Text
                };

                animal = newAnimal;
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
