using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Models
{
    public static class SettingPropertyNames
    {
        public const string OrderTopic = "OrderTopic";
        public const string OrderSubscription1 = "OrderSubscription1";
        public const string AzureWebJobsStorage = "AzureWebJobsStorage";
        public const string AzureServiceBusConnection = "AzureServiceBusConnection";
        public const string AzureStorageTableName = "AzureStorageTableName";

        public const string FraudDetectionSubscriptionName = "FraudDetectionSubscriptionName";
        public const string HighValueSubscriptionName = "HighValueSubscriptionName";
        public const string ShippingCostSubscriptionName = "ShippingCostSubscriptionName";
    }
}
