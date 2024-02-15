using System;
using Newtonsoft.Json;

namespace AkkaDemo2.Model
{
   public class ApiCheckoutProduct
   {
      [JsonProperty("productId")]
      public Guid ProductId { get; set; }

      [JsonProperty("productName")]
      public string ProductName { get; set; } 

      [JsonProperty("quantity")]
      public int Quantity { get; set; }

      [JsonProperty("price")]
      public double Price { get; set; }
   }
}