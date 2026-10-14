using Pachakkari.Domain.Entities;

namespace Pachakkari.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<List<Order>> GetByUserIdAsync(int userId);
        Task<bool> UpdateStatusAsync(int orderId, string status);
    }
}