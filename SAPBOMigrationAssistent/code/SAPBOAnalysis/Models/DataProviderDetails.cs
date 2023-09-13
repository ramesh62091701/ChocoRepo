using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SAPBOAnalysis.Models
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataProviderDetails
    {
        [JsonProperty("@refreshable")]
        public string refreshable { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string dataSourceId { get; set; }
        public string dataSourceCuid { get; set; }
        public string dataSourcePrefix { get; set; }
        public string dataSourceType { get; set; }
        public string dataSourceName { get; set; }
        public DateTime updated { get; set; }
        public int duration { get; set; }
        public string isPartial { get; set; }
        public int rowCount { get; set; }
        public int flowCount { get; set; }
        public DataProviderExpressionDictionary dictionary { get; set; }
        public DataProviderProperties properties { get; set; }
    }

    public class DataProviderExpressionDictionary
    {
        public List<DataProviderExpression> expression { get; set; }
    }

    public class DataProviderExpression
    {
        [JsonProperty("@dataType")]
        public string dataType { get; set; }

        [JsonProperty("@qualification")]
        public string qualification { get; set; }

        [JsonProperty("@customSort")]
        public string customSort { get; set; }

        [JsonProperty("@stripped")]
        public string stripped { get; set; }

        [JsonProperty("@dataSourceEnriched")]
        public string dataSourceEnriched { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string dataSourceObjectId { get; set; }
        public string formulaLanguageId { get; set; }
        public string natureId { get; set; }
        public string geoQualification { get; set; }
        public string geoMappingResolution { get; set; }
        public string dataProviderId { get; set; }
        public string dataProviderName { get; set; }
        public string dataSourceId { get; set; }
        public string dataSourceName { get; set; }
        public string description { get; set; }
        public string associatedDimensionId { get; set; }

        [JsonProperty("@allowUserValues")]
        public string allowUserValues { get; set; }

        [JsonProperty("@highPrecision")]
        public string highPrecision { get; set; }
        public string aggregationFunction { get; set; }
        public DataSourcePath dataSourcePath { get; set; } = new DataSourcePath();
    }

    public class DataSourcePath
    {
        public List<string> value { get; set; } = new List<string>();
    }

    public class DataProviderProperties
    {
        public List<DataProviderProperty> property { get; set; }
    }

    public class DataProviderProperty
    {
        [JsonProperty("@key")]
        public string key { get; set; }

        [JsonProperty("$")]
        public string value { get; set; }
    }

    public class DataProviderDetailsRoot
    {
        public DataProviderDetails dataprovider { get; set; }
    }


}
