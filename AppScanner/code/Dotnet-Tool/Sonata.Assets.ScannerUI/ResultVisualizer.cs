using Microsoft.Web.Administration;
using Microsoft.Win32;
using Sonata.Assets.DotnetScanner.System;
using Sonata.Assets.DotnetScanner.System.Entities;
using System.Data;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace WinFormsApp1
{
    public partial class ResultVisualizer : Form
    {
        List<ProjProperty> CSprojProperies = new List<ProjProperty>();
        List<ProjReferences> CSprojReferences = new List<ProjReferences>();
        List<ProjProperty> AllCSprojProperies = new List<ProjProperty>();
        List<WebProjProperty> webProjProperty = new List<WebProjProperty>();
        public ResultVisualizer()
        {
            InitializeComponent();
        }
        public static string strResult = "";
        private void btnScanSystems_Click(object sender, EventArgs e)
        {
            //Show all the installed versions
            //Get1To45VersionFromRegistry();
            //Get45PlusFromRegistry();
            //Get5PlusFromRegistry();
            //txtResult.Text = strResult;

            SystemScanner scanner = new SystemScanner();
            SystemScannerOutput output = scanner.Scan(new SystemScannerInput { SystemName = "LocalMachine" });

            txtResult.Text = string.Join("\t\n", output.Result);
        }
        private static void Get1To45VersionFromRegistry()
        {
            // Opens the registry key for the .NET Framework entry.
            using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                if (ndpKey != null)
                {
                    foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                    {
                        // Skip .NET Framework 4.5 version information.
                        if (versionKeyName == "v4")
                        {
                            continue;
                        }

                        if (versionKeyName.StartsWith("v"))
                        {

                            RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                            // Get the .NET Framework version value.
                            string name = (string)versionKey.GetValue("Version", "");
                            // Get the service pack (SP) number.
                            string sp = versionKey.GetValue("SP", "").ToString();

                            // Get the installation flag, or an empty string if there is none.
                            string install = versionKey.GetValue("Install", "").ToString();
                            if (string.IsNullOrEmpty(install)) // No install info; it must be in a child subkey.
                                WriteVersion(name);
                            else
                            {
                                if (!(string.IsNullOrEmpty(sp)) && install == "1")
                                {
                                    WriteVersion(name, sp);
                                }
                            }
                            if (!string.IsNullOrEmpty(name))
                            {
                                continue;
                            }
                            foreach (string subKeyName in versionKey.GetSubKeyNames())
                            {
                                RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                                name = (string)subKey.GetValue("Version", "");
                                if (!string.IsNullOrEmpty(name))
                                    sp = subKey.GetValue("SP", "").ToString();

                                install = subKey.GetValue("Install", "").ToString();
                                if (string.IsNullOrEmpty(install)) //No install info; it must be later.
                                    WriteVersion(name);
                                else
                                {
                                    if (!(string.IsNullOrEmpty(sp)) && install == "1")
                                    {
                                        WriteVersion(name, sp);
                                    }
                                    else if (install == "1")
                                    {
                                        WriteVersion("V" + name);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private static void Get45PlusFromRegistry()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(subkey))
            {
                if (ndpKey == null)
                    return;
                //First check if there's an specific version indicated
                if (ndpKey.GetValue("Version") != null)
                {
                    WriteVersion("V" + ndpKey.GetValue("Version").ToString());
                }
                else
                {
                    if (ndpKey != null && ndpKey.GetValue("Release") != null)
                    {
                        WriteVersion(
                            "V" + CheckFor45PlusVersion((int)ndpKey.GetValue("Release"))
                        );
                    }
                }
            }

            // Checking the version using >= enables forward compatibility.
            string CheckFor45PlusVersion(int releaseKey)
            {
                if (releaseKey >= 533325)
                    return "4.8.1";
                if (releaseKey >= 528040)
                    return "4.8";
                if (releaseKey >= 461808)
                    return "4.7.2";
                if (releaseKey >= 461308)
                    return "4.7.1";
                if (releaseKey >= 460798)
                    return "4.7";
                if (releaseKey >= 394802)
                    return "4.6.2";
                if (releaseKey >= 394254)
                    return "4.6.1";
                if (releaseKey >= 393295)
                    return "4.6";
                if (releaseKey >= 379893)
                    return "4.5.2";
                if (releaseKey >= 378675)
                    return "4.5.1";
                if (releaseKey >= 378389)
                    return "4.5";
                // This code should never execute. A non-null release key should mean
                // that 4.5 or later is installed.
                return "";
            }
        }
        private static void Get5PlusFromRegistry()
        {
            const string subkey = @"SOFTWARE\dotnet\Setup\InstalledVersions";
            var baseKey = Registry.LocalMachine.OpenSubKey(subkey);
            if (baseKey.SubKeyCount == 0)
                return;

            foreach (var platformKey in baseKey.GetSubKeyNames())
            {
                using (var platform = baseKey.OpenSubKey(platformKey))
                {
                    Console.WriteLine($"Platform: {platform.Name.Substring(platform.Name.LastIndexOf("\\") + 1)}");
                    if (platform.SubKeyCount == 0)
                        continue;

                    var sharedHost = platform.OpenSubKey("sharedhost");
                    foreach (var version in sharedHost.GetValueNames())
                        //Console.WriteLine("{0,-8}: {1}", version, sharedHost.GetValue(version));
                        WriteVersion("V" + sharedHost.GetValue("Version").ToString());
                }
            }
        }
        //Writes the version
        private static void WriteVersion(string version, string spLevel = "")
        {
            version = version.Trim();
            if (string.IsNullOrEmpty(version))
                return;

            string spLevelString = "";
            if (!string.IsNullOrEmpty(spLevel))
                spLevelString = " Service Pack " + spLevel;
            strResult = strResult + Environment.NewLine;

            strResult = strResult + ($"{version}{spLevelString}");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtPath_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.Show("Please provide the location", txtPath);
        }
        //Read from provided location
        private void btnScanProjects_Click(object sender, EventArgs e)
        {
            if (txtPath.Text.Trim() == "")
            {
                MessageBox.Show("Please provide the path");
                return;
            }
            string strPath = txtPath.Text.Trim();

            if (!Directory.Exists(strPath))
            {
                string message = "Directory does not exists";
                string title = "Title";
                MessageBox.Show(message, title);
                return;
                //Directory.CreateDirectory(strPath);
            }
            foreach (string csProjFileNmae in Directory.GetFiles(strPath, "*.csProj", SearchOption.AllDirectories))
            {
                //Get details from PropertyGroup
                getDetailsFromPropertyGroup(Path.Combine(strPath, csProjFileNmae));
                //get details from ItemGroup, //Get project references
                readReferencesFromItemGroup(csProjFileNmae);
            }
            WriToExcel();
            BindProjToGridView(@"D:\temp\list.csv");
        }
        private void getDetailsFromPropertyGroup(string fileNameAndPath)
        {
            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";

            XDocument projDefinition = XDocument.Load(fileNameAndPath);

            //Project_ToolsVersion

            IEnumerable<string> projToolVersions = projDefinition
                .Element(msbuild + "Project")
                .Attributes("ToolsVersion")
                .Select(refElem => refElem.Value);
            foreach (string projToolVersion in projToolVersions)
            {
                CSprojProperies.Add(new ProjProperty
                {
                    ProjectName = fileNameAndPath,
                    Project_ToolsVersion = projToolVersion
                });
            }
            //End Project_ToolsVersion

            IEnumerable<string> projGUIDs = projDefinition
                .Element(msbuild + "Project")
                .Elements(msbuild + "PropertyGroup")
                .Elements(msbuild + "ProjectGuid")
                .Select(refElem => refElem.Value);

            foreach (string projGUID in projGUIDs)
            {
                CSprojProperies.Add(new ProjProperty
                {
                    ProjectName = fileNameAndPath,
                    ProjectGuid = projGUID
                });
            }
            //OutputType
            IEnumerable<string> OutputTypes = projDefinition
            .Element(msbuild + "Project")
            .Elements(msbuild + "PropertyGroup")
            .Elements(msbuild + "OutputType")
            .Select(refElem => refElem.Value);

            foreach (string outputType in OutputTypes)
            {
                CSprojProperies.Add(new ProjProperty
                {
                    ProjectName = fileNameAndPath,
                    OutputType = outputType
                });
                Console.WriteLine(outputType);
            }
            //TargetFrameworkVersion
            IEnumerable<string> TargetFrameworkVersion = projDefinition
            .Element(msbuild + "Project")
            .Elements(msbuild + "PropertyGroup")
            .Elements(msbuild + "TargetFrameworkVersion")
            .Select(refElem => refElem.Value);

            foreach (string targetFrameworkVersion in TargetFrameworkVersion)
            {
                CSprojProperies.Add(new ProjProperty
                {
                    ProjectName = fileNameAndPath,
                    TargetFrameworkVersion = targetFrameworkVersion
                });
                Console.WriteLine(targetFrameworkVersion);
            }
        }
        private void readReferencesFromItemGroup(string fileNameAndPath)
        {
            //Concole Application
            //Get reference
            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
            XDocument projDefinition = XDocument.Load(fileNameAndPath);
            IEnumerable<string> references = projDefinition
                .Element(msbuild + "Project")
                .Elements(msbuild + "ItemGroup")
                .Elements(msbuild + "Reference")
                .Attributes("Include")    // This is where the reference is mentioned
                .Select(refElem => refElem.Value);
            foreach (string reference in references)
            {
                CSprojReferences.Add(new ProjReferences
                {
                    ProjectName = fileNameAndPath,
                    Reference = reference
                });
                Console.WriteLine(reference);
            }

            //Projectreference
            IEnumerable<string> projectReferences = projDefinition
                .Element(msbuild + "Project")
                .Elements(msbuild + "ItemGroup")
                .Elements(msbuild + "ProjectReference")
                .Attributes("Include")    // This is where the reference is mentioned
                .Select(refElem => refElem.Value);
            foreach (string projReference in projectReferences)
            {
                CSprojReferences.Add(new ProjReferences
                {
                    ProjectName = fileNameAndPath,
                    ProjectReference = projReference
                });
                Console.WriteLine(projReference);
            }

            //Content include
            IEnumerable<string> projectContents = projDefinition
                .Element(msbuild + "Project")
                .Elements(msbuild + "ItemGroup")
                .Elements(msbuild + "Content")
                .Attributes("Include")    // This is where the reference is mentioned
                .Select(refElem => refElem.Value);
            foreach (string projectContent in projectContents)
            {
                CSprojReferences.Add(new ProjReferences
                {
                    ProjectName = fileNameAndPath,
                    ProjectContent = projectContent
                });
                Console.WriteLine(projectContent);
            }

            //Compile include
            IEnumerable<string> projectCompiles = projDefinition
                .Element(msbuild + "Project")
                .Elements(msbuild + "ItemGroup")
                .Elements(msbuild + "Compile")
                .Attributes("Include")    // This is where the reference is mentioned
                .Select(refElem => refElem.Value);
            foreach (string projCompile in projectCompiles)
            {
                CSprojReferences.Add(new ProjReferences
                {
                    ProjectName = fileNameAndPath,
                    ProjectCompile = projCompile
                });
                Console.WriteLine(projCompile);
            }
        }
        //Write to excel
        private void WriToExcel()
        {
            string tempFileName = string.Empty;
            using (StreamWriter sw = File.CreateText(@"d:\temp\list.csv"))
            {
                sw.WriteLine("FileName  ,ProjectGuide , OutputType , TargetFrameworkVersion");
                for (int i = 0; i < CSprojProperies.Count; i++)
                {

                    if (i > 0)
                    {
                        if (tempFileName == CSprojProperies[i].ProjectName)
                        {
                            if (!string.IsNullOrEmpty(CSprojProperies[i].ProjectGuid))
                            {
                                sw.Write(" , " + CSprojProperies[i].ProjectGuid);

                            }
                            if (!string.IsNullOrEmpty(CSprojProperies[i].OutputType))
                            {
                                sw.Write(" , " + CSprojProperies[i].OutputType);
                            }
                            if (!string.IsNullOrEmpty(CSprojProperies[i].TargetFrameworkVersion))
                            {
                                sw.Write(" , " + CSprojProperies[i].TargetFrameworkVersion);
                            }
                            //if (!string.IsNullOrEmpty(CSprojProperies[i].Project_ToolsVersion))
                            //{
                            //sw.Write(" , " + CSprojProperies[i].Project_ToolsVersion);
                            //}
                        }
                        else
                        {
                            //sw.WriteLine();
                            sw.Write(CSprojProperies[i].ProjectName);
                            if (!string.IsNullOrEmpty(CSprojProperies[i].ProjectGuid))
                            {
                                sw.Write(" , " + CSprojProperies[i].ProjectGuid);
                            }
                            if (!string.IsNullOrEmpty(CSprojProperies[i].OutputType))
                            {
                                sw.Write(" , " + CSprojProperies[i].OutputType);
                            }
                            if (!string.IsNullOrEmpty(CSprojProperies[i].TargetFrameworkVersion))
                            {
                                sw.Write(" , " + CSprojProperies[i].TargetFrameworkVersion);
                            }
                            //if (!string.IsNullOrEmpty(CSprojProperies[i].Project_ToolsVersion))
                            //{
                            //sw.Write(" , " + CSprojProperies[i].Project_ToolsVersion);
                            //}
                        }
                    }
                    else
                    {
                        //sw.WriteLine();
                        sw.Write(CSprojProperies[i].ProjectName);
                        if (!string.IsNullOrEmpty(CSprojProperies[i].ProjectGuid))
                        {
                            sw.Write(" , " + CSprojProperies[i].ProjectGuid);
                        }
                        if (!string.IsNullOrEmpty(CSprojProperies[i].OutputType))
                        {
                            sw.Write(" , " + CSprojProperies[i].OutputType);
                        }
                        if (!string.IsNullOrEmpty(CSprojProperies[i].TargetFrameworkVersion))
                        {
                            sw.Write(" , " + CSprojProperies[i].TargetFrameworkVersion);
                        }
                        //if (!string.IsNullOrEmpty(CSprojProperies[i].Project_ToolsVersion))
                        //{
                        //sw.Write(" , " + CSprojProperies[i].Project_ToolsVersion);
                        //}
                    }
                    tempFileName = CSprojProperies[i].ProjectName;
                    //sw.WriteLine(CSprojProperies[i].ProjectName + " , " + CSprojProperies[i].ProjectGuid +
                    //            " , " + CSprojProperies[i].OutputType + " , " + CSprojProperies[i].TargetFrameworkVersion);
                }
            }

            using (StreamWriter sw = File.CreateText(@"d:\temp\Referencelist.csv"))
            {
                sw.WriteLine("ProjectName , ProjectReference , Reference , ProjectContent , ProjectCompile");
                for (int i = 0; i < CSprojReferences.Count; i++)
                {
                    sw.WriteLine(CSprojReferences[i].ProjectName + " , " + CSprojReferences[i].ProjectReference +
                                " , " + CSprojReferences[i].Reference +
                                " , " + CSprojReferences[i].ProjectContent +
                                " , " + CSprojReferences[i].ProjectCompile);

                }
            }
        }
        public void BindProjToGridView(string filePath)
        {
            DataTable dt = new DataTable();
            string[] lines = System.IO.File.ReadAllLines(filePath);
            if (lines.Length > 0)
            {
                //first line to create header
                string firstLine = lines[0];
                string[] headerLabels = firstLine.Split(',');
                foreach (string headerWord in headerLabels)
                {
                    dt.Columns.Add(new DataColumn(headerWord));
                }
                //For Data
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] dataWords = lines[i].Split(',');
                    DataRow dr = dt.NewRow();
                    int columnIndex = 0;
                    foreach (string headerWord in headerLabels)
                    {
                        dr[headerWord] = dataWords[columnIndex++];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
        }
        // Read from IIS
        public void readFromIIS()
        {
            using (ServerManager serverManager = new ServerManager())
            {

                var sites = serverManager.Sites;
                int iisNumber = 0;
                string strVersion = "0";

                foreach (Site site in sites)
                {
                    iisNumber = iisNumber + 1;
                    Console.WriteLine(site.Name);
                    var sitemgr = serverManager.Sites.Where(s => s.Id == iisNumber).Single();
                    var applicationRoot = sitemgr.Applications.Where(a => a.Path == "/").Single();
                    var virtualRoot = applicationRoot.VirtualDirectories.Where(v => v.Path == "/").Single();
                    Console.WriteLine(virtualRoot.PhysicalPath);
                    string strPath = virtualRoot.PhysicalPath;

                    var site1 = serverManager.Sites.Where(s => s.Id == iisNumber).SingleOrDefault();
                    string dllFilename = "";

                    if (site.Name != "Default Web Site")
                    {
                        foreach (string dllFileName in Directory.GetFiles(strPath, "*.dll"))
                        {
                            strVersion = WebsiteDetails(Path.Combine(strPath, dllFileName));
                            dllFilename = Path.GetFileNameWithoutExtension(dllFileName);
                        }

                    }

                    webProjProperty.Add(new WebProjProperty
                    {
                        SiteName = site.Name,
                        TargetFrameworkVersion = strVersion,
                        ApplicationName = dllFilename,
                        VirtualPath = strPath,

                    });

                }
                WriToExcelIIS();
            }

        }

        private string WebsiteDetails(string strDllpath)
        {
            string frmVersion;
            System.Reflection.Assembly myDllAssembly = System.Reflection.Assembly.LoadFile(strDllpath);

            using var fs = new FileStream(strDllpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var peReader = new PEReader(fs);

            if (peReader.HasMetadata)
            {
                MetadataReader reader = peReader.GetMetadataReader();
                frmVersion = reader.MetadataVersion.ToString();
            }
            else
                frmVersion = "v0.0";

            return frmVersion;

        }

        private void btnScanIIS_Click(object sender, EventArgs e)
        {
            readFromIIS();

        }
        private void WriToExcelIIS()
        {
            using (StreamWriter sw = File.CreateText(@"c:\temp\IISlist.csv"))
            {
                sw.WriteLine("SiteName , ApplicationName, TargetFrameworkVersion , VirtualPath");
                for (int i = 0; i < webProjProperty.Count; i++)
                {
                    sw.WriteLine(webProjProperty[i].SiteName + " , " + webProjProperty[i].ApplicationName +
                                " , " + webProjProperty[i].TargetFrameworkVersion +
                                " , " + webProjProperty[i].VirtualPath);

                }
            }
        }
    }
    public class ProjProperty
    {
        public string ProjectName { get; set; }
        public string ProjectGuid { get; set; }
        public string TargetFrameworkVersion { get; set; }
        public string OutputType { get; set; }
        public string Project_ToolsVersion { get; set; }
    }

    public class ProjReferences
    {
        public string ProjectName { get; set; }
        public string Reference { get; set; }
        public string ProjectReference { get; set; }
        public string ProjectContent { get; set; }
        public string ProjectCompile { get; set; }
    }
    public class WebProjProperty
    {
        public string SiteName { get; set; }
        public string TargetFrameworkVersion { get; set; }
        public string ApplicationName { get; set; }
        public string VirtualPath { get; set; }

    }
}