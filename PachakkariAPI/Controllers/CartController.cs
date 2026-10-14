using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pachakkari.Application.Interfaces;

namespace PachakkariAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(
            int userId,
            int productId,
            int quantity)
        {
            try
            {
                var cart = await _cartService.AddToCartAsync(
                    userId,
                    productId,
                    quantity);

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCart(int userId)
        {
            try
            {
                var cart = await _cartService.GetCartAsync(userId);

                if (cart == null)
                {
                    return NotFound("Cart not found");
                }

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateCartItem(
            int productId,
            int userId,
            int quantity)
        {
            try
            {
                await _cartService.UpdateCartItemAsync(
                    userId,
                    productId,
                    quantity);

                return Ok("Cart item updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart(
            int productId,
            int userId)
        {
            try
            {
                await _cartService.RemoveFromCartAsync(
                    userId,
                    productId);

                return Ok("Product removed from cart");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}