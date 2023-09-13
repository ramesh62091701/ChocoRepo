using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SAPBOAnalysis.Models
{
    public class DocumentAnalysisReport
    {
        public int AnalyzedDocumentCount { get { return AnalyzedDocuments.Count; } }
        public List<object> AnalyzedDocuments { get; set; } = new List<object>();
        public List<object> AnalyzedReports { get; set; } = new List<object>();
        public List<object> AnalyzedReportElements { get; set; } = new List<object>();
        public List<object> AnalyzedDataProviderExpressions { get; set; } = new List<object>();
        public List<object> AnalyzedDataProviderQueryPlan { get; set; } = new List<object>();
        public List<object> DocumentVariablesAnalysis { get; set; } = new List<object>();
        public List<object> AnalyzedConnections { get; set; } = new List<object>();
    }

    public class DocumentAnalysis
    {
        public string DocId { get; set; }
        public string DocName { get; set; }

        public string DocType { get; set; }

        public string CuID { get; set; }

        public int FolderId { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }

        public DateTimeOffset LastRunTime { get; set; }
        public string DocState { get; set; }
        public bool IsDeactivated { get; set; }
        public bool IsInstance { get; set; }
        public string ReadingDirection { get; set; }
        public int Size { get; set; }
        public bool IsScheduled { get; set; }

        public int ScehduleRunCount { get; set; }
        public int ReportCount { get; set; }
        public int DataProviderCount { get; set; }

        public string DataProviderDetails { get; set; }

        public int UniverseCount { get; set; }
        public string UniverseDetails { get; set; }
    }

    public class ReportAnalysis
    {
        public string DocId { get; set; }
        public string DocName { get; set; }
        public string ReportId { get; set; }
        public string ReportName { get; set; }

        public int ElementCount { get; set; }

        public int TableCount { get; set; }
        public int CellCount { get; set; }
    }

    public class ReportElementAnalysis
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public string DocumentId { get; set; }
        public string ReportId { get; set; }

        public string Type { get; set; }

        public string IsLinkedToSharedElement { get; set; }
        public string Reference { get; set; }
        public string UiRef { get; set; }

        public string Content { get; set; }

    }

    public class DataProviderExpressionAnalysis
    {
        public string DocId { get; set; }
        public string DocName { get; set; }
        public string DpId { get; set; }
        public string DpName { get; set; }
        public string DsId { get; set; }
        public string DsCuid { get; set; }
        public string DsPrefix { get; set; }
        public string DsType { get; set; }
        public string DsName { get; set; }
        public string DsPath { get; set; }
        public DateTime Updated { get; set; }
        public string DpExpId { get; set; }
        public string DpExpName { get; set; }
        public string DpExpDataType { get; set; }

        public string DpExpQualification { get; set; }

        public string DpExpCustomSort { get; set; }

        public string DpExpStripped { get; set; }

        public string DpExpDataSourceEnriched { get; set; }
        public string DpExpDataSourceObjectId { get; set; }
        public string DpExpFormulaLanguageId { get; set; }
        public string DpExpNatureId { get; set; }
        public string DpExpGeoQualification { get; set; }
        public string DpExpGeoMappingResolution { get; set; }
        public string DpExpDescription { get; set; }
        public string DpExpAssociatedDimensionId { get; set; }
        public string DpExpAllowUserValues { get; set; }
        public string DpExpAggregationFunction { get; set; }
    }

    public class DataProviderQueryPlanAnalysis
    {
        public string DocId { get; set; }
        public string DpId { get; set; }
        public string DpName { get; set; }
        public string DsId { get; set; }
        public dynamic QueryPlan { get; set; }
        public string TableCount { get; set; }
        public string ColumnCount { get; set; }
        public string Complexity { get; set; }
        public string TableList { get; set; }
        public string ColumnList { get; set; }
    }

    public class DocumentVariablesAnalysis
    {
        public string Id { get; set; }
        public string DocId { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Qualification { get; set; }
        public string HighPrecision { get; set; }
        public string CustomSort { get; set; }
        public string Stripped { get; set; }
        public string DataSourceEnriched { get; set; }
        public string Constant { get; set; }
    }

    public class ConnectionAnalysis
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public string Path { get; set; }
        public string Database { get; set; }
        public string NetworkLayer { get; set; }
    }
}