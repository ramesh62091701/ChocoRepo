using Extractor;
using Extractor.Model;
using Extractor.Service;
using Extractor.Utils;

namespace Aspx_To_React
{
    public partial class AspxConverter : Form
    {
        public AspxConverter()
        {
            InitializeComponent();
            Logger.LogCreated += UpdateLog;
        }

        private void UpdateLog(string message)
        {
            txtLogs.AppendText($"\r\n{message}");
        }

        private void ClearLog()
        {
            txtLogs.Text = string.Empty;
        }

        private async void btnConvert_Click(object sender, EventArgs e)
        {
            var request = new Request()
            {
                AspxPagePath = txtAspxPath.Text,
                ImagePath = txtFigmaPath.Text,
                OutputPath = txtOutput.Text,
                IsCSOD = radioButton2.Checked
            };
            await Processor.Migrate(request);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtFigmaPath.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtAspxPath.Text = openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtOutput.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
