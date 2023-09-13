using System.Diagnostics;
using System.Xml.Linq;

namespace Version_Scanner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //folder path taken from the user's input value in textbox
            string pathIn = textBox1.Text;

            try
            {
                //store all C# project files in a array for iterating 
                string[] projectFiles = Directory.GetFiles(pathIn, "*.csproj", SearchOption.AllDirectories);

                //checking the framework version
                foreach (string projectFile in projectFiles)
                {
                    XDocument projectXml = XDocument.Load(projectFile);
                    XNamespace ns = projectXml.Root.Name.Namespace;
                    string targetFramework = projectXml.Descendants(ns + "TargetFramework").FirstOrDefault()?.Value;

                    if (targetFramework != null)
                    {
                        listBox1.Items.Add($"{Path.GetFileNameWithoutExtension(projectFile)}  uses: {targetFramework}");
                    }
                    else MessageBox.Show("No project files found");
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Please Provide a valid path");
            }         
        }
        //method to check framework version in the system
        private void button2_Click(object sender, EventArgs e)
        {

            string sysVersion = "System version is ";
            listBox1.Items.Add(sysVersion + System.Environment.Version);

            //Process process = new Process();
            //process.StartInfo.FileName = "cmd.exe";
            //process.StartInfo.Arguments = "/c dotnet --version";
            //process.StartInfo.RedirectStandardOutput = true;
            //process.OutputDataReceived += (sender, args) => listBox1.Items.Add(args.Data);
            //process.Start();
            //process.BeginOutputReadLine();
            //process.WaitForExit();
        }
    }
}

