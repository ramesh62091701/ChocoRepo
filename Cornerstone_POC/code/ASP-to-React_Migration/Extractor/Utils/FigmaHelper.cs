using Extractor.Model;
using Extractor.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            var gptService = new GPTService();
            filteredJsonString = string.Join(string.Empty, filteredJsonString.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));

            filteredJsonString = filteredJsonString.Replace(" ", "");
            var prompt = $@"<Figma-json>{filteredJsonString}</Figma-json>
{Constants.FigmaUrlToHTMLPrompt}";
            var response = await gptService.GetAiResponse(prompt, String.Empty, Constants.Model, true);
            return response.Message;
            
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
