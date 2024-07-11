using Aspx_To_Microservice;
using Aspx_To_React;

namespace Main_App

{
    public partial class Main : Form
    {
        private AspxConverter uiMigrationForm;
        private BackEndConverter backendMigrationForm;

        public Main()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnUiMigration_Click(object sender, EventArgs e)
        {
            if (uiMigrationForm == null || uiMigrationForm.IsDisposed)
            {
                uiMigrationForm = new AspxConverter();
                uiMigrationForm.StartPosition = FormStartPosition.CenterScreen;
                uiMigrationForm.Show();
            }
            else
            {
                uiMigrationForm.Focus();
            }
        }

        private void btnBackendMigration_Click(object sender, EventArgs e)
        {
            if (backendMigrationForm == null || backendMigrationForm.IsDisposed)
            {
                backendMigrationForm = new BackEndConverter();
                backendMigrationForm.StartPosition = FormStartPosition.CenterScreen;
                backendMigrationForm.Show();
            }
            else
            {
                backendMigrationForm.Focus();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (uiMigrationForm == null || uiMigrationForm.IsDisposed)
            {
                uiMigrationForm = new AspxConverter();
                uiMigrationForm.StartPosition = FormStartPosition.CenterScreen;
                uiMigrationForm.Show();
            }
            else
            {
                uiMigrationForm.Focus();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (backendMigrationForm == null || backendMigrationForm.IsDisposed)
            {
                backendMigrationForm = new BackEndConverter();
                backendMigrationForm.StartPosition = FormStartPosition.CenterScreen;
                backendMigrationForm.Show();
            }
            else
            {
                backendMigrationForm.Focus();
            }
        }
    }
}
