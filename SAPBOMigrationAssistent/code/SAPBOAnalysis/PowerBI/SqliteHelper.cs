using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.Data.Sqlite;
using Microsoft.PowerBI.Api.Models;
using Newtonsoft.Json;
using SAPBOAnalysis.PowerBI.Models;
using System.Data;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace SAPBOAnalysis.PowerBI
{
    public class SqliteHelper
    {
        private bool allColumns = true;
        private Dictionary<string, string> primaryKeys = new Dictionary<string, string>();
        public PowerBITableResponses GetTables(PowerBITableRequest request)
        {
            var response = new PowerBITableResponses() { Relationships = new List<Relationship>() };


            response.PowerBITableResponseList = new List<PowerBITableResponse>();
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = request.Datasource;


            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();
                foreach (var item in request.Tables)
                {
                    Logger.Log($"Create model for {item.Key}.");
                    //This is not working.
                    /*var paragma = $"PRAGMA foreign_key_list(Shop_facts)";
                    var selectPragmaCmd = connection.CreateCommand();
                    using (var reader = selectPragmaCmd.ExecuteReader())
                    {
                        reader.GetString(0);
                    }*/
                    var selectCmd = connection.CreateCommand();
                    if (!allColumns)
                    {
                        var columnsString = string.Join(",", item.Value);
                        selectCmd.CommandText = $"SELECT {columnsString} FROM {item.Key}";
                    }
                    else
                    {
                        selectCmd.CommandText = $"SELECT * FROM {item.Key}";
                    }
                    using (var reader = selectCmd.ExecuteReader())
                    {
                        var powerBITable = GetTable(item.Key, item.Value, reader);
                        response.PowerBITableResponseList.Add(powerBITable);
                    }
                    Logger.Log($"Completed model for {item.Key}.");
                }
                if (request.Measures != null)
                {
                    foreach (var item in request.Measures)
                    {
                        var tab = response.PowerBITableResponseList.First(t => t.Table.Name == item.Key);
                        foreach (var item2 in item.Value)
                        {
                            if (tab.Table.Measures == null)
                            {
                                tab.Table.Measures = new List<Measure>();
                            }
                            tab.Table.Measures.Add(new Measure { Name = item2.Name, Expression = item2.Expression, FormatString = item2.Format });
                        }

                    }
                }
            }

            var listOfTables = new List<Table>();
            foreach (var item in response.PowerBITableResponseList)
            {
                listOfTables.Add(item.Table);
            }
            response.Relationships = GetRelationships(listOfTables);
            return response;
        }

        private PowerBITableResponse GetTable(string tableName, List<string> columns, SqliteDataReader reader)
        {
            var response = new PowerBITableResponse();
            var powerBIColumns = new List<Column>();
            if (!allColumns)
            {
                foreach (var column in columns)
                {
                    powerBIColumns.Add(new Column() { Name = column, DataType = "string" });
                }
            }

            var powerBITable = new Table { Name = tableName, Columns = powerBIColumns };
            /*powerBITable.Source = new List<ASMashupExpression>();
            var expression = new ASMashupExpression();
            expression.Expression = $@"let
                        ConnectionString=""DRIVER=Devart ODBC Driver for SQLite;Description=Sqlite;Database=D:\Sonata\SapBOAnalyzer\SapBOMigration\Data\efashion.db;Locking Mode=Exclusive;Synchronous=Off;"",
                        Source = Odbc.DataSource(ConnectionString,[CreateNavigationProperties=true]),
                        Query = ""SELECT * FROM {powerBITable.Name}""
                        in Source{{[Name=""Query""]}}[Data]";*/

            var records = new PostRowsRequest();
            records.Rows = new List<object>();

            int count = 0;
            Logger.Log($"Import data started.");
            while (reader.Read())
            {
                if(count == 9000)
                {
                    break;
                }
                count++;
                var expando = new ExpandoObject() as IDictionary<string, Object>;
                if (!allColumns)
                {
                    foreach (var col in columns)
                    {
                        expando.Add(col, reader[col]);
                    }
                }
                else
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (count == 1)
                        {
                            powerBIColumns.Add(new Column() { Name = reader.GetName(i), DataType = "string" });
                            if (i == 0) //only check first row for primary key
                            {
                                var isPrimaryKey = Convert.ToBoolean(reader.GetSchemaTable().Rows[0].ItemArray[5]);
                                if (isPrimaryKey)
                                {
                                    primaryKeys.Add(tableName, reader.GetName(i));
                                }
                            }
                        }
                        expando.Add(reader.GetName(i), reader[reader.GetName(i)]);
                    }
                }

                records.Rows.Add(expando);
            }
            Logger.Log($"Import data completed.");
            response.Table = powerBITable;
            response.Data = records;
            return response;
        }

        private List<Relationship> GetRelationships(List<Table> tables)
        {
            var relationships = new List<Relationship>();
            foreach (Table table in tables)
            {
                foreach (var item in table.Columns)
                {
                    foreach (var pk in primaryKeys)
                    {
                        if (table.Name.ToLower() == pk.Key.ToLower())
                        {
                            continue;
                        }
                        if (item.Name == pk.Value)
                        {
                            var relationShip = new Relationship();
                            relationShip.FromTable = pk.Key;
                            relationShip.FromColumn = pk.Value;
                            relationShip.ToTable = table.Name;
                            relationShip.ToColumn = item.Name;
                            relationShip.CrossFilteringBehavior = new CrossFilteringBehavior();
                            relationShip.CrossFilteringBehavior = CrossFilteringBehavior.BothDirections;
                            relationShip.Name = Guid.NewGuid().ToString();
                            relationships.Add(relationShip);
                        }
                    }
                }
            }
            return relationships;
        }
    }
}
