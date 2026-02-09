using System.Data;
using Zoorganize.Database.Models;
using Zoorganize.Functions;
using Zoorganize.Models.Api;

namespace Zoorganize.Pages
{
    public partial class AnimalsPage : Form
    {
        private readonly AnimalFunctions animalFunctions;
        private readonly RoomFunctions roomFunctions;
        private readonly StaffFunctions staffFunctions;
        public List<Animal> animals = [];
        public List<Species> species = [];

        public AnimalsPage(AnimalFunctions animalFunctions, RoomFunctions roomFunctions, StaffFunctions staffFunctions)
        {
            this.animalFunctions = animalFunctions;
            this.roomFunctions = roomFunctions;
            this.staffFunctions = staffFunctions;
            InitializeComponent();

            //Das muss dann weg und anpassen
            Button addSpeciesButton = new Button
            {
                Text = "Tierart hinzufügen",
                Left = 20,
                Top = 50, // Passe die Position an
                Width = 150,
                Height = 30
            };
            addSpeciesButton.Click += AddSpecies_Click;
            Controls.Add(addSpeciesButton);

            LoadAnimals();
            LoadSpecies();
            
        }
        private async void LoadAnimals()
        {
            animals = await animalFunctions.GetAnimals();
            RefreshAnimalButtons();
        }

        private async void LoadSpecies()
        {
            species = await animalFunctions.GetSpecies();
            RefreshSpeciesButtons(); // Zeigt ALLE Species
        }

        //Button zum zurückkehren zum Hauptmenü
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

        private async void delete_Click(object sender, EventArgs e)
        {
            using (DeleteAnimalsForm form = new DeleteAnimalsForm(animals))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Tiere aus der DATENBANK löschen
                        foreach (Animal animal in form.animaltodelete)
                        {
                            await animalFunctions.DeleteAnimal(animal.Id);
                        }

                        // Liste komplett neu laden
                        LoadAnimals();

                        // UI aktualisieren
                        RefreshSpeciesButtons();
                        animalOverview.Controls.Clear();

                        MessageBox.Show(
                            $"{form.animaltodelete.Count} Tier(e) erfolgreich gelöscht!",
                            "Erfolg",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"Fehler beim Löschen: {ex.Message}",
                            "Fehler",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
        }

        //Button der eine neue  Instanz von Animal erstellt und diese in die Liste der Tiere hinzufügt
        //Dieser Button öffnet ein Fenster, wo Informationen über das Tier eingegeben werden können
        private async void addAnimal_Click(object sender, EventArgs e)
        {
            using (var dlg = new AnimalForm(animalFunctions, roomFunctions, staffFunctions))
            {
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.ShowInTaskbar = false;
                dlg.StartPosition = FormStartPosition.CenterParent;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    animals = await animalFunctions.GetAnimals();

                    // UI aktualisieren
                    RefreshAnimalButtons();

                }
            }
        }

        private async void AddSpecies_Click(object sender, EventArgs e)
        {
            using (var dlg = new SpeciesForm(animalFunctions))
            {
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.ShowInTaskbar = false;
                dlg.StartPosition = FormStartPosition.CenterParent;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    MessageBox.Show("Tierart erfolgreich hinzugefügt!");
                    LoadSpecies(); 
                    RefreshSpeciesButtons(); 
                }
            }
        }
        private void RefreshAnimalButtons()
        {
            if (animalOverview == null || animals == null)
                return;

            animalOverview.Controls.Clear();

            foreach (var animal in animals.OrderBy(a => a.Name))
            {
                // Prüfe ob Species geladen wurde
                string speciesName = animal.Species?.CommonName ?? "Unbekannte Art";

                Button animalButton = new Button
                {
                    Text = $"{animal.Name} ({speciesName})",
                    AutoSize = true,
                    Tag = animal
                };
                animalButton.Click += AnimalButton_Click;
                animalOverview.Controls.Add(animalButton);
            }
        }
        private void RefreshSpeciesButtons()
        {
            typeOverview.Controls.Clear();
            foreach (var spec in species.OrderBy(s => s.CommonName))
            {
                Button speciesButton = new Button
                {
                    Text = spec.CommonName,
                    AutoSize = true,
                    Tag = spec.Id // Speichere die Species-ID
                };
                speciesButton.Click += SpeciesButton_Click;
                typeOverview.Controls.Add(speciesButton);
            }
        }

        

        //Wenn auf eine Tierart geklickt wird, werden alle Tiere dieser Art in der Tierübersicht angezeigt
        private void SpeciesButton_Click(object? sender, EventArgs e)
        {
            if ((sender as Button)?.Tag is Guid speciesId)
            {
                ShowAnimalsOfSpecies(speciesId);
            }
        }
        private void ShowAnimalsOfSpecies(Guid speciesId)
        {
            animalOverview.Controls.Clear();

            var filteredAnimals = animals
                .Where(a => a.SpeciesId == speciesId) 
                .ToList();

            if (!filteredAnimals.Any())
            {
                Label noAnimalsLabel = new Label
                {
                    Text = "Keine Tiere dieser Art vorhanden",
                    AutoSize = true,
                    ForeColor = Color.Gray
                };
                animalOverview.Controls.Add(noAnimalsLabel);
                return;
            }

            foreach (var animal in filteredAnimals.OrderBy(a => a.Name))
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

        //Wenn auf ein Tier geklickt wird, öffnet sich ein Fenster, welches Informationen über das Tier anzeigt
        private void AnimalButton_Click(object? sender, EventArgs e)
        {
            if ((sender as Button)?.Tag is Animal animal)
            {
                string details = $"Name: {animal.Name}\n" +
                    $"Tierart: {animal.Species?.CommonName ?? "Unbekannt"}\n" +
                    $"Wissenschaftlicher Name: {animal.Species?.ScientificName ?? "N/A"}\n" +
                    $"Alter: {animal.Age?.ToString() ?? "Unbekannt"}\n" +
                    $"Ankunftsdatum: {animal.ArrivalDate:dd.MM.yyyy}\n" +
                    $"Herkunft: {animal.Origin}\n" +
                    $"Geschlecht: {animal.Sex}\n" +
                    $"Kastriert: {(animal.IsNeutered ? "Ja" : "Nein")}\n";

                //Trächtig nur bei weiblichen Tieren anzeigen
                if (animal.Sex == Sex.female)
                {
                    details += $"Trächtig: {(animal.IsPregnant ? "Ja" : "Nein")}\n";
                }

                details += $"Gesundheitsstatus: {animal.HealthStatus}\n";

                //Gewicht nur wenn vorhanden
                if (animal.WeightKg.HasValue)
                {
                    details += $"Gewicht: {animal.WeightKg:F2} kg\n";
                }

                details += $"In Quarantäne: {(animal.InQuarantine ? "Ja" : "Nein")}\n" +
                    $"Aggressiv: {(animal.Aggressive ? "Ja" : "Nein")}\n" +
                    $"Benötigt Trennung: {(animal.RequiresSeparation ? "Ja" : "Nein")}\n" +
                    $"Gehege: {animal.CurrentEnclosure?.Name ?? "Kein Gehege zugewiesen"}\n";

                //Pfleger anzeigen(wie bei Mitarbeitern -> Tierarten)
                details += $"Pfleger: {animal.Keeper?.Name ?? "Unbekannt"}\n";
                

                //Notizen am Ende
                if (!string.IsNullOrWhiteSpace(animal.Note))
                {
                    details += $"Notizen: {animal.Note}\n";
                }

                if (!string.IsNullOrWhiteSpace(animal.BehavioralNotes))
                {
                    details += $"Verhaltensnotizen: {animal.BehavioralNotes}\n";
                }

                MessageBox.Show(details, $"Details: {animal.Name}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        //Das ist das Formular, welches geöffnet wird, hier müssen die Informationen über das Tier eingegeben werden, damit es zur Liste der Tiere hinzugefügt werden kann
        //Input ist noch nicht Optional
        class AnimalForm : Form
        {
            private readonly AnimalFunctions _animalFunctions;
            private readonly RoomFunctions _roomFunctions;
            private readonly StaffFunctions _staffFunctions;

            // Basis-Informationen
            TextBox name = new TextBox();
            ComboBox speciesCombo = new ComboBox();
            TextBox age = new TextBox();
            TextBox note = new TextBox();
            DateTimePicker arrivalDate = new DateTimePicker();

            // Herkunft & Geschlecht
            ComboBox origin = new ComboBox();
            ComboBox sex = new ComboBox();
            CheckBox isNeutered = new CheckBox();
            CheckBox isPregnant = new CheckBox();

            //Gehege-Auswahl hinzugefügt
            ComboBox enclosureCombo = new ComboBox();

            //Pfleger-Auswahl
            ComboBox currentKeeper = new();

            // Gesundheit
            ComboBox healthStatus = new ComboBox();
            TextBox weightKg = new TextBox();
            CheckBox inQuarantine = new CheckBox();

            // Verhalten
            CheckBox aggressive = new CheckBox();
            CheckBox requiresSeparation = new CheckBox();
            TextBox behavioralNotes = new TextBox();

            Button submit = new Button();

            private List<Staff> allKeepers = new List<Staff>();

            public Animal animal { get; private set; }

            public AnimalForm(AnimalFunctions animalFunctions, RoomFunctions roomFunctions, StaffFunctions staffFunctions)
            {
                _animalFunctions = animalFunctions;
                _roomFunctions = roomFunctions;
                _staffFunctions = staffFunctions;

                Text = "Tier hinzufügen";
                Width = 500;
                Height = 850;
                AutoScroll = true;

                int leftLabel = 20;
                int leftControl = 180;
                int top = 20;
                int spacing = 40;
                int controlWidth = 250;

                // === BASIS-INFORMATIONEN ===
                AddLabel("Name:", leftLabel, top);
                name.Left = leftControl;
                name.Top = top;
                name.Width = controlWidth;
                Controls.Add(name);
                top += spacing;

                AddLabel("Tierart:", leftLabel, top);
                speciesCombo.Left = leftControl;
                speciesCombo.Top = top;
                speciesCombo.Width = controlWidth;
                speciesCombo.DropDownStyle = ComboBoxStyle.DropDownList;
                speciesCombo.SelectedIndexChanged += SpeciesCombo_SelectedIndexChanged;
                Controls.Add(speciesCombo);
                top += spacing;

                AddLabel("Alter:", leftLabel, top);
                age.Left = leftControl;
                age.Top = top;
                age.Width = controlWidth;
                Controls.Add(age);
                top += spacing;

                AddLabel("Ankunftsdatum:", leftLabel, top);
                arrivalDate.Left = leftControl;
                arrivalDate.Top = top;
                arrivalDate.Width = controlWidth;
                arrivalDate.Format = DateTimePickerFormat.Short;
                Controls.Add(arrivalDate);
                top += spacing;

                AddLabel("Notizen:", leftLabel, top);
                note.Left = leftControl;
                note.Top = top;
                note.Width = controlWidth;
                note.Multiline = true;
                note.Height = 60;
                Controls.Add(note);
                top += 70;

                // === HERKUNFT & GESCHLECHT ===
                top += 10;
                AddLabel("--- HERKUNFT & GESCHLECHT ---", leftLabel, top);
                top += spacing;

                AddLabel("Herkunft:", leftLabel, top);
                origin.Left = leftControl;
                origin.Top = top;
                origin.Width = controlWidth;
                origin.DropDownStyle = ComboBoxStyle.DropDownList;
                origin.Items.AddRange(Enum.GetNames(typeof(AnimalOrigin)));
                origin.SelectedIndex = 0;
                Controls.Add(origin);
                top += spacing;

                AddLabel("Geschlecht:", leftLabel, top);
                sex.Left = leftControl;
                sex.Top = top;
                sex.Width = controlWidth;
                sex.DropDownStyle = ComboBoxStyle.DropDownList;
                sex.Items.Add("(Nicht angegeben)");
                sex.Items.AddRange(Enum.GetNames(typeof(Sex)));
                sex.SelectedIndex = 0;
                Controls.Add(sex);
                top += spacing;

                isNeutered.Text = "Kastriert/Sterilisiert";
                isNeutered.Left = leftControl;
                isNeutered.Top = top;
                isNeutered.Width = controlWidth;
                Controls.Add(isNeutered);
                top += spacing;

                isPregnant.Text = "Trächtig/Schwanger";
                isPregnant.Left = leftControl;
                isPregnant.Top = top;
                isPregnant.Width = controlWidth;
                Controls.Add(isPregnant);
                top += spacing;

                // === HALTUNG ===
                top += 10;
                AddLabel("--- HALTUNG ---", leftLabel, top);
                top += spacing;

                // Gehege-Auswahl
                AddLabel("Gehege:", leftLabel, top);
                enclosureCombo.Left = leftControl;
                enclosureCombo.Top = top;
                enclosureCombo.Width = controlWidth;
                enclosureCombo.DropDownStyle = ComboBoxStyle.DropDownList;
                enclosureCombo.Items.Add("(Kein Gehege)");
                Controls.Add(enclosureCombo);
                top += spacing;

                // Pfleger-Auswahl => Problem, nicht alle Keeper können diese Species auch betreuen, aber das weiß ich erst, wenn ich die Tierart ausgewählt habe, also muss ich die Liste der Pfleger aktualisieren, wenn eine Tierart ausgewählt wird
                AddLabel("Pfleger:", leftLabel, top);
                currentKeeper.Left = leftControl;
                currentKeeper.Top = top;
                currentKeeper.Width = controlWidth;
                currentKeeper.DropDownStyle = ComboBoxStyle.DropDownList;
                currentKeeper.Items.Add("(Kein Pfleger)");
                currentKeeper.Enabled = false; // ÄNDERUNG: Initial deaktiviert bis Tierart gewählt
                Controls.Add(currentKeeper);
                top += spacing;

                // === GESUNDHEIT ===
                top += 10;
                AddLabel("--- GESUNDHEIT ---", leftLabel, top);
                top += spacing;

                AddLabel("Gesundheitsstatus:", leftLabel, top);
                healthStatus.Left = leftControl;
                healthStatus.Top = top;
                healthStatus.Width = controlWidth;
                healthStatus.DropDownStyle = ComboBoxStyle.DropDownList;
                healthStatus.Items.Add("(Nicht angegeben)");
                healthStatus.Items.AddRange(Enum.GetNames(typeof(HealthStatus)));
                healthStatus.SelectedIndex = 0;
                Controls.Add(healthStatus);
                top += spacing;

                AddLabel("Gewicht (kg):", leftLabel, top);
                weightKg.Left = leftControl;
                weightKg.Top = top;
                weightKg.Width = controlWidth;
                Controls.Add(weightKg);
                top += spacing;

                inQuarantine.Text = "In Quarantäne";
                inQuarantine.Left = leftControl;
                inQuarantine.Top = top;
                inQuarantine.Width = controlWidth;
                Controls.Add(inQuarantine);
                top += spacing;

                // === VERHALTEN ===
                top += 10;
                AddLabel("--- VERHALTEN ---", leftLabel, top);
                top += spacing;

                aggressive.Text = "Aggressiv";
                aggressive.Left = leftControl;
                aggressive.Top = top;
                aggressive.Width = controlWidth;
                Controls.Add(aggressive);
                top += spacing;

                requiresSeparation.Text = "Benötigt Trennung";
                requiresSeparation.Left = leftControl;
                requiresSeparation.Top = top;
                requiresSeparation.Width = controlWidth;
                Controls.Add(requiresSeparation);
                top += spacing;

                AddLabel("Verhaltensnotizen:", leftLabel, top);
                behavioralNotes.Left = leftControl;
                behavioralNotes.Top = top;
                behavioralNotes.Width = controlWidth;
                behavioralNotes.Multiline = true;
                behavioralNotes.Height = 60;
                Controls.Add(behavioralNotes);
                top += 70;

                // === SUBMIT BUTTON ===
                top += 20;
                submit.Text = "Tier hinzufügen";
                submit.Left = leftLabel;
                submit.Top = top;
                submit.Width = 150;
                submit.Height = 35;
                submit.Click += Submit_Click;
                Controls.Add(submit);

                LoadSpecies();
                LoadEnclosures();
                LoadKeepers();
            }


            private void AddLabel(string text, int left, int top)
            {
                Label label = new Label
                {
                    Text = text,
                    Left = left,
                    Top = top,
                    Width = 150,
                    AutoSize = false
                };
                Controls.Add(label);
            }

            private async void LoadEnclosures()
            {
                try
                {
                    var enclosures = await _roomFunctions.GetAnimalEnclosures(); 

                    // Füge Gehege zur ComboBox hinzu (nach "(Kein Gehege)")
                    foreach (var enclosure in enclosures.OrderBy(e => e.Name))
                    {
                        enclosureCombo.Items.Add(enclosure);
                    }

                    enclosureCombo.DisplayMember = "Name";
                    enclosureCombo.SelectedIndex = 0; 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Laden der Gehege: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private async void LoadKeepers()
            {
                try
                {
                    allKeepers = await _staffFunctions.GetKeepers();

                    
                    foreach (var keeper in allKeepers)
                    {
                        currentKeeper.Items.Add(keeper);
                    }

                    currentKeeper.DisplayMember = "Name";
                    currentKeeper.SelectedIndex = 0;
                    //Aktualisiere Pfleger-Liste für die initial ausgewählte Tierart
                    if (speciesCombo.SelectedValue is Guid selectedSpeciesId)
                    {
                        UpdateKeeperList(selectedSpeciesId);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Laden der Keeper: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private void SpeciesCombo_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (speciesCombo.SelectedValue is Guid selectedSpeciesId)
                {
                    UpdateKeeperList(selectedSpeciesId);
                }
            }

            //Aktualisiere Pfleger-Liste basierend auf ausgewählter Tierart
            private void UpdateKeeperList(Guid speciesId)
            {
                currentKeeper.Items.Clear();
                currentKeeper.Items.Add("(Kein Pfleger)");
                // ÄNDERUNG: Filtere Keeper die für diese Species autorisiert sind
                var authorizedKeepers = allKeepers
                    .Where(k => k.AuthorizedSpecies.Any(s => s.Id == speciesId))
                    .OrderBy(k => k.Name)
                    .ToList();

                if (authorizedKeepers.Any())
                {
                    foreach (var keeper in authorizedKeepers)
                    {
                        currentKeeper.Items.Add(keeper);
                    }

                    currentKeeper.DisplayMember = "Name";
                    currentKeeper.Enabled = true; // ÄNDERUNG: Aktiviere ComboBox
                    currentKeeper.SelectedIndex = 0;
                }
                else
                {
                    // ÄNDERUNG: Keine autorisierten Keeper vorhanden
                    Label noKeeperLabel = new Label
                    {
                        Text = "(Keine autorisierten Pfleger)",
                        ForeColor = Color.Gray
                    };
                    currentKeeper.Items.Add(noKeeperLabel.Text);
                    currentKeeper.Enabled = false; // ÄNDERUNG: Deaktiviere wenn keine Keeper
                    currentKeeper.SelectedIndex = 0;
                }
            }

            private async void LoadSpecies()
            {
                try
                {
                    var speciesList = await _animalFunctions.GetSpecies();
                    speciesCombo.DataSource = speciesList;
                    speciesCombo.DisplayMember = "CommonName";
                    speciesCombo.ValueMember = "Id";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Laden der Tierarten: {ex.Message}");
                }
            }
            private async void Submit_Click(object? sender, EventArgs e)
            {
                try
                {
                    // Validierung
                    if (string.IsNullOrWhiteSpace(name.Text))
                    {
                        MessageBox.Show("Bitte geben Sie einen Namen ein.");
                        return;
                    }

                    // Validierung Tierart
                    if (speciesCombo.SelectedValue == null)
                    {
                        MessageBox.Show("Bitte wählen Sie eine Tierart aus.");
                        return;
                    }

                    // Validierung Alter
                    int? parsedAge = ParseInt(age.Text);
                    if (!string.IsNullOrWhiteSpace(age.Text) && (!parsedAge.HasValue || parsedAge < 0))
                    {
                        MessageBox.Show("Bitte geben Sie ein gültiges Alter ein (positive Zahl).", "Validierung", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Validierung Gewicht
                    double? parsedWeight = ParseDouble(weightKg.Text);
                    if (!string.IsNullOrWhiteSpace(weightKg.Text) && (!parsedWeight.HasValue || parsedWeight <= 0))
                    {
                        MessageBox.Show("Bitte geben Sie ein gültiges Gewicht ein (positive Zahl).", "Validierung", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    //Gehege-ID extrahieren (null wenn "(Kein Gehege)" ausgewählt)
                    Guid? selectedEnclosureId = null;
                    if (enclosureCombo.SelectedIndex > 0 && enclosureCombo.SelectedItem is AnimalEnclosure selectedEnclosure)
                    {
                        selectedEnclosureId = selectedEnclosure.Id;
                    }

                    Guid? selectedKeeperId = null;
                    if (currentKeeper.SelectedIndex > 0 && currentKeeper.SelectedItem is Staff selectedKeeper)
                    {
                        selectedKeeperId = selectedKeeper.Id;
                    }

                    // Erstelle AddAnimalType mit allen Feldern
                    AddAnimalType newAnimal = new()
                    {
                        Name = name.Text,
                        SpeciesId = ((Guid)speciesCombo.SelectedValue),
                        Note = string.IsNullOrWhiteSpace(note.Text) ? null : note.Text,
                        Age = ParseInt(age.Text),
                        ArrivalDate = arrivalDate.Value.ToString("yyyy-MM-dd"),

                        // Herkunft & Geschlecht
                        Origin = origin.SelectedIndex,
                        Sex = sex.SelectedIndex == 0 ? null : sex.SelectedIndex - 1, // -1 wegen "(Nicht angegeben)"
                        IsNeutered = isNeutered.Checked ? true : null,
                        IsPregnant = isPregnant.Checked ? true : null,

                        // Gesundheit
                        HealthStatus = healthStatus.SelectedIndex == 0 ? null : healthStatus.SelectedIndex - 1,
                        WeightKg = ParseDouble(weightKg.Text),
                        InQuarantine = inQuarantine.Checked ? true : null,

                        // Verhalten
                        Aggressive = aggressive.Checked ? true : null,
                        RequiresSeparation = requiresSeparation.Checked ? true : null,
                        BehavioralNotes = string.IsNullOrWhiteSpace(behavioralNotes.Text) ? null : behavioralNotes.Text,

                        KeeperId = selectedKeeperId ?? Guid.Empty,
                        CurrentEnclosureId = selectedEnclosureId ?? Guid.Empty 

                    };

                    await _animalFunctions.AddAnimal(newAnimal);

                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            private int? ParseInt(string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return null;
                return int.TryParse(text, out var result) ? result : null;
            }

            private double? ParseDouble(string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return null;
                return double.TryParse(text, out var result) ? result : null;
            }
        }

        class SpeciesForm : Form
        {
            private readonly AnimalFunctions _animalFunctions;

            // Basis-Informationen
            TextBox commonName = new TextBox();
            TextBox scientificName = new TextBox();

            // Haltung
            CheckBox isSolitaryByNature = new CheckBox();

            // Klimaanforderungen
            TextBox minTemperature = new TextBox();
            TextBox maxTemperature = new TextBox();
            TextBox minHumidity = new TextBox();
            TextBox maxHumidity = new TextBox();
            CheckBox requiresOutdoorAccess = new CheckBox();

            // Sicherheitsmerkmale
            ComboBox requiredSecurityLevel = new ComboBox();
            CheckBox isDangerous = new CheckBox();
            CheckBox requiresSpecialPermit = new CheckBox();

            // Infrastruktur
            CheckBox requiresWaterFeature = new CheckBox();
            CheckBox requiresClimbingStructures = new CheckBox();
            CheckBox requiresShelter = new CheckBox();

            Button submit = new Button();

            public Species species { get; private set; }

            public SpeciesForm(AnimalFunctions animalFunctions)
            {
                _animalFunctions = animalFunctions;
                Text = "Tierart hinzufügen";
                Width = 500;
                Height = 700;
                AutoScroll = true;

                int leftLabel = 20;
                int leftControl = 200;
                int top = 20;
                int spacing = 35;
                int controlWidth = 250;

                // Basis-Informationen
                AddLabel("Name (Common):", leftLabel, top);
                commonName.Left = leftControl;
                commonName.Top = top;
                commonName.Width = controlWidth;
                Controls.Add(commonName);
                top += spacing;

                AddLabel("Wissenschaftlicher Name:", leftLabel, top);
                scientificName.Left = leftControl;
                scientificName.Top = top;
                scientificName.Width = controlWidth;
                Controls.Add(scientificName);
                top += spacing;

                // Haltung Sektion
                top += 10;
                AddLabel("--- HALTUNG ---", leftLabel, top);
                top += spacing;

                isSolitaryByNature.Text = "Einzelgänger";
                isSolitaryByNature.Left = leftControl;
                isSolitaryByNature.Top = top;
                isSolitaryByNature.Width = controlWidth;
                Controls.Add(isSolitaryByNature);
                top += spacing;

                // Klimaanforderungen Sektion
                top += 10;
                AddLabel("--- KLIMA ---", leftLabel, top);
                top += spacing;

                AddLabel("Min. Temperatur (°C):", leftLabel, top);
                minTemperature.Left = leftControl;
                minTemperature.Top = top;
                minTemperature.Width = controlWidth;
                Controls.Add(minTemperature);
                top += spacing;

                AddLabel("Max. Temperatur (°C):", leftLabel, top);
                maxTemperature.Left = leftControl;
                maxTemperature.Top = top;
                maxTemperature.Width = controlWidth;
                Controls.Add(maxTemperature);
                top += spacing;

                AddLabel("Min. Luftfeuchtigkeit (%):", leftLabel, top);
                minHumidity.Left = leftControl;
                minHumidity.Top = top;
                minHumidity.Width = controlWidth;
                Controls.Add(minHumidity);
                top += spacing;

                AddLabel("Max. Luftfeuchtigkeit (%):", leftLabel, top);
                maxHumidity.Left = leftControl;
                maxHumidity.Top = top;
                maxHumidity.Width = controlWidth;
                Controls.Add(maxHumidity);
                top += spacing;

                requiresOutdoorAccess.Text = "Benötigt Außenzugang";
                requiresOutdoorAccess.Left = leftControl;
                requiresOutdoorAccess.Top = top;
                requiresOutdoorAccess.Width = controlWidth;
                Controls.Add(requiresOutdoorAccess);
                top += spacing;

                // Sicherheit Sektion
                top += 10;
                AddLabel("--- SICHERHEIT ---", leftLabel, top);
                top += spacing;

                AddLabel("Sicherheitsstufe:", leftLabel, top);
                requiredSecurityLevel.Left = leftControl;
                requiredSecurityLevel.Top = top;
                requiredSecurityLevel.Width = controlWidth;
                requiredSecurityLevel.DropDownStyle = ComboBoxStyle.DropDownList;
                requiredSecurityLevel.Items.AddRange(Enum.GetNames(typeof(SecurityLevel)));
                requiredSecurityLevel.SelectedIndex = 0;
                Controls.Add(requiredSecurityLevel);
                top += spacing;

                isDangerous.Text = "Gefährlich";
                isDangerous.Left = leftControl;
                isDangerous.Top = top;
                isDangerous.Width = controlWidth;
                Controls.Add(isDangerous);
                top += spacing;

                requiresSpecialPermit.Text = "Benötigt Spezialgenehmigung";
                requiresSpecialPermit.Left = leftControl;
                requiresSpecialPermit.Top = top;
                requiresSpecialPermit.Width = controlWidth;
                Controls.Add(requiresSpecialPermit);
                top += spacing;

                // Infrastruktur Sektion
                top += 10;
                AddLabel("--- INFRASTRUKTUR ---", leftLabel, top);
                top += spacing;

                requiresWaterFeature.Text = "Benötigt Wasserstelle";
                requiresWaterFeature.Left = leftControl;
                requiresWaterFeature.Top = top;
                requiresWaterFeature.Width = controlWidth;
                Controls.Add(requiresWaterFeature);
                top += spacing;

                requiresClimbingStructures.Text = "Benötigt Kletter-Strukturen";
                requiresClimbingStructures.Left = leftControl;
                requiresClimbingStructures.Top = top;
                requiresClimbingStructures.Width = controlWidth;
                Controls.Add(requiresClimbingStructures);
                top += spacing;

                requiresShelter.Text = "Benötigt Unterschlupf";
                requiresShelter.Left = leftControl;
                requiresShelter.Top = top;
                requiresShelter.Width = controlWidth;
                Controls.Add(requiresShelter);
                top += spacing;

                // Submit Button
                top += 20;
                submit.Text = "Tierart hinzufügen";
                submit.Left = leftLabel;
                submit.Top = top;
                submit.Width = 150;
                submit.Height = 35;
                submit.Click += Submit_Click;
                Controls.Add(submit);
            }

            private void AddLabel(string text, int left, int top)
            {
                Label label = new Label
                {
                    Text = text,
                    Left = left,
                    Top = top,
                    Width = 170,
                    AutoSize = false
                };
                Controls.Add(label);
            }

            private async void Submit_Click(object? sender, EventArgs e)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(commonName.Text))
                    {
                        MessageBox.Show("Bitte geben Sie einen Namen ein.");
                        return;
                    }

                    AddSpeciesType newSpecies = new()
                    {
                        Name = commonName.Text,
                        ScientificName = string.IsNullOrWhiteSpace(scientificName.Text) ? null : scientificName.Text,
                        IsSolitaryByNature = isSolitaryByNature.Checked ? true : null,
                        MinTemperature = ParseDouble(minTemperature.Text),
                        MaxTemperature = ParseDouble(maxTemperature.Text),
                        MinHumidity = ParseDouble(minHumidity.Text),
                        MaxHumidity = ParseDouble(maxHumidity.Text),
                        RequiresOutdoorAccess = requiresOutdoorAccess.Checked ? true : null,
                        RequiredSecurityLevel = ParseInt(requiredSecurityLevel.Text),
                        IsDangerous = isDangerous.Checked ? true : null,
                        RequiresSpecialPermit = requiresSpecialPermit.Checked ? true : null,
                        RequiresWaterFeature = requiresWaterFeature.Checked ? true : null,
                        RequiresClimbingStructures = requiresClimbingStructures.Checked ? true : null,
                        RequiresShelter = requiresShelter.Checked ? true : null
                    };

                    await _animalFunctions.AddSpecies(newSpecies);

                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private double? ParseDouble(string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return null;
                return double.TryParse(text, out var result) ? result : null;
            }

            private int? ParseInt(string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                    return null;
                return int.TryParse(text, out var result) ? result : null;
            }
        }
    }

        public partial class DeleteAnimalsForm : Form
        {
            private List<Animal> addedanimals;
            public List<Animal> animaltodelete { get; private set; } = new List<Animal>();
            private FlowLayoutPanel flowAnimals = new FlowLayoutPanel();
            private Button btnDelete = new Button();
            private Button btnCancel = new Button();

            public DeleteAnimalsForm(List<Animal> animals)
            {
                this.addedanimals = animals;
                flowAnimals.Top = 20;
                flowAnimals.Left = 20;

                btnDelete.Top = flowAnimals.Bottom + 20;
                btnDelete.Left = 20;
                btnDelete.Text = "Delete";

                btnCancel.Top = flowAnimals.Bottom + 20;
                btnCancel.Left = btnDelete.Right + 20;
                btnCancel.Text = "Cancel";

                BuildCheckboxList();

                Controls.Add(flowAnimals);
                Controls.Add(btnDelete);
                Controls.Add(btnCancel);

                btnDelete.Click += btnDelete_Click;
                btnCancel.Click += btnCancel_Click;
            }

            private void BuildCheckboxList()
            {
                flowAnimals.Controls.Clear();
                flowAnimals.FlowDirection = FlowDirection.TopDown;
                flowAnimals.WrapContents = false;
                flowAnimals.AutoScroll = true;

                if (!addedanimals.Any())
                {
                    Label noAnimalsLabel = new Label
                    {
                        Text = "Keine Tiere vorhanden",
                        AutoSize = true,
                        ForeColor = Color.Gray
                    };
                    flowAnimals.Controls.Add(noAnimalsLabel);
                    btnDelete.Enabled = false;
                    return;
                }

                foreach (var animal in addedanimals.OrderBy(a => a.Name))
                {
                    CheckBox cb = new CheckBox();
                    cb.Text = $"{animal.Name} ({animal.Species.CommonName})";
                    cb.AutoSize = true;
                    cb.Tag = animal; // store reference

                    flowAnimals.Controls.Add(cb);
                }
            }

            private void btnDelete_Click(object sender, EventArgs e)
            {
                animaltodelete.Clear();

                foreach (CheckBox cb in flowAnimals.Controls)
                {
                    if (cb.Checked && cb.Tag is Animal animal)
                    {
                        animaltodelete.Add((Animal)cb.Tag);
                    }
                }

                if (!animaltodelete.Any())
                {
                    MessageBox.Show(
                        "Bitte wählen Sie mindestens ein Tier zum Löschen aus.",
                        "Keine Auswahl",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show(
                $"Möchten Sie wirklich {animaltodelete.Count} Tier(e) löschen?\n\n" +
                string.Join("\n", animaltodelete.Select(a => $"• {a.Name}")),
                "Löschen bestätigen",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
        }

            private void btnCancel_Click(object sender, EventArgs e)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

    
}
