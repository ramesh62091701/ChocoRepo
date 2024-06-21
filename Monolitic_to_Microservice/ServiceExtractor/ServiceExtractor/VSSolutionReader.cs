using System.Text;
using System.Text.RegularExpressions;

namespace Service.Extractor.Console
{
    public class VSSolution
    {
        public VSHeader VSHeader { get; set; }

        public VSBody VSBody { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(VSHeader.ToString());
            sb.AppendLine(VSBody.ToString());

            return sb.ToString();
        }
    }

    public class VSProject
    {
        private const string ProjectPattern = @"Project\(""(.+)""\) = ""(.+)"", ""(.+)"", ""(.+)""\r\n(.*)EndProject";

        private Guid _type;
        private string _name;
        private string _relativePath;
        private Guid _identifier;

        public Guid Type => _type;
        public string Name => _name;
        public string RelativePath => _relativePath;
        public Guid Identifier => _identifier;

        public VSProject(string projectText)
        {
            var regEx = new Regex(ProjectPattern);
            var result = regEx.Match(projectText);
            if (result.Success)
            {
                _type = Guid.Parse(result.Groups[1].Value);
                _name = result.Groups[2].Value;
                _relativePath = result.Groups[3].Value;
                _identifier = Guid.Parse(result.Groups[4].Value);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"", Type.ToString("B").ToUpper(), Name, RelativePath, Identifier.ToString("B").ToUpper()));
            sb.AppendLine("EndProject");
            return sb.ToString();
        }
    }

    public class VSBody
    {
        private List<VSProject> _projects = new List<VSProject>();
        private Global _global;

        public List<VSProject> Projects => _projects;
        public Global Global => _global;

        public VSBody(string text)
        {
            var allProjects = text.Substring(0, text.LastIndexOf("EndProject") + "EndProject".Length);
            while (true)
            {
                var project = allProjects.Substring(0, allProjects.IndexOf("EndProject") + "EndProject".Length);
                _projects.Add(new VSProject(project));

                var startIndex = allProjects.IndexOf("EndProject") + "EndProject".Length + Environment.NewLine.Length;

                if (allProjects.Length < startIndex)
                    break;

                allProjects = allProjects.Substring(startIndex, allProjects.Length - startIndex);
            }

            var globalText = text.Substring(text.IndexOf("Global"), text.LastIndexOf("EndGlobal") + "EndGlobal".Length - text.IndexOf("Global"));
            _global = new Global(globalText);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var project in _projects)
            {
                sb.Append(project.ToString());
            }

            sb.Append(_global.ToString());

            return sb.ToString();
        }
    }

    public class GlobalSectionFactory
    {
        private static string SectionPattern = @"GlobalSection\((.+)\) = (.+)";

        public static GlobalSectionBase Create(string globalSection)
        {
            var sectionStart = globalSection.Substring(0, globalSection.IndexOf(Environment.NewLine));
            var result = Regex.Match(sectionStart, SectionPattern);

            if (result.Success)
            {
                switch (result.Groups[1].Value)
                {
                    case "SolutionConfigurationPlatforms":
                        return new SolutionConfigurationPlatformGlobalSection(result.Groups[1].Value, globalSection);
                    case "ProjectConfigurationPlatforms":
                        return new ProjectConfigurationPlatformsGlobalSection(result.Groups[1].Value, globalSection);
                    case "SolutionProperties":
                        return new SolutionPropertiesGlobalSection(result.Groups[1].Value, globalSection);
                    case "NestedProjects":
                        return new NestedProjectsGlobalSection(result.Groups[1].Value, globalSection);
                    case "ExtensibilityGlobals":
                        return new ExtensibilityGlobalsGlobalSection(result.Groups[1].Value, globalSection);
                    case "SharedMSBuildProjectFiles":
                        return new SharedMSBuildProjectFilesGlobalSection(result.Groups[1].Value, globalSection);
                }
            }

            throw new NotSupportedException();
        }
    }

    public class Global
    {
        private List<GlobalSectionBase> _globalSections = new List<GlobalSectionBase>();

        public List<GlobalSectionBase> GlobalSections => _globalSections;

        public Global(string globalText)
        {
            var allGlobalSections = globalText.Substring(globalText.IndexOf("GlobalSection"), globalText.LastIndexOf("EndGlobalSection") + "EndGlobalSection".Length - globalText.IndexOf("GlobalSection"));
            while (true)
            {
                var globalSection = allGlobalSections.Substring(0, allGlobalSections.IndexOf("EndGlobalSection") + "EndGlobalSection".Length);
                _globalSections.Add(GlobalSectionFactory.Create(globalSection));

                var startIndex = allGlobalSections.IndexOf("EndGlobalSection") + "EndGlobalSection".Length + Environment.NewLine.Length;

                if (allGlobalSections.Length < startIndex)
                    break;

                allGlobalSections = allGlobalSections.Substring(startIndex, allGlobalSections.Length - startIndex);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Global");
            foreach (var globalSection in GlobalSections)
            {
                sb.Append(globalSection.ToString());
            }
            sb.AppendLine("EndGlobal");
            return sb.ToString();
        }
    }

    public abstract class GlobalSectionBase
    {
        protected const string StartFormat = "\tGlobalSection({0}) = {1}";
        protected const string EndFormat = "\tEndGlobalSection";
        protected const string ItemsFormat = "\t\t{0}";

        private readonly string _name;
        private readonly SolutionEnvironmentCycle _environmentCycle;

        public string Name => _name;
        public SolutionEnvironmentCycle EnvironmentCycle => _environmentCycle;

        protected GlobalSectionBase(string name, SolutionEnvironmentCycle environmentCycle)
        {
            _name = name;
            _environmentCycle = environmentCycle;
        }

        protected string SolutionEnvironmentAsString()
        {
            switch (_environmentCycle)
            {
                case SolutionEnvironmentCycle.PostSolution:
                    return "postSolution";
                case SolutionEnvironmentCycle.PreSolution:
                    return "preSolution";
                case SolutionEnvironmentCycle.Project:
                    return "project";
            }

            throw new NotSupportedException();
        }

    }

    public class SolutionConfigurationPlatformGlobalSection : GlobalSectionBase
    {
        private List<SolutionConfiguration> _solutionConfigurations = new List<SolutionConfiguration>();

        private const string SolutionConfigurationSplitPattern = @"\s*=\s*|\s*\|\s*";
        public class SolutionConfiguration
        {
            public required string ActiveConfigurationName { get; set; }
            public required string ActivePlatform { get; set; }

            public required string ConfigurationName { get; set; }
            public required string Platform { get; set; }
        }

        public List<SolutionConfiguration> SolutionConfigurations => _solutionConfigurations;

        public SolutionConfigurationPlatformGlobalSection(string name, string solutionConfigText) :
            base(name, SolutionEnvironmentCycle.PreSolution)
        {
            var fullSection = solutionConfigText.Split(new string[] { Environment.NewLine }, StringSplitOptions.TrimEntries);

            // Ignore the first line as it is preset, also the last line as it is end of GlobalSection
            for (var i = 1; i < fullSection.Length - 1; i++)
            {
                var result = Regex.Split(fullSection[i], SolutionConfigurationSplitPattern);

                if (result.Length == 4)
                {
                    _solutionConfigurations.Add(
                        new SolutionConfiguration
                        {
                            ActiveConfigurationName = result[0],
                            ActivePlatform = result[1],
                            ConfigurationName = result[2],
                            Platform = result[3]
                        });
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format(StartFormat, Name, SolutionEnvironmentAsString()));
            foreach (var configuration in SolutionConfigurations)
            {
                var item = string.Format("{0}|{1} = {2}|{3}", configuration.ActiveConfigurationName, configuration.ActivePlatform, configuration.ConfigurationName, configuration.Platform);
                sb.AppendLine(string.Format(ItemsFormat, item));
            }
            sb.AppendLine(EndFormat);

            return sb.ToString();
        }
    }

    public class ProjectConfigurationPlatformsGlobalSection : GlobalSectionBase
    {
        private const string ProjectConfigurationSplitPattern = @"\s*.\s*|\s*.|\s*=\s*|\s*";

        public enum EConfigSelection
        {
            ActiveCfg,
            Build0,
            Deploy0
        }

        private string ConfigSelected(EConfigSelection selection)
        {
            switch (selection)
            {
                case EConfigSelection.ActiveCfg:
                    return "ActiveCfg";
                case EConfigSelection.Build0:
                    return "Build.0";
                case EConfigSelection.Deploy0:
                    return "Deploy.0";
            }

            throw new NotImplementedException();
        }

        private EConfigSelection ConfigSelected(string selection)
        {
            switch (selection)
            {
                case "ActiveCfg":
                    return EConfigSelection.ActiveCfg;
                case "Build.0":
                    return EConfigSelection.Build0;
                case "Deploy.0":
                    return EConfigSelection.Deploy0;
            }

            throw new NotImplementedException();
        }

        public class ProjectConfiguration
        {
            public required Guid Identifier { get; set; }
            public required string ActiveConfigurationName { get; set; }
            public required string ActivePlatform { get; set; }

            public required EConfigSelection ConfigurationSelection { get; set; }

            public required string ConfigurationName { get; set; }
            public required string Platform { get; set; }
        }

        private List<ProjectConfiguration> _projConfigurations = new List<ProjectConfiguration>();
        public List<ProjectConfiguration> ProjectConfigurations => _projConfigurations;

        public ProjectConfigurationPlatformsGlobalSection(string name, string projectConfigText) :
            base(name, SolutionEnvironmentCycle.PostSolution)
        {
            var fullSection = projectConfigText.Split(new string[] { Environment.NewLine }, StringSplitOptions.TrimEntries);

            // Ignore the first line as it is preset, also the last line as it is end of GlobalSection
            for (var i = 1; i < fullSection.Length - 1; i++)
            {
                //var result = Regex.Split(fullSection[i], ProjectConfigurationSplitPattern);
                var result = fullSection[i].Split(new char[] { '.', '|', '=' }, StringSplitOptions.TrimEntries);

                if (result.Length == 6 || result.Length == 7)
                {
                    _projConfigurations.Add(
                        new ProjectConfiguration
                        {
                            Identifier = Guid.Parse(result[0]),
                            ActiveConfigurationName = result[1],
                            ActivePlatform = result[2],
                            ConfigurationSelection = ConfigSelected(result.Length == 7 ? $"{result[3]}.{result[4]}" : result[3]),
                            ConfigurationName = result.Length == 7 ? result[5] : result[4],
                            Platform = result.Length == 7 ? result[6] : result[5],
                        });
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format(StartFormat, Name, SolutionEnvironmentAsString()));
            foreach (var configuration in ProjectConfigurations)
            {
                var item = string.Format("{0}.{1}|{2}.{3} = {4}|{5}", configuration.Identifier.ToString("B").ToUpper(), configuration.ActiveConfigurationName,
                    configuration.ActivePlatform, ConfigSelected(configuration.ConfigurationSelection), configuration.ConfigurationName, configuration.Platform);
                sb.AppendLine(string.Format(ItemsFormat, item));
            }
            sb.AppendLine(EndFormat);
            return sb.ToString();
        }
    }

    public class SolutionPropertiesGlobalSection : GlobalSectionBase
    {
        private static bool _hideSolutionNode;
        public bool HideSolutionNode = _hideSolutionNode;

        public SolutionPropertiesGlobalSection(string name, string projectConfigText) :
            base(name, SolutionEnvironmentCycle.PreSolution)
        {
            var fullSection = projectConfigText.Split(new string[] { Environment.NewLine }, StringSplitOptions.TrimEntries);

            // Currently handling only one line
            HideSolutionNode = bool.Parse(fullSection[1].Split('=', StringSplitOptions.TrimEntries)[1]);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format(StartFormat, Name, SolutionEnvironmentAsString()));

            var item = string.Format("HideSolutionNode = {0}", HideSolutionNode.ToString().ToUpper());
            sb.AppendLine(string.Format(ItemsFormat, item));

            sb.AppendLine(EndFormat);
            return sb.ToString();
        }
    }

    public class NestedProjectsGlobalSection : GlobalSectionBase
    {
        public class NestedProject
        {
            public required Guid NestedProjectIdentifier { get; set; }
            public required Guid VSProjectIdentifer { get; set; }
        }

        private readonly List<NestedProject> _nestedProjects = new List<NestedProject>();
        public List<NestedProject> NestedProjects => _nestedProjects;

        public NestedProjectsGlobalSection(string name, string nestedProjectsText) :
            base(name, SolutionEnvironmentCycle.PreSolution)
        {
            var fullSection = nestedProjectsText.Split(new string[] { Environment.NewLine }, StringSplitOptions.TrimEntries);

            // Ignore the first line as it is preset, also the last line as it is end of GlobalSection
            for (var i = 1; i < fullSection.Length - 1; i++)
            {
                //var result = Regex.Split(fullSection[i], ProjectConfigurationSplitPattern);
                var result = fullSection[i].Split(new char[] { '=' }, StringSplitOptions.TrimEntries);

                if (result.Length == 2)
                {
                    _nestedProjects.Add(
                        new NestedProject
                        {
                            NestedProjectIdentifier = Guid.Parse(result[0]),
                            VSProjectIdentifer = Guid.Parse(result[1])
                        });
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format(StartFormat, Name, SolutionEnvironmentAsString()));

            foreach (var proj in NestedProjects)
            {
                var item = string.Format("{0} = {1}", proj.NestedProjectIdentifier.ToString("B").ToUpper(), proj.VSProjectIdentifer.ToString("B").ToUpper());
                sb.AppendLine(string.Format(ItemsFormat, item));
            }

            sb.AppendLine(EndFormat);
            return sb.ToString();
        }
    }

    public class ExtensibilityGlobalsGlobalSection : GlobalSectionBase
    {
        public Guid SolutionGuid { get; set; }

        public ExtensibilityGlobalsGlobalSection(string name, string extensibilityGlobalsText) :
            base(name, SolutionEnvironmentCycle.PostSolution)
        {
            var fullSection = extensibilityGlobalsText.Split(new string[] { Environment.NewLine }, StringSplitOptions.TrimEntries);

            // Currently handling only one line
            var result = fullSection[1].Split(new char[] { '=' }, StringSplitOptions.TrimEntries);
            SolutionGuid = Guid.Parse(result[1]);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format(StartFormat, Name, SolutionEnvironmentAsString()));

            var item = string.Format("SolutionGuid = {0}", SolutionGuid.ToString("B").ToUpper());
            sb.AppendLine(string.Format(ItemsFormat, item));

            sb.AppendLine(EndFormat);
            return sb.ToString();
        }
    }

    public class SharedMSBuildProjectFilesGlobalSection : GlobalSectionBase
    {
        public SharedMSBuildProjectFilesGlobalSection(string name, string extensibilityGlobalsText) :
              base(name, SolutionEnvironmentCycle.PreSolution)
        {

        }
    }

    public enum SolutionEnvironmentCycle
    {
        PreSolution,
        Project,
        PostSolution
    }

    public class VSHeader
    {
        private const string StandardHeaderFormat = @"(.+)\,\s(.+)";
        private const string VisualStudioCommentFormat = @"#\s.+";
        private const string VisualStudionVersionFormat = @"^VisualStudioVersion = (.+)";
        private const string MinimumVisualStudioVersionFormat = @"^MinimumVisualStudioVersion = (.+)";
        private readonly string _name;
        private readonly string _formatVersion;
        private readonly string _comment;
        private readonly string _visualStudioVersion;
        private readonly string _minimumVisualStudioVersion;

        public string Name => _name;
        public string FormatVersion => _formatVersion;
        public string Comment => _comment;
        public string VisualStudioVersion => _visualStudioVersion;
        public string MinimumVisualStudioVersion => _minimumVisualStudioVersion;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"{Name}, {FormatVersion}");
            sb.AppendLine(Comment);
            sb.AppendLine($"VisualStudioVersion = {VisualStudioVersion}");
            sb.AppendLine($"MinimumVisualStudioVersion = {MinimumVisualStudioVersion}");
            return sb.ToString();
        }

        public VSHeader(string text)
        {
            var lines = text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var regex = new Regex(StandardHeaderFormat);
                var result = regex.Match(line);
                if (result.Success)
                {
                    _name = result.Groups[1].Value;
                    _formatVersion = result.Groups[2].Value;

                    continue;
                }

                regex = new Regex(VisualStudioCommentFormat);
                if (regex.IsMatch(line))
                    _comment = line;

                regex = new Regex(VisualStudionVersionFormat);
                result = regex.Match(line);
                if (result.Success)
                {
                    _visualStudioVersion = result.Groups[1].Value;
                    continue;
                }

                regex = new Regex(MinimumVisualStudioVersionFormat);
                result = regex.Match(line);
                if (result.Success)
                {
                    _minimumVisualStudioVersion = result.Groups[1].Value;
                    continue;
                }
            }
        }
    }

    public static class SolutionEnvironmentCycleExtension
    {
        public static string ToString(this SolutionEnvironmentCycle cycle)
        {
            switch (cycle)
            {
                case SolutionEnvironmentCycle.PreSolution:
                    return "preSolution";
                case SolutionEnvironmentCycle.Project:
                    return "globalSection";
                case SolutionEnvironmentCycle.PostSolution:
                    return "postSolution";
            }

            throw new NotImplementedException();
        }
    }

    public class VSSolutionReader
    {
        public VSSolution VSSolution { get; set; }

        public VSSolutionReader(string path)
        {
            var text = File.ReadAllText(path, Encoding.UTF8);
            var header = text.Substring(0, text.IndexOf("Project"));
            var body = text.Substring(text.IndexOf("Project"), text.Length - text.IndexOf("Project"));

            VSSolution = new VSSolution
            {
                VSHeader = new VSHeader(header),
                VSBody = new VSBody(body)
            };
        }
    }
}
