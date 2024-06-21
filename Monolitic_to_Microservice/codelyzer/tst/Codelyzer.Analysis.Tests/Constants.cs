﻿namespace Codelyzer.Analysis.Tests
{
    internal class Constants
    {
        // Do not change these values without updating the corresponding line in .gitignore:
        //  **/Projects/Temp
        //  **/Projects/Downloads
        // This is to prevent test projects from being picked up in git after failed unit tests.
        internal static readonly string[] TempProjectDirectories = { "Projects", "Temp" };
        internal static readonly string[] TempProjectDownloadDirectories = { "Projects", "Downloads" };

        internal static string programFiles = "Program Files";
        internal static string programFilesx86 = "Program Files (x86)";
        internal static string vs2022MSBuildPath = @"Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe";
        internal static string vs2019MSBuildPath = @"Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe";
        internal static string vs2017MSBuildPath = @"Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe";
        internal static string vs2019BuildToolsMSBuildPath = @"Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe";
        internal static string MSBuild14Path = @"Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe";

        internal static string SampleSolutionFile = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.0.31903.59
MinimumVisualStudioVersion = 10.0.40219.1
Project(""{ 9A19103F - 16F7 - 4668 - BE54 - 9A1E7A4F7556}"") = ""Codelyzer.Analysis"", ""Analysis\Codelyzer.Analysis\Codelyzer.Analysis.csproj"", ""{ BDE5AC46 - B937 - 4EAE - 92F8 - AE59E9005C29}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Codelyzer.Analysis.Build"", ""Analysis\Codelyzer.Analysis.Build\Codelyzer.Analysis.Build.csproj"", ""{DE7DE201-A18C-49A1-9501-EC0053FF0B89}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Codelyzer.Analysis.Model"", ""Analysis\Codelyzer.Analysis.Model\Codelyzer.Analysis.Model.csproj"", ""{19F5F605-BC7B-4122-965C-3B2468C8C227}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Codelyzer.Analysis.CSharp"", ""Analysis\Codelyzer.Analysis.CSharp\Codelyzer.Analysis.CSharp.csproj"", ""{D6C91EEA-08F6-48D5-9D26-AB3C7CA36AF1}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Codelyzer.Analysis.Common"", ""Analysis\Codelyzer.Analysis.Common\Codelyzer.Analysis.Common.csproj"", ""{E85A6836-B5BB-4D10-8AD0-88BF8E093C96}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Codelyzer.Analysis.Tests"", ""..\tst\Codelyzer.Analysis.Tests\Codelyzer.Analysis.Tests.csproj"", ""{5EB36B53-4B93-4819-8BEB-6D44D18EA47B}""
EndProject
Global
    GlobalSection(SolutionConfigurationPlatforms) = preSolution
        Debug | Any CPU = Debug | Any CPU
        Release | Any CPU = Release | Any CPU
        EndGlobalSection
    GlobalSection(ProjectConfigurationPlatforms) = postSolution
        { BDE5AC46 - B937 - 4EAE - 92F8 - AE59E9005C29}.Debug | Any CPU.ActiveCfg = Debug | Any CPU
        { BDE5AC46 - B937 - 4EAE - 92F8 - AE59E9005C29}.Debug | Any CPU.Build.0 = Debug | Any CPU
        { BDE5AC46 - B937 - 4EAE - 92F8 - AE59E9005C29}.Release | Any CPU.ActiveCfg = Release | Any CPU
        { BDE5AC46 - B937 - 4EAE - 92F8 - AE59E9005C29}.Release | Any CPU.Build.0 = Release | Any CPU
        { DE7DE201 - A18C - 49A1 - 9501 - EC0053FF0B89}.Debug | Any CPU.ActiveCfg = Debug | Any CPU
        { DE7DE201 - A18C - 49A1 - 9501 - EC0053FF0B89}.Debug | Any CPU.Build.0 = Debug | Any CPU
        { DE7DE201 - A18C - 49A1 - 9501 - EC0053FF0B89}.Release | Any CPU.ActiveCfg = Release | Any CPU
        { DE7DE201 - A18C - 49A1 - 9501 - EC0053FF0B89}.Release | Any CPU.Build.0 = Release | Any CPU
        { 19F5F605 - BC7B - 4122 - 965C - 3B2468C8C227}.Debug | Any CPU.ActiveCfg = Debug | Any CPU
        { 19F5F605 - BC7B - 4122 - 965C - 3B2468C8C227}.Debug | Any CPU.Build.0 = Debug | Any CPU
        { 19F5F605 - BC7B - 4122 - 965C - 3B2468C8C227}.Release | Any CPU.ActiveCfg = Release | Any CPU
        { 19F5F605 - BC7B - 4122 - 965C - 3B2468C8C227}.Release | Any CPU.Build.0 = Release | Any CPU
        { D6C91EEA - 08F6 - 48D5 - 9D26 - AB3C7CA36AF1}.Debug | Any CPU.ActiveCfg = Debug | Any CPU
        { D6C91EEA - 08F6 - 48D5 - 9D26 - AB3C7CA36AF1}.Debug | Any CPU.Build.0 = Debug | Any CPU
        { D6C91EEA - 08F6 - 48D5 - 9D26 - AB3C7CA36AF1}.Release | Any CPU.ActiveCfg = Release | Any CPU
        { D6C91EEA - 08F6 - 48D5 - 9D26 - AB3C7CA36AF1}.Release | Any CPU.Build.0 = Release | Any CPU
        { E85A6836 - B5BB - 4D10 - 8AD0 - 88BF8E093C96}.Debug | Any CPU.ActiveCfg = Debug | Any CPU
        { E85A6836 - B5BB - 4D10 - 8AD0 - 88BF8E093C96}.Debug | Any CPU.Build.0 = Debug | Any CPU
        { E85A6836 - B5BB - 4D10 - 8AD0 - 88BF8E093C96}.Release | Any CPU.ActiveCfg = Release | Any CPU
        { E85A6836 - B5BB - 4D10 - 8AD0 - 88BF8E093C96}.Release | Any CPU.Build.0 = Release | Any CPU
        { 5EB36B53 - 4B93 - 4819 - 8BEB - 6D44D18EA47B}.Debug | Any CPU.ActiveCfg = Debug | Any CPU
        { 5EB36B53 - 4B93 - 4819 - 8BEB - 6D44D18EA47B}.Debug | Any CPU.Build.0 = Debug | Any CPU
        { 5EB36B53 - 4B93 - 4819 - 8BEB - 6D44D18EA47B}.Release | Any CPU.ActiveCfg = Release | Any CPU
        { 5EB36B53 - 4B93 - 4819 - 8BEB - 6D44D18EA47B}.Release | Any CPU.Build.0 = Release | Any CPU
EndGlobalSection
    GlobalSection(SolutionProperties) = preSolution
        HideSolutionNode = FALSE
    EndGlobalSection
    GlobalSection(ExtensibilityGlobals) = postSolution
        SolutionGuid = { ADDD44A7 - A0FB - 40FB - AFB4 - C4971093C89A}
                EndGlobalSection
            EndGlobal
";
    }
}