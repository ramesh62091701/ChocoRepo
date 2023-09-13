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
using SAPBOAnalysis.Models.Connection;
using SAPBOAnalysis.Models.DocumentDetails;
using SAPBOAnalysis.Models.DocumentVariables;
using SAPBOAnalysis.Models.QueryHelper;
using SAPBOAnalysis.Models.ReportDetails;
using SAPBOAnalysis.Models.ReportElement;
using SAPBOAnalysis.Models.Universe;
using SAPBOAnalysis.Models.UniverseDetails;

namespace SAPBOAnalysis
{
    public class ConnectionAnalyzer
    {
        HttpHelper httpHelper = new HttpHelper();
        TokenHelper tokenHelper = new TokenHelper();

        string token = string.Empty;


        private Configuration config;
        public ConnectionAnalyzer()
        {
            config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("appconfig.json"))!;
        }

        public async Task<Dictionary<string, List<object>>> Analyze()
        {
            Logger.Log("==========================================================");
            Logger.Log($"CONNECTION ANALYSIS");
            Logger.Log("==========================================================");

            token = await tokenHelper.GetToken();

            var analysisReport = new DocumentAnalysisReport();

            //Get Connections
            Logger.Log($"Getting Connections...");
            string url = $"{config.sapConnection.server}/raylight/v1/connections?limit=50";
            var connectionsResp = await httpHelper.ExecuteGet<ConnectionRoot>("application/json", token, url);

            Logger.Log($"{connectionsResp.connections.connection.Count} connections to process");

            foreach (var item in connectionsResp.connections.connection)
            {
                var connAnalysis = new ConnectionAnalysis();

                Logger.Log($"====> Processing Connection [{item.id}] [{item.name}]");

                // Get Document Details
                Logger.Log($"--> Getting Connection Details");
                var connDetailsUrl = $"{config.sapConnection.server}/raylight/v1/connections/{item.id}";
                var connDetailsResp = await httpHelper.ExecuteGet<ConnectionDetailsRoot>("application/json", token, connDetailsUrl);
                var connDetails = connDetailsResp.connection;

                // *** Analysis Update -- START *** //
                connAnalysis.Id = connDetails.id;
                connAnalysis.Name = connDetails.name;
                connAnalysis.Type = connDetails.type;
                connAnalysis.Database = connDetails.database;
                connAnalysis.NetworkLayer = connDetails.networkLayer;
                connAnalysis.Path = connDetails.path;

                // *** Analysis Update -- END *** //               

                analysisReport.AnalyzedConnections.Add(connAnalysis);
            }

            var excelWorkSheets = new Dictionary<string, List<object>>();
            excelWorkSheets.Add("ConnectionAnalysis", analysisReport.AnalyzedConnections);
            return excelWorkSheets;
        }

        public async Task<DocumentDataSchema> GetDataSchemaForDocument(string documentId)
        {
            var result = new DocumentDataSchema();

            token = await tokenHelper.GetToken();

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
