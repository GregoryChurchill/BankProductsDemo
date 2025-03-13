namespace Admin
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            ShowVersions();
        }

        private void ShowVersions()
        {
            labelAppName.Text = Application.ProductName;
            labelVersion.Text = @"Version: " + Application.ProductVersion;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {

        }
    }
}
