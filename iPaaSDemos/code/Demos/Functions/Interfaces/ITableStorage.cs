using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Data.Tables;

namespace Functions.Interfaces
{
    public interface ITableStorageService<T> where T : class, ITableEntity, new()
    {
        Task<List<T>> GetAsync();

        Task<List<T>> GetAsync(string partitionKey);

        Task<T?> GetAsync(string pKey, string rKey);

        Task<T> UpsertAsync(T item);

        Task<bool> DeleteAsync(string pKey, string rKey);

        Task<bool> DeleteBulkAsync(string pKey);

        IEnumerable<T> Get(string filter);

        Task<List<T>> GetByDateAsync(DateTime fromDate, DateTime toDate);

        IEnumerable<T> GetAllRows();
    }
}
