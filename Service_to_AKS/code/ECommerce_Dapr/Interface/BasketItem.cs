using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce_Dapr.Interfaces
{
   public class BasketItem
   {
      public Guid ProductId { get; set; }

      public int Quantity { get; set; }
   }
}
