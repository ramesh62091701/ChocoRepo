using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SAPBOAnalysis
{


    public static class SpreadsheetReader
    {
        public static List<string> Read(string filePath, string worksheetName, string columnName)
        {
            var values = new List<string>();
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false))
            {
                WorkbookPart workbookPart = document.WorkbookPart;
                WorksheetPart worksheetPart = GetWorksheetPartByName(workbookPart, worksheetName);

                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                foreach (Row row in sheetData.Elements<Row>())
                {
                    Cell cell = GetCellByColumnName(row, columnName);
                    if (cell != null)
                    {
                        string cellValue = GetCellValue(workbookPart, cell);
                        values.Add(cellValue);
                    }
                }
            }
            return values;

        }

        private static WorksheetPart GetWorksheetPartByName(WorkbookPart workbookPart, string sheetName)
        {
            Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
            if (sheet == null)
            {
                throw new ArgumentException("Worksheet with the specified name does not exist.");
            }

            return (WorksheetPart)workbookPart.GetPartById(sheet.Id);
        }

        private static Cell GetCellByColumnName(Row row, string columnName)
        {
            foreach (Cell cell in row.Elements<Cell>())
            {
                string cellColumnName = GetColumnNameFromCellReference(cell.CellReference);
                if (cellColumnName == columnName)
                {
                    return cell;
                }
            }

            return null;
        }

        private static string GetCellValue(WorkbookPart workbookPart, Cell cell)
        {
            string cellValue = string.Empty;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                SharedStringTablePart stringTablePart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                SharedStringItem[] sharedStringItems = stringTablePart.SharedStringTable.Elements<SharedStringItem>().ToArray();
                cellValue = sharedStringItems[int.Parse(cell.CellValue.Text)].InnerText;
            }
            else if (cell.CellValue != null)
            {
                cellValue = cell.CellValue.Text;
            }

            return cellValue;
        }

        private static string GetColumnNameFromCellReference(string cellReference)
        {
            string columnName = "";

            foreach (char c in cellReference)
            {
                if (char.IsLetter(c))
                    columnName += c;
                else
                    break;
            }

            return columnName;
        }
    }
}
