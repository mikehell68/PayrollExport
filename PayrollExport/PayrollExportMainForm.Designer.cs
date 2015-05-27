namespace PayrollExport
{
    partial class _payrollExportMainForm
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
            this._mainContainerPanel = new System.Windows.Forms.Panel();
            this._contentPanel = new System.Windows.Forms.Panel();
            this._exportPeriodLabel = new System.Windows.Forms.Label();
            this._exportPeriodComboBox = new System.Windows.Forms.ComboBox();
            this._endDateLabel = new System.Windows.Forms.Label();
            this._startDateLabel = new System.Windows.Forms.Label();
            this._endDate = new System.Windows.Forms.DateTimePicker();
            this._startDate = new System.Windows.Forms.DateTimePicker();
            this._bottomButtonPanel = new System.Windows.Forms.Panel();
            this._closeButton = new System.Windows.Forms.Button();
            this._doExportPayrollButton = new System.Windows.Forms.Button();
            this._mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._mainContainerPanel.SuspendLayout();
            this._contentPanel.SuspendLayout();
            this._bottomButtonPanel.SuspendLayout();
            this._mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _mainContainerPanel
            // 
            this._mainContainerPanel.Controls.Add(this._contentPanel);
            this._mainContainerPanel.Controls.Add(this._bottomButtonPanel);
            this._mainContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainContainerPanel.Location = new System.Drawing.Point(0, 24);
            this._mainContainerPanel.Name = "_mainContainerPanel";
            this._mainContainerPanel.Size = new System.Drawing.Size(244, 172);
            this._mainContainerPanel.TabIndex = 0;
            // 
            // _contentPanel
            // 
            this._contentPanel.Controls.Add(this._exportPeriodLabel);
            this._contentPanel.Controls.Add(this._exportPeriodComboBox);
            this._contentPanel.Controls.Add(this._endDateLabel);
            this._contentPanel.Controls.Add(this._startDateLabel);
            this._contentPanel.Controls.Add(this._endDate);
            this._contentPanel.Controls.Add(this._startDate);
            this._contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._contentPanel.Location = new System.Drawing.Point(0, 0);
            this._contentPanel.Name = "_contentPanel";
            this._contentPanel.Size = new System.Drawing.Size(244, 118);
            this._contentPanel.TabIndex = 1;
            // 
            // _exportPeriodLabel
            // 
            this._exportPeriodLabel.AutoSize = true;
            this._exportPeriodLabel.Location = new System.Drawing.Point(3, 37);
            this._exportPeriodLabel.Name = "_exportPeriodLabel";
            this._exportPeriodLabel.Size = new System.Drawing.Size(73, 13);
            this._exportPeriodLabel.TabIndex = 5;
            this._exportPeriodLabel.Text = "Export Period:";
            // 
            // _exportPeriodComboBox
            // 
            this._exportPeriodComboBox.FormattingEnabled = true;
            this._exportPeriodComboBox.Location = new System.Drawing.Point(82, 29);
            this._exportPeriodComboBox.Name = "_exportPeriodComboBox";
            this._exportPeriodComboBox.Size = new System.Drawing.Size(147, 21);
            this._exportPeriodComboBox.TabIndex = 4;
            this._exportPeriodComboBox.SelectedIndexChanged += new System.EventHandler(this._exportPeriodComboBox_SelectedIndexChanged);
            // 
            // _endDateLabel
            // 
            this._endDateLabel.AutoSize = true;
            this._endDateLabel.Location = new System.Drawing.Point(3, 62);
            this._endDateLabel.Name = "_endDateLabel";
            this._endDateLabel.Size = new System.Drawing.Size(55, 13);
            this._endDateLabel.TabIndex = 3;
            this._endDateLabel.Text = "End Date:";
            // 
            // _startDateLabel
            // 
            this._startDateLabel.AutoSize = true;
            this._startDateLabel.Location = new System.Drawing.Point(3, 9);
            this._startDateLabel.Name = "_startDateLabel";
            this._startDateLabel.Size = new System.Drawing.Size(58, 13);
            this._startDateLabel.TabIndex = 2;
            this._startDateLabel.Text = "Start Date:";
            // 
            // _endDate
            // 
            this._endDate.CustomFormat = "ddd dd MMM yyyy";
            this._endDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this._endDate.Location = new System.Drawing.Point(82, 56);
            this._endDate.Name = "_endDate";
            this._endDate.Size = new System.Drawing.Size(147, 20);
            this._endDate.TabIndex = 1;
            // 
            // _startDate
            // 
            this._startDate.CustomFormat = "ddd dd MMM yyyy";
            this._startDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this._startDate.Location = new System.Drawing.Point(82, 3);
            this._startDate.Name = "_startDate";
            this._startDate.Size = new System.Drawing.Size(147, 20);
            this._startDate.TabIndex = 0;
            // 
            // _bottomButtonPanel
            // 
            this._bottomButtonPanel.Controls.Add(this._closeButton);
            this._bottomButtonPanel.Controls.Add(this._doExportPayrollButton);
            this._bottomButtonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomButtonPanel.Location = new System.Drawing.Point(0, 118);
            this._bottomButtonPanel.Name = "_bottomButtonPanel";
            this._bottomButtonPanel.Size = new System.Drawing.Size(244, 54);
            this._bottomButtonPanel.TabIndex = 0;
            // 
            // _closeButton
            // 
            this._closeButton.Location = new System.Drawing.Point(154, 19);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(75, 23);
            this._closeButton.TabIndex = 1;
            this._closeButton.Text = "Close";
            this._closeButton.UseVisualStyleBackColor = true;
            this._closeButton.Click += new System.EventHandler(this._closeButton_Click);
            // 
            // _doExportPayrollButton
            // 
            this._doExportPayrollButton.Location = new System.Drawing.Point(73, 19);
            this._doExportPayrollButton.Name = "_doExportPayrollButton";
            this._doExportPayrollButton.Size = new System.Drawing.Size(75, 23);
            this._doExportPayrollButton.TabIndex = 0;
            this._doExportPayrollButton.Text = "Export";
            this._doExportPayrollButton.UseVisualStyleBackColor = true;
            this._doExportPayrollButton.Click += new System.EventHandler(this._doExportPayrollButton_Click);
            // 
            // _mainMenuStrip
            // 
            this._mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this._mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this._mainMenuStrip.Name = "_mainMenuStrip";
            this._mainMenuStrip.Size = new System.Drawing.Size(244, 24);
            this._mainMenuStrip.TabIndex = 1;
            this._mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(104, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Visible = false;
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // _payrollExportMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 196);
            this.Controls.Add(this._mainContainerPanel);
            this.Controls.Add(this._mainMenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this._mainMenuStrip;
            this.Name = "_payrollExportMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Payroll Export";
            this._mainContainerPanel.ResumeLayout(false);
            this._contentPanel.ResumeLayout(false);
            this._contentPanel.PerformLayout();
            this._bottomButtonPanel.ResumeLayout(false);
            this._mainMenuStrip.ResumeLayout(false);
            this._mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel _mainContainerPanel;
        private System.Windows.Forms.Panel _contentPanel;
        private System.Windows.Forms.DateTimePicker _endDate;
        private System.Windows.Forms.DateTimePicker _startDate;
        private System.Windows.Forms.Panel _bottomButtonPanel;
        private System.Windows.Forms.Label _endDateLabel;
        private System.Windows.Forms.Label _startDateLabel;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.Button _doExportPayrollButton;
        private System.Windows.Forms.MenuStrip _mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label _exportPeriodLabel;
        private System.Windows.Forms.ComboBox _exportPeriodComboBox;
    }
}

