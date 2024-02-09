using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Model
{
   public interface IProductCatalogService
   {
      Task<Product[]> GetAllProductsAsync();

      Task AddProductAsync(Product product);

      Task<Product> GetProductAsync(Guid productId);
   }

}