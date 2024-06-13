using Extractor.Model;
using Extractor.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Extractor.Utils
{
    public static class FigmaHelper
    {
        private static List<string> ContentsToRemove = new List<string>{ "id", "scrollBehavior" , "imageRef", "blendMode" , "layoutAlign" , "layoutGrow", "scaleMode" , "exportSettings", "letterSpacing", "clipsContent" , "itemSpacing", "layoutWrap" , "lineIndentations", "lineTypes", "characterStyleOverrides", "visible" , "x" ,"y" , "key"};
        public async static Task<string> GetContents(string fileUrl)
        {
            var contents = await HttpHelper.ExecuteGet("application/json", "X-Figma-Token", Configuration.FigmaToken, fileUrl);
            JObject json = JObject.Parse(contents);
            //RemoveKeys(json, ContentsToRemove);
            string filteredJsonString = JsonConvert.SerializeObject(json, Formatting.Indented);
            filteredJsonString = string.Join(string.Empty, filteredJsonString.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));

            filteredJsonString = filteredJsonString.Replace(" ", "");
            var gptService = new GPTService();
            var prompt = $@"<Figma-json>{filteredJsonString}</Figma-json>
{Constants.FigmaUrlToHTMLPrompt}";
            var response = await gptService.GetAiResponse(prompt, String.Empty, Constants.Model, true);
            return response.Message;

        }

        public async static Task<string> GetFigmaJsonFromUrl(string fileUrl)
        {
            var contents = await HttpHelper.ExecuteGet("application/json", "X-Figma-Token", Configuration.FigmaToken, fileUrl);
            string allChildrenString = string.Empty;
            List<JsonElement> allChildren = new List<JsonElement>();
            using (JsonDocument doc = JsonDocument.Parse(contents))
            {
                JsonElement root = doc.RootElement;
                if (root.TryGetProperty("nodes", out JsonElement nodesElement))
                {
                    foreach (JsonProperty nodeProperty in nodesElement.EnumerateObject())
                    {
                        JsonElement nodeElement = nodeProperty.Value;
                        if (nodeElement.TryGetProperty("document", out JsonElement documentElement) &&
                            documentElement.TryGetProperty("children", out JsonElement childrenElement))
                        {
                            if (childrenElement.ValueKind == JsonValueKind.Array)
                            {
                                Console.WriteLine($"Children for node {nodeProperty.Name}:");
                                foreach (JsonElement child in childrenElement.EnumerateArray())
                                {
                                    allChildren.Add(child);
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Children for node {nodeProperty.Name} is not an array.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Node {nodeProperty.Name} does not have a document or children.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Could not find the nodes element in JSON.");
                }


            allChildrenString = System.Text.Json.JsonSerializer.Serialize(allChildren);
            }
            List<FigmaJsonModel> childrenJson = JsonConvert.DeserializeObject<List<FigmaJsonModel>>(allChildrenString);
            string childNames = string.Empty;
            
            foreach (var child in childrenJson)
            {
                childNames += child.name + ",";
            }
            //Logger.Log(childNames);

            return string.Empty;
        }

        private static void RemoveKeys(JToken token, List<string> keysToRemove)
        {
            if (token.Type == JTokenType.Object)
            {
                var obj = (JObject)token;
                var properties = obj.Properties().ToList();

                foreach (var prop in properties)
                {
                    if (keysToRemove.Contains(prop.Name)
                      || prop.Value.Type == JTokenType.String && string.IsNullOrEmpty(prop.Value.Value<string>())
                      || prop.Value.Type == JTokenType.Integer && prop.Value.Value<int>() == 0
                        )
                    {
                        prop.Remove();
                    }
                    else
                    {
                        if(prop.Value.Type == JTokenType.Float)
                        {
                            prop.Value = Math.Round(prop.Value.Value<float>(), 2);
                        }
                        RemoveKeys(prop.Value, keysToRemove);
                    }
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                var array = (JArray)token;
                if (!array.HasValues)
                {
                    array.Parent?.Remove();
                    return;
                }
                foreach (var item in array)
                {
                    RemoveKeys(item, keysToRemove);
                }
            }
        }
    }
}
