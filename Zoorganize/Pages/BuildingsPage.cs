using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Zoorganize.Database.Models;
using Zoorganize.Functions;
using Zoorganize.Models.Api;
using static Zoorganize.Pages.WorkersPage;

namespace Zoorganize.Pages
{
    public partial class BuildingsPage : Form
    {
        private readonly RoomFunctions roomFunctions;

        private List<AnimalEnclosure> animalEnclosures = [];
        private List<StaffRooms> staffRooms = [];
        private List<VisitorRoom> visitorRooms = [];
        public BuildingsPage(RoomFunctions roomFunctions)
        {
            this.roomFunctions = roomFunctions;
            InitializeComponent();
            showBuildings.Size = new Size(400, 400);
            LoadRooms();
        }
        private async void LoadRooms()
        {
            try
            {
                // Alle Räume aus der Datenbank laden
                animalEnclosures = await roomFunctions.GetAnimalEnclosures();
                staffRooms = await roomFunctions.GetStaffRooms();
                visitorRooms = await roomFunctions.GetVisitorRooms();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Räume: {ex.Message}", "Fehler");
            }
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

        private void delete_Click(object sender, EventArgs e)
        {
            var allRooms = new List<Room>();
            allRooms.AddRange(animalEnclosures);
            allRooms.AddRange(staffRooms);
            allRooms.AddRange(visitorRooms);
            using (DeleteBuildingsForm form = new DeleteBuildingsForm(allRooms, roomFunctions))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadRooms();
                    showBuildings.Controls.Clear();

                }
            }
        }

        //fügt ein Gebäude hinzu, öffnet die Liste der Gebäude in dem Panel die zu dem erstellten gehören
        private void button2_Click(object sender, EventArgs e)
        {
            using (AddBuildingForm form = new AddBuildingForm(roomFunctions))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadRooms();
                    ShowRoomsByType(form.CreatedRoomType);

                }

            }
        }
        //Zeigt alle Gehege an
        private void penButton_Click(object sender, EventArgs e)
        {
            ShowRoomsByType(RoomType.AnimalEnclosure);
        }
        //Zeigt alle Besucher Gebäude an
        private void visitorButton_Click(object sender, EventArgs e)
        {
            ShowRoomsByType(RoomType.VisitorRoom);
        }
        //Zeigt alle Mitarbeiter Gebäude an
        private void workerButton_Click(object sender, EventArgs e)
        {
            ShowRoomsByType(RoomType.StaffRoom);
        }

        //Button funktion zum anzeigen
        private void BuildingButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Room room = btn.Tag as Room;
            MessageBox.Show($"Gebäude: {room.Name}\nTyp: {room.Type}");
        }

        private void ShowRoomsByType(RoomType type)
        {
            showBuildings.Controls.Clear();

            List<Room> roomsToShow = type switch
            {
                RoomType.AnimalEnclosure => animalEnclosures.Cast<Room>().ToList(),
                RoomType.StaffRoom => staffRooms.Cast<Room>().ToList(),
                RoomType.VisitorRoom => visitorRooms.Cast<Room>().ToList(),
                _ => new List<Room>()
            };

            if (!roomsToShow.Any())
            {
                MessageBox.Show($"Keine {GetRoomTypeDisplayName(type)} vorhanden.", "Information");
                return;
            }

            foreach (var room in roomsToShow.OrderBy(r => r.Name))
            {
                Button roomButton = new Button
                {
                    Text = room.Name,
                    AutoSize = true,
                    Tag = room,
                    Width = 150,
                    Margin = new Padding(5)
                };
                roomButton.Click += RoomButton_Click;
                showBuildings.Controls.Add(roomButton);
            }
        }
        private string GetRoomTypeDisplayName(RoomType type)
        {
            return type switch
            {
                RoomType.AnimalEnclosure => "Gehege",
                RoomType.StaffRoom => "Mitarbeiterräume",
                RoomType.VisitorRoom => "Besucherräume",
                _ => "Räume"
            };
        }

        private void RoomButton_Click(object sender, EventArgs e)
        {
            if ((sender as Button)?.Tag is Room room)
            {
                ShowRoomDetails(room);
            }
        }
        private void ShowRoomDetails(Room room)
        {
            string details = $"Name: {room.Name}\n";
            details += $"Standort: {room.Location ?? "Nicht angegeben"}\n";
            details += $"Beschreibung: {room.Description ?? "Keine"}\n";
            details += $"Fläche: {room.AreaInSquareMeters?.ToString() ?? "Nicht angegeben"} m²\n";
            details += $"Status: {room.Status}\n\n";

            // Spezifische Details basierend auf Typ
            if (room is AnimalEnclosure enclosure)
            {
                details += "═══ GEHEGE-DETAILS ═══\n";
                details += $"Kapazität: {enclosure.MaxCapacity} Tiere\n";
                details += $"Außengehege: {(enclosure.IsOutdoor ? "Ja" : "Nein")}\n";
                details += $"Temperaturkontrolle: {(enclosure.TemperatureControlled ? "Ja" : "Nein")}\n";
                details += $"Temperatur: {enclosure.MinTemperature}°C - {enclosure.MaxTemperature}°C\n";
                details += $"Sicherheitsstufe: {enclosure.SecurityLevel}\n";
                details += $"Aktuelle Tiere: {enclosure.Animals?.Count ?? 0}\n";

                if (enclosure.Animals?.Any() == true)
                {
                    details += "\nTiere im Gehege:\n";
                    details += string.Join("\n", enclosure.Animals.Select(a => $"  • {a.Name}"));
                }
            }
            else if (room is StaffRooms staffRoom)
            {
                details += "═══ MITARBEITERRAUM-DETAILS ═══\n";
                details += $"Autorisiertes Personal: {staffRoom.AuthorizedStaff?.Count ?? 0}\n";

                if (staffRoom.AuthorizedStaff?.Any() == true)
                {
                    details += "\nZugang für:\n";
                    details += string.Join("\n", staffRoom.AuthorizedStaff.Select(s => $"  • {s.Name}"));
                }
            }
            else if (room is VisitorRoom visitorRoom)
            {
                details += "═══ BESUCHERRAUM-DETAILS ═══\n";
                details += $"Öffnungszeiten: {visitorRoom.OpeningHours}\n";
                details += $"Mitarbeiter: {visitorRoom.Staff?.Count ?? 0}\n";

                if (visitorRoom.Staff?.Any() == true)
                {
                    details += "\nMitarbeiter:\n";
                    details += string.Join("\n", visitorRoom.Staff.Select(s => $"  • {s.Name}"));
                }
            }

            MessageBox.Show(details, $"Details: {room.Name}", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public enum RoomType
        {
            AnimalEnclosure,
            StaffRoom,
            VisitorRoom
        }

        //Form für die Eingabe der neuen Gebäude Informationen
        public partial class AddBuildingForm : Form
        {
            private readonly RoomFunctions _roomFunctions;
            public RoomType CreatedRoomType { get; private set; }
            private TextBox txtName;
            private TextBox txtLocation;
            private TextBox txtDescription;
            private TextBox txtArea;
            private RadioButton rbEnclosure;
            private RadioButton rbVisitor;
            private RadioButton rbWorker;
            private Button btnSubmit;

            public AddBuildingForm(RoomFunctions roomFunctions)
            {
                _roomFunctions = roomFunctions;
                InitializeForm();
            }

            private void InitializeForm()
            {
                Text = "Raum hinzufügen";
                Width = 400;
                Height = 400;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                StartPosition = FormStartPosition.CenterParent;

                int yPos = 20;

                // Name
                Label lblName = new Label { Text = "Name:", Left = 20, Top = yPos, AutoSize = true };
                txtName = new TextBox { Left = 120, Top = yPos, Width = 240 };
                Controls.Add(lblName);
                Controls.Add(txtName);
                yPos += 35;

                // Location
                Label lblLocation = new Label { Text = "Standort:", Left = 20, Top = yPos, AutoSize = true };
                txtLocation = new TextBox { Left = 120, Top = yPos, Width = 240 };
                Controls.Add(lblLocation);
                Controls.Add(txtLocation);
                yPos += 35;

                // Description
                Label lblDescription = new Label { Text = "Beschreibung:", Left = 20, Top = yPos, AutoSize = true };
                txtDescription = new TextBox { Left = 120, Top = yPos, Width = 240, Height = 60, Multiline = true };
                Controls.Add(lblDescription);
                Controls.Add(txtDescription);
                yPos += 70;

                // Area
                Label lblArea = new Label { Text = "Fläche (m²):", Left = 20, Top = yPos, AutoSize = true };
                txtArea = new TextBox { Left = 120, Top = yPos, Width = 100 };
                Controls.Add(lblArea);
                Controls.Add(txtArea);
                yPos += 35;

                // Type Selection
                Label lblType = new Label { Text = "Typ:", Left = 20, Top = yPos, AutoSize = true, Font = new Font(Font, FontStyle.Bold) };
                Controls.Add(lblType);
                yPos += 25;

                rbEnclosure = new RadioButton { Text = "Tiergehege", Left = 20, Top = yPos, AutoSize = true, Checked = true };
                Controls.Add(rbEnclosure);
                yPos += 25;

                rbVisitor = new RadioButton { Text = "Besucherraum", Left = 20, Top = yPos, AutoSize = true };
                Controls.Add(rbVisitor);
                yPos += 25;

                rbWorker = new RadioButton { Text = "Mitarbeiterraum", Left = 20, Top = yPos, AutoSize = true };
                Controls.Add(rbWorker);
                yPos += 40;

                // Submit Button
                btnSubmit = new Button
                {
                    Text = "Raum erstellen",
                    Left = 120,
                    Top = yPos,
                    Width = 150,
                    Height = 35
                };
                btnSubmit.Click += btnSubmit_Click;
                Controls.Add(btnSubmit);
            }
            

            //Fügt das erstellte object in die Liste der Gebäude ein, abhängig von der Auswahl des Typs
            private async void btnSubmit_Click(object sender, EventArgs e)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        MessageBox.Show("Bitte geben Sie einen Namen ein.", "Validierung");
                        return;
                    }

                    double? area = null;
                    if (!string.IsNullOrWhiteSpace(txtArea.Text))
                    {
                        if (!double.TryParse(txtArea.Text, out var parsedArea))
                        {
                            MessageBox.Show("Bitte geben Sie eine gültige Fläche ein.", "Validierung");
                            return;
                        }
                        area = parsedArea;
                    }

                    // Typ bestimmen und entsprechenden Room erstellen
                    if (rbEnclosure.Checked)
                    {
                        var addEnclosureType = new AddAnimalEnclosureType
                        {
                            Name = txtName.Text,
                            Location = txtLocation.Text,
                            Description = txtDescription.Text,
                            AreaInSquareMeters = area ?? 100,
                            MaxCapacity = 10, // Default-Werte
                            IsOutdoor = false,
                            SecurityLevel = 0
                        };

                        await _roomFunctions.AddAnimalEnclosure(addEnclosureType);
                        CreatedRoomType = RoomType.AnimalEnclosure;
                    }
                    else if (rbVisitor.Checked)
                    {
                        var addVisitorRoomType = new AddVisitorRoomType
                        {
                            Name = txtName.Text,
                            Location = txtLocation.Text,
                            Description = txtDescription.Text,
                            AreaInSquareMeters = area,
                            OpeningHours = "08:00-18:00" // Default
                        };

                        await _roomFunctions.AddVisitorRoom(addVisitorRoomType);
                        CreatedRoomType = RoomType.VisitorRoom;
                    }
                    else // rbWorker
                    {
                        var addStaffRoomType = new AddStaffRoomType
                        {
                            Name = txtName.Text,
                            Location = txtLocation.Text,
                            Description = txtDescription.Text,
                            AreaInSquareMeters = area
                        };

                        await _roomFunctions.AddStaffRoom(addStaffRoomType);
                        CreatedRoomType = RoomType.StaffRoom;
                    }

                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Erstellen: {ex.Message}", "Fehler");
                }
            }
        }
        public partial class DeleteBuildingsForm : Form
        {
            private readonly RoomFunctions _roomFunctions;
            internal List<Room> roomsToDelete = new List<Room>();
            private List<Room> addedRooms;
            private FlowLayoutPanel flowRooms = new FlowLayoutPanel();
            private Button btnDelete = new Button();
            private Button btnCancel = new Button();
            private Label lblTitle = new Label();
            private CheckBox chkSelectAll = new CheckBox();

            public DeleteBuildingsForm(List<Room> rooms, RoomFunctions roomFunctions)
            {
                _roomFunctions = roomFunctions;
                this.addedRooms = rooms;
                InitializeForm();
                BuildCheckboxList();
            }

            private void InitializeForm()
            {
                Text = "Räume löschen";
                Width = 450;
                Height = 500;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                StartPosition = FormStartPosition.CenterParent;

                lblTitle.Text = "Wählen Sie die zu löschenden Räume:";
                lblTitle.Font = new Font(Font, FontStyle.Bold);
                lblTitle.AutoSize = true;
                lblTitle.Left = 20;
                lblTitle.Top = 10;

                chkSelectAll.Text = "Alle auswählen";
                chkSelectAll.AutoSize = true;
                chkSelectAll.Left = 20;
                chkSelectAll.Top = lblTitle.Bottom + 10;
                chkSelectAll.CheckedChanged += (s, e) =>
                {
                    foreach (CheckBox cb in flowRooms.Controls.OfType<CheckBox>())
                        cb.Checked = chkSelectAll.Checked;
                };

                flowRooms.Left = 20;
                flowRooms.Top = chkSelectAll.Bottom + 10;
                flowRooms.Width = 390;
                flowRooms.Height = 300;
                flowRooms.BorderStyle = BorderStyle.FixedSingle;
                flowRooms.AutoScroll = true;
                flowRooms.FlowDirection = FlowDirection.TopDown;
                flowRooms.WrapContents = false;

                btnDelete.Text = "Löschen";
                btnDelete.Width = 120;
                btnDelete.Height = 35;
                btnDelete.Left = 20;
                btnDelete.Top = flowRooms.Bottom + 20;
                btnDelete.Click += btnDelete_Click;

                btnCancel.Text = "Abbrechen";
                btnCancel.Width = 120;
                btnCancel.Height = 35;
                btnCancel.Left = btnDelete.Right + 10;
                btnCancel.Top = btnDelete.Top;
                btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

                Controls.Add(lblTitle);
                Controls.Add(chkSelectAll);
                Controls.Add(flowRooms);
                Controls.Add(btnDelete);
                Controls.Add(btnCancel);
            }

            private void BuildCheckboxList()
            {
                flowRooms.Controls.Clear();

                if (!addedRooms.Any())
                {
                    flowRooms.Controls.Add(new Label { Text = "Keine Räume vorhanden", ForeColor = Color.Gray });
                    btnDelete.Enabled = false;
                    return;
                }

                var grouped = addedRooms
                    .OrderBy(r => r.GetType().Name)
                    .ThenBy(r => r.Name)
                    .GroupBy(r => r.GetType().Name);

                foreach (var group in grouped)
                {
                    string groupName = group.Key switch
                    {
                        nameof(AnimalEnclosure) => "Gehege",
                        nameof(StaffRooms) => "Mitarbeiterräume",
                        nameof(VisitorRoom) => "Besucherräume",
                        _ => "Räume"
                    };

                    Label groupLabel = new Label
                    {
                        Text = $"━━━ {groupName} ({group.Count()}) ━━━",
                        Font = new Font(Font, FontStyle.Bold),
                        ForeColor = Color.DarkBlue,
                        AutoSize = true
                    };
                    flowRooms.Controls.Add(groupLabel);

                    foreach (var room in group)
                    {
                        CheckBox cb = new CheckBox
                        {
                            Text = $"{room.Name} - {room.Location ?? "Kein Standort"}",
                            AutoSize = true,
                            Tag = room,
                            Padding = new Padding(20, 2, 5, 2)
                        };
                        flowRooms.Controls.Add(cb);
                    }
                }
            }

            private async void btnDelete_Click(object sender, EventArgs e)
            {

                roomsToDelete.Clear();

                foreach (CheckBox cb in flowRooms.Controls.OfType<CheckBox>())
                {
                    if (cb.Checked && cb.Tag is Room room)
                    {
                        roomsToDelete.Add(room);
                    }
                }

                if (!roomsToDelete.Any())
                {
                    MessageBox.Show("Bitte wählen Sie mindestens einen Raum aus.", "Keine Auswahl");
                    return;
                }

                var result = MessageBox.Show(
                    $"Möchten Sie wirklich {roomsToDelete.Count} Raum/Räume löschen?\n\n" +
                    string.Join("\n", roomsToDelete.Select(r => $"• {r.Name}")),
                    "Löschen bestätigen",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        foreach (var room in roomsToDelete)
                        {
                            await _roomFunctions.DeleteRoom(room.Id);
                        }

                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Fehler beim Löschen: {ex.Message}", "Fehler");
                    }
                }
            }

            
        }
    }

}

