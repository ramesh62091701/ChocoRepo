using CsvHelper;
using DocumentFormat.OpenXml.Packaging;

using DocumentFormat.OpenXml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections;

namespace SAPBOAnalysis
{
    public static class CsvHelper
    {
        public static string GetCSV(List<dynamic> records)
        {
            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);

                return writer.ToString();
            }
        }

        public static dynamic CreateDynamic(object obj, List<PropertyInfo> properties, List<string> propertiesToIgnore)
        {
            var expando = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)expando;

            foreach (var property in properties)
            {
                if (propertiesToIgnore == null || !propertiesToIgnore.Contains(property.Name.ToLower()))
                    dictionary.Add(property.Name, property.GetValue(obj));
            }

            return expando;

        }

        public static List<PropertyInfo> GetProperties(object obj)
        {
            var list = new List<PropertyInfo>();

            foreach (var property in obj.GetType().GetProperties())
            {
                list.Add(property);
            }
            return list;
        }

        public static List<object> GetData(object obj, List<PropertyInfo> properties)
        {
            var list = new List<object>();
            foreach (var property in properties)
            {

                list.Add(property.GetValue(obj));
            }
            return list;
        }

        public static void WriteExcelFile(string path, Dictionary<string, List<object>> records)
        {

            using (var document = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                UInt32Value counter = 0;
                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                foreach (var item in records)
                {
                    if (item.Value == null || !item.Value.Any()) continue;
                    counter++;
                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new SheetData();
                    worksheetPart.Worksheet = new Worksheet(sheetData);


                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = counter, Name = item.Key };

                    sheets.Append(sheet);

                    Row headerRow = new Row();

                    List<String> columns = new List<string>();
                    var properties = GetProperties(item.Value[0]);
                    foreach (var propertyInfo in properties)
                    {
                        columns.Add(propertyInfo.Name);
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(propertyInfo.Name);
                        headerRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(headerRow);

                    foreach (var record in item.Value)
                    {
                        Row newRow = new Row();
                        foreach (var data in GetData(record, properties))
                        {
                            Cell cell = new Cell();
                            if (data != null && data.GetType() == typeof(int))
                            {
                                cell.DataType = CellValues.Number;
                            }
                            else
                            {

                                cell.DataType = CellValues.String;
                            }
                            cell.CellValue = new CellValue(data == null ? string.Empty : data.ToString());
                            newRow.AppendChild(cell);
                        }
                        sheetData.AppendChild(newRow);
                    }


                }
                workbookPart.Workbook.Save();
            }
        }

    }
}
