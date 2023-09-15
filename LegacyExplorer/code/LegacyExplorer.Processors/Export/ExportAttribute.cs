using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyExplorer.Processors.Export
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExportAttribute : Attribute
    {
        public bool Export { get; set; }
        public string CsvFileName { get; set; }

        public ExportAttribute(bool export = true, string csvFileName = "")
        {
            Export = export;
            CsvFileName = csvFileName;

        }
    }
}