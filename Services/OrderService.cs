using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechFixBackend.Dtos;
using TechFixBackend._Models;
using TechFixBackend.Repository;

namespace TechFixBackend.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var order = new Order
            {
                CustomerId = createOrderDto.CustomerId,
                Items = createOrderDto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList(),
                DeliveryAddress = createOrderDto.DeliveryAddress
            };

            order.TotalAmount = order.Items.Sum(item => item.TotalPrice);

            await _orderRepository.CreateOrderAsync(order);
        }

        public async Task<(List<Order> orders, long totalOrders)> GetAllOrdersAsync(int pageNumber, int pageSize, string customerId = null)
        {
            return await _orderRepository.GetAllOrdersAsync(pageNumber, pageSize, customerId);
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task UpdateOrderAsync(string orderId, OrderUpdateDto updateDto)
        {
            var existingOrder = await GetOrderByIdAsync(orderId);
            if (existingOrder == null) throw new Exception("Order not found.");

            if (!string.IsNullOrEmpty(updateDto.DeliveryAddress))
            {
                existingOrder.DeliveryAddress = updateDto.DeliveryAddress;
            }

            if (updateDto.Items != null && updateDto.Items.Any())
            {
                foreach (var item in updateDto.Items)
                {
                    var existingItem = existingOrder.Items.FirstOrDefault(i => i.ProductId == item.ProductId);

                    if (existingItem != null)
                    {
                        if (existingItem.Status == "Processing")
                        {
                            existingItem.Quantity = item.Quantity;
                        }
                        else
                        {
                            throw new InvalidOperationException($"Cannot update item '{existingItem.ProductId}' as it is not in 'Processing' status.");
                        }
                    }
                    else
                    {
                        existingOrder.Items.Add(new OrderItem
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Price = item.Price
                        });
                    }
                }
            }

            existingOrder.TotalAmount = existingOrder.Items.Sum(i => i.TotalPrice);
            await _orderRepository.UpdateOrderAsync(existingOrder);
        }

        public async Task CancelOrderAsync(string orderId)
        {
            var existingOrder = await GetOrderByIdAsync(orderId);
            if (existingOrder == null) throw new Exception("Order not found.");

            await _orderRepository.CancelOrderAsync(existingOrder);
        }

        public async Task UpdateOrderStatusAsync(string orderId, string status)
        {
            var existingOrder = await GetOrderByIdAsync(orderId);
            if (existingOrder == null) throw new Exception("Order not found.");

            existingOrder.Status = status;
            if (status == "Shipped") existingOrder.DispatchedDate = DateTime.UtcNow;

            await _orderRepository.UpdateOrderAsync(existingOrder);
        }

        public async Task UpdateOrderItemStatusAsync(string orderId, string productId, string status)
        {
            var existingOrder = await GetOrderByIdAsync(orderId);
            if (existingOrder == null) throw new Exception("Order not found.");

            var orderItem = existingOrder.Items.FirstOrDefault(i => i.ProductId == productId);
            if (orderItem == null) throw new Exception("Order item not found.");

            orderItem.Status = status;
            await _orderRepository.UpdateOrderAsync(existingOrder);
        }
    }
}
