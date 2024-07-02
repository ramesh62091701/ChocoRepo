//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text.Json;
//using System.Linq;
//using HuggingFace;
//using Azure.AI.OpenAI.

//namespace Extractor.Service
//{


//    class FigmaJsonHandler
//    {
//        static void Main(string[] args)
//        {

//            // Load the provided JSON data
//            string jsonData = File.ReadAllText("./Select-json/MainFigma.json");
//            dynamic data = JsonSerializer.Deserialize<dynamic>(jsonData);

//            // Check the token count of the entire data and process if within limits
//            var promptTokens = TokenizerGpt3.Encode("sdsd").Count();
//            if (EstimateTokenCount(data) <= 100000)
//            {
//                // Find the key for the desired node dynamically
//                var nodeKey = ((KeyValuePair<string, object>)data["nodes"]).Key;

//                // Extracting the children
//                var children = data["nodes"][nodeKey]["document"]["children"];

//                // Creating a directory to save the children files
//                string outputDir = "children_files";
//                Directory.CreateDirectory(outputDir);

//                // Saving each child to a separate JSON file
//                int index = 1;
//                foreach (var child in children)
//                {
//                    SaveChildren(child, outputDir, $"child_{index++}");
//                }
//            }
//            else
//            {
//                Console.WriteLine("The entire data exceeds the token limit.");
//            }
//        }

//        static int EstimateTokenCount(dynamic data)
//        {
//            var tokenizer = new Tokenizer("gpt-4o");
//            string dataStr = (data is Dictionary<string, object> || data is List<object>) ?
//                JsonSerializer.Serialize(data) : data.ToString();
//            var tokens = tokenizer.Encode(dataStr);
//            Console.WriteLine(tokens.Count());
//            return tokens.Count();
//        }

//        static void SaveChildren(dynamic data, string outputDir, string baseFilename)
//        {
//            // Ensure the output directory exists
//            Directory.CreateDirectory(outputDir);

//            // Function to recursively save children
//            void RecursiveSave(dynamic item, string path, int index = 1)
//            {
//                if (EstimateTokenCount(item) < 100000)
//                {
//                    string filename = Path.Combine(path, $"{baseFilename}_{index}.json");
//                    File.WriteAllText(filename, JsonSerializer.Serialize(item));
//                    Console.WriteLine($"Data saved to {filename}");
//                }
//                else
//                {
//                    if (item.ContainsKey("children") && item["children"] is List<object>)
//                    {
//                        string subdir = Path.Combine(path, $"{baseFilename}_{index}_children");
//                        Directory.CreateDirectory(subdir);
//                        int subindex = 1;
//                        foreach (var subitem in item["children"])
//                        {
//                            RecursiveSave(subitem, subdir, subindex++);
//                        }
//                    }
//                    else
//                    {
//                        Console.WriteLine($"Item too large and has no children: {item["id"] ?? "Unknown ID"}");
//                    }
//                }
//            }

//            // Start saving with the root data
//            RecursiveSave(data, outputDir);
//        }
//    }

//}
