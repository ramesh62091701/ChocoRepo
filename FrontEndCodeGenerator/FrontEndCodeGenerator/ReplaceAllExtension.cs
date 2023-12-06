using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndCodeGenerator
{
    public static class ReplaceAllExtension
    {
        public static string RemoveAll(this string text, string replacer)
        {
            return string.Join("", text.Split(new string[] { replacer }, StringSplitOptions.None));
        }

        public static string ReplaceAll(this string text, string finder, string replacer)
        {
            return string.Join(replacer, text.Split(new string[] { finder }, StringSplitOptions.None));
        }

        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }
    }
}
