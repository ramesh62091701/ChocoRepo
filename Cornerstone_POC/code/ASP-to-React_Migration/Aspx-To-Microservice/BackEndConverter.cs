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
            ClearLog();
            var request = new BERequest()
            {
                SolutionPath = txtSolutionPath.Text,
                ClassName = txtClassName.Text,
            };
            await ReadCSFile.GetAllMethodsInClass(request);

        }

    }
}
