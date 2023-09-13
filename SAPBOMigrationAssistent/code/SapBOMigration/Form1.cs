using System.ComponentModel;
using System.Diagnostics;
using SAPBOAnalysis;
using SAPBOAnalysis.Models;
using SAPBOAnalysis.PowerBI;
using SAPBOAnalysis.PowerBI.Models;

namespace SapBOMigration
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            BindingList<Connection> objects = new BindingList<Connection>();
            foreach (var conn in ConnectionsManager.GetConnections())
            {
                objects.Add(conn);
            }
            cbConnList.DataSource = objects;
            cbConnList.ValueMember = "ConnectionString";
            cbConnList.DisplayMember = "Name";
            cbConnList.SelectedItem = objects.First(x => x.Name.StartsWith("efashion"));

            //Load Universes
            var universeAnalyser = new UniverseAnalyzer();
            var universes = await universeAnalyser.GetUniverses();
            cbUniverseList.DataSource = universes;
            cbUniverseList.ValueMember = "id";
            cbUniverseList.DisplayMember = "name";
            cbUniverseList.SelectedItem = universes.First(x => x.name.Contains("eFashion"));

            //Load Modes
            var generator = new Generator();
            var modes = generator.GetModes();
            cbMode.DataSource = modes;
            cbMode.ValueMember = "Id";
            cbMode.DisplayMember = "Name";
            cbMode.SelectedItem = modes.First(x => x.Name.StartsWith("Push"));
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                tbAnalysisLog.Text = "Starting...";
                Logger.LogCreated += UpdateAnalyzerLog;
                var powerBiService = new PowerBIService();
                var request = new DataSetRequest();
                request.Conn = ((Connection)cbConnList.SelectedItem).ConnectionString;
                if (rbDocument.Checked)
                {
                    request.Id = textBox1.Text;
                    request.IdType = "Document";
                }
                else
                {
                    request.Id = cbUniverseList.SelectedValue.ToString();
                    request.IdType = "Universe";
                }
                request.Mode = cbMode.SelectedValue.ToString();
                request.Type = "sqlite";
                var response = await powerBiService.GenerateDataSet(request);

                if (!response.Success)
                {
                    MessageBox.Show($"Dataset failed.");
                }
                else
                {
                    MessageBox.Show($"Dataset created {response.Dataset}.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Dataset failed. {ex}");
            }
            Logger.Log("Completed");
            Logger.LogCreated -= UpdateAnalyzerLog;
        }

        private async void btnAnalyze_Click(object sender, EventArgs e)
        {
            var analysisSettings = new AnalysisSettings();
            analysisSettings.universes = chkUniverses.Checked;
            analysisSettings.documents = chkDocuments.Checked;
            analysisSettings.reports = chkReports.Checked;
            analysisSettings.connections = chkConnection.Checked;

            tbAnalysisLog.Text = "Analyzing...";

            Logger.LogCreated += UpdateAnalyzerLog;
            var generator = new Generator();
            var response = await generator.Generate(analysisSettings);
            Logger.LogCreated -= UpdateAnalyzerLog;
            btnAnalyze.Enabled = true;
        }

        private void UpdateAnalyzerLog(string message)
        {
            tbAnalysisLog.AppendText($"\r\n{message}");
        }

        private async void btnCsv_Click(object sender, EventArgs e)
        {
            tbAnalysisLog.Text = string.Empty;
            Logger.LogCreated += UpdateAnalyzerLog;
            var powerBiService = new PowerBIService();
            var request = new DataSetRequest();
            var response = await powerBiService.GetDocumentData(textBox1.Text);
            Logger.LogCreated -= UpdateAnalyzerLog;
        }

        private void cbConnList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var selection = 
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void lnkPrevReport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var processInfo = new ProcessStartInfo("microsoft-edge:https://app.powerbi.com/links/umS8x4h_yq?ctid=7571a489-bd29-4f38-b9a6-7c880f8cddf0&pbi_source=linkShare");
                processInfo.UseShellExecute = true;
                Process.Start(processInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }
        }

        private void rbUniverse_CheckedChanged(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;
            cbUniverseList.Enabled = rb.Checked;
            btnCsv.Enabled = textBox1.Enabled = !rb.Checked;
            if (rb.Checked)
            {
                cbMode.SelectedIndex = 1;
            }
            else
            {
                cbMode.SelectedIndex = 0;
            }

        }

        private void rbDocument_CheckedChanged(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;
            cbUniverseList.Enabled = !rb.Checked;
            btnCsv.Enabled = textBox1.Enabled = rb.Checked;
            if (rb.Checked)
            {
                cbMode.SelectedIndex = 0;
            }
            else
            {
                cbMode.SelectedIndex = 1;
            }
        }
    }
}