using Services.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Interface
{
   public interface IProductCatalogService 
   {
      Task<Product[]> GetAllProductsAsync();

      Task AddProductAsync(Product product);

      Task<Product> GetProductAsync(Guid productId);
   }

}