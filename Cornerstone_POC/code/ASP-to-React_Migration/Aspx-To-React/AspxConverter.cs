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
            if (string.IsNullOrEmpty(txtLogs.Text))
            {
                txtLogs.AppendText($"\r\n{message}");
            }
            else
            {
                txtLogs.AppendText($"\r\n{message}");
            }
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
            /*if (rdbImage.Checked)
            {
                txtFigmaUrl.Text = string.Empty;
            }
            else
            {
                txtFigmaPath.Text = string.Empty;
            }*/
            txtFigmaUrl.Enabled = !rdbImage.Checked;
        }

        private async void btnConvertToReact_Click(object sender, EventArgs e)
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

            if (request.IsCustom && !request.IsFigmaUrl)
            {
                using (var aspxToFigmaFrm = new AspxToFigmaMapping())
                {
                    request.Components = await Processor.GetControls(request);
                    if (request.Components.AspComponents?.Count > 0)
                    {
                        Logger.Log("Use the Mapping window to map controls.");
                        aspxToFigmaFrm.Initialize(request);
                        aspxToFigmaFrm.ShowDialog(this);
                        request.MappedControls = aspxToFigmaFrm.MappedControls;

                    }
                }
            }
            // Get Controls to Map
            await Processor.MigrateToReact(request);
        }
    }
}
