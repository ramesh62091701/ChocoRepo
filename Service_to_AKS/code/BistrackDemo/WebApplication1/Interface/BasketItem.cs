using Orleans;
using Orleans.CodeGeneration;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplication1.Interface
{
    [Serializable()]
   public class BasketItem
   {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
   }
}
