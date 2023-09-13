using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SAPBOAnalysis.Models;
using SAPBOAnalysis.Models.DocumentDetails;
using SAPBOAnalysis.Models.DocumentVariables;
using SAPBOAnalysis.Models.QueryHelper;
using SAPBOAnalysis.Models.ReportDetails;
using SAPBOAnalysis.Models.ReportElement;
using SAPBOAnalysis.Models.Universe;
using SAPBOAnalysis.Models.UniverseDetails;

namespace SAPBOAnalysis
{
    public class ReportAnalyzer
    {
        HttpHelper httpHelper = new HttpHelper();
        TokenHelper tokenHelper = new TokenHelper();
        int apiLimit = ConfigHelper.apiLimit;

        string token = string.Empty;


        private Configuration config;
        public ReportAnalyzer()
        {
            config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("appconfig.json"))!;
        }

        public async Task<Dictionary<string, List<object>>> Analyze(bool analyzeReports)
        {
            Logger.Log("==========================================================");
            Logger.Log($"REPORT ANALYSIS");
            Logger.Log("==========================================================");

            token = await tokenHelper.GetToken();

            var analysisReport = new DocumentAnalysisReport();

            //Get Documents

            var offset = 0;
            var resultCount = 0;

            do
            {
                string url = $"{config.sapConnection.server}/raylight/v1/documents?offset={offset}&limit={apiLimit}";
                Logger.Log($"Getting Docuements...");
                var documentsResp = await httpHelper.ExecuteGet<DocumentRoot>("application/json", token, url);
                resultCount = documentsResp.documents.document.Count;
                Logger.Log($"{resultCount} documents to process");

                foreach (var item in documentsResp.documents.document)
                {
                    var analysisDoc = new DocumentAnalysis();
                    var analysisRep = new List<ReportAnalysis>();
                    var analysisRepElem = new List<ReportElementAnalysis>();


                    Logger.Log($"====> Processing Document [{item.id}] [{item.name}]");

                    // Get Document Details
                    Logger.Log($"--> Getting Docuement Details");
                    var docDetailsUrl = $"{config.sapConnection.server}/raylight/v1/documents/{item.id}";
                    var docDetailsResp = await httpHelper.ExecuteGet<DocumentDetailRoot>("application/json", token, docDetailsUrl);
                    var docDetails = docDetailsResp.document;

                    // *** Analysis Update -- START *** //
                    analysisDoc.DocId = docDetails.id;
                    analysisDoc.DocName = docDetails.name;
                    analysisDoc.CuID = docDetails.cuid;
                    analysisDoc.Size = docDetails.size;
                    analysisDoc.IsScheduled = bool.Parse(docDetails.scheduled);
                    analysisDoc.ModifiedDate = docDetails.updated;
                    analysisDoc.FolderId = docDetails.folderId;
                    analysisDoc.DocState = docDetails.state;
                    // *** Analysis Update -- END *** //

                    // Get Document Properties
                    Logger.Log($"--> Getting Docuement Properties");
                    var docPropertiesUrl = $"{config.sapConnection.server}/raylight/v1/documents/{item.id}/properties";
                    var docPropertiesResp = await httpHelper.ExecuteGet<DocumentPropertiesRoot>("application/json", token, docPropertiesUrl);
                    var docProps = docPropertiesResp.properties.PropertiesDict;

                    // *** Analysis Update -- START *** //
                    analysisDoc.ReadingDirection = docProps["readingdirection"];
                    analysisDoc.DocType = docProps["documenttype"];
                    analysisDoc.CreatedDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(docProps["creationtime"]));
                    analysisDoc.ModifiedDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(docProps["modificationtime"]));
                    analysisDoc.LastRunTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(docProps["lastrefreshtime"]));
                    analysisDoc.IsDeactivated = bool.Parse(docProps["tdcactivated"]);
                    analysisDoc.IsInstance = bool.Parse(docProps["isinstance"]);
                    // *** Analysis Update -- END *** //

                    var dpProps = GetDpProps(docProps["dpprops"]);

                    analysisDoc.DataProviderCount = dpProps.Count;

                    Console.Write($"--> Getting DataProviders");
                    var docDataProvidersUrl = $"{config.sapConnection.server}/raylight/v1/documents/{item.id}/dataproviders";
                    var docDataProvidersResp = await httpHelper.ExecuteGet<DataProviderRoot>("application/json", token, docDataProvidersUrl);
                    var dataProviders = docDataProvidersResp.dataproviders.dataprovider;
                    Console.Write($"...{dataProviders.Count} DataProviders to process");

                    foreach (var dp in dataProviders)
                    {
                        // *** Analysis Update -- START *** //
                        analysisDoc.DataProviderDetails += $"{dp.id};";
                        // *** Analysis Update -- END *** //

                        if (dp.dataSourceType.Equals("unv") || dp.dataSourceType.Equals("unx"))
                        {
                            // *** Analysis Update -- START *** //
                            analysisDoc.UniverseCount += 1;
                            analysisDoc.UniverseDetails += $"{dp.dataSourceId};";
                            // *** Analysis Update -- END *** //
                        }

                        await AnalyzeDataProviders(item, dp, analysisReport);

                        await AnalyzeDataProviderQueryPlans(item, dp, analysisReport);

                    }
                    // Get Variables
                    Logger.Log($"--> Getting Document Variables");
                    await AnalyzeDocumentVariables(item, analysisReport);

                    // Get Reports
                    if (analyzeReports)
                    {
                        Logger.Log($"--> Getting Reports");
                        var reportUrl = $"{config.sapConnection.server}/raylight/v1/documents/{item.id}/reports";
                        var reportsResp = await httpHelper.ExecuteGet<ReportsRoot>("application/json", token, reportUrl);
                        var reports = reportsResp.reports.report;

                        // *** Analysis Update -- START *** //
                        analysisDoc.ReportCount = reports.Count;
                        // *** Analysis Update -- END *** //

                        foreach (var report in reports)
                        {
                            var reportAnalysis = new ReportAnalysis();
                            reportAnalysis.DocId = item.id;
                            reportAnalysis.DocName = item.name;

                            Logger.Log($"----> Analyzing Report [{report.id}] [{report.name}]");

                            var reportDetailUrl = $"{config.sapConnection.server}/raylight/v1/documents/{item.id}/reports/{report.id}";
                            var reportDetailsResp = await httpHelper.ExecuteGet<ReportDetailsRoot>("application/json", token, reportDetailUrl);
                            var reportDetails = reportDetailsResp.report;

                            var reportElementsUrl = $"{config.sapConnection.server}/raylight/v1/documents/{item.id}/reports/{report.id}/elements";
                            var reportElementsResp = await httpHelper.ExecuteGet<ReportElementsRoot>("application/json", token, reportElementsUrl);
                            var reportElements = reportElementsResp.elements?.element;

                            if (reportElements != null)
                            {
                                reportAnalysis.ElementCount = reportElements.Count;
                                reportAnalysis.TableCount = reportElements.Count(x => x.type.Equals("vTable", StringComparison.InvariantCultureIgnoreCase));
                                reportAnalysis.CellCount = reportElements.Count(x => x.type.Equals("cell", StringComparison.InvariantCultureIgnoreCase));
                            }
                            reportAnalysis.ReportId = reportDetails.id.ToString();
                            reportAnalysis.ReportName = reportDetails.name;

                            analysisReport.AnalyzedReports.Add(reportAnalysis);


                            await AnalyzeReportElements(item, report, reportElements, analysisReport);
                        }
                    }
                    analysisReport.AnalyzedDocuments.Add(analysisDoc);
                }

                offset += apiLimit;
            } while (resultCount == apiLimit);

            var excelWorkSheets = new Dictionary<string, List<object>>();
            excelWorkSheets.Add("DocumentAnalysis", analysisReport.AnalyzedDocuments);
            if (analyzeReports)
            {
                excelWorkSheets.Add("ReportAnalysis", analysisReport.AnalyzedReports);
                excelWorkSheets.Add("ReportElementAnalysis", analysisReport.AnalyzedReportElements);
            }
            excelWorkSheets.Add("DataProviderExpressionAnalysis", analysisReport.AnalyzedDataProviderExpressions);
            excelWorkSheets.Add("DataProviderQueryPlanAnalysis", analysisReport.AnalyzedDataProviderQueryPlan);
            excelWorkSheets.Add("DocumentVariables", analysisReport.DocumentVariablesAnalysis);
            return excelWorkSheets;
        }

        public async Task<DocumentDataSchema> GetDataSchemaForDocument(string documentId)
        {
            var result = new DocumentDataSchema();
            result.DocId = documentId;
            token = await tokenHelper.GetToken();

            var docUrl = $"{config.sapConnection.server}/raylight/v1/documents/{documentId}";
            var docResp = await httpHelper.ExecuteGet<DocumentDetailRoot>("application/json", token, docUrl);
            result.Name = docResp.document.name;

            var docDataProvidersUrl = $"{config.sapConnection.server}/raylight/v1/documents/{documentId}/dataproviders";
            var docDataProvidersResp = await httpHelper.ExecuteGet<DataProviderRoot>("application/json", token, docDataProvidersUrl);
            var dataProviders = docDataProvidersResp.dataproviders.dataprovider;

            foreach (var dp in dataProviders)
            {
                var ddp = new DocumentDataProvider();
                ddp.DataProviderId = dp.id;
                ddp.DataProviderName = dp.name;
                ddp.DataSourceId = dp.dataSourceId;
                ddp.DataSourceType = dp.dataSourceType;

                var docDataProviderQueryPlanUrl = $"{config.sapConnection.server}/raylight/v1/documents/{documentId}/dataproviders/{dp.id}/queryplan";
                var docDataProvidResperQueryPlanResp = await httpHelper.ExecuteGet<dynamic>("application/json", token, docDataProviderQueryPlanUrl);
                var docDataProvidResperQueryPlan = JsonConvert.SerializeObject(docDataProvidResperQueryPlanResp);

                if (!string.IsNullOrEmpty(config.genai?.completionApi))
                {
                    var prompt = @"You can find all SQL queries in given input and analyze SQL queries and give a list of tables, columns, filters and relationships used in each query in the following json format:
                                    [
	                                    {
		                                    'queryId': '',
		                                    'tableInfo': [{
			                                    'tableName': '',
			                                    'columns': ['','',''],
                                                            'joins':[{
                                                                    'fields':['',''], 
                                                                    'joinType':'', 
                                                                    'filters':['','','']
                                                            }],
                                                            'filters': [{
                                                                    'field':'', 
                                                                    'operator':'', 
                                                                    'operands':['','','']
                                                             }]
		                                    }]
	                                    }
                                    ]
                                    Try to break the filters into field, operator and operands\n\n" + docDataProvidResperQueryPlan + "\n";
                    var gptResponse = await GenAIHelper.GetCompletionResponse(prompt, config.genai?.completionApi);
                    var gptObject = JsonConvert.DeserializeObject<List<QueryDetails>>(gptResponse);

                    ddp.queryList = gptObject;

                    result.documentDataProviders.Add(ddp);
                }
            }

            return result;
        }


       public async Task<DocumentData> GetDocumentData(string documentId)
        {
            var result = new DocumentData();
            result.DocId = documentId;
            result.ReportDataList = new List<ReportData>();
            token = await tokenHelper.GetToken();
            var reportUrl = $"{config.sapConnection.server}/raylight/v1/documents/{documentId}/reports";
            var reportsResp = await httpHelper.ExecuteGet<ReportsRoot>("application/json", token, reportUrl);
            var reports = reportsResp.reports.report;
            foreach (var report in reports)
            {
                var reportData = new ReportData();
                var reportDetailUrl = $"{config.sapConnection.server}/raylight/v1/documents/{documentId}/reports/{report.id}";
                var reportDetailsResp = await httpHelper.ExecuteGet("text/csv", token, reportDetailUrl);
                reportData.CSVData = reportDetailsResp;
                reportData.ReportName = report.name;
                reportData.ReportId= report.id.ToString();
                result.ReportDataList.Add(reportData);
            }
            return result;
        }

        private async Task<DataProviderDetailsRoot> AnalyzeDataProviders(Models.Document item, Dataprovider dataProvider, DocumentAnalysisReport analysisReport)
        {
            // Collect Additional DataProvider Details
            Logger.Log($"----> Analyzing DataProvider [{dataProvider.id}] [{dataProvider.name}]");
            var docDataProviderDetailsUrl = $"{config.sapConnection.server}/raylight/v1/documents/{item.id}/dataproviders/{dataProvider.id}";
            var docDataProviderDetailsResp = await httpHelper.ExecuteGet<DataProviderDetailsRoot>("application/json", token, docDataProviderDetailsUrl);
            var dataProviderDetails = docDataProviderDetailsResp.dataprovider;
            Logger.Log($"----> {dataProviderDetails.dictionary.expression.Count} to process");


            foreach (var exp in dataProviderDetails.dictionary.expression)
            {
                var analysisObj = new DataProviderExpressionAnalysis();

                analysisObj.DocId = item.id;
                analysisObj.DocName = item.name;
                analysisObj.DpId = exp.dataProviderId;
                analysisObj.DpName = exp.dataProviderName;
                analysisObj.Updated = dataProviderDetails.updated;
                analysisObj.DsId = exp.dataSourceId;
                analysisObj.DsName = exp.dataSourceName;
                analysisObj.DsPath = String.Join(';', exp.dataSourcePath.value);
                analysisObj.DsCuid = dataProviderDetails.dataSourceCuid;
                analysisObj.DsType = dataProviderDetails.dataSourceType;
                analysisObj.DsPrefix = dataProviderDetails.dataSourcePrefix;
                analysisObj.DpExpAggregationFunction = exp.aggregationFunction;
                analysisObj.DpExpAllowUserValues = exp.allowUserValues;
                analysisObj.DpExpAssociatedDimensionId = exp.associatedDimensionId;
                analysisObj.DpExpCustomSort = exp.customSort;
                analysisObj.DpExpName = exp.name;
                analysisObj.DpExpId = exp.id;
                analysisObj.DpExpNatureId = exp.natureId;
                analysisObj.DpExpDataSourceEnriched = exp.dataSourceEnriched;
                analysisObj.DpExpDataSourceObjectId = exp.dataSourceObjectId;
                analysisObj.DpExpDataType = exp.dataType;
                analysisObj.DpExpDescription = exp.description;
                analysisObj.DpExpFormulaLanguageId = exp.formulaLanguageId;
                analysisObj.DpExpGeoMappingResolution = exp.geoMappingResolution;
                analysisObj.DpExpGeoQualification = exp.geoQualification;
                analysisObj.DpExpStripped = exp.stripped;
                analysisObj.DpExpQualification = exp.qualification;

                analysisReport.AnalyzedDataProviderExpressions.Add(analysisObj);

                Logger.Log($"--------> Processed Expression [{exp.id}] [{exp.name}]");
            }
            return docDataProviderDetailsResp;
        }

        private async Task AnalyzeReportElements(Models.Document document, Report report, List<Models.ReportDetails.ReportElement>? reportElements, DocumentAnalysisReport analysisReport)
        {
            Logger.Log($"----> {reportElements.Count} Report Elements to process");
            foreach (var element in reportElements)
            {
                Logger.Log($"--------> Analyzing Report Element [{element.id}] [{element.name}]");
                var url = $"{config.sapConnection.server}/raylight/v1/documents/{document.id}/reports/{report.id}/elements/{element.id}";
                var resp = await httpHelper.ExecuteGet<ReportElementRoot>("application/json", token, url);
                var reportElement = resp.element;

                var analysisObj = new ReportElementAnalysis();
                analysisObj.ReportId = report.id.ToString();
                analysisObj.DocumentId = document.id.ToString();
                analysisObj.Reference = reportElement.reference;
                analysisObj.Name = reportElement.name;
                analysisObj.Id = reportElement.id.ToString();
                analysisObj.UiRef = reportElement.uiref;
                analysisObj.Type = reportElement.type;
                analysisObj.IsLinkedToSharedElement = reportElement.isLinkedToSharedElement;
                analysisObj.Content = reportElement.content?.ToString();

                analysisReport.AnalyzedReportElements.Add(analysisObj);
            }
        }

        private async Task AnalyzeDataProviderQueryPlans(Models.Document item, Dataprovider dataProvider, DocumentAnalysisReport analysisReport)
        {
            // Collect Additional DataProvider Query Plan Details
            Logger.Log($"----> Analyzing DataProvider query Plan [{dataProvider.id}] [{dataProvider.name}]");
            var docDataProviderQueryPlanUrl = $"{config.sapConnection.server}/raylight/v1/documents/{item.id}/dataproviders/{dataProvider.id}/queryplan";
            var docDataProvidResperQueryPlan = await httpHelper.ExecuteGet<dynamic>("application/json", token, docDataProviderQueryPlanUrl);

            var queryPlanAnalysis = new DataProviderQueryPlanAnalysis();

            queryPlanAnalysis.DocId = item.id;
            queryPlanAnalysis.DpId = dataProvider.id;
            queryPlanAnalysis.DpName = dataProvider.name;
            queryPlanAnalysis.DsId = dataProvider.dataSourceId;
            queryPlanAnalysis.QueryPlan = JsonConvert.SerializeObject(docDataProvidResperQueryPlan);

            if (!string.IsNullOrEmpty(config.genai?.completionApi))
            {
                var prompt = @"Analyze the below query plan and return total count of tables, total count of columns, complexity, list of tables separated by semicolon, list of columns separated by semicolon in json format like '{ """"tableCount"""": 0, """"columnCount"""": 0 , """"complexity"""": false, """"tablelist"""": '', """"columnlist"""": ''}'. If table count and column is not there, give 0. If table count is more that 4, complexity is true else false.Always return in exact format./n" +
                    queryPlanAnalysis.QueryPlan;
                var gptResponse = await GenAIHelper.GetCompletionResponse(prompt, config.genai?.completionApi);
                var gptObject = JsonConvert.DeserializeObject<dynamic>(gptResponse);
                if (gptObject != null)
                {
                    queryPlanAnalysis.TableCount = gptObject.tableCount;
                    queryPlanAnalysis.ColumnCount = gptObject.columnCount;
                    queryPlanAnalysis.Complexity = gptObject.complexity;
                    queryPlanAnalysis.TableList = gptObject.tablelist;
                    queryPlanAnalysis.ColumnList = gptObject.columnlist;
                }
            }

            analysisReport.AnalyzedDataProviderQueryPlan.Add(queryPlanAnalysis);
            Logger.Log($"--------> Processed Queryplan [{queryPlanAnalysis.DpId}] [{queryPlanAnalysis.DpName}]");
        }

        private async Task AnalyzeDocumentVariables(Models.Document item, DocumentAnalysisReport analysisReport)
        {
            // Collect Document variables
            Logger.Log($"----> Analyzing Document Varaibles [{item.id}] [{item.name}]");
            var docVariableUrl = $"{config.sapConnection.server}/raylight/v1/documents/{item.id}/variables";
            var docVariableResponse = await httpHelper.ExecuteGet<DocumentVariablesRoot>("application/json", token, docVariableUrl);

            if (docVariableResponse?.variables?.variable != null)
            {
                foreach (var variable in docVariableResponse.variables.variable)
                {
                    var queryPlanAnalysis = new DocumentVariablesAnalysis();

                    queryPlanAnalysis.Id = variable.id;
                    queryPlanAnalysis.DocId = item.id;
                    queryPlanAnalysis.Name = variable.name;
                    queryPlanAnalysis.DataSourceEnriched = variable.dataSourceEnriched;
                    queryPlanAnalysis.Qualification = variable.qualification;
                    queryPlanAnalysis.CustomSort = variable.customSort;
                    queryPlanAnalysis.DataType = variable.dataType;
                    queryPlanAnalysis.HighPrecision = variable.highPrecision;
                    analysisReport.DocumentVariablesAnalysis.Add(queryPlanAnalysis);
                }
            }
            Logger.Log($"--------> Processed Document Variables [{item.id}]");
        }

        private List<DocumentDpProp> GetDpProps(string dpProps)
        {
            var result = new List<DocumentDpProp>();

            var dps = dpProps.Split(';');
            foreach (var dp in dps)
            {
                var dpProp = new DocumentDpProp();
                var validObj = false;

                string[] propertyValuePairs = dp.Split(':');

                foreach (string pair in propertyValuePairs)
                {
                    string[] parts = pair.Split('=');
                    if (parts.Length == 2)
                    {
                        validObj = true;

                        string propertyName = parts[0].Trim().ToLower();
                        string propertyValue = parts[1].Trim();

                        PropertyInfo property = typeof(DocumentDpProp).GetProperty(propertyName);
                        if (property != null && property.CanWrite)
                        {
                            property.SetValue(dpProp, propertyValue);
                        }
                    }
                }

                if (validObj)
                {
                    result.Add(dpProp);
                }

            }

            return result;
        }
    }
}
