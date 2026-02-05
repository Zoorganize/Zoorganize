namespace Zoorganize.Pages
{
    partial class MainPage
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
            button2 = new Button();
            button3 = new Button();
            sqliteCommand1 = new Microsoft.Data.Sqlite.SqliteCommand();
            label2 = new Label();
            appointmentList = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 15F);
            button1.Location = new Point(12, 85);
            button1.Name = "button1";
            button1.Size = new Size(140, 50);
            button1.TabIndex = 0;
            button1.Text = "Tiere";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 25F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(258, 46);
            label1.TabIndex = 1;
            label1.Text = "Zoo Name Here";
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 15F);
            button2.Location = new Point(158, 85);
            button2.Name = "button2";
            button2.Size = new Size(140, 50);
            button2.TabIndex = 2;
            button2.Text = "Gebäude";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 15F);
            button3.Location = new Point(304, 85);
            button3.Name = "button3";
            button3.Size = new Size(140, 50);
            button3.TabIndex = 3;
            button3.Text = "Mitarbeiter";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // sqliteCommand1
            // 
            sqliteCommand1.CommandTimeout = 30;
            sqliteCommand1.Connection = null;
            sqliteCommand1.Transaction = null;
            sqliteCommand1.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15F);
            label2.Location = new Point(12, 154);
            label2.Name = "label2";
            label2.Size = new Size(84, 28);
            label2.TabIndex = 4;
            label2.Text = "Termine:";
            // 
            // appointmentList
            // 
            appointmentList.Location = new Point(12, 185);
            appointmentList.Multiline = true;
            appointmentList.Name = "appointmentList";
            appointmentList.Size = new Size(204, 403);
            appointmentList.TabIndex = 5;
            // 
            // MainPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 600);
            Controls.Add(appointmentList);
            Controls.Add(label2);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(label1);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "MainPage";
            Text = "MainPage";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Button button2;
        private Button button3;
        private Microsoft.Data.Sqlite.SqliteCommand sqliteCommand1;
        private Label label2;
        private TextBox appointmentList;
    }
}