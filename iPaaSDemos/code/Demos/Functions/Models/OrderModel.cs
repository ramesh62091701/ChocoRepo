﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Functions.Models
{
    public class OrderModel
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("approver")]
        public string Approver { get; set; }

        [JsonProperty("orderType")]
        public string OrderType { get; set; }

        [JsonProperty("orderDescription")]
        public string OrderDescription { get; set; }

        [JsonProperty("totalAmount")]
        public double TotalAmount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("sellerFullName")]
        public string SellerFullName { get; set; }

        [JsonProperty("sellerFirstName")]
        public string SellerFirstName { get; set; }

        [JsonProperty("sellerLastName")]
        public string SellerLastName { get; set; }

        [JsonProperty("sellerEmail")]
        public string SellerEmail { get; set; }

        [JsonProperty("approvalStatus")]
        public string ApprovalStatus { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("orderStatus")]
        public OrderStatusEnum OrderStatus { get; set; }

        [JsonProperty("items")]
        public List<ProductModel> Items { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
