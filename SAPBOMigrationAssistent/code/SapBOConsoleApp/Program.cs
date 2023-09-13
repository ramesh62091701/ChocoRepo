// See https://aka.ms/new-console-template for more information
using SAPBOAnalysis;

Console.WriteLine("Started");
var generator = new Generator();
await generator.Generate(new SAPBOAnalysis.Models.AnalysisSettings());
Console.WriteLine("Completed");
