using System.Collections.Generic;

namespace Functions.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public OrderStatusEnum OrderStatus { get; set; }
        public List<ProductModel> Products { get; set; }
        public double   TotalPrice { get; set; }
      }
}
