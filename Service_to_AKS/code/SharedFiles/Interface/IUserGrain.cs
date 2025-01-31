﻿using Orleans;

namespace SharedFiles.Interface
{
    public interface IUserGrain : IGrainWithIntegerKey
    {
        Task AddToBasket(Guid productId, int quantity);

        Task<string> GetBasket(string id);

        Task ClearBasket();
    }
}
