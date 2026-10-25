using Microsoft.EntityFrameworkCore;
using Pachakkari.Application.Interfaces;
using Pachakkari.Domain.Entities;
using Pachakkari.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Infrastructure.Repositories
{
    public class CartRepository:ICartRepository
    {
        private readonly PachakkariDbContext _context;

        public CartRepository(PachakkariDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartById(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<CartItem?>GetCartItemAsync(int cartId,int productId)
        {
            return await _context.CartItems.FirstOrDefaultAsync(ci =>
            ci.CartId == cartId &&
            ci.ProductId == productId);
        }

        public async Task<Cart>CreateCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            return cart;
        }

        public async Task<CartItem>AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            return cartItem;
        }

        public Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            return Task.CompletedTask;
        }

        public Task RemoveCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return;

            _context.CartItems.RemoveRange(cart.CartItems);

            await _context.SaveChangesAsync();
        }


    }
}
