using Microsoft.EntityFrameworkCore;
using System.Data;
using Zoorganize.Database;
using Zoorganize.Functions;

namespace Zoorganize.Pages
{
    public partial class MainPage : Form
    {
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

            context = new AppDbContext();

            
            animalFunctions = new AnimalFunctions(context, null);  
            staffFunctions = new StaffFunctions(animalFunctions, context);
            animalFunctions.SetKeeperFunctions(staffFunctions); 
            roomFunctions = new RoomFunctions(context);

            LoadAppointments();
        }

        private async void LoadAppointments()
        {
            try
            {
                // Lade alle Termine aus der Datenbank
                var appointments = await animalFunctions.GetUpcomingAppointments();

                if (appointments.Count == 0)
                {
                    appointmentList.Text = "Keine bevorstehenden Termine.";
                    return;
                }

                // Formatiere Termine für die Anzeige
                var appointmentTexts = appointments.Select(a =>
                    $"• {a.AppointmentDate:dd.MM.yyyy} - {a.Title}\n" +
                    $"  Tier: {a.Animal?.Name ?? "Unbekannt"}\n" +
                    (!string.IsNullOrWhiteSpace(a.Description) ? $"  {a.Description}\n" : "")
                );

                appointmentList.Text = string.Join(Environment.NewLine, appointmentTexts);
            }
            catch (Exception ex)
            {
                appointmentList.Text = $"Fehler beim Laden der Termine: {ex.Message}";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            AnimalsPage animals = new(this.animalFunctions, this.roomFunctions, this.staffFunctions)
            {
                Dock = DockStyle.Fill,
                TopLevel = false
            };
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(animals);
            animals.Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            BuildingsPage building = new(this.roomFunctions)
            {
                Dock = DockStyle.Fill,
                TopLevel = false
            };
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(building);
            building.Show();

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            WorkersPage worker = new(this.staffFunctions, this.animalFunctions)
            {
                Dock = DockStyle.Fill,
                TopLevel = false
            };
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(worker);
            worker.Show();
        }
    }
}
