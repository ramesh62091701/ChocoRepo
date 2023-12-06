using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BackendServiceExtractor.Shared.SolutionFileReader;

namespace BackendServiceGenerator
{
    public partial class BackendServiceGenerator : Form
    {

        private string filePath;
        public BackendServiceGenerator()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                filePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(openFileDialog.FileName);
                if (!fileName.ToLower().EndsWith(".sln"))
                {
                    MessageBox.Show("Please enter a valid solution file!", "Error:");
                    return;
                }

                txtFilePath.Text = filePath;
            }
        }

        private void btnShowFiles_Click(object sender, EventArgs e)
        {
            SolutionReader reader = new SolutionReader();
            var solutionNode = reader.LoadSolution(filePath);
            treeView1.Nodes.Add(GetSolutionNodes(solutionNode));
        }

        static TreeNode GetSolutionNodes(SolutionModel solutionModel)
        {
            TreeNode solutionNode = new TreeNode(solutionModel.SolutionName);

            foreach (var projectModel in solutionModel.Projects)
            {
                TreeNode projectNode = new TreeNode(projectModel.ProjectName);

                foreach (var documentModel in projectModel.Documents)
                {
                    TreeNode documentNode = new TreeNode(documentModel.DocumentName);
                    projectNode.Nodes.Add(documentNode);
                }

                solutionNode.Nodes.Add(projectNode);
            }
            return solutionNode;
        }
    }
}
