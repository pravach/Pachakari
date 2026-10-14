using Pachakkari.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Application.Interfaces
{
    public interface IWishlistRepository
    {
        Task CreateAsync(Wishlist wishlist);
        Task<Wishlist?> GetByUserIdAsync(int userId);
        Task AddAsync(WishlistItem item);
        Task<bool> RemoveAsync(int userId, int productId);
        Task SaveChangesAsync();
    }
}
