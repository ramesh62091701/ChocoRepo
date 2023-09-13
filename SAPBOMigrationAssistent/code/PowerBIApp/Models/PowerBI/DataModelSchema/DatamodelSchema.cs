using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBIApp.Models.PowerBI.DataModelSchema
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Annotation
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Column
    {
        public string type { get; set; }
        public string name { get; set; }
        public string dataType { get; set; }
        public bool isNameInferred { get; set; }
        public bool isDataTypeInferred { get; set; }
        public bool isHidden { get; set; }
        public string sourceColumn { get; set; }
        public string formatString { get; set; }
        public string lineageTag { get; set; }
        public string dataCategory { get; set; }
        public string summarizeBy { get; set; }
        public List<Annotation> annotations { get; set; }
        public string expression { get; set; }
        public string sortByColumn { get; set; }
    }

    public class Content
    {
        public string Version { get; set; }
        public string Language { get; set; }
        public string DynamicImprovement { get; set; }
    }

    public class Culture
    {
        public string name { get; set; }
        public LinguisticMetadata linguisticMetadata { get; set; }
    }

    public class DataAccessOptions
    {
        public bool legacyRedirects { get; set; }
        public bool returnErrorValuesAsNull { get; set; }
    }

    public class Hierarchy
    {
        public string name { get; set; }
        public string lineageTag { get; set; }
        public List<Level> levels { get; set; }
        public List<Annotation> annotations { get; set; }
    }

    public class Level
    {
        public string name { get; set; }
        public int ordinal { get; set; }
        public string column { get; set; }
        public string lineageTag { get; set; }
    }

    public class LinguisticMetadata
    {
        public Content content { get; set; }
        public string contentType { get; set; }
    }

    public class Model
    {
        public string culture { get; set; }
        public DataAccessOptions dataAccessOptions { get; set; }
        public string defaultPowerBIDataSourceVersion { get; set; }
        public string sourceQueryCulture { get; set; }
        public List<Table> tables { get; set; }
        public List<Culture> cultures { get; set; }
        public List<Annotation> annotations { get; set; }
    }

    public class Partition
    {
        public string name { get; set; }
        public string mode { get; set; }
        public Source source { get; set; }
    }

    public class DataModelSchema
    {
        public string name { get; set; }
        public int compatibilityLevel { get; set; }
        public Model model { get; set; }
    }

    public class Source
    {
        public string type { get; set; }
        public string expression { get; set; }
    }

    public class Table
    {
        public string name { get; set; }
        public bool isHidden { get; set; }
        public bool isPrivate { get; set; }
        public string lineageTag { get; set; }
        public List<Column> columns { get; set; }
        public List<Partition> partitions { get; set; }
        public List<Hierarchy> hierarchies { get; set; }
        public List<Annotation> annotations { get; set; }
    }


}
