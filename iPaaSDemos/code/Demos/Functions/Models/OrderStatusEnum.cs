
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Functions.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderStatusEnum
    {
        Pending,
        Processing,
        Complete,
        Completed,
        Cancelled,
        Incomplete
    }
}
