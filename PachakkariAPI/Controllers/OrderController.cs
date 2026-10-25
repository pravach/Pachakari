
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pachakkari.Application.DTOs;
using Pachakkari.Application.Interfaces;
using System.Security.Claims;

namespace PachakkariAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
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

        // Place an order for the logged-in user

        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder(
            [FromBody] PlaceOrderDto dto)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User ID not found in token");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _orderService.PlaceOrderAsync(
                userId.Value,
                dto);

            if (order == null)
            {
                return BadRequest("Cart is empty or cart not found");
            }

            return Ok(order);
        }


        // Get orders belonging to the logged-in user
        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User ID not found in token");
            }

            var orders = await _orderService.GetUserOrdersAsync(
                userId.Value);

            return Ok(orders);
        }

        // Get a specific order
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User ID not found in token");
            }

            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            // Admin can view any order; users can view only their own
            if (!User.IsInRole("Admin") && order.UserId != userId.Value)
            {
                return Forbid();
            }

            return Ok(order);
        }

        // Update order status - Admin only
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(
            int id,
            [FromQuery] string status)
        {
            var result = await _orderService.UpdateOrderStatusAsync(
                id,
                status);

            if (!result)
            {
                return NotFound("Order not found");
            }

            return Ok("Order status updated successfully");
        }

        // Get all orders - Admin only
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            return Ok(orders);
        }
    }
}
