using Pachakkari.Application.DTOs;
using Pachakkari.Application.DTOs.Order;

namespace Pachakkari.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDto?> PlaceOrderAsync(int userId,PlaceOrderDto dto);
        Task<List<OrderResponseDto>> GetUserOrdersAsync(int userId);
        Task<OrderResponseDto?> GetOrderByIdAsync(int orderId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);

        Task<List<OrderResponseDto>> GetAllOrdersAsync();
    }
}