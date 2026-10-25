using Pachakkari.Application.DTOs;
using Pachakkari.Application.DTOs.Order;
using Pachakkari.Application.Interfaces;
using Pachakkari.Domain.Entities;
using System.Linq.Expressions;

namespace Pachakkari.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }


        public async Task<OrderResponseDto?> PlaceOrderAsync(
            int userId,
            PlaceOrderDto dto)
        {
            // Get the logged-in user's cart
            var cart = await _cartRepository.GetCartById(userId);

            if (cart == null || cart.CartItems.Count == 0)
                return null;

            // Create the order and save delivery details
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",

                FullName = dto.FullName,
                Phone = dto.Phone,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                Pincode = dto.Pincode,
                PaymentMethod = dto.PaymentMethod,

                // Copy the cart items into the order
                OrderItems = cart.CartItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                }).ToList()
            };

            // Calculate the total on the backend
            order.TotalAmount = order.OrderItems.Sum(
                item => item.Quantity * item.UnitPrice);

            // Save the order
            var savedOrder = await _orderRepository.CreateAsync(order);

            // Clear the cart after saving the order
            foreach (var cartItem in cart.CartItems.ToList())
            {
                await _cartRepository.RemoveCartItemAsync(cartItem);
            }

            await _cartRepository.SaveChangesAsync();

            // Retrieve the saved order with product details
            var orderWithProducts =
                await _orderRepository.GetByIdAsync(savedOrder.Id);

            return orderWithProducts == null
                ? null
                : MapToDto(orderWithProducts);
        }


        public async Task<List<OrderResponseDto>> GetUserOrdersAsync(int userId)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);

            return orders.Select(MapToDto).ToList();
        }

        public async Task<OrderResponseDto?> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            return order == null ? null : MapToDto(order);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            return await _orderRepository.UpdateStatusAsync(orderId, status);
        }

        private static OrderResponseDto MapToDto(Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderDate = order.OrderDate,
                FullName = order.FullName,
                Phone = order.Phone,
                Address = order.Address,
                City = order.City,
                State = order.State,
                Pincode = order.Pincode,
                PaymentMethod = order.PaymentMethod,

                Products = order.OrderItems.Select(item => new OrderItemResponseDto
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,

                    Product = new ProductResponseDto
                    {
                        Id = item.Product.Id,
                        Title = item.Product.Title,
                        Price = item.UnitPrice,
                        Category = item.Product.Category,
                        Image = item.Product.Image,
                        Discription = item.Product.Description
                    }
                }).ToList()
            };
        }

            public async Task<List<OrderResponseDto>> GetAllOrdersAsync()
      
        {
            var orders = await _orderRepository.GetAllAsync();
            var orderResponses = orders.Select(order => new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,

                // Step 3: Convert each order item
                Products = order.OrderItems.Select(item => new OrderItemResponseDto
                {
                    Id = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()

            }).ToList();

            // Step 4: Return all order responses
            return orderResponses;
        }
    }
    }
    
