using Microsoft.EntityFrameworkCore;
using Pachakkari.Application.Interfaces;
using Pachakkari.Domain.Entities;
using Pachakkari.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;


namespace Pachakkari.Infrastructure.Repositories
{
    public class WishlistRepository:IWishlistRepository
    {
        private readonly PachakkariDbContext _context;
        public WishlistRepository(PachakkariDbContext context)
        {
            _context = context;
        }

        public async Task<Wishlist?> GetByUserIdAsync(int userId)
        {
            return await _context.Wishlists
                .Include(w => w.WishlistItems)
                .ThenInclude(wi => wi.Product)
                .FirstOrDefaultAsync(w => w.UserId == userId);

        }

        public async Task AddAsync(WishlistItem item)
        {
            await _context.WishlistItems.AddAsync(item);
        }

        public async Task<bool> RemoveAsync(int userId, int productId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.WishlistItems)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
            {
                return false;
            }

            var item = wishlist.WishlistItems
                .FirstOrDefault(wi => wi.ProductId == productId);

            if (item == null)
            {
                return false;
            }

            _context.WishlistItems.Remove(item);

            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Wishlist wishlist)
        {
            await _context.Wishlists.AddAsync(wishlist);
            await _context.SaveChangesAsync();
        }

    }
}
