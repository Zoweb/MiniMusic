namespace MiniMusic.Installer.Version2
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.Title = new System.Windows.Forms.Label();
            this.UseAdministrator = new System.Windows.Forms.Label();
            this.runAsAdmin = new System.Windows.Forms.Button();
            this.dontRunAsAdministrator = new System.Windows.Forms.Button();
            this.runAsAdministratorPanel = new System.Windows.Forms.Panel();
            this.setLocationPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.locationCalculate = new System.Windows.Forms.Button();
            this.locationUsual = new System.Windows.Forms.Button();
            this.checkLocationPanel = new System.Windows.Forms.Panel();
            this.locationBad = new System.Windows.Forms.Button();
            this.locationAndSpace = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.locationOk = new System.Windows.Forms.Button();
            this.installLogPanel = new System.Windows.Forms.Panel();
            this.logOutput = new System.Windows.Forms.ListBox();
            this.runAsAdministratorPanel.SuspendLayout();
            this.setLocationPanel.SuspendLayout();
            this.checkLocationPanel.SuspendLayout();
            this.installLogPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.Title.Location = new System.Drawing.Point(18, 15);
            this.Title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(149, 25);
            this.Title.TabIndex = 0;
            this.Title.Text = "Let\'s get started!";
            // 
            // UseAdministrator
            // 
            this.UseAdministrator.AutoSize = true;
            this.UseAdministrator.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.UseAdministrator.Location = new System.Drawing.Point(1, 1);
            this.UseAdministrator.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UseAdministrator.Name = "UseAdministrator";
            this.UseAdministrator.Size = new System.Drawing.Size(315, 21);
            this.UseAdministrator.TabIndex = 1;
            this.UseAdministrator.Text = "Do you want to run me as an administrator?";
            // 
            // runAsAdmin
            // 
            this.runAsAdmin.Location = new System.Drawing.Point(5, 25);
            this.runAsAdmin.Name = "runAsAdmin";
            this.runAsAdmin.Size = new System.Drawing.Size(256, 35);
            this.runAsAdmin.TabIndex = 2;
            this.runAsAdmin.Text = "I want POWER!!!";
            this.runAsAdmin.UseVisualStyleBackColor = true;
            this.runAsAdmin.Click += new System.EventHandler(this.runAsAdmin_Click);
            // 
            // dontRunAsAdministrator
            // 
            this.dontRunAsAdministrator.Location = new System.Drawing.Point(267, 25);
            this.dontRunAsAdministrator.Name = "dontRunAsAdministrator";
            this.dontRunAsAdministrator.Size = new System.Drawing.Size(132, 35);
            this.dontRunAsAdministrator.TabIndex = 3;
            this.dontRunAsAdministrator.Text = "how even‽";
            this.dontRunAsAdministrator.UseVisualStyleBackColor = true;
            this.dontRunAsAdministrator.Click += new System.EventHandler(this.button2_Click);
            // 
            // runAsAdministratorPanel
            // 
            this.runAsAdministratorPanel.Controls.Add(this.UseAdministrator);
            this.runAsAdministratorPanel.Controls.Add(this.dontRunAsAdministrator);
            this.runAsAdministratorPanel.Controls.Add(this.runAsAdmin);
            this.runAsAdministratorPanel.Location = new System.Drawing.Point(12, 55);
            this.runAsAdministratorPanel.Name = "runAsAdministratorPanel";
            this.runAsAdministratorPanel.Size = new System.Drawing.Size(402, 67);
            this.runAsAdministratorPanel.TabIndex = 4;
            // 
            // setLocationPanel
            // 
            this.setLocationPanel.Controls.Add(this.label1);
            this.setLocationPanel.Controls.Add(this.locationCalculate);
            this.setLocationPanel.Controls.Add(this.locationUsual);
            this.setLocationPanel.Location = new System.Drawing.Point(12, 147);
            this.setLocationPanel.Name = "setLocationPanel";
            this.setLocationPanel.Size = new System.Drawing.Size(402, 67);
            this.setLocationPanel.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(225, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Where do you want me to live?";
            // 
            // locationCalculate
            // 
            this.locationCalculate.Location = new System.Drawing.Point(267, 25);
            this.locationCalculate.Name = "locationCalculate";
            this.locationCalculate.Size = new System.Drawing.Size(132, 35);
            this.locationCalculate.TabIndex = 3;
            this.locationCalculate.Text = "lets see...";
            this.locationCalculate.UseVisualStyleBackColor = true;
            this.locationCalculate.Click += new System.EventHandler(this.locationCalculate_Click);
            // 
            // locationUsual
            // 
            this.locationUsual.Location = new System.Drawing.Point(5, 25);
            this.locationUsual.Name = "locationUsual";
            this.locationUsual.Size = new System.Drawing.Size(256, 35);
            this.locationUsual.TabIndex = 2;
            this.locationUsual.Text = "Just the usual...";
            this.locationUsual.UseVisualStyleBackColor = true;
            this.locationUsual.Click += new System.EventHandler(this.locationUsual_Click);
            // 
            // checkLocationPanel
            // 
            this.checkLocationPanel.Controls.Add(this.locationBad);
            this.checkLocationPanel.Controls.Add(this.locationAndSpace);
            this.checkLocationPanel.Controls.Add(this.label2);
            this.checkLocationPanel.Controls.Add(this.locationOk);
            this.checkLocationPanel.Location = new System.Drawing.Point(12, 233);
            this.checkLocationPanel.Name = "checkLocationPanel";
            this.checkLocationPanel.Size = new System.Drawing.Size(402, 119);
            this.checkLocationPanel.TabIndex = 6;
            // 
            // locationBad
            // 
            this.locationBad.Location = new System.Drawing.Point(267, 81);
            this.locationBad.Name = "locationBad";
            this.locationBad.Size = new System.Drawing.Size(132, 35);
            this.locationBad.TabIndex = 5;
            this.locationBad.Text = "Woops! Wrong!";
            this.locationBad.UseVisualStyleBackColor = true;
            this.locationBad.Click += new System.EventHandler(this.locationBad_Click);
            // 
            // locationAndSpace
            // 
            this.locationAndSpace.AutoSize = true;
            this.locationAndSpace.Location = new System.Drawing.Point(20, 25);
            this.locationAndSpace.Name = "locationAndSpace";
            this.locationAndSpace.Size = new System.Drawing.Size(135, 21);
            this.locationAndSpace.TabIndex = 4;
            this.locationAndSpace.Text = "[an error occured]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label2.Location = new System.Drawing.Point(1, 1);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Is this OK?";
            // 
            // locationOk
            // 
            this.locationOk.Location = new System.Drawing.Point(3, 81);
            this.locationOk.Name = "locationOk";
            this.locationOk.Size = new System.Drawing.Size(258, 35);
            this.locationOk.TabIndex = 3;
            this.locationOk.Text = "Good, that\'s good!";
            this.locationOk.UseVisualStyleBackColor = true;
            this.locationOk.Click += new System.EventHandler(this.locationOk_Click);
            // 
            // installLogPanel
            // 
            this.installLogPanel.Controls.Add(this.logOutput);
            this.installLogPanel.Location = new System.Drawing.Point(12, 369);
            this.installLogPanel.Name = "installLogPanel";
            this.installLogPanel.Size = new System.Drawing.Size(402, 136);
            this.installLogPanel.TabIndex = 7;
            // 
            // logOutput
            // 
            this.logOutput.FormattingEnabled = true;
            this.logOutput.ItemHeight = 21;
            this.logOutput.Location = new System.Drawing.Point(3, 3);
            this.logOutput.Name = "logOutput";
            this.logOutput.Size = new System.Drawing.Size(396, 130);
            this.logOutput.TabIndex = 0;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 588);
            this.Controls.Add(this.installLogPanel);
            this.Controls.Add(this.checkLocationPanel);
            this.Controls.Add(this.setLocationPanel);
            this.Controls.Add(this.runAsAdministratorPanel);
            this.Controls.Add(this.Title);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "MiniMusic Installer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.runAsAdministratorPanel.ResumeLayout(false);
            this.runAsAdministratorPanel.PerformLayout();
            this.setLocationPanel.ResumeLayout(false);
            this.setLocationPanel.PerformLayout();
            this.checkLocationPanel.ResumeLayout(false);
            this.checkLocationPanel.PerformLayout();
            this.installLogPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label UseAdministrator;
        private System.Windows.Forms.Button runAsAdmin;
        private System.Windows.Forms.Button dontRunAsAdministrator;
        private System.Windows.Forms.Panel runAsAdministratorPanel;
        private System.Windows.Forms.Panel setLocationPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button locationCalculate;
        private System.Windows.Forms.Button locationUsual;
        private System.Windows.Forms.Panel checkLocationPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button locationOk;
        private System.Windows.Forms.Label locationAndSpace;
        private System.Windows.Forms.Button locationBad;
        private System.Windows.Forms.Panel installLogPanel;
        private System.Windows.Forms.ListBox logOutput;
    }
}

