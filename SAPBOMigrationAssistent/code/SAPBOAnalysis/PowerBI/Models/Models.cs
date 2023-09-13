using System;
using Microsoft.PowerBI.Api.Models;

namespace SAPBOAnalysis.PowerBI.Models
{
    public class DataSetRequest
    {
        public string Id { get; set; }
        public string IdType { get; set; }
        public string Conn { get; set; }
        public string Type { get; set; }

        public string Mode { get; set; }
    }


    public class DataSetResponse
    {
        public bool Success { get; set; }
        public string Dataset { get; set; }
    }

    public class PowerBITableRequest
    {
        public string Type { get; set; }
        public string Datasource { get; set; }
        public Dictionary<string, List<string>> Tables { get; set; }
        public Dictionary<string, List<SAPBOAnalysis.Models.QueryHelper.Measures>> Measures { get; set; }

        public List<Relationship> Relationships { get; set; }
    }

    public class PowerBITableResponses
    {
        public List<PowerBITableResponse> PowerBITableResponseList { get; set; }

        public List<Relationship> Relationships { get; set; }
    }

    public class PowerBITableResponse
    {
        public Table Table { get; set; }
        public PostRowsRequest Data { get; set; }
    }
}
