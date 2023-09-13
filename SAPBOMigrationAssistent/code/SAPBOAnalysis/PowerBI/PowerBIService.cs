using Microsoft.Rest;
using Newtonsoft.Json;
using SAPBOAnalysis.Models;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using SAPBOAnalysis.PowerBI.Models;
using Table = Microsoft.PowerBI.Api.Models.Table;
using SAPBOAnalysis.Models.QueryHelper;

namespace SAPBOAnalysis.PowerBI
{
    public class PowerBIService
    {

        private Configuration config;
        private HttpHelper httpHelper;
        //public const string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ii1LSTNROW5OUjdiUm9meG1lWm9YcWJIWkdldyIsImtpZCI6Ii1LSTNROW5OUjdiUm9meG1lWm9YcWJIWkdldyJ9.eyJhdWQiOiJodHRwczovL2FuYWx5c2lzLndpbmRvd3MubmV0L3Bvd2VyYmkvYXBpIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvNzU3MWE0ODktYmQyOS00ZjM4LWI5YTYtN2M4ODBmOGNkZGYwLyIsImlhdCI6MTY5NDA4ODM4NywibmJmIjoxNjk0MDg4Mzg3LCJleHAiOjE2OTQwOTMzODUsImFjY3QiOjAsImFjciI6IjEiLCJhaW8iOiJBWVFBZS84VUFBQUFLZGg4SnIwdS9NbTNKTld6ZVZyeUNmY05Ic0lCUjE3Z0E2clZMTFkydU5YUHF2am9HcHNPQ0pLUFpNaXN5MjlzOVE2ZmJ5T3YyUU9kRUMyM2J3bnpVcCs4QWV5Tmd0eXk1b1JkYjlRWTgyUjZQeDNqamdpQ0lHV2xlSmtONGdKQ0U2OTAzdEhjYmdxMFBvNm9UMzEvMGFNYTdTMEYrN01YZ0txbGNJMHNzYVk9IiwiYW1yIjpbInB3ZCJdLCJhcHBpZCI6IjE4ZmJjYTE2LTIyMjQtNDVmNi04NWIwLWY3YmYyYjM5YjNmMyIsImFwcGlkYWNyIjoiMCIsImdpdmVuX25hbWUiOiJTaGFyYWQgS2hhbmRlbHdhbCIsImlwYWRkciI6IjE2NS4yMjUuMTIwLjI1MiIsIm5hbWUiOiJTaGFyYWQgS2hhbmRlbHdhbCIsIm9pZCI6IjczNjNlNTcyLWQxNTktNDVmYi1hZjE2LTJiOTdiMjNhZmVmMCIsIm9ucHJlbV9zaWQiOiJTLTEtNS0yMS0yMTUyOTQ3NjM0LTEwNTc2NDk5Ni0yMTQzNDQ1MjQyLTQxMjMiLCJwdWlkIjoiMTAwMzNGRkY4N0I5QjQ0OCIsInJoIjoiMC5BU3NBaWFSeGRTbTlPRS01cG55SUQ0emQ4QWtBQUFBQUFBQUF3QUFBQUFBQUFBQXJBSU0uIiwic2NwIjoiQXBwLlJlYWQuQWxsIENhcGFjaXR5LlJlYWQuQWxsIENhcGFjaXR5LlJlYWRXcml0ZS5BbGwgQ29udGVudC5DcmVhdGUgRGFzaGJvYXJkLlJlYWQuQWxsIERhc2hib2FyZC5SZWFkV3JpdGUuQWxsIERhdGFmbG93LlJlYWQuQWxsIERhdGFmbG93LlJlYWRXcml0ZS5BbGwgRGF0YXNldC5SZWFkLkFsbCBEYXRhc2V0LlJlYWRXcml0ZS5BbGwgR2F0ZXdheS5SZWFkLkFsbCBHYXRld2F5LlJlYWRXcml0ZS5BbGwgUGlwZWxpbmUuRGVwbG95IFBpcGVsaW5lLlJlYWQuQWxsIFBpcGVsaW5lLlJlYWRXcml0ZS5BbGwgUmVwb3J0LlJlYWQuQWxsIFJlcG9ydC5SZWFkV3JpdGUuQWxsIFN0b3JhZ2VBY2NvdW50LlJlYWQuQWxsIFN0b3JhZ2VBY2NvdW50LlJlYWRXcml0ZS5BbGwgVGVuYW50LlJlYWQuQWxsIFRlbmFudC5SZWFkV3JpdGUuQWxsIFVzZXJTdGF0ZS5SZWFkV3JpdGUuQWxsIFdvcmtzcGFjZS5SZWFkLkFsbCBXb3Jrc3BhY2UuUmVhZFdyaXRlLkFsbCIsInNpZ25pbl9zdGF0ZSI6WyJrbXNpIl0sInN1YiI6ImtscElCTGFNU080aVFBbDlmV2FOZjRjSTlkUjkwaHRkd1l4ZHhqZnowTlUiLCJ0aWQiOiI3NTcxYTQ4OS1iZDI5LTRmMzgtYjlhNi03Yzg4MGY4Y2RkZjAiLCJ1bmlxdWVfbmFtZSI6InNoYXJhZC5rQHNvbmF0YS1zb2Z0d2FyZS5jb20iLCJ1cG4iOiJzaGFyYWQua0Bzb25hdGEtc29mdHdhcmUuY29tIiwidXRpIjoiTExBdEdaZmFRVXVHUHVJZ1d3VXRBUSIsInZlciI6IjEuMCIsIndpZHMiOlsiYjc5ZmJmNGQtM2VmOS00Njg5LTgxNDMtNzZiMTk0ZTg1NTA5Il19.kudUC8hK5iRPIT9R9UYbg2hRIM5r_UoQrrgA7e0_NCDxiEDb6Dy-1kQ5puq5KuWaCywFY5_ZERXQP8WYqAtx8HsrPxw4QCTqSlqgRaHw4x0Exfp5VdFt5x2Z2ufssyU6B4a-nYn4Vzhtg5iJ_Xcwb2uMwWOccCMrehzONXJpxrjtpLjyJUomZdIKPc3SasphrJSD4QLq2mf9jqJauRCI5fpNW_SQ4GsS1IlYro02BXJgSaimb_H7Xq3452O1aO2b4VafWF7mjwds6dnYYfMcWyF7cUsPlEa0h9k7OB2bKiVe837WRr9TLAOTz_RJqVWpCnVIgDJ6hXiCIIidfQbCPA";
        public PowerBIService()
        {
            config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("appconfig.json"))!;
            httpHelper = new HttpHelper();
        }

        public async Task<DataSetResponse> GenerateDataSet(DataSetRequest request)
        {
            var response = new DataSetResponse();
            var list = new List<Table>();
            var tokenCredentials = await GetTokenCredentials();
            var powerBIClient = new PowerBIClient(new Uri(config.powerbi.apiUrl), tokenCredentials);

            var powerBITableRequestItems = await GetPowerBITableRequest(request);
            if(powerBITableRequestItems == null)
            {
                return response;
            }
            var powerBITableResponses = GetTables(powerBITableRequestItems.Item1);

            foreach (var responseInfo in powerBITableResponses.PowerBITableResponseList)
            {
                list.Add(responseInfo.Table);
            }
            string dataSetId = string.Empty;
            // Create dataset request
            var datasetRequest = new CreateDatasetRequest(powerBITableRequestItems.Item2, list);
            datasetRequest.DefaultMode = DatasetMode.Push;
            if (powerBITableRequestItems.Item1.Relationships != null)
            {
                datasetRequest.Relationships = powerBITableRequestItems.Item1.Relationships;
            }
            else
            {
                datasetRequest.Relationships = powerBITableResponses.Relationships;
            }
            switch (request.Mode)
            {
                case "Import":
                case "DirectQuery":
                    var xmla = new XMLAService();
                    dataSetId = await xmla.Execute(config.powerbi.workspace, datasetRequest, request, config);
                    break;
                default:
                    // Create the dataset
                    var dataset = powerBIClient.Datasets.PostDatasetInGroup(config.powerbi.workspaceId, datasetRequest);
                    dataSetId = dataset.Id;
                    //powerBIClient.Datasets.PostRowsInGroup(config.powerbi.workspaceId, dataset.Id, responseInfo.Table.Name, resp.Data);
                    foreach (var responseInfo in powerBITableResponses.PowerBITableResponseList)
                    {
                        string url = $"{config.powerbi.apiUrl}/v1.0/myorg/groups/{config.powerbi.workspaceId}/datasets/{dataSetId}/tables/{responseInfo.Table.Name}/rows";
                        await httpHelper.ExecutePowerBI<PostRowsRequest, dynamic>(responseInfo.Data, HttpMethod.Post, "application/json", config.token, url);
                    }
                    break;
            }

            response.Dataset = powerBITableRequestItems.Item2;
            response.Success = true;
            //LayoutGenerator generator = new LayoutGenerator();
            //generator.GenerateLayout(analyzerResponse.Name, list);
            return response;
        }


        private async Task<TokenCredentials> GetTokenCredentials()
        {
            /*var context = new AuthenticationContext(config.powerbi.authorityUrl + "/" + config.powerbi.tenantId);
            var credential = new ClientCredential(config.powerbi.clientId, config.powerbi.clientSecret);
            var result = await context.AcquireTokenAsync(config.powerbi.resourceUrl, credential);

            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the access token.");
            }
            return new TokenCredentials(result.AccessToken, "Bearer");*/
            return new TokenCredentials(config.token, "Bearer");
        }

        private PowerBITableResponses GetTables(PowerBITableRequest request)
        {
            switch (request.Type)
            {
                case "excel":
                    //var helper = new ExcelHelper();
                    //return helper.GetTables(request);
                    throw new NotImplementedException();

                case "sqlite":
                    var sqliteHelper = new SqliteHelper();
                    return sqliteHelper.GetTables(request);
                default:
                    throw new NotImplementedException();
            }
        }

        private List<string> Clean(List<string> list)
        {
            var response = new List<string>();
            foreach (var item in list)
            {
                var modifiedItem = item.Replace("$", string.Empty);
                response.Add(modifiedItem);
            }
            return response;
        }

        public async Task<string> GetDocumentData(string documentId)
        {
            try
            {
                Logger.Log("Started");
                var analyzer = new ReportAnalyzer();
                var analyzerResponse = await analyzer.GetDocumentData(documentId);
                foreach (var item in analyzerResponse.ReportDataList)
                {
                    File.WriteAllText($"{config.csvFilePath}/{item.ReportName}.csv", item.CSVData.Replace(";", ","));
                }
                Logger.Log($"CSV files dropped at {config.csvFilePath}.");
                return config.csvFilePath;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return string.Empty;
            }
        }

        private async Task<Tuple<PowerBITableRequest, string>> GetPowerBITableRequest(DataSetRequest request)
        {
            string name = string.Empty;
            var queryDetails = new List<QueryDetails>();
            PowerBITableRequest powerBITableRequest = new PowerBITableRequest()
            {
                Datasource = request.Conn,
                Type = request.Type,
                Tables = new Dictionary<string, List<string>>(),
                Measures = new Dictionary<string, List<Measures>> ()

            };
            Logger.Log("Fetching metadata");
            if (request.IdType == "Document")
            {
                var analyzer = new ReportAnalyzer();

                var analyzerResponse = await analyzer.GetDataSchemaForDocument(request.Id);
                name = analyzerResponse.Name;
                if (analyzerResponse.documentDataProviders == null ||
                    analyzerResponse.documentDataProviders.Count == 0 ||
                    analyzerResponse.documentDataProviders[0].queryList == null)
                {
                    return null;
                }
                queryDetails = analyzerResponse.documentDataProviders[0].queryList;
            }
            else //Universe
            {
                powerBITableRequest.Relationships = new List<Relationship>();
                var analyzer = new UniverseManager();
                var schema = await analyzer.GetUniverseClassesAndObjects(request.Id);
                name = schema.Name;
                queryDetails.Add(new QueryDetails { TableInfo = schema.queryDetails });
                foreach (var item in schema.RelationShips)
                {
                    var relation = new Relationship();
                    relation.Name = Guid.NewGuid().ToString();
                    relation.ToTable = item.ToTable;
                    relation.ToColumn = item.ToColumn;
                    relation.FromColumn = item.FromColumn;
                    relation.FromTable = item.FromTable;
                    relation.CrossFilteringBehavior = new CrossFilteringBehavior();
                    relation.CrossFilteringBehavior = CrossFilteringBehavior.BothDirections;
                    powerBITableRequest.Relationships.Add(relation);
                }
            }

            foreach (var queryInfo in queryDetails)
            {
                foreach (var tableInfo in queryInfo.TableInfo)
                {
                    if (powerBITableRequest.Tables.ContainsKey(tableInfo.TableName))
                    {
                        var columns = powerBITableRequest.Tables[tableInfo.TableName];
                        foreach (var item in Clean(tableInfo.Columns))
                        {
                            if (!columns.Contains(item))
                            {
                                columns.Add(item);
                            }
                        }
                    }
                    else
                    {
                        powerBITableRequest.Tables.Add(tableInfo.TableName, Clean(tableInfo.Columns));
                    }

                    if (tableInfo.Measures != null)
                    {
                        if (powerBITableRequest.Measures.ContainsKey(tableInfo.TableName))
                        {
                            var measures = powerBITableRequest.Measures[tableInfo.TableName];
                            foreach (var item in tableInfo.Measures)
                            {
                                if (!measures.Contains(item))
                                {
                                    measures.Add(item);
                                }
                            }
                        }
                        else
                        {
                            powerBITableRequest.Measures.Add(tableInfo.TableName, tableInfo.Measures);
                        }
                    }

                }
            }
            return new Tuple<PowerBITableRequest, string>(powerBITableRequest, name);
        }
    }
}
