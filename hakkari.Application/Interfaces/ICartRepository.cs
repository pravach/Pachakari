using Pachakkari.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?>GetCartById(int userId);
        Task<CartItem?> GetCartItemAsync(int cartId, int productId);
        Task<Cart> CreateCartAsync(Cart cart);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task RemoveCartItemAsync(CartItem cartItem);

        Task SaveChangesAsync();
        Task ClearCartAsync(int userId);

    }
}
