using System;
using System.Collections.Generic;
using System.Text;

namespace MVCWebAPI.Interface
{
   public class BasketItem
   {
      public Guid ProductId { get; set; }

      public int Quantity { get; set; }
   }
}
