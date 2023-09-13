using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2013.Excel;

namespace SAPBOAnalysis.Models.QueryHelper
{
    public class DocumentDataSchema
    {
        public string DocId { get; set; }
        public string Name { get; set; }
        public List<DocumentDataProvider> documentDataProviders = new List<DocumentDataProvider>();
    }

    public class DocumentData
    {
        public string DocId { get; set; }
        public string Name { get; set; }
        public List<ReportData> ReportDataList { get; set; }

    }

    public class ReportData
    {
        public string ReportId { get; set; }
        public string ReportName { get; set; }
        public string CSVData { get; set; }
    }


    public class DocumentDataProvider
    {
        public string DataProviderId { get; set; }

        public string DataProviderName { get; set; }
        public string DataSourceId { get; set; }
        public string DataSourceName { get; set; }
        public string DataSourceType { get; set; }

        public List<QueryDetails> queryList { get; set; }
    }

    public class QueryDetails
    {
        public string QueryId { get; set; }
        public List<QueryTableDetails> TableInfo { get; set; }
    }

    public class QueryTableDetails
    {
        public string TableName { get; set; }
        public List<string> Columns { get; set; }

        public List<QueryFilter> Filters { get; set; }
        public List<Measures> Measures { get; set; }
    }

    public class Measures
    {
        public string Name { get; set; }
        public string Expression { get; set; }

        public string Format { get; set; }
    }
    public class RelationShip
    {
        public string FromTable { get; set; }
        public string FromColumn { get; set; }

        public string ToTable { get; set; }
        public string ToColumn { get; set; }
    }

    public class QueryFilter
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public string[] Operands { get; set; }
    }
}
