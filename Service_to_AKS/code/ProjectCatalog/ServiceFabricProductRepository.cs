﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectCatalog
{
   class ServiceFabricProductRepository : IProductRepository
   {
      private readonly IReliableStateManager _stateManager;

      public ServiceFabricProductRepository(IReliableStateManager stateManager)
      {
         _stateManager = stateManager;
      }

      public async Task AddProduct(Product product)
      {
         IReliableDictionary<Guid, Product> products = await _stateManager
            .GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");

         using(ITransaction tx = _stateManager.CreateTransaction())
         {
            await products
               .AddOrUpdateAsync(tx, product.Id, product, (id, value) => product);

            await tx.CommitAsync();
         }
      }

      public async Task<IEnumerable<Product>> GetAllProducts()
      {
         IReliableDictionary<Guid, Product> products =
            await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");
         var result = new List<Product>();

         using(ITransaction tx = _stateManager.CreateTransaction())
         {
            IAsyncEnumerable<KeyValuePair<Guid, Product>> allProducts =
               await products.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

            using(IAsyncEnumerator<KeyValuePair<Guid, Product>> enumerator = 
               allProducts.GetAsyncEnumerator())
            {
               while(await enumerator.MoveNextAsync(CancellationToken.None))
               {
                  KeyValuePair<Guid, Product> current = enumerator.Current;
                  result.Add(current.Value);
               }
            }
         }

         return result;
      }

      public async Task<Product> GetProduct(Guid productId)
      {
         IReliableDictionary<Guid, Product> products = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");

         using(ITransaction tx = _stateManager.CreateTransaction())
         {
            ConditionalValue<Product> product = await products.TryGetValueAsync(tx, productId);

            return product.HasValue ? product.Value : null;
         }
      }
   }
}
