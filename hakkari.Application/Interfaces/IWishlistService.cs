using Pachakkari.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Application.Interfaces
{
    public interface IWishlistService
    {
        Task<List<WishlistItemDto>> GetMyWishlistAsync(int userId);
        Task<bool> AddToWishlistAsync(int userId, int productId);
        Task<bool> RemoveFromWishlistAsync(int userId, int productId);
    }
}
