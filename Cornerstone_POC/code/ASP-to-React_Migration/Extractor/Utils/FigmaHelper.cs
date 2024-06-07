using Extractor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extractor.Utils
{
    public static class FigmaHelper
    {
        private static List<string> ContentsToRemove = new List<string>{ "id", "scrollBehavior" };
        public async static Task<string> GetContents(string fileUrl)
        {
            var contents = await HttpHelper.ExecuteGet("application/json", "X-Figma-Token", Configuration.FigmaToken, fileUrl);

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
    }
}
