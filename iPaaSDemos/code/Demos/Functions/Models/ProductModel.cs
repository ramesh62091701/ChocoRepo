
using Newtonsoft.Json;

namespace Functions.Models
{
    public class ProductModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }
    }
}
