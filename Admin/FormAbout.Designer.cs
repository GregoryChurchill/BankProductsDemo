using System.Windows.Forms;

namespace Admin
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            buttonClose = new Button();
            panelMain = new Panel();
            labelAppName = new Label();
            labelVersion = new Label();
            pictureBoxMain = new PictureBox();
            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMain).BeginInit();
            SuspendLayout();
            // 
            // buttonClose
            // 
            buttonClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonClose.BackColor = SystemColors.Control;
            buttonClose.DialogResult = DialogResult.Cancel;
            buttonClose.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            buttonClose.Location = new Point(399, 252);
            buttonClose.Margin = new Padding(4, 3, 4, 3);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(103, 30);
            buttonClose.TabIndex = 9;
            buttonClose.Text = "Close";
            buttonClose.UseVisualStyleBackColor = false;
            buttonClose.Click += buttonClose_Click;
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.White;
            panelMain.BackgroundImageLayout = ImageLayout.None;
            panelMain.BorderStyle = BorderStyle.FixedSingle;
            panelMain.Controls.Add(pictureBoxMain);
            panelMain.Controls.Add(labelAppName);
            panelMain.Controls.Add(labelVersion);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Margin = new Padding(4, 3, 4, 3);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(516, 295);
            panelMain.TabIndex = 17;
            // 
            // labelAppName
            // 
            labelAppName.AutoSize = true;
            labelAppName.BackColor = Color.Transparent;
            labelAppName.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            labelAppName.Location = new Point(192, 107);
            labelAppName.Margin = new Padding(4, 0, 4, 0);
            labelAppName.Name = "labelAppName";
            labelAppName.Size = new Size(160, 24);
            labelAppName.TabIndex = 13;
            labelAppName.Text = "App Name here...";
            // 
            // labelVersion
            // 
            labelVersion.AutoSize = true;
            labelVersion.BackColor = Color.Transparent;
            labelVersion.Location = new Point(193, 145);
            labelVersion.Margin = new Padding(4, 0, 4, 0);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new Size(105, 15);
            labelVersion.TabIndex = 14;
            labelVersion.Text = "App Version here...";
            // 
            // pictureBoxMain
            // 
            pictureBoxMain.Image = (Image)resources.GetObject("pictureBoxMain.Image");
            pictureBoxMain.Location = new Point(54, 85);
            pictureBoxMain.Name = "pictureBoxMain";
            pictureBoxMain.Size = new Size(99, 100);
            pictureBoxMain.TabIndex = 15;
            pictureBoxMain.TabStop = false;
            // 
            // FormAbout
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = buttonClose;
            ClientSize = new Size(516, 295);
            Controls.Add(buttonClose);
            Controls.Add(panelMain);
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAbout";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "About";
            Load += FormAbout_Load;
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMain).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button buttonClose;
        private Panel panelMain;
        private Label labelAppName;
        private Label labelVersion;
        private PictureBox pictureBoxMain;
    }
}