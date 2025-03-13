using Admin.Properties;

namespace Admin
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            SetFormSize();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void SetFormSize()
        {
            if (Settings.Default.FormSettingsLocation.IsEmpty) return;

            if (Settings.Default.FormSettingsMaximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                Location = Settings.Default.FormSettingsLocation;
                Size = Settings.Default.FormSettingsSize;
            }
        }

        private void SaveFormSettings()
        {
            switch (WindowState)
            {
                case FormWindowState.Maximized:
                    Settings.Default.FormSettingsMaximized = true;
                    break;
                case FormWindowState.Normal:
                    Settings.Default.FormSettingsLocation = Location;
                    Settings.Default.FormSettingsSize = Size;
                    Settings.Default.FormSettingsMaximized = false;
                    break;
            }

            Settings.Default.Save();
        }

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFormSettings();
        }

        private void SetDirectory(TextBox textbox)
        {
            if (Directory.Exists(textbox.Text))
            {
                folderBrowserDialog.SelectedPath = textbox.Text;
            }

            var result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                textbox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void buttonBrowseDirectory_Click(object sender, EventArgs e)
        {
            SetDirectory(textBoxCacheDirectory);
        }
    }
}
