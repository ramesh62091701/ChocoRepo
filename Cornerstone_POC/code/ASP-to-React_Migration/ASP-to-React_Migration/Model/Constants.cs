using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_to_React_Migration.Model
{
    internal class Constants
    {
        public const string Model = "gpt-4o";

        public const string SysPrompt= "You are expert in understanding the aspx files. Your job is to convert aspx frontend file to its equivalent React files";

        public const string ReactSysPrompt = "You are expert in converting plain HTML / CSS to react.\r\nRules to follow while converting HTML to CSS:\r\n1.Use React UI for all the components";

        public const string AspxPrompt = $@"Understand the above ASPX-File and divide it into multiple parts which would be easier to later convert it to react code. 
Rules to follow:
1. Output should consists of only ASPX code
2. Do not generate any react files or anything related to react";

        public const string ImagePrompt = "You are a senior developer.\r\nUse the provided design context to create idiomatic HTML/CSS code based on the user request.\r\nEverything must be inline in one file and your response must be directly renderable by the browser.\r\nWrite code that matches the Figma file nodes and metadata as exactly as you can.";
    
    
    
    }
}
