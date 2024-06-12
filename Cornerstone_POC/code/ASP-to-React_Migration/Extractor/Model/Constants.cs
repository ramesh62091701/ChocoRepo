﻿namespace Extractor.Model
{
    internal class Constants
    {
        public const string Model = "gpt-4o";

        public const string SysPrompt = "You are expert in understanding the aspx files. Your job is to convert aspx frontend file to its equivalent React files";

        public const string ReactSysPrompt = "You are expert in converting plain HTML / CSS to react.\r\nRules to follow while converting HTML to CSS:\r\n1.Use MUI components for all the controls";

        public const string AspxPrompt = $@"Understand the above ASPX-File and divide it into multiple parts which would be easier to later convert it to react code. 
Rules to follow:
1. Output should consists of only ASPX code
2. Do not generate any react files or anything related to react";

        public const string FigmaImageToHTMLPrompt = @"You are a senior developer.
Use the provided design context to create idiomatic HTML/CSS code based on the user request. Create responsive UI.
Everything must be inline in one file and your response must be directly renderable by the browser.
Write code that matches the Figma file nodes and metadata as exactly as you can.";

        public const string FigmaUrlToHTMLPrompt = @"From above Figma json create a HTML and CSS in single file.
Use the provided design context to create idiomatic HTML/CSS code based on the user request. Create responsive UI.
Everything must be inline in one file and your response must be directly renderable by the browser.
Remember generate only HTML markup with CSS, do not give any explanation.";

        public const string AspxCodeToJson = @"Above is the code for aspx page.Can you provide the details in the json format for all the controls starting with <asp:x> and <uc:x> (user controls) controls defined at in this page.
Only return the json code in lower case. Don't add any description or notes.
The sample format of json is:

  [
    {
      ""type"": ""asp:Content"",
      ""Id"": ""idname"",
      ""runat"": ""server"",
      ""contentPlaceHolderID"": ""placeholderid"",
      ""alternateText"": """",
      ""class"": ""css class name"",
    },
    {
      ""type"": ""uc:Header"",
      ""ID"": ""idname"",
      ""runat"": ""server"",
      ""OnItemDataBound"": ""event_OnItemDataBound"",
      ""startDate"": ""today"",
    },
    {
      ""type"": ""asp:Repeater"",
      ""ID"": ""idname"",
      ""runat"": ""server"",
      ""OnItemDataBound"": ""event_OnItemDataBound"",
      ""startDate"": ""today"",
    }]";
    }
}
