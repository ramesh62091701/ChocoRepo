using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FrontEndCodeGenerator
{
    public  class ParserGenerator
    {
        private List<string> AllCslines = new List<string>();
        private List<string> AllDesignerlines = new List<string>();
        private string FormTitle = "";
        private int FormWidth = 0;
        private readonly log4net.ILog log;

        public ParserGenerator(log4net.ILog _log)
        {
            log = _log;
        }
        public ParserGenerator()
        {

        }
        private void ExtractControlsInfo(string filePath, out Dictionary<string, string> uiControls, out string formName, out List<UIControl> mainControls, out Dictionary<string, Event> eventsInfo)
        {
            uiControls = new Dictionary<string, string>();
            List<string> uniqueEvents = new List<string>();
            formName = "";
            var allLines = File.ReadAllLines(filePath);
            AllDesignerlines = allLines.ToList();
            AllCslines = File.ReadAllLines(filePath.Replace("Designer.cs", "cs")).ToList();
            foreach (var line in allLines)
            {
                if (line.Contains(" class ") && formName == "")
                {
                    formName = line.Split(new string[] { "class " }, StringSplitOptions.None)[1];
                    if (formName.Contains(":"))
                        formName = formName.Split(':')[0];
                }

                if (line.Contains(" = ") && line.Contains("new ") && (line.Contains("()") || line.Contains("BTGrid(")))
                {
                    var parts = line.Split('=');
                    var rightSide = parts[1];
                    string controlType = rightSide.Replace("new ", "");
                    if (controlType.Contains("("))
                        controlType = controlType.Substring(0, controlType.IndexOf("("));
                    else if (controlType.Contains("["))
                        controlType = controlType.Substring(0, controlType.IndexOf("["));
                    else
                    {
                    }

                    if (controlType.Contains("."))
                    {
                        controlType = controlType.Substring(controlType.LastIndexOf('.') + 1, (controlType.Length - controlType.LastIndexOf('.') - 1));
                    }

                    string controlName = parts[0].Replace("this.", "").Replace(" ", "");
                    controlName = controlName.RemoveAll("\t").RemoveAll("//");

                    if (!uiControls.ContainsKey(controlName))
                        uiControls.Add(controlName, controlType);
                }

                if (line.Contains("+="))
                {
                    var parts = line.Split(new string[] { "+=" }, StringSplitOptions.None);

                    var left = parts[0].Replace("this.", "");

                    var right = parts[1].Substring(parts[1].IndexOf('(') + 1, parts[1].Length - parts[1].IndexOf('(') - 3);
                    var controlName = left.Split('.')[0];

                    var leftSplits = left.Split('.');
                    var event_name = left.Split('.')[0] + "|" + right.Replace("this.", "");

                    if (leftSplits.Length > 1)
                        event_name = left.Split('.')[1] + "|" + right.Replace("this.", "");

                    if (!uniqueEvents.Contains(right.Replace("this.", "")))
                        uniqueEvents.Add(right.Replace("this.", ""));
                }
            }

            var textLine = AllDesignerlines.Where(x => x.Contains("this.Text =")).FirstOrDefault();
            if (!string.IsNullOrEmpty(textLine))
                FormTitle = textLine.Split('=')[1].Replace("\"", "").Replace(";", "");

            textLine = AllDesignerlines.Where(x => x.Contains("this.ClientSize =")).FirstOrDefault();
            if (!string.IsNullOrEmpty(textLine))
                FormWidth = Convert.ToInt32(textLine.Split(new string[] { "new System.Drawing.Size(" }, StringSplitOptions.None)[1].Split(',')[0]);

            mainControls = GetFormControlInfo(AllDesignerlines, AllCslines, uiControls, "", "", uniqueEvents, null);
            eventsInfo = GetEventDetails(filePath, uiControls.Keys.ToList());
        }

        private Dictionary<string, Event> GetEventDetails(string filePath, List<string> uiControls)
        {
            filePath = filePath.Replace("Designer.", "");

            Dictionary<string, Event> events = new Dictionary<string, Event>();
            string lastMethodName = "";
            long lineNumber = 0;
            foreach (var line in File.ReadAllLines(filePath))
            {
                lineNumber++;
                if ((line.IndexOf("private ") >= 0 || line.IndexOf("public ") >= 0) && line.Contains("("))
                {
                    int endPosition = line.IndexOf("(");
                    string left = line.Substring(0, endPosition);
                    int startPosition = left.LastIndexOf(" ");
                    lastMethodName = left.Substring(startPosition, (left.Length - startPosition));
                    var eventData = new Event();
                    eventData.Name = lastMethodName;
                    eventData.ExactLine = line;
                    eventData.StartLine = lineNumber;
                    if (events.Count > 0)
                    {
                        events.ElementAt(events.Count - 1).Value.EndLine = (lineNumber - 1);
                    }
                    eventData.Type = line.Contains("private") ? "private" : "public";
                    if (!events.ContainsKey(lastMethodName))
                        events.Add(lastMethodName, eventData);
                    else
                    {
                    }
                }
                else
                {
                    if (events.ContainsKey(lastMethodName))
                    {
                        var eventData = events[lastMethodName];
                        var catrgorized = false;

                        foreach (var uiControl in uiControls)
                        {
                            if (line.Contains(uiControl))
                            {
                                eventData.FrontEndLines.Add(line);
                                catrgorized = true;
                                break;
                            }
                        }

                        if (!catrgorized)
                        {
                            if (line.Contains("mbln") || line.Contains("mcls") || line.Contains(".DeafultInstance."))
                            {
                                eventData.BackEndLines.Add(line);
                            }
                            else
                            {
                                eventData.UnKnownLines.Add(line);
                            }
                        }
                    }
                    else
                    {
                    }
                }
            }

            return events;
        }

        public List<UIControl> GetFormControlInfo(List<string> allDesignerLines, List<string> allCSLines, Dictionary<string, string> uiControls, string controlName, string controlType, List<string> events, UIControl parentControl)
        {
            List<UIControl> controls = new List<UIControl>();
            List<string> controlLines;
            var subControlFindText = "Controls.Add(";


            controlLines = allDesignerLines.Where(x => x.Contains("this." + controlName + subControlFindText)).Select(y => y).ToList();


            if (controlType == "RadSplitContainer" && controlLines.Count == 0)
            {
                subControlFindText = "SplitPanels.Add(";

                controlLines = allDesignerLines.Where(x => x.Contains("this." + controlName + subControlFindText)).Select(y => y).ToList();
            }
            else if (controlType == "TabControlExtender" && controlLines.Count == 0)
            {
                subControlFindText = "TabPages.Add(";

                controlLines = allDesignerLines.Where(x => x.Contains("this." + controlName + subControlFindText)).Select(y => y).ToList();

            }

            foreach (var line in controlLines)
            {
                var pControl = new UIControl();
                pControl.Name = line.Split(new string[] { "this." + controlName + subControlFindText }, StringSplitOptions.None)[1];
                pControl.Name = pControl.Name.Split(',')[0];
                pControl.Name = string.Join("", pControl.Name.Split('\t'));
                pControl.Name = pControl.Name.Trim().Replace(");", "").Replace("this.", "");
                var textLine1 = allDesignerLines.Where(x => x.Contains("this." + pControl.Name + ".Text ")).FirstOrDefault();

                if (uiControls.ContainsKey(pControl.Name))
                    pControl.Type = uiControls[pControl.Name];

                if (!string.IsNullOrEmpty(textLine1))
                {
                    var right = textLine1.Split(new string[] { "=" }, StringSplitOptions.None)[1];
                    right = right.Replace(";", "");
                    right = string.Join("", right.Split('"'));

                    //right = string.Join("", right.Split(' '));
                    right = string.Join("", right.Split('\t'));
                    pControl.Text = string.Join("", right.Trim().RemoveAll("<").RemoveAll(">"));
                    if (pControl.Text.Contains('&'))
                    {
                        pControl.HotKey = "'Alt+" + pControl.Text[pControl.Text.IndexOf('&') + 1].ToString().ToLower() + "'";
                    }
                }
                else
                {
                    var toolTipText = allDesignerLines.Where(x => x.Contains("this.ToolTipMain.SetToolTip(this." + pControl.Name)).FirstOrDefault();
                    if (!string.IsNullOrEmpty(toolTipText))
                    {
                        toolTipText = toolTipText.Replace("this.ToolTipMain.SetToolTip(this." + pControl.Name + ",", "").Replace(";", "").RemoveAll("\"").Replace(")", "").Trim();
                        pControl.ToolTip = toolTipText;
                    }
                }

                if (pControl.Type == "ComboBox")
                {
                    var DropDownStyleTag = allDesignerLines.Where(x => x.Contains("this." + pControl.Name + ".DropDownStyle")).Select(y => y).ToList();
                    if (DropDownStyleTag.Count() > 0)
                    {
                        string ValueOfDropDownStyleTag = DropDownStyleTag[0].Split('=')[1];
                        if (ValueOfDropDownStyleTag.Contains("DropDownList"))
                        {
                            pControl.IsEditableDropdown = false;
                        }
                    }
                }
                var locationLine = allDesignerLines.Where(x => x.Contains("this." + pControl.Name + ".Location")).Select(y => y).ToList();
                if (locationLine.Count() > 0)
                {
                    string location = locationLine[0].Split('=')[1];
                    location = location.Replace("new System.Drawing.Point(", "").Replace(");", "");
                    try
                    {
                        pControl.Left = Convert.ToInt32(location.Split(',')[0]);
                        pControl.Top = Convert.ToInt32(location.Split(',')[1]);
                    }
                    catch (Exception ex)
                    {
                        log.Warn(pControl.Name + " - Location property issue");
                    }
                }
                var sizeLine = allDesignerLines.Where(x => x.Contains("this." + pControl.Name + ".Size")).Select(y => y).ToList();
                if (sizeLine.Count() > 0)
                {
                    string sizeL = sizeLine[0].Split('=')[1];
                    if (sizeL.Contains("new System.Drawing.Size("))
                    {
                        sizeL = sizeL.Replace("new System.Drawing.Size(", "").Replace(");", "");
                        try
                        {
                            pControl.Width = Convert.ToInt32(sizeL.Split(',')[0]);
                            pControl.Height = Convert.ToInt32(sizeL.Split(',')[1]);
                        }
                        catch (Exception ex)
                        {
                            log.Warn(pControl.Name + " - Size property issue");
                        }
                    }
                }
                if (parentControl != null)
                {
                    pControl.ParentHeight = parentControl.Height;
                }
                var maxLengthCsLine = allCSLines.Where(x => x.Contains(pControl.Name + ".MaxLength")).Select(y => y).ToList();
                if (maxLengthCsLine.Count() > 0)
                    pControl.IsMaxLengthModified = true;
                var maxLengthLine = allDesignerLines.Where(x => x.Contains(pControl.Name + ".MaxLength =")).Select(y => y).ToList();
                if (maxLengthLine.Count() > 0)
                {
                    pControl.IsMaxLengthModified = true;
                    pControl.MaxLength = Convert.ToInt32(maxLengthLine[0].Split('=')[1].Replace(";", ""));
                }
                var visibleCsLine = allCSLines.Where(x => x.Contains(pControl.Name + ".Visible")).Select(y => y).ToList();
                if (visibleCsLine.Count() > 0)
                {
                    pControl.IsVisibleModified = true;
                    pControl.Visible = true;
                }
                else if (pControl.Name.IndexOf('_') == 0)
                {
                    var groupedControlName = pControl.Name.Split('_')[1];
                    var visibleGroupCsine = allCSLines.Where(x => x.Contains(groupedControlName + "[") && x.Contains("].Visible")).Select(y => y).ToList();
                    if (visibleGroupCsine.Count() > 0)
                    {
                        pControl.IsVisibleModified = true;
                        pControl.Visible = false;
                    }
                }
                var visibleLine = allDesignerLines.Where(x => x.Contains("this." + pControl.Name + ".Visible =")).Select(y => y).ToList();
                if (visibleLine.Count() > 0)
                {
                    pControl.IsVisibleModified = true;
                    pControl.Visible = Convert.ToBoolean(visibleLine[0].Split('=')[1].Replace(";", ""));
                }

                var disableCSLine = allCSLines.Where(x => x.Contains("saCommon.saGlobals.DefaultInstance.saTextBoxLock(" + pControl.Name)).Select(y => y).ToList();
                if (disableCSLine.Count() > 0)
                {
                    pControl.IsDisabledModified = true;
                }

                pControl.Events = events.Where(x => x.Contains(pControl.Name + "_")).Select(y => y).ToList();

                pControl.Child = GetFormControlInfo(allDesignerLines, allCSLines, uiControls, pControl.Name + ".", pControl.Type, events, pControl);
                controls.Add(pControl);
            }

            return controls;
        }
        public void Parse(string filePath)
        {
            ExtractControlsInfo(filePath, out Dictionary<string, string> uiControls, out string formName, out List<UIControl> mainControls, out Dictionary<string, Event> eventsInfo);
            // Front end code generation
            var fronendCodeGenerator = new FrontendCodeGenerator(FormTitle, formName, FormWidth, AllCslines);
            fronendCodeGenerator.Execute(formName, mainControls);        
        }
        public void Parse(string filePath, WorkflowType workflowType)
        {
            ExtractControlsInfo(filePath, out Dictionary<string, string> uiControls, out string formName, out List<UIControl> mainControls, out Dictionary<string, Event> eventsInfo);
                        
        }
    }
}