using DotNetAppReader.Shared;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace DotNetAppReader
{
    public partial class frmDotNetAppReader : Form
    {
        public frmDotNetAppReader()
        {
            InitializeComponent();
        }

        private void btnSelectSolutionFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog(this);
            openFileDialog1.Filter = "Solution files (*.sln)|*.sln";
            openFileDialog1.Title = "Please select the visual studio solution file.";
            openFileDialog1.FileName = "D:\\Code\\CleanArchitecture.WebApi\\CleanArchitecture.WebApi.sln";
            txtFileName.Text = openFileDialog1.FileName;
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            SolutionReader reader = new SolutionReader();
            var solutionNode = reader.LoadSolution(txtFileName.Text);
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