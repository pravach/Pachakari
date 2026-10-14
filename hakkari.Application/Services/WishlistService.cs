using Pachakkari.Application.DTOs;
using Pachakkari.Application.Interfaces;
using Pachakkari.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Application.Services
{
    public class WishlistService:IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        public WishlistService(IWishlistRepository wishlistRepository) {
            _wishlistRepository= wishlistRepository;


        }

        public async Task<List<WishlistItemDto>> GetMyWishlistAsync(int userId)
        {
            var wishlist = await _wishlistRepository.GetByUserIdAsync(userId);

            if (wishlist == null)
            {
                return new List<WishlistItemDto>();
            }

            return wishlist.WishlistItems.Select(wi => new WishlistItemDto
            {
                Id = wi.Id,
                ProductId = wi.ProductId,
                Name = wi.Product.Title,
                Price = wi.Product.Price,
                Image = wi.Product.Image,
                Category = wi.Product.Category
            }).ToList();
        }


        public async Task<bool> AddToWishlistAsync(int userId, int productId)
        {
            // Step 1: Get the user's existing wishlist
            var wishlist = await _wishlistRepository.GetByUserIdAsync(userId);

            // Step 2: If the wishlist doesn't exist, create it
            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    UserId = userId
                };

                // Save the new wishlist to the database
                await _wishlistRepository.CreateAsync(wishlist);
            }

            // Step 3: Check whether the product is already added
            if (wishlist.WishlistItems.Any(wi => wi.ProductId == productId))
            {
                return false;
            }

            // Step 4: Create a wishlist item
            var item = new WishlistItem
            {
                WishlistId = wishlist.Id,
                ProductId = productId
            };

            // Step 5: Add the item to the database
            await _wishlistRepository.AddAsync(item);

            // Step 6: Save the item
            await _wishlistRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveFromWishlistAsync(int userId, int productId)
        {
            var removed = await _wishlistRepository.RemoveAsync(userId, productId);

            if (removed)
            {
                await _wishlistRepository.SaveChangesAsync();
            }

            return removed;
        }



    }
}
