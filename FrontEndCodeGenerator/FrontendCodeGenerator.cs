 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.Json;

using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;


namespace FrontEndCodeGenerator
{
    public class FrontendCodeGenerator
    {
        private static List<string> AllCslines;
        private static string FormName;
        private static string FormTitle;
        private static int FormWidth;
        private static List<string> typeScriptBindUI;
        private static List<string> typeScriptBindUISetupGrids;
        private static List<string> typeScriptCoreImports;
        private static List<string> typeScriptKieticImports;
        private static List<string> typeScriptMethods;
        private static List<string> typeScriptEventDataStringCase;
        private static List<string> typeScriptVariables;

        public FrontendCodeGenerator(string formTitle, string formName, int formWidth, List<string> allCsLines)
        {
            typeScriptVariables = new List<string>();
            typeScriptKieticImports = new List<string>();
            typeScriptCoreImports = new List<string>();
            typeScriptMethods = new List<string>();
            typeScriptBindUI = new List<string>();
            typeScriptBindUISetupGrids = new List<string>();
            typeScriptEventDataStringCase = new List<string>();
            FormWidth = formWidth;
            FormName = formName;
            FormTitle = formTitle;
            AllCslines = allCsLines;

            //typeScriptVariables.Add(" uiModel:any;");
        }

//        private static void GenerateController(string formName)
//        {
//            string KineticImports = "EpDialogInstance";

//            string CoreImport = "Component, Injector, OnInit ";
//            if (typeScriptCoreImports.Count() > 0)
//            {
//                CoreImport += ", " + string.Join(", ", typeScriptCoreImports.Distinct());
//            }

//            string fromType = "frm";
//            if (formName.IndexOf("ctl") == 0)
//            {
//                fromType = "ctl";
//                KineticImports = " ";
//            }
//            if (typeScriptKieticImports.Count() > 0)
//                KineticImports += (KineticImports == " " ? "" : ", ") + string.Join(", ", typeScriptKieticImports.Distinct());
//            List<string> result = new List<string>();
//            string formNameSpaced = SplitCamelCase(formName.Replace("frm", ""));
//            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));

//            result.Add("import {" + CoreImport + "} from '@angular/core';");
//            result.Add("import {" + KineticImports + " } from '@epicor/ux-platform';");
//            result.Add("import { EpbtBaseDialog } from '../core/abstract-components/ep-bt-base-dialog';");
//            result.Add("import { ActorExecutionContext } from '../core/abstract-components/ep-bt-base-form.interface';");
//            result.Add("import { Payload } from '../core/model/ep-bt-actor-models';");

//            result.Add("// import { " + formName.Replace("frm", "") + "Options } from './epbt-" + formNameFormated + ".options';");
//            result.Add(" import {FormChangeDetectionService} from '../utility/ui-change-detection.service';");
//            result.Add("import { GridUtilityService, jgexColumnTypeConstants, jgexEditTypeConstants, jgexAlignmentConstants, jgexSortTypeConstants } from '../utility/grid-utility.service';  ");
//            result.Add("");
//            result.Add("@Component({");

//            result.Add("   selector: 'frm-epbt-attachment2',");
//            result.Add("   templateUrl: './epbt-attachments-browser.component.html',");
//            result.Add("  styleUrls:['./epbt-attachments-browser.component.css']");
//            result.Add("   selector: '" + "ep-bt-" + formNameFormated + "',");
//            result.Add("   templateUrl: './ep-bt-" + formNameFormated + ".component.html',");
//            result.Add("   styleUrls:['./ep-bt-" + formNameFormated + ".component.css']");
//            result.Add("})");
//            result.Add("");
//            result.Add("export class EpBt" + formName.Replace("frm", "") + "Component extends EpbtBaseDialog implements OnInit {");

//            result.Add("export class EpbtAttachmentsBrowserComponent implements OnInit {");
//            if (fromType == "frm")
//                result.Add(" instance: EpDialogInstance;");
//            result.Add(" formIdentifier: string ='';");
//            result.AddRange(typeScriptVariables);
//            result.Add("");
//            result.Add("constructor(private gridService: GridUtilityService, injector: Injector) {");
//            result.Add("    super(injector);");
//            result.Add("    this.component = this;");
//            result.Add("}");
//            result.Add("");
//            result.Add(@"ngOnInit(): void {
//    this.formIdentifier = this.instance.options.data;
//    this.executeEvent('" + formName + "_Load');\r\n" +
//                            "}");
//            result.Add("");
//            result.Add("bindUI(executionContext: ActorExecutionContext): void {");

//            result.AddRange(typeScriptBindUISetupGrids);
//            result.Add("    if (this.uiModel) {");

//            result.AddRange(typeScriptBindUI);
//            result.Add("    }");

//            result.Add("}");
//            result.Add("");
//            result.Add(@"executeEvent(eventName: string) {
//    var eventDataString = '';
//    switch(eventName)
//    {");
//            result.AddRange(typeScriptEventDataStringCase);
//            result.Add(@"   }
//        let apiPayload = new Payload();
//        apiPayload.init(this.formIdentifier, '" + formName + @"', eventName, '', eventDataString, false);
//        this.executeCall(apiPayload);
//}");

//            result.AddRange(typeScriptMethods);
//            result.Add("");
//            result.Add(@"ngOnDestroy() {
//    this.closeForm();
//}");
//            result.Add("}");
//            fromType = "controls";
//            if (formName.IndexOf("frm") == 0)
//                fromType = "forms";

//            var directoryPath = Directory.GetCurrentDirectory();
//            directoryPath = directoryPath.Split('\\')[0];
//            string path = directoryPath + "\\" + fromType + "\\" + formName;

//            Console.WriteLine("..........Creating front end files at " + path);
//            using (StreamWriter sw = File.CreateText("/" + fromType + "/" + formName + "/" + "ep-bt-" + formNameFormated + ".component.ts"))
//            {
//                foreach (var line in result)
//                    sw.WriteLine(line);
//            }
//            ProcessStartInfo startInfo = new ProcessStartInfo
//            {
//                Arguments = path,
//                FileName = "explorer.exe"
//            };

//            Process.Start(startInfo);
//        }

        private static List<string> GenerateGridModelBinding(string gridName)
        {
            List<string> result = new List<string>();
            typeScriptBindUISetupGrids.Add("this.set" + gridName + "Model([]);");
            typeScriptKieticImports.Add("IEpGridSelectableSettings");
            typeScriptKieticImports.Add("IEpGridSelectionOptions");
            typeScriptKieticImports.Add("EpGridEditMode");
            typeScriptKieticImports.Add("IEpComponentEventArg");

            result.Add("set" + gridName + "Model(lines:any){");
            result.Add("this.gridService.columns =[]");
            var gridColumnLines = AllCslines.Where(x => x.Contains("disJanusGrid.disJanusGridGlobals.DefaultInstance.JanusAddColumn(" + gridName)).ToList();
            foreach (var cLine in gridColumnLines)
            {
                string cText = cLine.Replace("disJanusGrid.disJanusGridGlobals.DefaultInstance.JanusAddColumn(" + gridName + ",", "");
                cText = cText.RemoveAll("\t").RemoveAll("GridEX20.");
                cText = cText.Replace("Environment.NewLine", @"'\n'");
                if (cText.Contains("disCoreSVC") || cText.Contains(" str") || cText.Contains("StringsHelper."))
                    cText = "// this.gridService.addColumn(''," + cText;
                else
                    cText = "this.gridService.addColumn(''," + cText;
                result.Add(cText);
            }
            result.Add("const selectSettings: IEpGridSelectableSettings = { checkboxOnly: false,mode: 'single' };");
            result.Add("const selectionOptions: IEpGridSelectionOptions = { showCheckBox: false, showSelectAll: false, selectByField: 'tempID', selectedKeys:[] };");
            result.Add("// if (results.length > 0)");
            result.Add("//   selectionOptions.selectedKeys.push(results[0].uId);");
            result.Add("const model: IEpGridModel = {");
            result.Add("resizable: true, reorderable: false, groupable: false, filterable: false,");
            result.Add("selectable: selectSettings,");
            result.Add("selectionOptions: selectionOptions,");
            result.Add("scrollable: 'scrollable', rowHeight: 20,  sortable: false,  height: 250, editable: true,  editOptions: { editMode: EpGridEditMode.cell},");
            result.Add("data: lines,");
            result.Add("columns:this.gridService.columns,");

            result.Add("epOnCellClose: (item: IEpComponentEventArg) => {");
            result.Add("var colindex = item.event.column.epGridColumn.userData.colindex;");
            result.Add("var rowIndex = item.event.dataItem.tempID;");
            result.Add("var currentValue = item.event.dataItem[item.event.column.field];");
            result.Add("}");
            result.Add("};");

            result.Add("this." + gridName + "Model = model;");
            result.Add("}");

            return result;
        }
        private static void GenerateJson(string formName, List<UIControl> controls)
        {
            controls = controls.OrderBy(x => x.Top).ThenBy(y => y.Left).ToList();
            controls = MergeTextBoxandLabel(controls);           
         
            string jsonString = string.Empty;
            jsonString = JsonConvert.SerializeObject(controls, Formatting.Indented);

            string currentPath = Directory.GetCurrentDirectory();
             
            File.WriteAllText(@"D:\" + formName + ".json", jsonString);
            Console.WriteLine(jsonString);
        }
        private static void GenerateHtml(string formName, List<UIControl> controls)
        {
            List<string> html = new List<string>();
            string formNameSpaced = SplitCamelCase(formName.Replace("frm", ""));
            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));
            controls = controls.OrderBy(x => x.Top).ThenBy(y => y.Left).ToList();
            controls = MergeTextBoxandLabel(controls);

            //string jsonString = JsonSerializer.Serialize(controls);
            //Console.WriteLine(jsonString);

           // string json = JsonSerializer.Serialize(controls);
           // File.WriteAllText(@"D:\path.json", json);            

            string fromType = "controls";
            if (formName.IndexOf("frm") == 0)
            {
                fromType = "forms";
            }

            if (!System.IO.Directory.Exists("/" + fromType + "/" + formName))
            {
                Directory.CreateDirectory("/" + fromType + "/" + formName);
            }
            using (StreamWriter sw = File.CreateText("/" + fromType + "/" + formName + "/" + "ep-bt-" + formNameFormated + ".component.html"))
            {
                foreach (var line in html)
                    sw.WriteLine(line);
            }
        }

        private static List<string> GetApiCallCode(string methodName)
        {
            List<string> result = new List<string>();

            result.Add("var apiPayload = new Payload();");
            result.Add("apiPayload.init({ FormIdentifier: this.formIdentifier , FormName: '" + FormName + "', MethodName: '" + methodName + "', uiModelString: this.deltaService.getChanges(this.uiModel) , eventDataString:''});");

            result.Add("this.actorService.Execute(this.actorService.getSessionId(),apiPayload).then((resp)=>");
            result.Add("{ ");
            result.Add(" this.bindUI(resp); ");
            result.Add("},(err)=> ");
            result.Add("{ ");
            result.Add("console.error(err); ");
            result.Add("}); ");

            return result;
        }

        private static string GetEventHTML(List<string> events)
        {
            string result = "";
            foreach (var eve in events)
            {
                var controlName = eve.Split('_')[0];
                var eventName = eve.Split('_')[1];

                switch (eventName)
                {
                    case "Click":
                        {
                            result += " (epOnClick)=\"executeEvent('" + eve + "')\" ";

                            break;
                        }
                    case "SelectedIndexChanged":
                        {
                            result += " (selectionChange)=\"executeEvent('" + eve + "')\" ";
                            break;
                        }
                    case "DoubleClick":
                    case "AfterSelectClick":
                    case "SelectClick":
                        {
                            //typeScriptCoreImports.Add("Output");
                            //typeScriptCoreImports.Add("EventEmitter");
                            //typeScriptVariables.Add("  @Output()  " + eventName.ToCamelCase() + "  = new EventEmitter<any>();");
                            //result += " (" + eventName.ToCamelCase() + ")=\"" + eve + "(data)\" ";
                            //typeScriptMethods.Add(eve + "()");
                            //typeScriptMethods.Add("{");
                            //typeScriptMethods.Add("this." + eventName.ToCamelCase() + ".next(undefined);");
                            //typeScriptMethods.Add("}");
                            break;
                        }
                    case "BeforeColEdit":
                    case "BeforeColUpdate":
                    case "RowFormat":
                    case "UnboundReadData":
                    case "ItemClicked":
                    case "Leave":
                    case "Resize":
                    case "KeyPress":
                        {
                            break;
                        }
                    default:
                        {
                            break;
                        }
                    case "DblClick":
                    case "FetchIcon":
                    case "ColButtonClick":

                    case "KeyUp":
                    case "BrowseClick":
                    case "CheckStateChanged":
                    case "Change":
                    case "MouseUpEvent":
                    case "BeforePrintPage":
                    case "DropDown":
                    case "MouseMoveEvent":
                    case "RowColChange":
                    case "MouseDownEvent":
                    case "KeyUpEvent":
                    case "KeyDownEvent":
                    case "UnboundUpdate":
                    case "SelectionChange":

                    case "ColResize":
                    case "BeforeColumnDrag":
                    case "ClickEvent":
                    case "OLEDragDrop":
                    case "ClientSizeChanged":
                    case "TextChanged":
                    case "Enter":
                    case "AfterGroupChange":
                    case "HeightChanged":
                    case "TallyChanged":
                    case "WidthChanged":
                    case "KeyPressEvent":
                    case "AfterColUpdate":
                    case "BeforeUpdate":
                    case "UnboundAddNew":
                    case "UnboundDelete":
                    case "UnboundUpdate)":
                    case "SelectionChangeCommitted":
                    case "Navigating":
                    case "AfterSelect":
                    case "MouseUp":
                    case "ValueChanged":
                    case "GroupChanged":
                    case "NodeClicked":
                    case "BeforeControlEdit":
                    case "BrowseButtonClicked":
                    case "BrowseControlCleared":
                    case "ControlInfoClicked":
                    case "InitControlData":
                    case "ValueListRead":
                    case "ValueRead":
                    case "ValueUpdated":
                    case "AfterColEdit":
                    case "ColumnHeaderClick":
                    case "EndCustomEdit":
                    case "InitCustomEdit":
                    case "ListSelected":
                    case "ShowCustomEdit":
                    case "InsertAutoText":
                    case "Selecting":
                    case "Deselecting":
                    case "ObjectAddNew":
                    case "ObjectOpen":
                    case "ObjectProperties":
                    case "ObjectsDelete":
                    case "ObjectDelete":
                    case "ObjectCopy":
                    case "ChangeSupplier":
                    case "ObjectPrint":
                    case "ObjectPrintLabel":
                    case "ProductSelectionPrintLabel":
                    case "ObjectRelatedDocuments":
                    case "ObjectOrderSummary":
                    case "ObjectRefresh":
                    case "JanusExport":
                    case "JanusPreview":
                    case "JanusSummary":
                    case "ObjectReports":
                    case "NotImplemented":
                    case "ToolbarProcessHelp":
                    case "ChangePassword":
                    case "WebLinkClicked":
                    case "ExitApplication":
                    case "Logout":
                    case "SystemOptions":
                    case "MyOptions":
                    case "TestEpicorTaxConnect":
                    case "Messages":
                    case "MessageRules":
                    case "OpenLastMessage":
                    case "SendMessage":
                    case "OpenNewPerformanceWindow":
                    case "EditBudget":
                    case "ProductGroupEdit":
                    case "WebTrackProductGroupEdit":
                    case "FolderEdit":
                    case "ResetOptions":
                    case "ProductKitRestoreCalculatedPrices":
                    case "ProductKitFindReplace":
                    case "ProductKitUpdateBillOfMaterials":
                    case "NewOpportunity":
                    case "AddToNewCampaign":
                    case "UDOLinkedOptionClicked":
                    case "ObjectCustomerProperties":
                    case "AddNotepadNote":
                    case "AverageCostAdjustment":
                    case "CalculateMetricBoundaries":
                    case "CalculateMetricData":
                    case "Calendar":
                    case "CreateLetter":
                    case "CreUpdateRBA":
                    case "CreUploadBlankImage":
                    case "CreUploadIdleImage":
                    case "CRMDetail":
                    case "eBusinessStatistics":
                    case "KitStockEnquiry":
                    case "ObjectAddTobisTrack":
                    case "ObjectCustomerStatementBrowser":
                    case "ObjectSchedule":
                    case "GetShift4AccessToken":
                    case "ObjectSupplierProperties":
                    case "OpenNotepad":
                    case "SendEmail":
                    case "ProductKitBuilder":
                    case "ProductPriceHistory":
                    case "ProductsSpecificPrices":
                    case "ProductsPurchased":
                    case "StockAdjustment":
                    case "StockAllocations":
                    case "StockBatches":
                    case "StockPacks":
                    case "StockTransactions":
                    case "StockInformation":
                    case "MapEDIPer":
                    case "KeyDown":
                    case "TabIndexChanged":
                    case "LeftColChange":
                    case "Paint":
                    case "Scroll":
                    case "ContextMenuAction":
                    case "DisplayChanged":
                    case "DocumentComplete":
                    case "ShowContextMenu":
                    case "SelectionChanged":
                    case "MouseExit":
                    case "MouseMove":
                    case "PreviewKeyDown":
                    case "NodeMouseClick":
                    case "MouseDown":
                    case "GotFocus":
                    case "EditorRequired":
                    case "CellEditorInitialized":
                    case "CellFormatting":
                    case "Pain":
                    case "CreateCell":
                    case "RowFormatting":
                    case "ViewRowFormatting":
                    case "ViewCellFormatting":
                    case "CellBeginEdit":
                    case "CellEndEdit":
                    case "ValueChanging":
                    case "RowValidating":
                    case "RowValidated":
                    case "CellValidating":
                    case "CellValidated":
                    case "CurrentCellChanged":
                    case "CurrentRowChanging":
                    case "CurrentColumnChanged":
                    case "SelectionChanging":
                    case "GroupExpanded":
                    case "UserAddedRow":
                    case "UserDeletingRow":
                    case "UserDeletedRow":
                    case "RowsChanged":
                    case "RowsChanging":
                    case "DefaultValuesNeeded":
                    case "CellValueChanged":
                    case "ColumnWidthChanged":
                    case "ColumnWidthChanging":
                    case "GroupSummaryEvaluate":
                    case "ContextMenuOpening":
                    case "SortChanged":
                    case "CustomSorting":
                    case "GroupByChanged":
                    case "PrintCellPaint":
                    case "PrintCellFormatting":
                    case "ToolTipTextNeeded":
                    case "DragDrop":
                    case "DragOver":
                    case "FetchAttributes":
                    case "QueryCompleted":
                    case "Pivot":
                    case "AfterCollapse":
                    case "BeforeExpand":
                    case "AfterExpand":
                    case "DocumentAdd":
                    case "DocumentDragDrop":
                    case "DocumentOpen":
                    case "DocumentProperties":
                    case "DocumentRemove":
                    case "AfterUpdate":
                    case "Validating":
                    case "Validated":
                    case "NavigateComplete":
                    case "CaptionChanged":
                    case "DrillDownToCube":
                    case "DrillDownToNewWindow":
                    case "DrillDownToReport":
                    case "DrillDownToSmartView":
                    case "RunSQLScript":
                    case "StatusChange":
                    case "updMonthlyEvery":
                    case "AfterColMove":
                    case "RowDrag":
                    case "OLECompleteDrag":
                    case "OLEStartDrag":
                    case "RightClickOption2":
                    case "RightClickOptionSV":
                    case "RightClickOptionSV2":
                    case "UDORightClickOption":
                    case "Navigated":
                    case "DragOverNode":
                    case "NodeMouseHover":
                    case "NodeMouseDoubleClick":
                    case "OnDemandFetch":
                    case "DragEnter":
                    case "GetCurrentView":
                    case "SetView":
                    case "DataPointClicked":
                    case "CheckedChanged":
                    case "FilterIDChanged":
                    case "OptionClicked":
                    case "tvwViews":
                    case "PanelDblClick":
                    case "BeforeDropDown":
                    case "DropList":
                    case "ClientAreaChanged":
                    case "JourneyModified":
                    case "OLEDragStarted":
                    case "OLEDragCompleted":
                    case "ClassicDragStarted":
                    case "ClassicDragStopped":
                    case "AppointmentMoving":
                    case "AppointmentDropping":
                    case "AppointmentResizeStart":
                    case "AppointmentResizeEnd":
                    case "AppointmentChanged":
                    case "AppointmentElementMouseMove":
                    case "AppointmentElementMouseUp":
                    case "AppointmentElementDoubleClick":
                    case "CellElementMouseMove":
                    case "CellElementMouseUp":
                    case "ActiveViewChanged":
                    case "AppointmentFormatting":
                    case "CellPrintElementFormatting":
                    case "AppointmentPrintElementFormatting":
                    case "AppointmentEditDialogShowing":
                    case "RulerTextFormatting":
                    case "OLEDragOver":
                    case "BeforeColMove":
                    case "SplitterMoved":
                    case "DateClick":
                    case "VisibilityChanged":
                    case "DockChanged":
                    case "SmartViewDragStarted":
                    case "SmartViewDragStopped":
                    case "AutoAddDocument":
                    case "RedrawSelectedPlannerRow":
                    case "FindFromDocument":
                    case "AddProduct":
                    case "Collapsed":
                    case "Expanded":
                    case "ButtonClicked":
                    case "CellSelectionChanged":
                    case "CellElementDoubleClick":
                    case "ActiveViewChanging":
                    case "AppointmentResizing":
                    case "OverflowButtonClicked":
                    case "MouseWheel":
                    case "CellElementKeyDown":
                    case "BeforeControlUpdate":
                    case "CountUpdated":
                    case "EmailAttachments":
                    case "CellSelectionChanging":
                    case "AppointmentMouseUp":
                    case "AppointmentMouseDown":
                    case "AppointmentElementMouseDown":
                    case "CellElementMouseDown":
                    case "QueryContinueDrag":
                    case "NotInList":
                    case "ParentObjectChanged":
                    case "StockInformationButtonClicked":
                    case "AfterGraphRefresh":
                    case "TileWindows":
                    case "BeforeDeleteEX":
                    case "ItemDrag)":
                    case "SelectionMade":
                    case "ObjectDepositsAndPayments":
                    case "VehicleLocation":
                    case "DispatchQueue":
                    case "ViewCustomizeColumnsShow":
                    case "FollowUpQuote":
                    case "AcceptQuote":
                    case "RejectQuote":
                    case "EnableAddSelectionButton":
                    case "EnterSelectionTally":
                    case "Activated":
                    case "Load":
                    case "ItemDrag":
                    case "AfterPaginating":
                    case "ZoomChanged":
                    case "NodeExpandedChanged":
                    case "NodesNeeded":
                    case "NodeFormatting":
                    case "CloseEvent":
                    case "ConnectEvent":
                    case "DataArrival":
                    case "Error":
                    case "DataDeleted":
                    case "DocumentCompleted":
                    case "EditPermissions":
                    case "AddUser":
                    case "Remove":
                    case "Go":
                    case "Start":
                    case "Stop":
                    case "Continue":
                    case "Pause":
                        {
                            break;
                        }
                }

                typeScriptEventDataStringCase.Add("         case '" + eve + "':");
                typeScriptEventDataStringCase.Add("         eventDataString ='';");
                typeScriptEventDataStringCase.Add("         break;");

                //typeScriptMethods.Add(eve + "()");
                //typeScriptMethods.Add("{");
                //typeScriptMethods.AddRange(GetApiCallCode(eve));
                //typeScriptMethods.Add("}");
            }
            return result;
        }

        private static string GetRunTimeLabelText(UIControl control)
        {
            var labelTextCSline = AllCslines.Where(x => x.Contains(control.Name + ".Text =")).Select(y => y).ToList();
            if (labelTextCSline.Count() > 0)
            {
                return labelTextCSline[0].RemoveAll("\"").RemoveAll(";");
            }
            else if (control.Name.IndexOf('_') == 0)
            {
                var groupedControlName = control.Name.Split('_')[1];
                var labelTextGroupCsine = AllCslines.Where(x => x.Contains(groupedControlName + "[") && x.Contains("].Text =")).Select(y => y).ToList();
                if (labelTextGroupCsine.Count() > 0)
                {
                    var result = labelTextGroupCsine[0].Split('=')[1].RemoveAll("\"").RemoveAll(";");
                    return result;
                }
            }
            return "";
        }

        private static List<string> GetTypeBasedHtml(UIControl control, UIControl parentControl)
        {
            List<string> html = new List<string>();

            var eventHtml = GetEventHTML(control.Events);
            string disabledSyntax = "";
            string visibleSyntax = (control.IsVisibleModified ? " [hidden]=\"!uiModel." + control.Name + "Visible\"  " : "");
            string maxLengthSyntax = (control.IsMaxLengthModified ? " [maxLength]=\"!uiModel." + control.Name + "MaxLength\"  " : "");
            if (control.IsVisibleModified)
            {
                var visibleValue = control.Visible ? "true" : "false";

                //typeScriptVariables.Add(" " + control.Name + "Visible:boolean = " + visibleValue + ";");
                //typeScriptBindUI.Add("this." + control.Name + "Visible = resp." + control.Name + "Visible;");
            }
            if (control.IsMaxLengthModified)
            {
                var maxLenthValue = control.MaxLength + "";

                // typeScriptVariables.Add(" " + control.Name + "MaxLength:number = " + maxLenthValue + ";");
                // typeScriptBindUI.Add("this." + control.Name + "MaxLength = resp." + control.Name + "MaxLength;");
            }
            if (control.IsDisabledModified)
            {
                disabledSyntax = " [disabled]=\"uiModel." + control.Name + "Disabled\" ";

                //  typeScriptVariables.Add(" " + control.Name + "Disabled:boolean = false;");
                //typeScriptBindUI.Add("this." + control.Name + "Disabled = resp." + control.Name + "Disabled;");
            }
            var widthPercent = 100;

            if (FormWidth > 100)
                widthPercent = Convert.ToInt32((control.Width * 100) / FormWidth);
            string className = "col-3";
            if (widthPercent <= 10 && control.Type == "Label" && (string.IsNullOrEmpty(control.Text) || control.Text.Length <= 10))
                className = "col-1";
            else if (widthPercent <= 18 && (string.IsNullOrEmpty(control.Text) || control.Text.Length <= 10))
                className = "col-2";
            else if (widthPercent <= 28)
                className = "col-3";
            else if (widthPercent <= 33)
                className = "col-4";
            else if (widthPercent <= 45)
                className = "col-5";
            else if (widthPercent <= 60)
                className = "col-6";
            else
                className = "col-3";
            string logicBindings = visibleSyntax + maxLengthSyntax + disabledSyntax;
            string keyboardShortcut = "";
            if (control.HotKey != null)
                keyboardShortcut = " [ep_bt_hot_key]=\"" + control.HotKey + "\"";

            switch (control.Type)
            {
                case "Button":
                case "RadButton": // same as button

                    {
                        html.Add("<div class=\"" + className + "\">");

                        html.Add("<ep-button id=\"" + control.Name + "\"  [buttonStyle]=\"EpButtonStyle.Outline\"  [size]=\"EpButtonSize.Medium\" " + eventHtml + logicBindings + keyboardShortcut + ">");
                        if (!string.IsNullOrEmpty(control.Text))
                            html.Add(control.Text);
                        else if (!string.IsNullOrEmpty(control.ToolTip))
                            html.Add(control.ToolTip);
                        else
                            html.Add(control.Name);

                        html.Add("</ep-button>");
                        html.Add("</div>");

                        if (!typeScriptKieticImports.Contains("EpButtonStyle"))
                        {
                            typeScriptKieticImports.Add("EpButtonStyle");
                            typeScriptKieticImports.Add("EpButtonSize");
                            typeScriptVariables.Add(" EpButtonStyle = EpButtonStyle;");
                            typeScriptVariables.Add(" EpButtonSize = EpButtonSize;");
                        }

                        break;
                    }
                case "TextBox":
                case "RadTextBox": // same as textbox
                    {
                        // typeScriptVariables.Add(" " + control.Name + "Text:string = '';");
                        //typeScriptBindUI.Add("this." + control.Name + "Text = resp." + control.Name + "Text;");

                        html.Add("<div class=\"" + className + "\">");
                        html.Add("<ep-text-box id=\"" + control.Name + "\"  labelText=\"" + control.Text + "\" [(ngModel)]=\"uiModel." + control.Name + "Text\"" + eventHtml + logicBindings + keyboardShortcut + " > ");
                        html.Add("</ep-text-box>");
                        html.Add("</div>");

                        break;
                    }
                case "saSeparator":
                    {
                        html.Add("<hr/>");
                        break;
                    }
                case "BTGrid":
                    {
                        html.Add(" <ep-grid class=\"col-12\" *ngIf=\"" + control.Name + "Model\"  [model]=\"" + control.Name + "Model\" " + logicBindings + "></ep-grid>");
                        //typeScriptVariables.Add(" " + control.Name + "Model:IEpGridModel;");
                        //typeScriptMethods.AddRange(GenerateGridModelBinding(control.Name));
                        //typeScriptKieticImports.Add("IEpGridModel");

                        html.Add("<ep-bt-grid class=\"col-12\" [uiModel]='uiModel' [key]=\"'" + control.Name + "'\"></ep-bt-grid>");

                        break;
                    }
                case "Label":
                case "RadLabel": // same as label
                    {
                        if (string.IsNullOrEmpty(control.Text))
                        {
                            control.Text = GetRunTimeLabelText(control);
                        }
                        html.Add("<div class=\"" + className + "\">");

                        html.Add("<ep-label id=\"" + control.Name + "\" " + logicBindings + " >");
                        html.Add(control.Text);
                        html.Add("</ep-label>");
                        html.Add("</div>");

                        break;
                    }
                case "PictureBox":
                    {
                        if (control.Child.Count > 0)
                        {
                            html.Add("<div class=\"row\" " + logicBindings + ">");
                            html.Add("<div class=\"col\">");
                            control.Child = control.Child.OrderBy(x => x.Top).ThenBy(y => y.Left).ToList();
                            control.Child = MergeTextBoxandLabel(control.Child);
                            foreach (var child in control.Child)
                                html.AddRange(GetTypeBasedHtml(child, control));

                            html.Add("</div>");
                            html.Add("</div>");
                        }

                        break;
                    }
                case "ToolStripWrapper":
                    {
                        if (control.Type.IndexOf("ctl") == 0)
                        {
                            string formNameSpaced = SplitCamelCase(control.Type.Replace("ctl", ""));
                            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));

                            html.Add("<!-- <ctl-" + formNameFormated + " " + logicBindings + "></ctl-" + formNameFormated + ">  -->");
                        }
                        break;
                    }
                case "ComboBox":
                    {
                        html.Add("<div class=\"" + className + "\">");

                        html.Add("  <ep-bt-combo-dropdown id=\"" + control.Name + "\" [(uiModel)]=\"uiModel\" key=\"" + control.Name + "\" labelText=\"" + control.Text + "\" [isComboBox]=\"" + control.IsEditableDropdown + "\" " + keyboardShortcut + "></ep-bt-combo-dropdown>");
                        html.Add("</div>");

                        var sampleData = "{ id:1,text:'sample " + control.Name.Replace("cbo", "") + " 1'},{id:2,text:'sample " + control.Name.Replace("cbo", "") + " 2'}";

                        //typeScriptVariables.Add(" " + control.Name + "List:any=[" + sampleData + "];");
                        //typeScriptBindUI.Add("// this." + control.Name + "List = resp." + control.Name + "List;");

                        //typeScriptVariables.Add(" selected" + control.Name + " = 1;");

                        break;
                    }

                case "RadSplitContainer":
                    {
                        html.Add(" <ep-splitter orientation = \"vertical\"  >");
                        foreach (var child in control.Child)
                            html.AddRange(GetTypeBasedHtml(child, control));

                        html.Add(" </ep-splitter>");
                        break;
                    }
                case "SplitPanel":
                    {
                        html.Add("      <ep-splitter-pane [collapsible] = \"true\"  min = \"30%\" " + logicBindings + ">");
                        html.Add("      <div class=\"pane-content container-fluid\">");
                        foreach (var child in control.Child)
                            html.AddRange(GetTypeBasedHtml(child, control));
                        html.Add("      </div>");
                        html.Add("      </ep-splitter-pane>");
                        break;
                    }

                case "Panel":
                    {
                        html.Add("<div id=\"" + control.Name + "_Panel\" class=\"row\">");
                        foreach (var child in control.Child)
                            html.AddRange(GetTypeBasedHtml(child, control));
                        html.Add("</div>");
                        break;
                    }
                case "RadCheckBox":
                case "CheckBox":
                    {
                        html.Add("<div class=\"" + className + "\">");

                        html.Add("<ep-check-box id=\"" + control.Name + "\"  [(ngModel)]=\"uiModel." + control.Name + "Checked\" labelText=\"" + control.Text + "\" " + logicBindings + keyboardShortcut + "></ep-check-box>");
                        html.Add("      </div>");

                        //typeScriptVariables.Add(" " + control.Name + "Checked:boolean = false;");
                        //typeScriptBindUI.Add("//this." + control.Name + "Checked = resp." + control.Name + "Checked;");

                        break;
                    }
                case "RadioButton":
                case "RadRadioButton": // same as radio button
                    {
                        if (!string.IsNullOrEmpty(keyboardShortcut))
                        {
                            keyboardShortcut += "[ep_bt_hot_key_selectedText]=\"uiModel" + control.Name + "\"";
                        }

                        html.Add("<div class=\"" + className + "\">");

                        html.Add("<ep-radio-button  id=\"" + control.Name + "\" [data]=\"uiModel." + control.Name + "\" [verticalAlign]=\"true\" [(ngModel)]=\"uiModel." + control.Name + "Checked\" [disabled]=\"" + control.Name + "Disabled\" " + logicBindings + keyboardShortcut + "> </ep-radio-button>");
                        html.Add("      </div>");

                        //typeScriptVariables.Add(" " + control.Name + ":boolean = false;");
                        //typeScriptVariables.Add(" " + control.Name + "Checked:boolean = false;");

                        // typeScriptVariables.Add(" " + control.Name + "Disabled:boolean = false;");

                        break;
                    }
                case "RadTreeView":

                case "TreeView":
                    {
                        html.Add("<ep-tree-view id=\"" + control.Name + "\" [model]=\"" + control.Name + "Model\"" + logicBindings + " ></ep-tree-view>");
                        typeScriptVariables.Add(" " + control.Name + "Nodes:any =[];");
                        typeScriptVariables.Add(" " + control.Name + "Model:IEpTreeViewModel;");
                        typeScriptKieticImports.Add("IEpTreeViewModel");

                        break;
                    }

                case "TabControlExtender":
                    {
                        html.Add("<ep-tab-strip id=\"" + control.Name + "\" [height]=\"" + control.Height + "\" >");
                        if (control.Child != null)
                        {
                            foreach (var child in control.Child)
                                html.AddRange(GetTypeBasedHtml(child, control));
                        }

                        html.Add("</ep-tab-strip>");

                        break;
                    }
                case "TabPageHelper":
                    {
                        if (parentControl.Child[0] == control)
                            html.Add("<ep-tab-strip-tab  title=\"" + control.Text + "\" [selected]=\"true\" " + logicBindings + ">");
                        else

                            html.Add("<ep-tab-strip-tab  title=\"" + control.Text + "\" >");
                        html.Add("      <ng-template epTabContentDirective>");
                        html.Add(" <div class=\"container-fluid\"><div class=\"row\">");
                        control.Child = control.Child.OrderBy(x => x.TopRounded).ThenBy(y => y.Left).ToList();
                        control.Child = MergeTextBoxandLabel(control.Child);

                        foreach (var child in control.Child)
                            html.AddRange(GetTypeBasedHtml(child, control));
                        html.Add(" </div></div>");

                        html.Add("      </ng-template>");
                        html.Add("</ep-tab-strip-tab>");
                        break;
                    }
                case "saSelectButton":
                    {
                        string formNameSpaced = SplitCamelCase(control.Type.Replace("ctl", ""));
                        string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));
                        html.Add("<div class=\"" + className + "\">");

                        html.Add("<ep-bt-select-button [caption]= \"uiModel." + control.Name + "Caption\" [(value)]= \"uiModel." + control.Name + "txtSelectText\" [dropDownChoice]=\"uiModel." + control.Name + "DropDownChoice\" [dropDownChoices]=\"uiModel." + control.Name + "DropDownChoices\" [disabled]=\"uiModel." + control.Name + "Disabled\"></ep-bt-select-button>  <!-- Use (selectClick) for click & (keyPress) for enter key detect-->");
                        html.Add("</div>");
                        break;
                    }
                case "RadDateTimePicker":
                case "DateTimePicker":
                case "saDatePicker":
                    {
                        html.Add("<div class=\"" + className + "\">");
                        html.Add("<ep-bt-date-picker id=\"" + control.Name + "\" [uimodel]=\"uIModel\" [key]=\"'" + control.Name + "'\" [label]=\"" + control.Text + "\" " + keyboardShortcut + "></ep-bt-date-picker>");
                        html.Add("</div>");
                        break;
                    }
                case "ListBox":
                    {
                        html.Add("<div class=\"" + className + "\">");
                        html.Add("  <ep-bt-combo-dropdown id=\"" + control.Name + "\" [(uiModel)]=\"uiModel\" key=\"" + control.Name + "\" [isComboBox]=\"" + control.IsEditableDropdown + "\"></ep-bt-combo-dropdown>");
                        html.Add("      </div>");
                        typeScriptVariables.Add(" " + control.Name + "selectedIndex:any =[];");
                        typeScriptVariables.Add(" " + control.Name + "Model:IEpDropdownModel;");
                        typeScriptKieticImports.Add("IEpDropdownModel");
                        break;
                    }
                case "saBrowse":
                    {
                        if (control.Type.IndexOf("ctl") == 0)
                        {
                            string formNameSpaced = SplitCamelCase(control.Type.Replace("ctl", ""));
                            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));

                            html.Add("<!-- <ctl-" + formNameFormated + " " + logicBindings + "></ctl-" + formNameFormated + ">  -->");
                        }
                        break;
                    }
                case "ProgressBar":
                    {
                        html.Add("<ep-bt-progress-bar [id] = \"'" + control.Name + "'\" [maximum]= \"uIModel." + control.Name + "Maximum\" [minimum] = \"uIModel." + control.Name + "Minimum\" [value] = \"uIModel." + control.Name + "Value\"></ep-bt-progress-bar>");
                        break;
                    }
                case "StatusStrip": //Similar to header/footer
                    {
                        html.Add("<ep-bt-status-strip [items]= \"uIModel." + control.Name + ".Items\"></ep-bt-status-strip>");
                        html.Add("<!-- Add width like this [width] = [70,30] in case of two items in your status strip -->");
                        break;
                    }
                case "RichTextBox":
                case "RadRichTextEditor":
                    {
                        html.Add("<div class=\"" + className + "\">");

                        html.Add("<ep-rich-text-editor id=\"" + control.Name + "\"  [model]=\"" + control.Name + "Model\" labelText=\"" + control.Text + "\" " + logicBindings + "></ep-rich-text-editor>");
                        html.Add("      </div>");
                        typeScriptVariables.Add(" " + control.Name + "Model:IEpRichTextEditorModel = { /*to do*/ };");
                        typeScriptKieticImports.Add("IEpRichTextEditorModel");
                        break;
                    }
                case "WebBrowser":
                    {
                        if (control.Type.IndexOf("ctl") == 0)
                        {
                            string formNameSpaced = SplitCamelCase(control.Type.Replace("ctl", ""));
                            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));

                            html.Add("<!-- <ctl-" + formNameFormated + " " + logicBindings + "></ctl-" + formNameFormated + ">  -->");
                        }
                        break;
                    }
                case "ListView": //Grid type
                    {
                        html.Add("<div class=\"" + className + "\">");
                        html.Add("      <ep-bt-ctl-list-view [(uiModel)]=\"uIModel\" [key]=\"'" + control.Name + "'\"></ep-bt-ctl-list-view>");
                        html.Add("</div>");
                        break;
                    }
                case "saViewBar": //Need to look into this once
                    {
                        if (control.Type.IndexOf("ctl") == 0)
                        {
                            string formNameSpaced = SplitCamelCase(control.Type.Replace("ctl", ""));
                            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));

                            html.Add("<!-- <ctl-" + formNameFormated + " " + logicBindings + "></ctl-" + formNameFormated + ">  -->");
                        }
                        break;
                    }
                case "MenuStrip": //dropdown
                    {
                        html.Add("<div class=\"" + className + "\">");
                        html.Add("<ep-dropdown-button id=\"" + control.Name + "\"  [model]=\"" + control.Name + "Model\" labelText=\"" + control.Text + "\" " + logicBindings + "></ep-dropdown-button>"); //to add: [spinners]="true"
                        html.Add("      </div>");
                        typeScriptVariables.Add(" " + control.Name + "Model:IEpDropdownButtonModel;");
                        typeScriptKieticImports.Add("IEpDropdownButtonModel");

                        break;
                    }
                case "TabControl": //tab-strip
                    {
                        html.Add("<ep-tab-strip id=\"" + control.Name + "\" [height]=\"" + control.Height + "\" >");
                        if (control.Child != null)
                        {
                            foreach (var child in control.Child)
                                html.AddRange(GetTypeBasedHtml(child, control));
                        }

                        html.Add("</ep-tab-strip>");

                        break;
                    }
                case "RadSpinEditor":
                    {
                        html.Add("<div class=\"" + className + "\">");

                        html.Add("<ep-numeric-box id=\"" + control.Name + "\"  [(ngModel)]=\"" + control.Name + "Model\" labelText=\"" + control.Text + "\" " + logicBindings + "></ep-numeric-box>"); //to add: [spinners]="true"
                        html.Add("      </div>");

                        //typeScriptVariables.Add(" " + control.Name + "Model:IEpRichTextEditorModel = { //to do };");
                        break;
                    }
                case "GroupBox": // groups control
                case "RadGroupBox": //similar to group box
                    {
                        if (control.Child.Count > 0)
                        {
                            html.Add("<div class=\"row\" " + logicBindings + ">");
                            html.Add("<div class=\"col\">");
                            control.Child = control.Child.OrderBy(x => x.Top).ThenBy(y => y.Left).ToList();
                            control.Child = MergeTextBoxandLabel(control.Child);
                            foreach (var child in control.Child)
                                html.AddRange(GetTypeBasedHtml(child, control));

                            html.Add("</div>");
                            html.Add("</div>");
                        }

                        //typeScriptVariables.Add(" " + control.Name + "Model:any = { //to do };");
                        break;
                    }
                case "AxDHTMLEdit": // no such control
                case "ViewBarButton": // button for views in DT
                case "PictureBoxExtended": // to display picture
                    {
                        if (control.Type.IndexOf("ctl") == 0)
                        {
                            string formNameSpaced = SplitCamelCase(control.Type.Replace("ctl", ""));
                            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));

                            html.Add("<!-- <ctl-" + formNameFormated + " " + logicBindings + "></ctl-" + formNameFormated + ">  -->");
                        }
                        break;
                    }
                case "BTGridView": // same as rad grid view (customized)
                    {
                        if (control.Type.IndexOf("ctl") == 0)
                        {
                            string formNameSpaced = SplitCamelCase(control.Type.Replace("ctl", ""));
                            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));

                            html.Add("<!-- <ctl-" + formNameFormated + " " + logicBindings + "></ctl-" + formNameFormated + ">  -->");
                        }
                        break;
                    }
                case "RadStatusStrip":
                    {
                        html.Add("<div class=\"" + className + "\">");

                        html.Add("<!-- To Do -->"); //Status related information"
                        html.Add("      </div>");
                        break;
                    }
                case "NumericUpDown": // same as RadSpinEditor
                    {
                        html.Add("<div class=\"" + className + "\">");

                        html.Add("<ep-numeric-box id=\"" + control.Name + "\"  [(ngModel)]=\"" + control.Name + "Model\" labelText=\"" + control.Text + "\" " + logicBindings + "></ep-numeric-box>"); //to add: [spinners]="true"
                        html.Add("      </div>");

                        //typeScriptVariables.Add(" " + control.Name + "Model:IEpRichTextEditorModel = { //to do };");
                        break;
                    }

                case "saRichEdit": // similar to RichTextBox
                    {
                        html.Add("<div class=\"" + className + "\">");

                        html.Add("<ep-text-area id=\"" + control.Name + "\" labelText=\"" + control.Text + "\" " + logicBindings + "></ep-text-area>"); //to add maxlength & rows
                        html.Add("      </div>");
                        break;
                    }
                case "saListViewSelector": // check box list with text
                    {
                        if (control.Type.IndexOf("ctl") == 0)
                        {
                            string formNameSpaced = SplitCamelCase(control.Type.Replace("ctl", ""));
                            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));

                            html.Add("<!-- <ctl-" + formNameFormated + " " + logicBindings + "></ctl-" + formNameFormated + ">  -->");
                        }
                        break;
                    }
                case "RadScheduler": //EpScheduler Component
                    {
                        html.Add("<div class=\"" + className + "\">");

                        html.Add("<ep-scheduler id=\"" + control.Name + "\" [model]=\"" + control.Name + "Model\" labelText=\"" + control.Text + "\" " + logicBindings + "></ep-scheduler>");
                        html.Add("      </div>");
                        typeScriptVariables.Add(" " + control.Name + "Model:IEpSchedulerModel = { /*to do*/ };");
                        typeScriptKieticImports.Add("IEpSchedulerModel");
                        break;
                    }
                case "CheckedListBox": //check box with text
                    {
                        html.Add("<div class=\"" + className + "\">");

                        html.Add("<ep-selection-list id=\"" + control.Name + "\" [(ngModel)]=\"" + control.Name + "Model\" labelText=\"" + control.Text + "\" " + logicBindings + "></ep-selection-list>");
                        html.Add("      </div>");

                        //typeScriptVariables.Add(" " + control.Name + "Model:IEpSchedulerModel = { //to do };");
                        break;
                    }
                case "SplitContainer": //Same as Rad Split Container
                    {
                        html.Add(" <ep-splitter orientation = \"vertical\"  >");
                        foreach (var child in control.Child)
                            html.AddRange(GetTypeBasedHtml(child, control));

                        html.Add(" </ep-splitter>");
                        break;
                    }
                case "RadLayoutControl": //similar to other layout control
                case "TableLayoutPanel": //may be forms or (may be flex box)
                case "EtchedLine": //line
                    {
                        if (control.Type.IndexOf("ctl") == 0)
                        {
                            string formNameSpaced = SplitCamelCase(control.Type.Replace("ctl", ""));
                            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));

                            html.Add("<!-- <ctl-" + formNameFormated + " " + logicBindings + "></ctl-" + formNameFormated + ">  -->");
                        }
                        break;
                    }
                case "RadDropDownButton": // same as dropdown button
                    {
                        html.Add("<div class=\"" + className + "\">");
                        html.Add("<ep-dropdown-button id=\"" + control.Name + "\"  [model]=\"" + control.Name + "Model\" labelText=\"" + control.Text + "\" " + logicBindings + "></ep-dropdown-button>"); //to add: [spinners]="true"
                        html.Add("      </div>");
                        typeScriptVariables.Add(" " + control.Name + "Model:IEpDropdownButtonModel;");
                        typeScriptKieticImports.Add("IEpDropdownButtonModel");
                        break;
                    }
                case "RadPageView": //similar to dialog
                    {
                        typeScriptBindUI.Add("//EpDialogService.custom({title: '',message: '',description: '', //component: CustomDialogComponent});");
                        break;
                    }
                case "RadMenu": // similar to dropownButton
                case "RadPropertyGrid": // could be made similar to normal grid
                    {
                        if (control.Type.IndexOf("ctl") == 0)
                        {
                            string formNameSpaced = SplitCamelCase(control.Type.Replace("ctl", ""));
                            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));

                            html.Add("<!-- <ctl-" + formNameFormated + " " + logicBindings + "></ctl-" + formNameFormated + ">  -->");
                        }
                        break;
                    }
                case "RadListControl": //similar to list control
                    {
                        html.Add("<div class=\"" + className + "\">");
                        html.Add("<ep-selection-list  id=\"" + control.Name + "\"  [model]=\"" + control.Name + "Model\" labelText=\"" + control.Text + "\" " + logicBindings + "></ep-selection-list >");
                        html.Add("      </div>");
                        typeScriptVariables.Add(" " + control.Name + "Model:IEpSelectionListModel ;");
                        break;
                    }
                case "RadGridView": // similar to grid view
                    {
                        html.Add(" <ep-grid class=\"col-12\" *ngIf=\"" + control.Name + "Model\"  [model]=\"" + control.Name + "Model\" " + logicBindings + "></ep-grid>");
                        typeScriptVariables.Add(" " + control.Name + "Model:IEpGridModel;");
                        typeScriptMethods.AddRange(GenerateGridModelBinding(control.Name));
                        typeScriptKieticImports.Add("IEpGridModel");

                        break;
                    }
                case "WebView":
                    {
                        if (control.Type.IndexOf("ctl") == 0)
                        {
                            string formNameSpaced = SplitCamelCase(control.Type.Replace("ctl", ""));
                            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));

                            html.Add("<!-- <ctl-" + formNameFormated + " " + logicBindings + "></ctl-" + formNameFormated + ">  -->");
                        }
                        break;
                    }
                case "RadTextBoxControl":// similar to selection list
                    {
                        html.Add("<div class=\"" + className + "\">");
                        html.Add("<ep-selection-list  id=\"" + control.Name + "\"  [model]=\"" + control.Name + "Model\" labelText=\"" + control.Text + "\" " + logicBindings + "></ep-selection-list >");
                        html.Add("      </div>");
                        typeScriptVariables.Add(" " + control.Name + "Model:IEpSelectionListModel ;");
                        typeScriptKieticImports.Add("IEpSelectionListModel");
                        break;
                    }
                case "RadBrowseEditor": //similar t select file dialog
                case "RadDropDownList":// similar to dropdown list
                    {
                        html.Add("<div class=\"" + className + "\">");
                        html.Add("  <ep-bt-combo-dropdown id=\"" + control.Name + "\" [(uiModel)]=\"uiModel\" key=\"" + control.Name + "\" [isComboBox]=\"" + control.IsEditableDropdown + "\" " + keyboardShortcut + "></ep-bt-combo-dropdown>");
                        html.Add("</div>");
                        typeScriptVariables.Add(" " + control.Name + "List:any=[];");

                        break;
                    }
                case "RadWizard": //similar to Quick order wizard
                    {
                        if (control.Type.IndexOf("ctl") == 0)
                        {
                            string formNameSpaced = SplitCamelCase(control.Type.Replace("ctl", ""));
                            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));

                            html.Add("<!-- <ctl-" + formNameFormated + " " + logicBindings + "></ctl-" + formNameFormated + ">  -->");
                        }
                        break;
                    }
                case "DropDownControl"://dropdown
                    {
                        html.Add("<div class=\"" + className + "\">");
                        html.Add("  <ep-bt-combo-dropdown [(uiModel)]=\"uiModel\" key=\"" + control.Name + "\" [isComboBox]=\"" + control.IsEditableDropdown + "\"  " + keyboardShortcut + "></ep-bt-combo-dropdown>");
                        html.Add("</div>");
                        typeScriptVariables.Add(" " + control.Name + "List:any=[];");

                        break;
                    }
                case "RadSeparator": //<hr>
                    {
                        html.Add("<hr>");
                        break;
                    }
                case "RadRibbonBar": //contains tabs similar to tab strip
                case "ReportViewer": // custom control
                    {
                        if (control.Type.IndexOf("ctl") == 0)
                        {
                            string formNameSpaced = SplitCamelCase(control.Type.Replace("ctl", ""));
                            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));

                            html.Add("<!-- <ctl-" + formNameFormated + " " + logicBindings + "></ctl-" + formNameFormated + ">  -->");
                        }
                        break;
                    }
                case "ToolStrip":  //dropdown or context menus
                    {
                        html.Add("<div class=\"" + className + "\" #target>");
                        html.Add("<ep-context-menu id=\"" + control.Name + "\" [target] = \"target\"  [model]=\"" + control.Name + "Model\" labelText=\"" + control.Text + "\" " + logicBindings + "></ep-context-menu>");
                        html.Add("      </div>");
                        typeScriptVariables.Add(" " + control.Name + "Model:IEpContextMenuModel ;"); //showOn click
                        typeScriptKieticImports.Add("IEpContextMenuModel");
                        break;
                    }
                case "RadPanel": //similar to tree view
                    {
                        if (control.Type.IndexOf("ctl") == 0)
                        {
                            string formNameSpaced = SplitCamelCase(control.Type.Replace("ctl", ""));
                            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));

                            html.Add("<!-- <ctl-" + formNameFormated + " " + logicBindings + "></ctl-" + formNameFormated + ">  -->");
                        }
                        break;
                    }
                /*case "RadRichTextEditor":
                    {
                        html.Add("<!-- <not-handled-type-" + control.Type + "></not-handled-type-" + control.Type + " >  -->");

                        break;
                    }*/
                default:
                    {
                        if (control.Type.IndexOf("ctl") == 0)
                        {
                            string formNameSpaced = SplitCamelCase(control.Type.Replace("ctl", ""));
                            string formNameFormated = string.Join("-", formNameSpaced.Split(' ').Select(x => x.ToLower()));

                            html.Add("<!-- <ctl-" + formNameFormated + " " + logicBindings + "></ctl-" + formNameFormated + ">  -->");
                        }
                        else
                        {
                        }

                        break;
                    }
            }
            return html;
        }

        private static List<UIControl> MergeTextBoxandLabel(List<UIControl> controls)

        {
            List<UIControl> removableLabel = new List<UIControl>();
            for (int i = 0; i < controls.Count(); i++)
            {
                if (controls[i].Type == "TextBox")
                {
                    var matchedLabel = controls.Where(x => x.Type == "Label" && x.Text != null && x.Text.Replace(":", "") == controls[i].Name.Replace("txt", "")).Select(c => c).ToList();

                    if (matchedLabel.Count() > 0)
                    {
                        controls[i].Text = matchedLabel[0].Text.Replace(":", "");
                        removableLabel.Add(matchedLabel[0]);
                    }
                    else
                    {
                        var matchLabel = controls.Where(x => x.Name == controls[i].Name.Replace("txt", "lbl")).ToList();

                        if (matchLabel.Count() > 0)
                        {
                            controls[i].Text = matchLabel[0].Text;
                            removableLabel.Add(matchLabel[0]);
                        }
                    }
                }
                else if (controls[i].Type == "ComboBox")
                {
                    var matchedLabel = controls.Where(x => x.Type == "Label" && x.Text != null && x.Text.Replace(":", "").ToLower().Replace(" ", "").Replace("&", "") == controls[i].Name.Replace("cbo", "").ToLower()).Select(c => c).ToList();

                    if (matchedLabel.Count() > 0)
                    {
                        controls[i].Text = matchedLabel[0].Text.Replace(":", "");
                        if (matchedLabel[0].HotKey != null)
                            controls[i].HotKey = matchedLabel[0].HotKey;
                        removableLabel.Add(matchedLabel[0]);
                    }
                    else
                    {
                        var matchLabel = controls.Where(x => x.Name == controls[i].Name.Replace("cbo", "lbl")).ToList();

                        if (matchLabel.Count() > 0)
                        {
                            controls[i].Text = matchLabel[0].Text;
                            removableLabel.Add(matchLabel[0]);
                        }
                    }
                }
                else if (controls[i].Type == "saDatePicker")
                {
                    var matchedLabel = controls.Where(x => x.Type == "Label" && x.Text != null && x.Text.Replace(":", "").ToLower().Replace(" ", "").Replace("&", "") == controls[i].Name.Replace("dtp", "").ToLower()).Select(c => c).ToList();

                    if (matchedLabel.Count() > 0)
                    {
                        controls[i].Text = matchedLabel[0].Text.Replace(":", "");
                        if (matchedLabel[0].HotKey != null)
                            controls[i].HotKey = matchedLabel[0].HotKey;
                        removableLabel.Add(matchedLabel[0]);
                    }
                    else
                    {
                        var matchLabel = controls.Where(x => x.Name == controls[i].Name.Replace("dtp", "lbl")).ToList();

                        if (matchLabel.Count() > 0)
                        {
                            controls[i].Text = matchLabel[0].Text;
                            removableLabel.Add(matchLabel[0]);
                        }
                    }
                }
                else if (controls[i].Type == "CheckBox")
                {
                    var matchedLabel = controls.Where(x => x.Type == "Label" && x.Text != null && x.Text.Replace(":", "").ToLower().Replace(" ", "").Replace("&", "") == controls[i].Name.Replace("chk", "").ToLower()).Select(c => c).ToList();

                    if (matchedLabel.Count() > 0)
                    {
                        controls[i].Text = matchedLabel[0].Text.Replace(":", "");
                        if (matchedLabel[0].HotKey != null)
                            controls[i].HotKey = matchedLabel[0].HotKey;
                        removableLabel.Add(matchedLabel[0]);
                    }
                    else
                    {
                        var matchLabel = controls.Where(x => x.Name == controls[i].Name.Replace("chk", "lbl")).ToList();

                        if (matchLabel.Count() > 0)
                        {
                            controls[i].Text = matchLabel[0].Text;
                            removableLabel.Add(matchLabel[0]);
                        }
                    }
                }
            }
            foreach (var item in removableLabel)
            {
                controls.Remove(item);
            }
            return controls;
        }

        private static string SplitCamelCase(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }
        public void Execute(string formName, List<UIControl> controls)
        {
            GenerateJson(formName, controls);
        }
    }
}
