using Azure.Data.Tables;
using Azure;
using Functions.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Functions.Services
{
    public class TableStorageService<T> : ITableStorageService<T> where T : class, ITableEntity, new()
    {
        private readonly TableClient client;
        private readonly ILogger logger;

        public TableStorageService(TableServiceClient tableServiceClient, string tableName, LoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<TableStorageService<T>>();
            client = tableServiceClient.GetTableClient(tableName);
            var exists = false;
            foreach (var table in tableServiceClient.Query(t => t.Name == tableName))
            {
                exists = true;
            }
            if (!exists)
            {
                client.CreateIfNotExists();
            }
        }


        public async Task<List<T>> GetAsync()
        {
            //TODO - Pagination
            var response = client.QueryAsync<T>().AsPages();
            var list = new List<T>();
            await foreach (var page in response)
            {
                foreach (var item in page.Values)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public async Task<List<T>> GetAsync(string partitionKey)
        {
            var response = client.QueryAsync<T>(x => x.PartitionKey == partitionKey).AsPages();
            var list = new List<T>();
            await foreach (var page in response)
            {
                foreach (var item in page.Values)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public async Task<T> GetAsync(string pKey, string rKey)
        {

            var response = await client.GetEntityAsync<T>(pKey, rKey);
            return response.Value;

        }

        public async Task<T> UpsertAsync(T item)
        {

            var response = await client.UpsertEntityAsync<T>(item, TableUpdateMode.Merge);
            return await GetAsync(item.PartitionKey, item.RowKey);

        }

        public async Task<bool> DeleteAsync(string pKey, string rKey)
        {
            var response = await client.DeleteEntityAsync(pKey, rKey);
            if (!response.IsError)
            {
                return true;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> DeleteBulkAsync(string pKey)
        {
            var response = true;

            var queryResult = client.QueryAsync<TableEntity>(x => x.PartitionKey == pKey, 100, new[] { "RowKey" });
            await foreach (Page<TableEntity> page in queryResult.AsPages())
            {
                Console.WriteLine("This is a new page!");
                foreach (TableEntity qEntity in page.Values)
                {
                    var resp = await client.DeleteEntityAsync(pKey, qEntity.RowKey);
                    response = !resp.IsError && response;
                }
            }

            return response;
        }

        public IEnumerable<T> Get(string filter)
        {
            return client.Query<T>(filter);
        }

        public async Task<List<T>> GetByDateAsync(DateTime fromDate, DateTime toDate)
        {
            var response = client.QueryAsync<T>(x => x.Timestamp >= fromDate && x.Timestamp <= toDate).AsPages();

            var list = new List<T>();

            await foreach (var page in response)
            {
                foreach (var item in page.Values)
                {
                    list.Add(item);
                }
            }

            return list;
        }
        public IEnumerable<T> GetAllRows()
        {
            return client.Query<T>();
        }

    }
}
