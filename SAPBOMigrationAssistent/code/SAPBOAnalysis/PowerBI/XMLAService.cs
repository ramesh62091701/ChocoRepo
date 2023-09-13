using System;
using AMO = Microsoft.AnalysisServices;
using Microsoft.AnalysisServices.Tabular;
using SAPBOAnalysis.PowerBI.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace SAPBOAnalysis.PowerBI
{
    class XMLAService
    {

        public Server server = new Server();

        public async Task<string> Execute(string workspace, Microsoft.PowerBI.Api.Models.CreateDatasetRequest datasetRequest, 
            DataSetRequest request,
            SAPBOAnalysis.Models.Configuration config)
        {
            ConnectToPowerBIAsUser(workspace, config);
            var model = CreateDatabase(datasetRequest.Name);
            await CreateModel(model, datasetRequest, request, config);
            var db = server.Databases.GetByName(datasetRequest.Name);
            return db.ID;
        }

        public void ConnectToPowerBIAsUser(string workspace, SAPBOAnalysis.Models.Configuration config)
        {
            string workspaceConnection = $"powerbi://api.powerbi.com/v1.0/myorg/{workspace}";
            string accessToken = config.token;
            string connectStringUser = $"DataSource={workspaceConnection};Password={accessToken};";
            server.Connect(connectStringUser);
        }

        public Database CreateDatabase(string DatabaseName)
        {
            string newDatabaseName = server.Databases.GetNewName(DatabaseName);
            var database = new Database()
            {
                Name = newDatabaseName,
                ID = newDatabaseName,
                CompatibilityLevel = 1520,
                StorageEngineUsed = Microsoft.AnalysisServices.StorageEngineUsed.TabularMetadata,
                Model = new Model()
                {
                    Name = DatabaseName + "-Model",
                    Description = "A Demo Tabular data model with 1520 compatibility level."
                }
            };

            server.Databases.Add(database);
            database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);

            return database;
        }

        public async Task CreateModel(Database database, Microsoft.PowerBI.Api.Models.CreateDatasetRequest datasetRequest, 
            DataSetRequest request,
            SAPBOAnalysis.Models.Configuration config)
        {

            Model model = database.Model;
            var mode = (request.Mode == "Import") ? ModeType.Import : ModeType.DirectQuery;
            foreach (var table in datasetRequest.Tables)
            {
                Table customersTable = new Table()
                {
                    Name = table.Name,
                    Description = table.Description,
                    Partitions = { new Partition() { Name = table.Name, Mode =  mode,
                        Source = new MPartitionSource() {
                            Expression = $@"let
                        ConnectionString=""DRIVER=Devart ODBC Driver for SQLite;Description=Sqlite;Database=D:\Sonata\SapBOAnalyzer\SapBOMigration\Data\efashion.db;Locking Mode=Exclusive;Synchronous=Off;"",
                        Source = Odbc.DataSource(ConnectionString,[CreateNavigationProperties=true]),
                        Query = ""SELECT * FROM {table.Name}""
                        in Source{{[Name=""Query""]}}[Data]" } } }
                };




                foreach (var column in table.Columns)
                {
                    var col = new DataColumn() { Name = column.Name, DataType = DataType.String, SourceColumn = column.Name };
                    customersTable.Columns.Add(col);
                }

                if (table.Measures != null)
                {
                    foreach (var measure in table.Measures)
                    {
                        var prompt = $"Convert this SAP BO measure in ```{measure.Expression}```\r\nin Power BI measure. Response should be converted formula in json format {{result:''}}";
                        var gptResponse = await GenAIHelper.GetCompletionResponse(prompt, config.genai.completionApi);
                        var gptObject = JsonConvert.DeserializeObject<dynamic>(gptResponse);

                        if (gptObject?.result != null)
                        {
                            var mea = new Measure { Name = measure.Name, Expression = gptObject.result , FormatString = measure.FormatString };
                            customersTable.Measures.Add(mea);
                        }
                    }
                }
                model.Tables.Add(customersTable);

            }

            foreach (var relationShip in datasetRequest.Relationships)
            {
                model.Relationships.Add(new SingleColumnRelationship
                {
                    Name = relationShip.Name,
                    ToColumn = model.Tables[relationShip.ToTable].Columns[relationShip.ToColumn],
                    ToCardinality = RelationshipEndCardinality.One,
                    FromColumn = model.Tables[relationShip.FromTable].Columns[relationShip.FromColumn],
                    FromCardinality = RelationshipEndCardinality.Many
                });
            }

            model.SaveChanges();
            //model.RequestRefresh(RefreshType.Full);
            //model.SaveChanges();
        }


    }
}
