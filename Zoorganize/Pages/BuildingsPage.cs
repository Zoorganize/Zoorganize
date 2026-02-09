using System.Data;
using Zoorganize.Database.Models;
using Zoorganize.Functions;
using Zoorganize.Models.Api;

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
            MainPage mainPage = new()
            {
                Dock = DockStyle.Fill,
                TopLevel = false
            };
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
            string details = $"Name: {room.Name}\n" +
                $"Standort: {room.Location ?? "N/A"}\n" +
                $"Beschreibung: {room.Description ?? "N/A"}\n" +
                $"Fläche: {(room.AreaInSquareMeters.HasValue ? $"{room.AreaInSquareMeters:F2} m²" : "N/A")}\n" +
                $"Status: {room.Status}\n";

            // Spezifische Details basierend auf Typ
            if (room is AnimalEnclosure enclosure)
            {
                details += $"Kapazität: {enclosure.MaxCapacity} Tiere\n" +
                    $"Außengehege: {(enclosure.IsOutdoor ? "Ja" : "Nein")}\n" +
                    $"Temperaturkontrolle: {(enclosure.TemperatureControlled ? "Ja" : "Nein")}\n";

                // Temperatur nur anzeigen wenn vorhanden
                if (enclosure.MinTemperature > 0 && enclosure.MaxTemperature > 0)
                {
                    details += $"Temperatur: {enclosure.MinTemperature}°C - {enclosure.MaxTemperature}°C\n";
                }

                details += $"Sicherheitsstufe: {enclosure.SecurityLevel}\n" +
                    $"Aktuelle Tiere: {enclosure.Animals?.Count ?? 0}\n";

                // Tiere im Gehege
                if (enclosure.Animals?.Any() == true)
                {
                    details += $"Tiere: {string.Join(", ", enclosure.Animals.Select(a => a.Name))}\n";
                }

                // Erlaubte Tierarten
                if (enclosure.AllowedSpecies?.Any() == true)
                {
                    details += $"Erlaubte Tierarten: {string.Join(", ", enclosure.AllowedSpecies.Select(s => s.CommonName))}\n";
                }
            }
            else if (room is StaffRooms staffRoom)
            {
                details += $"Autorisiertes Personal: {staffRoom.AuthorizedStaff?.Count ?? 0}\n";

                // Zugangsberechtigte Mitarbeiter
                if (staffRoom.AuthorizedStaff?.Any() == true)
                {
                    details += $"Zugang für: {string.Join(", ", staffRoom.AuthorizedStaff.Select(s => s.Name))}\n";
                }
            }
            else if (room is VisitorRoom visitorRoom)
            {
                details += $"Öffnungszeiten: {visitorRoom.OpeningHours}\n" +
                    $"Mitarbeiter: {visitorRoom.Staff?.Count ?? 0}\n";

                // Zugewiesene Mitarbeiter
                if (visitorRoom.Staff?.Any() == true)
                {
                    details += $"Personal: {string.Join(", ", visitorRoom.Staff.Select(s => s.Name))}\n";
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

                // Zusätzliche Felder für AnimalEnclosure
                private NumericUpDown numMaxCapacity;
                private CheckBox chkIsOutdoor;
                private ComboBox cmbSecurityLevel;

                // Zusätzliches Feld für VisitorRoom
                private TextBox txtOpeningHours;

                private RadioButton rbEnclosure;
                private RadioButton rbVisitor;
                private RadioButton rbWorker;
                private Button btnSubmit;

                // Panel für spezifische Felder
                private Panel pnlEnclosureDetails;
                private Panel pnlVisitorDetails;

                public AddBuildingForm(RoomFunctions roomFunctions)
                {
                    _roomFunctions = roomFunctions;
                    InitializeForm();
                }

                private void InitializeForm()
                {
                    Text = "Raum hinzufügen";
                    Width = 450;
                    Height = 550;
                    FormBorderStyle = FormBorderStyle.FixedDialog;
                    MaximizeBox = false;
                    StartPosition = FormStartPosition.CenterParent;
                    AutoScroll = true;

                    int yPos = 20;
                    int leftLabel = 20;
                    int leftControl = 150;
                    int controlWidth = 260;

                    // Name
                    Label lblName = new Label { Text = "Name:", Left = leftLabel, Top = yPos, Width = 120 };
                    txtName = new TextBox { Left = leftControl, Top = yPos, Width = controlWidth };
                    Controls.Add(lblName);
                    Controls.Add(txtName);
                    yPos += 35;

                    // Location
                    Label lblLocation = new Label { Text = "Standort*:", Left = leftLabel, Top = yPos, Width = 120 };
                    txtLocation = new TextBox { Left = leftControl, Top = yPos, Width = controlWidth };
                    Controls.Add(lblLocation);
                    Controls.Add(txtLocation);
                    yPos += 35;

                    // Description
                    Label lblDescription = new Label { Text = "Beschreibung*:", Left = leftLabel, Top = yPos, Width = 120 };
                    txtDescription = new TextBox { Left = leftControl, Top = yPos, Width = controlWidth, Height = 60, Multiline = true };
                    Controls.Add(lblDescription);
                    Controls.Add(txtDescription);
                    yPos += 70;

                    // Area
                    Label lblArea = new Label { Text = "Fläche (m²)*:", Left = leftLabel, Top = yPos, Width = 120 };
                    txtArea = new TextBox { Left = leftControl, Top = yPos, Width = 100 };
                    Controls.Add(lblArea);
                    Controls.Add(txtArea);
                    yPos += 35;

                    // Type Selection (RadioButtons = Kategorie/Type des Raums)
                    Label lblType = new Label { Text = "Typ:", Left = leftLabel, Top = yPos, Width = 120, Font = new Font(Font, FontStyle.Bold) };
                    Controls.Add(lblType);
                    yPos += 25;

                    rbEnclosure = new RadioButton { Text = "Tiergehege", Left = leftLabel, Top = yPos, AutoSize = true, Checked = true };
                    rbEnclosure.CheckedChanged += RoomType_CheckedChanged;
                    Controls.Add(rbEnclosure);
                    yPos += 25;

                    rbVisitor = new RadioButton { Text = "Besucherraum", Left = leftLabel, Top = yPos, AutoSize = true };
                    rbVisitor.CheckedChanged += RoomType_CheckedChanged;
                    Controls.Add(rbVisitor);
                    yPos += 25;

                    rbWorker = new RadioButton { Text = "Mitarbeiterraum", Left = leftLabel, Top = yPos, AutoSize = true };
                    rbWorker.CheckedChanged += RoomType_CheckedChanged;
                    Controls.Add(rbWorker);
                    yPos += 40;

                    // Panel für Gehege-spezifische Felder
                    pnlEnclosureDetails = new Panel
                    {
                        Left = 0,
                        Top = yPos,
                        Width = 430,
                        Height = 150,
                        BorderStyle = BorderStyle.FixedSingle,
                        Visible = true
                    };

                    int panelY = 10;

                    Label lblMaxCapacity = new Label { Text = "Max. Kapazität*:", Left = 20, Top = panelY, Width = 120 };
                    numMaxCapacity = new NumericUpDown { Left = 150, Top = panelY, Width = 100, Minimum = 1, Maximum = 1000, Value = 10 };
                    pnlEnclosureDetails.Controls.Add(lblMaxCapacity);
                    pnlEnclosureDetails.Controls.Add(numMaxCapacity);
                    panelY += 35;

                    chkIsOutdoor = new CheckBox { Text = "Außengehege*", Left = 20, Top = panelY, AutoSize = true };
                    pnlEnclosureDetails.Controls.Add(chkIsOutdoor);
                    panelY += 30;

                    Label lblSecurity = new Label { Text = "Sicherheitsstufe*:", Left = 20, Top = panelY, Width = 120 };
                    cmbSecurityLevel = new ComboBox { Left = 150, Top = panelY, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
                    cmbSecurityLevel.Items.AddRange(Enum.GetNames(typeof(SecurityLevel)));
                    cmbSecurityLevel.SelectedIndex = 0;
                    pnlEnclosureDetails.Controls.Add(lblSecurity);
                    pnlEnclosureDetails.Controls.Add(cmbSecurityLevel);

                    Controls.Add(pnlEnclosureDetails);

                    // Panel für Besucherraum-spezifische Felder
                    pnlVisitorDetails = new Panel
                    {
                        Left = 0,
                        Top = yPos,
                        Width = 430,
                        Height = 80,
                        BorderStyle = BorderStyle.FixedSingle,
                        Visible = false
                    };

                    panelY = 10;

                    Label lblOpeningHours = new Label { Text = "Öffnungszeiten:", Left = 20, Top = panelY, Width = 120 };
                    txtOpeningHours = new TextBox { Left = 150, Top = panelY, Width = 200, Text = "08:00-18:00" };
                    pnlVisitorDetails.Controls.Add(lblOpeningHours);
                    pnlVisitorDetails.Controls.Add(txtOpeningHours);

                    Controls.Add(pnlVisitorDetails);

                    yPos += 160;

                    // Submit Button
                    btnSubmit = new Button
                    {
                        Text = "Raum erstellen",
                        Left = 150,
                        Top = yPos,
                        Width = 150,
                        Height = 35
                    };
                    btnSubmit.Click += btnSubmit_Click;
                    Controls.Add(btnSubmit);
                }

                private void RoomType_CheckedChanged(object sender, EventArgs e)
                {
                    if (rbEnclosure.Checked)
                    {
                        pnlEnclosureDetails.Visible = true;
                        pnlVisitorDetails.Visible = false;
                    }
                    else if (rbVisitor.Checked)
                    {
                        pnlEnclosureDetails.Visible = false;
                        pnlVisitorDetails.Visible = true;
                    }
                    else // rbWorker
                    {
                        pnlEnclosureDetails.Visible = false;
                        pnlVisitorDetails.Visible = false;
                    }
                }

                private async void btnSubmit_Click(object sender, EventArgs e)
                {
                    try
                    {
                        // Validierung Name
                        if (string.IsNullOrWhiteSpace(txtName.Text))
                        {
                            MessageBox.Show("Bitte geben Sie einen Namen ein.", "Validierung", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Validierung Fläche
                        double? area = null;
                        if (!string.IsNullOrWhiteSpace(txtArea.Text))
                        {
                            if (!double.TryParse(txtArea.Text, out var parsedArea) || parsedArea <= 0)
                            {
                                MessageBox.Show("Bitte geben Sie eine gültige Fläche ein (positive Zahl).", "Validierung", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            area = parsedArea;
                        }

                        // ÄNDERUNG: Type wird aus RadioButton-Auswahl bestimmt
                        // 0 = AnimalEnclosure, 1 = VisitorRoom, 2 = StaffRoom
                        int typeIndex = rbEnclosure.Checked ? 0 : rbVisitor.Checked ? 1 : 2;

                        // Typ bestimmen und entsprechenden Room erstellen
                        if (rbEnclosure.Checked)
                        {
                            if (!area.HasValue)
                            {
                                MessageBox.Show("Bitte geben Sie eine Fläche für das Gehege ein.", "Validierung", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            var addEnclosureType = new AddAnimalEnclosureType
                            {
                                Name = txtName.Text,
                                Location = string.IsNullOrWhiteSpace(txtLocation.Text) ? null : txtLocation.Text,
                                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text,
                                AreaInSquareMeters = area.Value,
                                MaxCapacity = (int)numMaxCapacity.Value,
                                IsOutdoor = chkIsOutdoor.Checked,
                                SecurityLevel = cmbSecurityLevel.SelectedIndex,
                                Type = typeIndex // ÄNDERUNG: 0 für AnimalEnclosure
                            };

                            await _roomFunctions.AddAnimalEnclosure(addEnclosureType);
                            CreatedRoomType = RoomType.AnimalEnclosure;
                        }
                        else if (rbVisitor.Checked)
                        {
                            if (string.IsNullOrWhiteSpace(txtOpeningHours.Text))
                            {
                                MessageBox.Show("Bitte geben Sie Öffnungszeiten ein.", "Validierung", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            var addVisitorRoomType = new AddVisitorRoomType
                            {
                                Name = txtName.Text,
                                Location = string.IsNullOrWhiteSpace(txtLocation.Text) ? null : txtLocation.Text,
                                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text,
                                AreaInSquareMeters = area,
                                OpeningHours = txtOpeningHours.Text,
                                Type = typeIndex // ÄNDERUNG: 1 für VisitorRoom
                            };

                            await _roomFunctions.AddVisitorRoom(addVisitorRoomType);
                            CreatedRoomType = RoomType.VisitorRoom;
                        }
                        else // rbWorker
                        {
                            var addStaffRoomType = new AddStaffRoomType
                            {
                                Name = txtName.Text,
                                Location = string.IsNullOrWhiteSpace(txtLocation.Text) ? null : txtLocation.Text,
                                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text,
                                AreaInSquareMeters = area,
                                Type = typeIndex // ÄNDERUNG: 2 für StaffRoom
                            };

                            await _roomFunctions.AddStaffRoom(addStaffRoomType);
                            CreatedRoomType = RoomType.StaffRoom;
                        }

                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Fehler beim Erstellen: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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



