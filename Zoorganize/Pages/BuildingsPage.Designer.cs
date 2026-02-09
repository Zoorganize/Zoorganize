using System;

namespace Zoorganize.Pages
{
    partial class BuildingsPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            label1 = new Label();
            penButton = new Button();
            workerButton = new Button();
            visitorButton = new Button();
            addBuilding = new Button();
            showBuildings = new FlowLayoutPanel();
            delete = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 15F);
            button1.Location = new Point(12, 548);
            button1.Name = "button1";
            button1.Size = new Size(150, 40);
            button1.TabIndex = 0;
            button1.Text = "Zurück";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 25F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(309, 46);
            label1.TabIndex = 1;
            label1.Text = "Gebäude verwalten";
            // 
            // penButton
            // 
            penButton.Font = new Font("Segoe UI", 15F);
            penButton.Location = new Point(12, 58);
            penButton.Name = "penButton";
            penButton.Size = new Size(140, 50);
            penButton.TabIndex = 2;
            penButton.Text = "Gehege";
            penButton.UseVisualStyleBackColor = true;
            penButton.Click += penButton_Click;
            // 
            // workerButton
            // 
            workerButton.Font = new Font("Segoe UI", 15F);
            workerButton.Location = new Point(348, 58);
            workerButton.Name = "workerButton";
            workerButton.Size = new Size(140, 50);
            workerButton.TabIndex = 3;
            workerButton.Text = "Mitarbeiter";
            workerButton.UseVisualStyleBackColor = true;
            workerButton.Click += workerButton_Click;
            // 
            // visitorButton
            // 
            visitorButton.Font = new Font("Segoe UI", 15F);
            visitorButton.Location = new Point(181, 58);
            visitorButton.Name = "visitorButton";
            visitorButton.Size = new Size(140, 50);
            visitorButton.TabIndex = 4;
            visitorButton.Text = "Besucher";
            visitorButton.UseVisualStyleBackColor = true;
            visitorButton.Click += visitorButton_Click;
            // 
            // addBuilding
            // 
            addBuilding.Location = new Point(12, 114);
            addBuilding.Name = "addBuilding";
            addBuilding.Size = new Size(202, 25);
            addBuilding.TabIndex = 5;
            addBuilding.Text = "Neues Gebäude hinzufügen";
            addBuilding.UseVisualStyleBackColor = true;
            addBuilding.Click += button2_Click;
            // 
            // showBuildings
            // 
            showBuildings.Location = new Point(14, 140);
            showBuildings.Name = "showBuildings";
            showBuildings.Size = new Size(500, 600);
            showBuildings.TabIndex = 6;
            // 
            // delete
            // 
            delete.Font = new Font("Segoe UI", 12F);
            delete.Location = new Point(230, 114);
            delete.Name = "delete";
            delete.Size = new Size(150, 25);
            delete.TabIndex = 0;
            delete.Text = "Löschen";
            delete.Click += delete_Click;
            // 
            // BuildingsPage
            // 
            ClientSize = new Size(660, 514);
            Controls.Add(showBuildings);
            Controls.Add(addBuilding);
            Controls.Add(visitorButton);
            Controls.Add(workerButton);
            Controls.Add(penButton);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(delete);
            FormBorderStyle = FormBorderStyle.None;
            Name = "BuildingsPage";
            Text = "BuildingsPage";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Button penButton;
        private Button workerButton;
        private Button visitorButton;
        private Button addBuilding;
        private FlowLayoutPanel showBuildings;
        private Button delete;
    }
}