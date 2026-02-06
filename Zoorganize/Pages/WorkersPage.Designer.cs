namespace Zoorganize.Pages
{
    partial class WorkersPage
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
            addWorker = new Button();
            workerOverview = new FlowLayoutPanel();
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
            label1.Size = new Size(212, 46);
            label1.TabIndex = 1;
            label1.Text = "Mitarbeiter verwalten";
            //
            // Panel1
            //
            workerOverview.Location = new Point(12, 125);
            workerOverview.AutoScroll = true;
            workerOverview.FlowDirection = FlowDirection.TopDown;
            workerOverview.WrapContents = false;
            workerOverview.Name = "typeOverview";
            workerOverview.Size = new Size(199, 412);
            workerOverview.TabIndex = 3;
            // 
            // button2
            // 
            addWorker.Location = new Point(12, 58);
            addWorker.Name = "addWorker";
            addWorker.Size = new Size(197, 29);
            addWorker.TabIndex = 2;
            addWorker.Text = "Mitarbeiter hinzufügen";
            addWorker.UseVisualStyleBackColor = true;
            addWorker.Click += AddWorker_Click;
            // 
            // WorkersPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(447, 600);
            Controls.Add(addWorker);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(workerOverview);
            FormBorderStyle = FormBorderStyle.None;
            Name = "WorkersPage";
            Text = "WorkersPage";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Button addWorker;
        private FlowLayoutPanel workerOverview;
    }
}