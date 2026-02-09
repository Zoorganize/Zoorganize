using Microsoft.EntityFrameworkCore;

namespace Zoorganize.Pages
{
    partial class AnimalsPage
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
            addAnimal = new Button();
            speciesLabel = new Label();
            animalLabel = new Label();
            typeOverview = new FlowLayoutPanel();
            animalOverview = new FlowLayoutPanel();
            addSpeciesButton = new Button();
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
            //detele Button
            //
            delete.Font = new Font("Segoe UI", 12F);
            delete.Location = new Point(200, 67);
            delete.Name = "delete";
            delete.Size = new Size(150, 31);
            delete.TabIndex = 0;
            delete.Text = "Löschen";
            delete.Click += delete_Click;
            //
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 25F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(259, 46);
            label1.TabIndex = 1;
            label1.Text = "Tier-Verwaltung";
            // 
            // addAnimal
            // 
            addAnimal.Font = new Font("Segoe UI", 12F);
            addAnimal.Location = new Point(12, 67);
            addAnimal.Name = "addAnimal";
            addAnimal.Size = new Size(175, 31);
            addAnimal.TabIndex = 2;
            addAnimal.Text = "Neues Tier anlegen";
            addAnimal.UseVisualStyleBackColor = true;
            addAnimal.Click += addAnimal_Click;
            //
            //add Species Button
            //
            addSpeciesButton.Font = new Font("Segoe UI", 12F);
            addSpeciesButton.Location = new Point(360, 67);
            addSpeciesButton.Name = "addSpecies";
            addSpeciesButton.Size = new Size(150, 31);
            addSpeciesButton.TabIndex = 0;
            addSpeciesButton.Text = "Neue Spezies hinzufügen";
            addSpeciesButton.Click += AddSpecies_Click;
            //
            // Species Label
            //
            speciesLabel.Font = new Font("Segoe UI", 12F);
            speciesLabel.Location = new Point(12, 100);
            speciesLabel.Name = "speciesLabel";
            speciesLabel.Size = new Size(175, 22);
            speciesLabel.Text = "Alle Tierarten";
            //
            // Ánimal Label
            //
            animalLabel.Font = new Font("Segoe UI", 12F);
            animalLabel.Location = new Point(255, 100);
            animalLabel.Name = "animalLabel";
            animalLabel.Size = new Size(175, 22);
            animalLabel.Text = "Alle Tiere";
            // 
            // typeOverview
            // 
            typeOverview.Location = new Point(12, 125);
            typeOverview.AutoScroll = true;
            typeOverview.FlowDirection = FlowDirection.TopDown;
            typeOverview.WrapContents = false;
            typeOverview.Name = "typeOverview";
            typeOverview.Size = new Size(199, 412);
            typeOverview.TabIndex = 3;
            // 
            // animalOverview
            // 
            animalOverview.Location = new Point(255, 125);
            animalOverview.AutoScroll = true;
            animalOverview.FlowDirection = FlowDirection.TopDown;
            animalOverview.WrapContents = false;
            animalOverview.Name = "animalOverview";
            animalOverview.Size = new Size(199, 412);
            animalOverview.TabIndex = 4;
            // 
            // AnimalsPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 600);
            Controls.Add(animalOverview);
            Controls.Add(typeOverview);
            Controls.Add(addAnimal);
            Controls.Add(label1);
            Controls.Add(animalLabel);
            Controls.Add(speciesLabel);
            Controls.Add(button1);
            Controls.Add(delete);
            Controls.Add(addSpeciesButton);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AnimalsPage";
            Text = "AnimalsPage";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Button addAnimal;
        private Label speciesLabel;
        private Label animalLabel;
        private FlowLayoutPanel typeOverview;
        private FlowLayoutPanel animalOverview;
        private Button delete;
        private Button addSpeciesButton;
    }
}