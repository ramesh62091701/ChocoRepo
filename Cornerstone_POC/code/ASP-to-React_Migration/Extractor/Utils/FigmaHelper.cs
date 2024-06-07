using Extractor.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Utils
{
    public static class FigmaHelper
    {
        private static List<string> ContentsToRemove = new List<string>{ "id", "scrollBehavior" , "imageRef", "blendMode" , "layoutAlign" , "layoutGrow", "scaleMode" , "exportSettings", "letterSpacing", "clipsContent" , "itemSpacing", "layoutWrap" , "lineIndentations", "lineTypes", "characterStyleOverrides" };
        public async static Task<string> GetContents(string fileUrl)
        {
            var contents = await HttpHelper.ExecuteGet("application/json", "X-Figma-Token", Configuration.FigmaToken, fileUrl);

            JObject json = JObject.Parse(contents);

            RemoveKeys(json, ContentsToRemove);

            string filteredJsonString = JsonConvert.SerializeObject(json, Formatting.Indented);






            var lines = contents.Split(Environment.NewLine);
            var builder = new StringBuilder();

            foreach (var line in lines)
            {
                if (!KeyExists(line))
                    builder.AppendLine(line);
            }

            return builder.ToString();
        }

        private static bool KeyExists(string key)
        {
            foreach (var line in ContentsToRemove)
            {
                if (key.Contains(line)) return true;
            }
            return false;
        }

        private static void RemoveKeys(JToken token, List<string> keysToRemove)
        {
            if (token.Type == JTokenType.Object)
            {
                var obj = (JObject)token;
                var properties = obj.Properties().ToList();

                foreach (var prop in properties)
                {
                    if(prop.Value.Type == JTokenType.String && prop.Value.Value<string>()  )
                    if (keysToRemove.Contains(prop.Name))
                    {
                        prop.Remove();
                    }
                    else
                    {
                        RemoveKeys(prop.Value, keysToRemove);
                    }
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                var array = (JArray)token;
                foreach (var item in array)
                {
                    RemoveKeys(item, keysToRemove);
                }
            }
        }
    }
}
