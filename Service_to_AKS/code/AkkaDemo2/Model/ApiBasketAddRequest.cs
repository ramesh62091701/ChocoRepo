﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AkkaDemo2.Model
{
   public class ApiBasketAddRequest
   {
      [JsonProperty("productId")]
      public Guid ProductId { get; set; }

      [JsonProperty("quantity")]
      public int Quantity { get; set; }
   }
}
