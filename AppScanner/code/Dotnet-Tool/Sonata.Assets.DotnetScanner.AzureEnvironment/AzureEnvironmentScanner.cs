using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Sonata.Assets.DotnetScanner.System.Entities;
using Sonata.Assets.Scanner.Core;

namespace Sonata.Assets.DotnetScanner.AzureEnvironment
{


    public class AzureEnvironmentScanner : IScanner<AzureScannerInput, AzureScannerOutput>
    {
        AzureScannerOutput output = new AzureScannerOutput()
        {
            Result = new List<string>()
        };

        public AzureScannerOutput Scan(AzureScannerInput input)
        {
            output.EnvironmentName = input.EnvironmentName;

            try
            {
                ScanSubscription();

                return output;
            }
            catch (Exception ex)
            {
                output.Error = ex;
                return output;
            }
        }

        private async void ScanSubscription()
        {
            //TODO:
            var credentialOptions = new DefaultAzureCredentialOptions() { TenantId = "c8e0e76c-7c24-48a7-8a20-97f1b16b017c" };
            ArmClient client = new ArmClient(new DefaultAzureCredential(credentialOptions));

            SubscriptionResource subscription = await client.GetDefaultSubscriptionAsync();
            ResourceGroupCollection resourceGroupsColl = subscription.GetResourceGroups();
            var resourceGroups = resourceGroupsColl.GetAll();
        }
    }

}