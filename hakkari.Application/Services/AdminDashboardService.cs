using Pachakkari.Application.DTOs;
using Pachakkari.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Application.Services
{
    public class AdminDashboardService:IAdminDashboardService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;

        public AdminDashboardService(
            IProductRepository productRepository,
            IUserRepository userRepository,
            IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;

        }

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var users = await _userRepository.GetAllAsync();
            var orders = await _orderRepository.GetAllAsync();

            return new DashboardSummaryDto
            {
                ProductCount = products.Count,
                UserCount=users.Count,
                OrderCount=orders.Count
            };

        }

        public async Task<List<MonthlyOrdersDto>>GetMonthlyOrdersAsync(int year)
        {
            var orders = await _orderRepository.GetAllAsync();
            var monthlyOrders = orders
        .Where(order => order.OrderDate.Year == year)
        .GroupBy(order => order.OrderDate.Month)
        .Select(group => new MonthlyOrdersDto
        {
            Month = group.Key,
            OrderCount = group.Count()
        })
        .OrderBy(item => item.Month)
        .ToList();

            return monthlyOrders;
        }
    }
}
