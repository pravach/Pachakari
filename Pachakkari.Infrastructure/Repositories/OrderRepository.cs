using Microsoft.EntityFrameworkCore;
using Pachakkari.Application.Interfaces;
using Pachakkari.Domain.Entities;
using Pachakkari.Infrastructure.Data;

namespace Pachakkari.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly PachakkariDbContext _context;

        public OrderRepository(PachakkariDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            foreach (var order in orders)
            {
                foreach (var item in order.OrderItems)
                {
                    Console.WriteLine(
                        $"OrderItemId: {item.Id}, " +
                        $"ProductId: {item.ProductId}, " +
                        $"ProductName: {item.Product?.Title ?? "NULL"}");
                }
            }

            return orders;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> UpdateStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
                return false;

            order.Status = status;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}