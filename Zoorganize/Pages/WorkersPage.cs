using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Zoorganize.Database.Models;
using Zoorganize.Functions;
using Zoorganize.Models.Api;

namespace Zoorganize.Pages
{
    
    public partial class WorkersPage : Form
    {
        private readonly KeeperFunctions keeperFunctions;
        List<Staff> worker = [];
        public WorkersPage(KeeperFunctions keeperFunctions)
        {
            this.keeperFunctions = keeperFunctions;
            InitializeComponent();
            LoadWorkers();
        }

        private async void LoadWorkers()
        {
            worker = await keeperFunctions.GetStaff();
            RefreshWorkerDisplay();
        }

        private void RefreshWorkerDisplay()
        {
            workerOverview.Controls.Clear();
            foreach (var staff in worker)
            {
                Button workersButton = new Button
                {
                    Text = staff.Name,
                    AutoSize = true,
                    Tag = staff
                };

                workersButton.Click += workersButton_Click;
                workerOverview.Controls.Add(workersButton);
            }
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

        private async void delete_Click(object sender, EventArgs e)
        {
            using (DeleteWorkersForm form = new DeleteWorkersForm(worker))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    foreach (Staff worker in form.workertodelete)
                    {
                        await keeperFunctions.DeletePersonal(worker.Id);
                        this.worker.Remove(worker);
                    }
                    RefreshWorkerDisplay();
                }
            }
        }

        private async void AddWorker_Click(object sender, EventArgs e)
        {
            using (var dlg = new WorkerForm())
            {
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.ShowInTaskbar = false;
                dlg.StartPosition = FormStartPosition.CenterParent;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    worker = await keeperFunctions.AddPersonal(dlg.newStaff);
                    RefreshWorkerDisplay();
                }
            }
        }

        private void workersButton_Click(object? sender, EventArgs e)
        {
            if ((sender as Button)?.Tag is Staff staff)
            {
                MessageBox.Show($"Name: {staff.Name}\n" +
                    $"Geschlecht: {staff.Sex}\n" +
                    $"Position: {staff.JobRole}\n" +
                    $"Anstellungsart: {staff.EmploymentType}\n" +
                    $"Jahresgehalt: {staff.YearlySalary}\n" +
                    $"Kontakt: {staff.ContactInfo ?? "N/A"}\n" +
                    $"Adresse: {staff.Address ?? "N/A"}\n" +
                    $"Einstellungsdatum: {staff.HireDate}\n" +
                    $"Austrittsdatum: {staff.ExitDate?.ToString() ?? "N/A"}\n" +
                    $"Notizen: {staff.Notes ?? "N/A"}\n" +
                    $"Aktiv: {(staff.IsActive ? "Ja" : "Nein")}");
            }
        }


        //ergänzen mit den anderen attributen
        class WorkerForm : Form
        {
            TextBox name = new TextBox();
            ComboBox sex = new ComboBox();
            ComboBox jobRole = new ComboBox();
            ComboBox employmentType = new ComboBox();
            TextBox yearlySalary = new TextBox();
            TextBox contactInfo = new TextBox();
            TextBox address = new TextBox();
            DateTimePicker hireDate = new DateTimePicker();
            TextBox notes = new TextBox();
            CheckBox isActive = new CheckBox();
            Button submit = new Button();
            public AddStaffType newStaff { get; private set; }
            public WorkerForm()
            {
                Text = "Arbeiter Hinzufügen";
                Width = 350;
                Height = 550;
                AutoScroll = true;

                int top = 20;
                int left = 20;
                int spacing = 40;

                // Name
                Label lblName = new Label { Text = "Name:", Left = left, Top = top, Width = 100 };
                name.Left = left + 110;
                name.Top = top;
                name.Width = 200;
                Controls.Add(lblName);
                Controls.Add(name);
                top += spacing;

                // Geschlecht
                Label lblSex = new Label { Text = "Geschlecht:", Left = left, Top = top, Width = 100 };
                sex.Left = left + 110;
                sex.Top = top;
                sex.Width = 200;
                sex.DropDownStyle = ComboBoxStyle.DropDownList;
                sex.Items.AddRange(Enum.GetNames(typeof(Sex)));
                sex.SelectedIndex = 0;
                Controls.Add(lblSex);
                Controls.Add(sex);
                top += spacing;

                // JobRole
                Label lblJobRole = new Label { Text = "Position:", Left = left, Top = top, Width = 100 };
                jobRole.Left = left + 110;
                jobRole.Top = top;
                jobRole.Width = 200;
                jobRole.DropDownStyle = ComboBoxStyle.DropDownList;
                jobRole.Items.AddRange(Enum.GetNames(typeof(JobRole)));
                jobRole.SelectedIndex = 0;
                Controls.Add(lblJobRole);
                Controls.Add(jobRole);
                top += spacing;

                // EmploymentType
                Label lblEmploymentType = new Label { Text = "Anstellungsart:", Left = left, Top = top, Width = 100 };
                employmentType.Left = left + 110;
                employmentType.Top = top;
                employmentType.Width = 200;
                employmentType.DropDownStyle = ComboBoxStyle.DropDownList;
                employmentType.Items.AddRange(Enum.GetNames(typeof(EmploymentType)));
                employmentType.SelectedIndex = 0;
                Controls.Add(lblEmploymentType);
                Controls.Add(employmentType);
                top += spacing;

                // Jahresgehalt
                Label lblSalary = new Label { Text = "Jahresgehalt:", Left = left, Top = top, Width = 100 };
                yearlySalary.Left = left + 110;
                yearlySalary.Top = top;
                yearlySalary.Width = 200;
                Controls.Add(lblSalary);
                Controls.Add(yearlySalary);
                top += spacing;

                // Kontaktinfo
                Label lblContact = new Label { Text = "Kontakt:", Left = left, Top = top, Width = 100 };
                contactInfo.Left = left + 110;
                contactInfo.Top = top;
                contactInfo.Width = 200;
                Controls.Add(lblContact);
                Controls.Add(contactInfo);
                top += spacing;

                // Adresse
                Label lblAddress = new Label { Text = "Adresse:", Left = left, Top = top, Width = 100 };
                address.Left = left + 110;
                address.Top = top;
                address.Width = 200;
                Controls.Add(lblAddress);
                Controls.Add(address);
                top += spacing;

                // Einstellungsdatum
                Label lblHireDate = new Label { Text = "Einstellung:", Left = left, Top = top, Width = 100 };
                hireDate.Left = left + 110;
                hireDate.Top = top;
                hireDate.Width = 200;
                hireDate.Format = DateTimePickerFormat.Short;
                Controls.Add(lblHireDate);
                Controls.Add(hireDate);
                top += spacing;

                // Notizen
                Label lblNotes = new Label { Text = "Notizen:", Left = left, Top = top, Width = 100 };
                notes.Left = left + 110;
                notes.Top = top;
                notes.Width = 200;
                notes.Multiline = true;
                notes.Height = 60;
                Controls.Add(lblNotes);
                Controls.Add(notes);
                top += 70;

                // Aktiv
                isActive.Text = "Aktiv";
                isActive.Left = left;
                isActive.Top = top;
                isActive.Checked = true;
                Controls.Add(isActive);
                top += spacing;

                // Submit Button
                submit.Text = "Hinzufügen";
                submit.Left = left;
                submit.Top = top;
                submit.Width = 100;
                submit.Click += Submit_Click;
                Controls.Add(submit);
            }
            private void Submit_Click(object? sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(name.Text))
                {
                    MessageBox.Show("Bitte geben Sie einen Namen ein.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                newStaff = new AddStaffType
                {
                    Name = name.Text,
                    Sex = sex.SelectedIndex,
                    JobRole = jobRole.SelectedIndex,
                    EmploymentType = employmentType.SelectedIndex,
                    YearlySalary = float.TryParse(yearlySalary.Text, out var salary) ? salary : null,
                    ContactInfo = string.IsNullOrWhiteSpace(contactInfo.Text) ? null : contactInfo.Text,
                    Address = string.IsNullOrWhiteSpace(address.Text) ? null : address.Text,
                    HireDate = hireDate.Value.ToString("yyyy-MM-dd"),
                    Notes = string.IsNullOrWhiteSpace(notes.Text) ? null : notes.Text,
                    IsActive = isActive.Checked,
                    AuthorizedSpecies = new List<Guid>() // Hier könntest du später eine Auswahl hinzufügen
                };

                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        public partial class DeleteWorkersForm : Form
        {
            internal List<Staff> workertodelete = [];
            private List<Staff> addedworkers;
            private FlowLayoutPanel flowWorkers = new FlowLayoutPanel();
            private Button btnDelete = new Button();
            private Button btnCancel = new Button();

            public DeleteWorkersForm(List<Staff> worker)
            {
                this.addedworkers = worker;
                flowWorkers.Top = 20;
                flowWorkers.Left = 20;
                btnDelete.Top = flowWorkers.Bottom + 20;
                btnDelete.Left = 20;
                btnDelete.Text = "Delete";
                btnCancel.Top = flowWorkers.Bottom + 20;
                btnCancel.Left = btnDelete.Right + 20;
                btnCancel.Text = "Cancel";
                BuildCheckboxList();
                Controls.Add(flowWorkers);
                Controls.Add(btnDelete);
                Controls.Add(btnCancel);

                btnDelete.Click += btnDelete_Click;
                btnCancel.Click += btnCancel_Click;
            }

            private void BuildCheckboxList()
            {
                flowWorkers.Controls.Clear();
                flowWorkers.FlowDirection = FlowDirection.TopDown;
                flowWorkers.WrapContents = false;
                flowWorkers.AutoScroll = true;

                foreach (var staff in addedworkers)
                {
                    CheckBox cb = new CheckBox();
                    cb.Text = $"{staff.Name} ({staff.JobRole})";
                    cb.AutoSize = true;
                    cb.Tag = staff; // store reference

                    flowWorkers.Controls.Add(cb);

                }
            }

            private void btnDelete_Click(object sender, EventArgs e)
            {

                foreach (CheckBox cb in flowWorkers.Controls)
                {
                    if (cb.Checked)
                    {
                        workertodelete.Add((Staff)cb.Tag);
                    }
                }

                if (workertodelete.Count == 0)
                {
                    MessageBox.Show("Bitte wählen Sie mindestens einen Mitarbeiter aus.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
            }

            private void btnCancel_Click(object sender, EventArgs e)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
