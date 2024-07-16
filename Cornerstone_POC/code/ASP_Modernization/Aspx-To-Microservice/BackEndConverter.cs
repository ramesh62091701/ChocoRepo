using Extractor.Model;
using Extractor.Utils;
using Extractor.Service;

namespace Aspx_To_Microservice
{
    public partial class BackEndConverter : Form
    {
        public BackEndConverter()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            Logger.LogCreated += UpdateLog;
        }

        private void UpdateLog(string message)
        {
            if (this.IsDisposed || txtBELogs.IsDisposed)
            {
                return;
            }

            if (txtBELogs.InvokeRequired)
            {
                txtBELogs.Invoke(new Action<string>(UpdateLog), message);
            }
            else
            {
                if (string.IsNullOrEmpty(txtBELogs.Text))
                {
                    txtBELogs.AppendText($"\r\n{message}");
                }
                else
                {
                    txtBELogs.AppendText($"\r\n{message}");
                }
            }
        }

        private void ClearLog()
        {
            txtBELogs.Text = string.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var result = openFileDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtSolutionPath.Text = openFileDialog1.FileName;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string framework = rbNet6.Checked ? "net6.0" : "net8.0";

            var request = new BERequest()
            {
                SolutionPath = txtSolutionPath.Text,
                OutputPath = txtOutputPath.Text,
                ClassName = txtClassName.Text,
                Framework = framework,
                MultipleProject = cbMultiProject.Checked ? true : false,
                AddComments = cbAddComments.Checked ? true : false,
                Swagger = cbAddSwagger.Checked ? true : false,
            };

            await BackendProcess.GetAllMethodsInClass(request);
            Logger.Log("Process Completed");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtOutputPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
