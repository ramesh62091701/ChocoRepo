﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MVCOrleans.Model
{
   public class ApiCheckoutSummary
   {
      [JsonProperty("products")]
      public List<ApiCheckoutProduct> Products { get; set; }

      [JsonProperty("totalPrice")]
      public double TotalPrice { get; set; }

      [JsonProperty("date")]
      public DateTime Date { get; set; }
   }
}
