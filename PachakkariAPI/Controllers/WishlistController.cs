
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pachakkari.Application.DTOs;
using Pachakkari.Application.Interfaces;
using System.Security.Claims;

namespace PachakkariAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WishlistController : ControllerBase
{
    private readonly IWishlistService _wishlistService;

    public WishlistController(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    // GET: api/Wishlist/my-wishlist
    [HttpGet("my-wishlist")]
    public async Task<IActionResult> GetMyWishlist()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var wishlist = await _wishlistService.GetMyWishlistAsync(userId);

        return Ok(wishlist);
    }

    // POST: api/Wishlist/add
    [HttpPost("add")]
    public async Task<IActionResult> AddToWishlist(
        [FromBody] AddWishlistItemDto dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var added = await _wishlistService.AddToWishlistAsync(
            userId,
            dto.ProductId);

        if (!added)
        {
            return BadRequest(
                "The product is already in your wishlist or could not be added.");
        }

        return Ok(new { message = "Product added to wishlist successfully." });
    }

    // DELETE: api/Wishlist/remove/3
    [HttpDelete("remove/{productId:int}")]
    public async Task<IActionResult> RemoveFromWishlist(int productId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var removed = await _wishlistService.RemoveFromWishlistAsync(
            userId,
            productId);

        if (!removed)
        {
            return NotFound(
                "The product was not found in your wishlist.");
        }

        return Ok(new { message = "Product removed from wishlist successfully." });
    }
}