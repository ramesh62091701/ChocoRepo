using System;
using System.Collections.Generic;
using System.Text;

namespace OrleansMVC.Interface
{
    [Serializable]
   public class BasketItem
   {
      public Guid ProductId { get; set; }

      public int Quantity { get; set; }
   }
}
