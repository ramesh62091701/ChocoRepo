using System;
using System.Collections.Generic;
using System.Text;

namespace MVCOrleans.Interface
{
    [GenerateSerializer]
   public class BasketItem
   {
      [Id(0)]public Guid ProductId { get; set; }

      [Id(1)] public int Quantity { get; set; }
   }
}
