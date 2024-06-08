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
            SetFigmaControls();
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
            ClearLog();
            var request = new Request()
            {
                AspxPagePath = txtAspxPath.Text,
                ImagePath = txtFigmaPath.Text,
                OutputPath = txtOutput.Text,
                IsCustom = radioButton2.Checked,
                IsFigmaUrl = rdbFileUrl.Checked,
                FigmaUrl = txtFigmaUrl.Text,
            };
            await Processor.MigrateToHtml(request);
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

        private void rdbImage_CheckedChanged(object sender, EventArgs e)
        {
            SetFigmaControls();
        }

        private void SetFigmaControls()
        {
            txtFigmaPath.Enabled = rdbImage.Checked;
            btnImage.Enabled = rdbImage.Checked;
            if (rdbImage.Checked)
            {
                txtFigmaPath.Text = string.Empty;
            }
            else
            {
                txtFigmaUrl.Text = string.Empty;
            }
            txtFigmaUrl.Enabled = !rdbImage.Checked;
        }

        private void btnConvertToReact_Click(object sender, EventArgs e)
        {

        }
    }
}
