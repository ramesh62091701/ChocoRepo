using Azure;
using Azure.Data.Tables;
using System;
using System.Text.Json.Serialization;

namespace Functions.Repository
{
    public abstract class BaseEntity : ITableEntity
    {
        [JsonIgnore]
        public string PartitionKey { get; set; }
        [JsonIgnore]
        public string RowKey { get; set; }
        [JsonIgnore]
        public DateTimeOffset? Timestamp { get; set; }
        [JsonIgnore]
        public ETag ETag { get; set; }

        public virtual string Id { get { return $"{PartitionKey}_{RowKey}"; } }

    }
}
