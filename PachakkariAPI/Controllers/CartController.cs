using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pachakkari.Application.Interfaces;
using System.Security.Claims;


namespace PachakkariAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Get the logged-in user's ID from the JWT token
        private int? GetUserId()
        {
            var userIdValue = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdValue, out var userId))
            {
                return userId;
            }

            return null;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(
    int productId,
    int quantity)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized("User ID not found in token");

            // Add the validation here
            if (quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            try
            {
                var cart = await _cartService.AddToCartAsync(
                    userId.Value,
                    productId,
                    quantity);

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized("User ID not found in token");

            try
            {
                var cart = await _cartService.GetCartAsync(
                    userId.Value);

                if (cart == null)
                    return NotFound("Cart not found");

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateCartItem(
    int productId,
    int quantity)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized("User ID not found in token");

            if (quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            try
            {
                await _cartService.UpdateCartItemAsync(
                    userId.Value,
                    productId,
                    quantity);

                return Ok("Cart item updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart(
            int productId)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized("User ID not found in token");

            try
            {
                await _cartService.RemoveFromCartAsync(
                    userId.Value,
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
