using Pachakkari.Application.Interfaces;
using Pachakkari.Domain.Entities;

namespace Pachakkari.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(
            ICartRepository cartRepository,
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<Cart> AddToCartAsync(
            int userId,
            int productId,
            int quantity)
        {
            // Check quantity
            if (quantity <= 0)
            {
                throw new Exception("Quantity must be greater than zero");
            }

            // Check whether product exists
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            // Get user's cart
            var cart = await _cartRepository.GetCartById(userId);

            // If cart doesn't exist, create one
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                await _cartRepository.CreateCartAsync(cart);

                // Save so Cart.Id is generated
                await _cartRepository.SaveChangesAsync();
            }

            // Check whether product is already in cart
            var existingItem = await _cartRepository.GetCartItemAsync(
                cart.Id,
                productId);

            if (existingItem != null)
            {
                // Product already exists
                // Increase quantity
                existingItem.Quantity += quantity;

                await _cartRepository.UpdateCartItemAsync(existingItem);
            }
            else
            {
                // Product is not in cart
                // Create new CartItem
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };

                await _cartRepository.AddCartItemAsync(cartItem);
            }

            // Save changes
            await _cartRepository.SaveChangesAsync();

            // Return updated cart
            return await _cartRepository.GetCartById(userId)
                   ?? cart;
        }

        public async Task<Cart?> GetCartAsync(int userId)
        {
            return await _cartRepository.GetCartById(userId);
        }

        public async Task UpdateCartItemAsync(
            int userId,
            int productId,
            int quantity)
        {
            // Quantity must be greater than zero
            if (quantity <= 0)
            {
                throw new Exception("Quantity must be greater than zero");
            }

            // Get user's cart
            var cart = await _cartRepository.GetCartById(userId);

            if (cart == null)
            {
                throw new Exception("Cart not found");
            }

            // Find the product inside the cart
            var cartItem = await _cartRepository.GetCartItemAsync(
                cart.Id,
                productId);

            if (cartItem == null)
            {
                throw new Exception("Product is not in the cart");
            }

            // Update quantity
            cartItem.Quantity = quantity;

            await _cartRepository.UpdateCartItemAsync(cartItem);

            // Save changes
            await _cartRepository.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(
            int userId,
            int productId)
        {
            // Get user's cart
            var cart = await _cartRepository.GetCartById(userId);

            if (cart == null)
            {
                throw new Exception("Cart not found");
            }

            // Find the product inside the cart
            var cartItem = await _cartRepository.GetCartItemAsync(
                cart.Id,
                productId);

            if (cartItem == null)
            {
                throw new Exception("Product is not in the cart");
            }

            // Remove item
            await _cartRepository.RemoveCartItemAsync(cartItem);

            // Save changes
            await _cartRepository.SaveChangesAsync();
        }
    }
}