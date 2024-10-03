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
        private readonly IUserRepository _userRepository; // Added to fetch Customer details.
        private readonly IProductRepository _productRepository; // Added to fetch Product details.


        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository; // Injecting UserRepository.
            _productRepository = productRepository; // Injecting ProductRepository.
        }



        // Updated: CreateOrderAsync method to validate CustomerId and ProductId.
        // public async Task CreateOrderAsync(CreateOrderDto createOrderDto)
        // {
        //     // Validate if the customer exists
        //     var customer = await _userRepository.GetUserByIdAsync(createOrderDto.CustomerId);
        //     if (customer == null)
        //         throw new Exception("Customer not found."); // CustomerId comes from User table.

        //     // Validate if all products exist before creating the order
        //     foreach (var item in createOrderDto.Items)
        //     {
        //         var product = await _productRepository.GetProductByIdAsync(item.ProductId);
        //         if (product == null)
        //             throw new Exception($"Product with ID {item.ProductId} not found."); // ProductId comes from Product table.
        //     }

        //     // Proceed with creating the order
        //     var order = new Order
        //     {
        //         CustomerId = createOrderDto.CustomerId,
        //         Items = createOrderDto.Items.Select(i => new OrderItem
        //         {
        //             ProductId = i.ProductId,
        //             Quantity = i.Quantity,
        //             Price = i.Price,
        //         }).ToList(),
        //         DeliveryAddress = createOrderDto.DeliveryAddress
        //     };

        //     // Calculate total amount
        //     order.TotalAmount = order.Items.Sum(item => item.TotalPrice);

        //     await _orderRepository.CreateOrderAsync(order);
        // }

        public async Task CreateOrderAsync(CreateOrderDto createOrderDto , String id)
        {
                //     // Validate if the customer exists
            var customer = await _userRepository.GetUserByIdAsync(id);
            if (customer == null)
                throw new Exception("Customer not found."); 

           // Validate if all products exist before creating the order
            foreach (var item in createOrderDto.Items)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product with ID {item.ProductId} not found."); // ProductId comes from Product table.
            }



            var order = new Order
            {
                CustomerId = id,
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



        public async Task<(List<GetOrderDto> orders, long totalOrders)> GetAllOrdersAsync(int pageNumber, int pageSize, string customerId = null)
        {
            var (orders, totalOrders) = await _orderRepository.GetAllOrdersAsync(pageNumber, pageSize, customerId);

            var ordersDto = new List<GetOrderDto>();

            foreach (var order in orders)
            {
                // Fetch Customer Information (Name and Email only)
                var customer = await _userRepository.GetUserByIdAsync(order.CustomerId);

                // Populate Order Items with Product Information
                var orderItemsDto = new List<GetOrderItemDto>();
                foreach (var item in order.Items)
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId);

                    var productDto = product != null ? new ProductWithVendorDto
                    {
                        Id = product.Id,
                        ProductName = product.ProductName,
                        ProductDescription = product.ProductDescription,
                        // Category = product.CategoryId,
                        Price = product.Price,
                        StockQuantity = product.StockQuantity,
                        ProductStatus = product.ProductStatus,
                        ProductImageUrl = product.ProductImageUrl
                    } : null;

                    orderItemsDto.Add(new GetOrderItemDto
                    {
                        ProductId = item.ProductId,
                        Product = productDto, // Full Product with Vendor
                        Quantity = item.Quantity,
                        Price = item.Price,
                        TotalPrice = item.Quantity * (decimal)item.Price,
                        Status = item.Status
                    });
                }

                // Create the Order DTO with customer name and email
                var orderDto = new GetOrderDto
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    CustomerName = customer?.Name ?? "Unknown", // Assuming the customer model has FullName
                    CustomerEmail = customer?.Email ?? "Unknown", // Assuming the customer model has Email
                    DeliveryAddress = order.DeliveryAddress,
                    TotalAmount = orderItemsDto.Sum(i => i.TotalPrice),
                    Status = order.Status,
                    Items = orderItemsDto,
                    OrderDate = order.OrderDate, 
                };

                ordersDto.Add(orderDto);
            }

            return (ordersDto, totalOrders);
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        // Updated: Validate ProductId when updating order items.
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
                    // Validate if the product exists before updating the item
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                    if (product == null)
                        throw new Exception($"Product with ID {item.ProductId} not found.");

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

        public async Task CancelRequestOrderAsync(string orderId, RequestCancelOrderDto cancelOrderDto)
        {
            var existingOrder = await GetOrderByIdAsync(orderId);
            if (existingOrder == null) throw new Exception("Order not found.");

            // Initialize the Cancellation property if it's null
            if (existingOrder.Cancellation == null)
            {
                existingOrder.Cancellation = new Cancellation();
            }

            if (!string.IsNullOrEmpty(cancelOrderDto.Reason))
            {
                existingOrder.Cancellation.Reason = cancelOrderDto.Reason;
            }

            // Set the cancellation as requested
            existingOrder.Cancellation.Requested = true;
            existingOrder.Cancellation.Status = "requested";
            existingOrder.Cancellation.RequestedAt = DateTime.UtcNow; // Optionally add the timestamp

            await _orderRepository.UpdateOrderAsync(existingOrder);
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
