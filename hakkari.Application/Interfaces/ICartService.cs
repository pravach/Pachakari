using Pachakkari.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Application.Interfaces
{
    public interface ICartService
    {
        Task<Cart> AddToCartAsync(int userId, int productId, int quantity);
        Task<Cart?> GetCartAsync(int userId);
        Task UpdateCartItemAsync(int userId, int productId, int quantity);
        Task RemoveFromCartAsync(int userId, int productId);
    }
}
