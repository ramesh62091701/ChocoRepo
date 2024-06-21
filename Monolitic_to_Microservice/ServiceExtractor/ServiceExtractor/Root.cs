using System.Text.Json.Serialization;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ServiceExtractor
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Argument
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("semantic-type")]
        public string SemanticType { get; set; }

        [JsonPropertyName("children")]
        public List<object> Children { get; set; }
    }

    public class Child
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("children")]
        public List<Child> Children { get; set; }

        [JsonPropertyName("full-identifier")]
        public string FullIdentifier { get; set; }

        [JsonPropertyName("references")]
        public Reference References { get; set; }

        [JsonPropertyName("method-_name")]
        public string MethodName { get; set; }

        [JsonPropertyName("semantic-namespace")]
        public string SemanticNamespace { get; set; }

        [JsonPropertyName("caller-identifier")]
        public string CallerIdentifier { get; set; }

        [JsonPropertyName("semantic-class-type")]
        public string SemanticClassType { get; set; }

        [JsonPropertyName("semantic-method-signature")]
        public string SemanticMethodSignature { get; set; }

        [JsonPropertyName("parameters")]
        public List<Parameter> Parameters { get; set; }

        [JsonPropertyName("arguments")]
        public List<Argument> Arguments { get; set; }

        [JsonPropertyName("semantic-return-type")]
        public string SemanticReturnType { get; set; }

        [JsonPropertyName("semantic-original-def")]
        public string SemanticOriginalDef { get; set; }

        [JsonPropertyName("semantic-properties")]
        public List<string> SemanticProperties { get; set; }

        [JsonPropertyName("semantic-is-extension")]
        public bool? SemanticIsExtension { get; set; }

        [JsonPropertyName("semantic-full-class-type")]
        public string SemanticFullClassType { get; set; }

        [JsonPropertyName("literal-type")]
        public string LiteralType { get; set; }

        [JsonPropertyName("semantic-type")]
        public string SemanticType { get; set; }

        [JsonPropertyName("base-type")]
        public string BaseType { get; set; }

        [JsonPropertyName("base-type-original-def")]
        public string BaseTypeOriginalDef { get; set; }

        [JsonPropertyName("base-list")]
        public List<string> BaseList { get; set; }

        [JsonPropertyName("modifiers")]
        public string Modifiers { get; set; }

        [JsonPropertyName("return-type")]
        public string ReturnType { get; set; }

        [JsonPropertyName("semantic-signature")]
        public string SemanticSignature { get; set; }
    }

    public class ExternalReferences
    {
        [JsonPropertyName("nuget")]
        public List<Nuget> Nuget { get; set; }

        [JsonPropertyName("nuget-dependencies")]
        public List<NugetDependency> NugetDependencies { get; set; }

        [JsonPropertyName("sdk")]
        public List<Sdk> Sdk { get; set; }

        [JsonPropertyName("project")]
        public List<Project> Project { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("start-char-position")]
        public int StartCharPosition { get; set; }

        [JsonPropertyName("end-char-position")]
        public int EndCharPosition { get; set; }

        [JsonPropertyName("start-line-position")]
        public int StartLinePosition { get; set; }

        [JsonPropertyName("end-line-position")]
        public int EndLinePosition { get; set; }
    }

    public class Nuget
    {
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("assembly-location")]
        public string AssemblyLocation { get; set; }
    }

    public class NugetDependency
    {
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("assembly-location")]
        public string AssemblyLocation { get; set; }
    }

    public class Parameter
    {
        [JsonPropertyName("_name")]
        public string Name { get; set; }

        [JsonPropertyName("semantic-type")]
        public string SemanticType { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class Project
    {
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("assembly-location")]
        public string AssemblyLocation { get; set; }
    }

    public class Reference
    {
        [JsonPropertyName("namespace")]
        public string Namespace { get; set; }

        [JsonPropertyName("assembly")]
        public string Assembly { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("assembly-location")]
        public string AssemblyLocation { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("generated-by")]
        public string GeneratedBy { get; set; }

        [JsonPropertyName("workspace-name")]
        public string WorkspaceName { get; set; }

        [JsonPropertyName("workspace-root-path")]
        public string WorkspaceRootPath { get; set; }

        [JsonPropertyName("source-files")]
        public List<string> SourceFiles { get; set; }

        [JsonPropertyName("content-files")]
        public List<string> ContentFiles { get; set; }

        [JsonPropertyName("errors-found")]
        public int ErrorsFound { get; set; }

        [JsonPropertyName("target-framework")]
        public string TargetFramework { get; set; }

        [JsonPropertyName("target-frameworks")]
        public List<string> TargetFrameworks { get; set; }

        [JsonPropertyName("external-references")]
        public ExternalReferences ExternalReferences { get; set; }

        [JsonPropertyName("source-file-results")]
        public List<SourceFileResult> SourceFileResults { get; set; }

        [JsonPropertyName("workspace-path")]
        public string WorkspacePath { get; set; }

        [JsonPropertyName("build-errors")]
        public List<string> BuildErrors { get; set; }

        [JsonPropertyName("project-guid")]
        public string ProjectGuid { get; set; }

        [JsonPropertyName("project-type")]
        public string ProjectType { get; set; }

        [JsonPropertyName("lines-of-code")]
        public int LinesOfCode { get; set; }

        [JsonPropertyName("solution-name")]
        public string SolutionName { get; set; }

        [JsonPropertyName("solution-path")]
        public string SolutionPath { get; set; }

        [JsonPropertyName("solution-root-path")]
        public string SolutionRootPath { get; set; }
    }

    public class Sdk
    {
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("assembly-location")]
        public string AssemblyLocation { get; set; }
    }

    public class SourceFileResult
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("file-path")]
        public string FilePath { get; set; }

        [JsonPropertyName("file-full-path")]
        public string FileFullPath { get; set; }

        [JsonPropertyName("lines-of-code")]
        public int LinesOfCode { get; set; }

        [JsonPropertyName("references")]
        public List<Reference> References { get; set; }

        [JsonPropertyName("children")]
        public List<Child> Children { get; set; }
    }
}
