using ExcelDataReader;
using System.Data;
using Microsoft.PowerBI.Api.Models;
using SAPBOAnalysis.PowerBI.Models;

namespace SAPBOAnalysis.PowerBI
{
    public class ExcelHelper
    {
        // Method to read data from excel
        public List<Table> GetTables(PowerBITableRequest request)
        {
            var tables = new List<Table>();
            using (var stream = File.Open(request.Datasource, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    foreach (DataTable table in result.Tables)
                    {
                        var columns = table.Columns
                                        .Cast<DataColumn>()
                                        .Select(column =>
                                            new Column(
                                                column.ColumnName,
                                                !string.IsNullOrEmpty(column.ColumnName) ? column.DataType.Name : string.Empty))
                                        .ToList();

                        tables.Add(new Table { Name = table.TableName, Columns = columns });
                    }
                }
            }
            return tables;
        }
    }
}
