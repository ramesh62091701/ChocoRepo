using Dapr.Client;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

public class Program
{

    private const string StoreName = "statestore";
    private static async Task Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Hello, World!");
            Thread.Sleep(1000);
            // Save state into the state store
            try
            {
                var client = new DaprClientBuilder().Build();
                await client.SaveStateAsync(StoreName, "Test", "Test1");
                Console.WriteLine("Saving Key = 'Test' and Value = 'Test1'");
                var response = await client.GetStateAsync<string>(StoreName, "Test");
                Console.WriteLine("Getting Test key value = " + response);
                await client.DeleteStateAsync(StoreName, "Test");
                Console.WriteLine("Deleted Test key");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }            

        }
        
    }
}

