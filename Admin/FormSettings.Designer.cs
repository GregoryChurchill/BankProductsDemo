namespace Admin
{
    partial class FormSettings
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            buttonClose = new Button();
            toolTipMain = new ToolTip(components);
            tabControl1 = new TabControl();
            tabPageGeneral = new TabPage();
            folderBrowserDialog = new FolderBrowserDialog();
            openFileDialog = new OpenFileDialog();
            buttonSave = new Button();
            buttonBrowseDirectory = new Button();
            textBoxCacheDirectory = new TextBox();
            labelCacheLocation = new Label();
            tabControl1.SuspendLayout();
            tabPageGeneral.SuspendLayout();
            SuspendLayout();
            // 
            // buttonClose
            // 
            buttonClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonClose.DialogResult = DialogResult.Cancel;
            buttonClose.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            buttonClose.Location = new Point(638, 192);
            buttonClose.Margin = new Padding(4, 3, 4, 3);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(98, 30);
            buttonClose.TabIndex = 9;
            buttonClose.Text = "Close";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += buttonClose_Click;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPageGeneral);
            tabControl1.Location = new Point(14, 14);
            tabControl1.Margin = new Padding(4, 3, 4, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(727, 159);
            tabControl1.TabIndex = 36;
            // 
            // tabPageGeneral
            // 
            tabPageGeneral.Controls.Add(buttonBrowseDirectory);
            tabPageGeneral.Controls.Add(textBoxCacheDirectory);
            tabPageGeneral.Controls.Add(labelCacheLocation);
            tabPageGeneral.Location = new Point(4, 24);
            tabPageGeneral.Margin = new Padding(4, 3, 4, 3);
            tabPageGeneral.Name = "tabPageGeneral";
            tabPageGeneral.Padding = new Padding(4, 3, 4, 3);
            tabPageGeneral.Size = new Size(719, 131);
            tabPageGeneral.TabIndex = 0;
            tabPageGeneral.Text = "General";
            tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            buttonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonSave.DialogResult = DialogResult.Cancel;
            buttonSave.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSave.Location = new Point(533, 192);
            buttonSave.Margin = new Padding(4, 3, 4, 3);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(98, 30);
            buttonSave.TabIndex = 8;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonBrowseDirectory
            // 
            buttonBrowseDirectory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonBrowseDirectory.Location = new Point(674, 15);
            buttonBrowseDirectory.Name = "buttonBrowseDirectory";
            buttonBrowseDirectory.Size = new Size(27, 23);
            buttonBrowseDirectory.TabIndex = 13;
            buttonBrowseDirectory.Text = "...";
            buttonBrowseDirectory.UseVisualStyleBackColor = true;
            buttonBrowseDirectory.Click += buttonBrowseDirectory_Click;
            // 
            // textBoxCacheDirectory
            // 
            textBoxCacheDirectory.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxCacheDirectory.Location = new Point(113, 16);
            textBoxCacheDirectory.Name = "textBoxCacheDirectory";
            textBoxCacheDirectory.ReadOnly = true;
            textBoxCacheDirectory.Size = new Size(555, 23);
            textBoxCacheDirectory.TabIndex = 12;
            // 
            // labelCacheLocation
            // 
            labelCacheLocation.AutoSize = true;
            labelCacheLocation.Location = new Point(15, 20);
            labelCacheLocation.Name = "labelCacheLocation";
            labelCacheLocation.Size = new Size(92, 15);
            labelCacheLocation.TabIndex = 11;
            labelCacheLocation.Text = "Cache Location:";
            // 
            // FormSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonClose;
            ClientSize = new Size(756, 235);
            Controls.Add(buttonSave);
            Controls.Add(tabControl1);
            Controls.Add(buttonClose);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(772, 273);
            Name = "FormSettings";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";
            FormClosing += FormSettings_FormClosing;
            Load += FormSettings_Load;
            tabControl1.ResumeLayout(false);
            tabPageGeneral.ResumeLayout(false);
            tabPageGeneral.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button buttonClose;
        private ToolTip toolTipMain;
        private TabControl tabControl1;
        private TabPage tabPageGeneral;
        private FolderBrowserDialog folderBrowserDialog;
        private OpenFileDialog openFileDialog;
        private Button buttonSave;
        private Button buttonBrowseDirectory;
        public TextBox textBoxCacheDirectory;
        private Label labelCacheLocation;
    }
}