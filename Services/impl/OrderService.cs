/*
 * File: OrderService.cs
 * Project: Healthy Bites
 * Description: This file contains the implementation of the OrderService class which handles all operations related to orders. 
 *              This includes creating orders, retrieving orders (with pagination), handling order cancellation requests, 
 *              updating order statuses, and managing vendor-specific orders. 
 *              The class depends on repositories for orders, users, and products to perform its operations.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyBites.Dtos;
using HealthyBites._Models;
using HealthyBites.Repository;

namespace HealthyBites.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly NotificationService _notificationService;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository, NotificationService notificationService)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _notificationService = notificationService;
        }


        public async Task CreateOrderAsync(CreateOrderDto createOrderDto, String id)
        {
            var customer = await _userRepository.GetUserByIdAsync(id);

            if (customer == null)
                throw new Exception("Customer not found.");

            var orderItems = new List<OrderItem>();

            foreach (var item in createOrderDto.Items)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);

                if (product == null)
                    throw new Exception($"Product with ID {item.ProductId} not found.");

                var decreasedProduct = await _productRepository.DecreaseProductQuantityAsync(item.ProductId, item.Quantity);

                if (!decreasedProduct)
                    throw new Exception($"Product with ID {item.ProductId} did not have enough stock.");

                var vendor = await _userRepository.GetUserByIdAsync(product.VendorId);

                if (vendor == null)
                    throw new Exception($"Vendor with ID {product.VendorId} not found.");

                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    VendorId = product.VendorId
                });
            }

            var order = new Order
            {
                CustomerId = id,
                Items = orderItems,
                DeliveryAddress = createOrderDto.DeliveryAddress,
            };

            order.TotalAmount = order.Items.Sum(item => item.TotalPrice);

            await _orderRepository.CreateOrderAsync(order);
            foreach (var item in order.Items)
            {
                await _notificationService.SendNotificationWithDetailsAsync(customer.Id, "Your order has been placed.", item.ProductId, order.Id);
            }
        }

        //get all orders and handle pagination
        public async Task<(List<GetOrderDetailsDto> orders, long totalOrders)> GetAllOrdersAsync(int pageNumber, int pageSize, string customerId = null)
        {
            var (orders, totalOrders) = await _orderRepository.GetAllOrdersAsync(pageNumber, pageSize, customerId);

            if (orders == null || !orders.Any())
            {
                return (new List<GetOrderDetailsDto>(), 0);
            }

            var orderDtos = new List<GetOrderDetailsDto>();
            foreach (var order in orders)
            {
                var orderItems = new List<GetOrderItemDto>();
                foreach (var item in order.Items)
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        throw new Exception($"Product with ID {item.ProductId} not found.");
                    }
                    var vendor = await _userRepository.GetUserByIdAsync(product.VendorId);
                    if (vendor == null)
                    {
                        throw new Exception($"Vendor with ID {product.VendorId} not found.");
                    }

                    orderItems.Add(new GetOrderItemDto
                    {
                        ProductId = item.ProductId,
                        Product = new ProductWithVendorDto
                        {
                            Vendor = vendor,
                            ProductName = product.ProductName,
                        },
                        Quantity = item.Quantity,
                        Price = item.Price,
                        TotalPrice = item.TotalPrice,
                        Status = item.Status
                    });
                }
                var customer = await _userRepository.GetUserByIdAsync(order.CustomerId);
                if (customer == null)
                {
                    throw new Exception($"Customer with ID {order.CustomerId} not found.");
                }

                orderDtos.Add(new GetOrderDetailsDto
                {
                    OrderId = order.Id,
                    Customer = customer,
                    DeliveryAddress = order.DeliveryAddress,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    Items = orderItems,
                    OrderDate = order.OrderDate,
                    DeliveryStatus = order.DeliveryStatus,
                    DispatchedDate = order.DispatchedDate
                });
            }

            return (orderDtos, totalOrders);
        }

        // The new GetOrdersByCustomerIdAsync function
        // The new GetOrdersByCustomerIdAsync function
        public async Task<(List<GetOrderDetailsDto> orders, long totalOrders)> GetOrdersByCustomerIdAsync(string customerId, int pageNumber, int pageSize)
        {
            var (orders, totalOrders) = await _orderRepository.GetAllOrdersAsync(pageNumber, pageSize, customerId);

            if (orders == null || !orders.Any())
            {
                return (new List<GetOrderDetailsDto>(), 0);
            }

            var orderDtos = new List<GetOrderDetailsDto>();

            foreach (var order in orders)
            {
                var orderItems = new List<GetOrderItemDto>();
                foreach (var item in order.Items)
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        throw new Exception($"Product with ID {item.ProductId} not found.");
                    }

                    var vendor = await _userRepository.GetUserByIdAsync(product.VendorId);
                    if (vendor == null)
                    {
                        throw new Exception($"Vendor with ID {product.VendorId} not found.");
                    }

                    orderItems.Add(new GetOrderItemDto
                    {
                        ProductId = item.ProductId,
                        Product = new ProductWithVendorDto
                        {
                            Vendor = vendor,
                            ProductName = product.ProductName,
                        },
                        Quantity = item.Quantity,
                        Price = item.Price,
                        TotalPrice = item.TotalPrice,
                        Status = item.Status
                    });
                }

                var customer = await _userRepository.GetUserByIdAsync(order.CustomerId);
                if (customer == null)
                {
                    throw new Exception($"Customer with ID {order.CustomerId} not found.");
                }

                orderDtos.Add(new GetOrderDetailsDto
                {
                    OrderId = order.Id,
                    Customer = customer,
                    DeliveryAddress = order.DeliveryAddress,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    Items = orderItems,
                    OrderDate = order.OrderDate,
                    DeliveryStatus = order.DeliveryStatus,
                    DispatchedDate = order.DispatchedDate
                });
            }

            return (orderDtos, totalOrders);
        }

        public async Task<(List<GetCancelledOrderDetailsDto> orders, long totalOrders)> GetAllCancelReqOrdersAsync(int pageNumber, int pageSize, string customerId = null)
        {
            var (orders, totalOrders) = await _orderRepository.GetAllCancelReqOrdersAsync(pageNumber, pageSize, customerId);

            if (orders == null || !orders.Any())
            {
                return (new List<GetCancelledOrderDetailsDto>(), 0);
            }

            var orderDtos = new List<GetCancelledOrderDetailsDto>();
            foreach (var order in orders)
            {
                var orderItems = new List<GetOrderItemDto>();
                foreach (var item in order.Items)
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId);

                    var vendor = await _userRepository.GetUserByIdAsync(product.VendorId);

                    orderItems.Add(new GetOrderItemDto
                    {
                        ProductId = item.ProductId,
                        Product = new ProductWithVendorDto
                        {
                            Vendor = vendor,
                            ProductName = product.ProductName,
                        },
                        Quantity = item.Quantity,
                        Price = item.Price,
                        TotalPrice = item.TotalPrice,
                        Status = item.Status
                    });
                }

                var customer = await _userRepository.GetUserByIdAsync(order.CustomerId);
                if (customer == null)
                {
                    throw new Exception($"Customer with ID {order.CustomerId} not found.");
                }
                // Map cancellation details
                var cancellationDto = new CancellationDetailsDto
                {
                    Requested = order.Cancellation.Requested,
                    Status = order.Cancellation.Status,
                    Reason = order.Cancellation.Reason,
                    RequestedAt = (DateTime)order.Cancellation.RequestedAt,
                    ResolvedAt = order.Cancellation.ResolvedAt
                };

                orderDtos.Add(new GetCancelledOrderDetailsDto
                {
                    OrderId = order.Id,
                    Customer = customer,
                    DeliveryAddress = order.DeliveryAddress,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    Items = orderItems,
                    OrderDate = order.OrderDate,
                    DeliveryStatus = order.DeliveryStatus,
                    DispatchedDate = order.DispatchedDate,
                    Cancellation = cancellationDto
                });
            }

            return (orderDtos, totalOrders);
        }



        public async Task<GetOrderDetailsDto> GetOrderByIdAsync(string orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception($"Order with ID {orderId} not found.");
            }

            var customer = await _userRepository.GetUserByIdAsync(order.CustomerId);
            if (customer == null)
            {
                throw new Exception($"Customer with ID {order.CustomerId} not found.");
            }

            var orderItems = new List<GetOrderItemDto>();
            foreach (var item in order.Items)
            {

                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Product with ID {item.ProductId} not found.");
                }


                var vendor = await _userRepository.GetUserByIdAsync(product.VendorId);
                if (vendor == null)
                {
                    throw new Exception($"Vendor with ID {product.VendorId} not found.");
                }

                orderItems.Add(new GetOrderItemDto
                {
                    ProductId = item.ProductId,
                    Product = new ProductWithVendorDto
                    {
                        Vendor = vendor,
                        ProductName = product.ProductName,
                        ProductImageUrl = product.ProductImageUrl

                    },
                    Quantity = item.Quantity,
                    Price = item.Price,
                    TotalPrice = item.TotalPrice,
                    Status = item.Status
                });
            }

            var orderDto = new GetOrderDetailsDto
            {
                OrderId = order.Id,
                Customer = customer,
                DeliveryAddress = order.DeliveryAddress,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Items = orderItems,
                OrderDate = order.OrderDate,
                DeliveryStatus = order.DeliveryStatus,
                DispatchedDate = order.DispatchedDate
            };

            return orderDto;
        }

        //send order cancellation request
        public async Task CancelRequestOrderAsync(string orderId, RequestCancelOrderDto cancelOrderDto)
        {
            // Fetch the actual Order entity, not the DTO
            var existingOrder = await _orderRepository.GetOrderByIdAsync(orderId);
            if (existingOrder == null) throw new Exception("Order not found.");

            // Initialize the Cancellation property if it's null
            if (existingOrder.Cancellation == null)
            {
                existingOrder.Cancellation = new Cancellation();
            }

            if (cancelOrderDto.Reason != null)
            {
                existingOrder.Cancellation.Reason = cancelOrderDto.Reason;
            }
            else
            {
                throw new Exception("No reason provided!");
            }

            // Set the cancellation as requested
            existingOrder.Cancellation.Requested = true;
            existingOrder.Cancellation.Status = "Requested";
            existingOrder.Cancellation.RequestedAt = DateTime.UtcNow; // Optionally add the timestamp

            // Persist the updated order entity to the database
            await _orderRepository.UpdateOrderAsync(existingOrder);

            var customer = await _userRepository.GetUserByIdAsync(existingOrder.CustomerId);
            foreach (var item in existingOrder.Items)
            {
                await _notificationService.SendNotificationWithDetailsAsync(customer.Id, "Your cancellation request has been submitted.", item?.ProductId, existingOrder.Id);
            }
        }

        //update order cancellation status
        // public async Task UpdateOrderCancelAsync(string orderId, CancellationResponseDto cancellationResponseDto)
        // {
        //     // Fetch the existing order
        //     var existingOrder = await _orderRepository.GetOrderByIdAsync(orderId);
        //     if (existingOrder == null) throw new Exception("Order not found.");
        //     if (existingOrder.Cancellation == null) throw new Exception("No cancellation requested for this order");

        //     // Fetch the customer associated with the order for notification
        //     var customer = await _userRepository.GetUserByIdAsync(existingOrder.CustomerId);
        //     if (customer == null) throw new Exception("Customer not found.");

        //     // Check if the cancellation request is approved
        //     if (cancellationResponseDto.Response == "Approved")
        //     {
        //         // Check if the order's cancellation has already been rejected
        //         if (existingOrder.Cancellation.Status == "Rejected")
        //         {
        //             throw new Exception("This cancellation has already been rejected and cannot be approved.");
        //         }

        //         // Loop through all the items in the order and set their status to "Cancelled"
        //         foreach (var item in existingOrder.Items)
        //         {
        //             item.Status = "Cancelled";
        //         }

        //         // Set the order status to "Cancelled"
        //         existingOrder.Status = "Cancelled";

        //         // Set the cancellation status to "Approved"
        //         existingOrder.Cancellation.Status = "Approved";

        //         // Set the ResolvedAt to the current time
        //         existingOrder.Cancellation.ResolvedAt = DateTime.Now;

        //         // Send notification to the customer
        //         string message = "Your cancellation request has been approved. Your order has been cancelled.";
        //         await _notificationService.SendNotificationWithDetailsAsync(customer.Id, message, null, orderId);

        //         // Optionally notify admin on web
        //         string adminMessage = $"Order ID: {orderId} has been cancelled by the user: {customer.Email}.";
        //         await _notificationService.SendNotificationToAdminAsync(adminMessage);

        //     }
        //     else if (cancellationResponseDto.Response == "Rejected")
        //     {
        //         // Check if the order's cancellation has already been approved
        //         if (existingOrder.Cancellation.Status == "Approved")
        //         {
        //             throw new Exception("This cancellation has already been approved and cannot be rejected.");
        //         }

        //         // Set the cancellation status to "Rejected"
        //         existingOrder.Cancellation.Status = "Rejected";

        //         // Set the ResolvedAt to the current time
        //         existingOrder.Cancellation.ResolvedAt = DateTime.Now;

        //         // Send notification to the customer
        //         string message = "Your cancellation request has been rejected.";
        //         await _notificationService.SendNotificationWithDetailsAsync(customer.Id, message, null, orderId);

        //         // Optionally notify admin on web
        //         string adminMessage = $"Order ID: {orderId} cancellation has been rejected by the user: {customer.Email}.";
        //         await _notificationService.SendNotificationToAdminAsync(adminMessage);
        //     }
        //     else
        //     {
        //         throw new Exception("Response is not valid");
        //     }

        //     // Save the updated order back to the database
        //     await _orderRepository.UpdateOrderAsync(existingOrder);
        // }

        public async Task UpdateOrderCancelAsync(string orderId, CancellationResponseDto cancellationResponseDto)
        {
            // Fetch the existing order
            var existingOrder = await _orderRepository.GetOrderByIdAsync(orderId);
            if (existingOrder == null)
            {
                throw new Exception("Order not found.");
            }

            if (existingOrder.Cancellation == null)
            {
                throw new Exception("No cancellation requested for this order.");
            }

            // Fetch the customer associated with the order for notification
            var customer = await _userRepository.GetUserByIdAsync(existingOrder.CustomerId);
            if (customer == null)
            {
                throw new Exception("Customer not found.");
            }

            // Check if the cancellation request is approved
            if (cancellationResponseDto.Response == "Approved")
            {
                // Check if the order's cancellation has already been rejected
                if (existingOrder.Cancellation.Status == "Rejected")
                {
                    throw new Exception("This cancellation has already been rejected and cannot be approved.");
                }

                // Loop through all the items in the order and set their status to "Cancelled"
                foreach (var item in existingOrder.Items)
                {
                    item.Status = "Cancelled";
                }

                // Set the order status to "Cancelled"
                existingOrder.Status = "Cancelled";

                // Set the cancellation status to "Approved"
                existingOrder.Cancellation.Status = "Approved";

                // Set the ResolvedAt to the current time
                existingOrder.Cancellation.ResolvedAt = DateTime.Now;

                // Send notification to the customer
                string message = "Your cancellation request has been approved. Your order has been cancelled.";
                await _notificationService.SendNotificationWithDetailsAsync(customer.Id, message, null, orderId);

                // Optionally notify admin on web
                string adminMessage = $"Order ID: {orderId} has been cancelled by the user: {customer.Email}.";
                await _notificationService.SendNotificationToAdminAsync(adminMessage);

                // Call to notify the customer about the cancellation status
                await GetCancellationStatus(orderId);
            }
            else if (cancellationResponseDto.Response == "Rejected")
            {
                // Check if the order's cancellation has already been approved
                if (existingOrder.Cancellation.Status == "Approved")
                {
                    throw new Exception("This cancellation has already been approved and cannot be rejected.");
                }

                // Set the cancellation status to "Rejected"
                existingOrder.Cancellation.Status = "Rejected";

                // Set the ResolvedAt to the current time
                existingOrder.Cancellation.ResolvedAt = DateTime.Now;

                // Send notification to the customer
                string message = "Your cancellation request has been rejected.";
                await _notificationService.SendNotificationWithDetailsAsync(customer.Id, message, null, orderId);

                // Optionally notify admin on web
                string adminMessage = $"Order ID: {orderId} cancellation has been rejected by the user: {customer.Email}.";
                await _notificationService.SendNotificationToAdminAsync(adminMessage);

                // Call to notify the customer about the cancellation status
                await GetCancellationStatus(orderId);
            }
            else
            {
                throw new Exception("Response is not valid.");
            }

            // Save the updated order back to the database
            await _orderRepository.UpdateOrderAsync(existingOrder);
        }

        //update order status
        public async Task UpdateOrderStatusAsync(string orderId, string status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            order.Status = status;

            await _orderRepository.UpdateOrderAsync(order);
        }

        //update order item status
        public async Task UpdateOrderItemStatusAsync(string orderId, string productId, string newStatus)
        {

            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            var item = order.Items.FirstOrDefault(i => i.ProductId.ToString() == productId);
            if (item == null)
            {
                throw new Exception("Product not found in the order.");
            }

            if (item.Status == newStatus)
            {
                throw new Exception("The status is already set to the selected value.");
            }

            item.Status = newStatus;

            await _orderRepository.UpdateOrderAsync(order);
        }


        public async Task<List<VendorOrderDto>> GetOrdersByVendorIdAsync(string vendorId)
        {
            var orders = await _orderRepository.GetOrdersByVendorIdAsync(vendorId);
            if (orders == null || orders.Count == 0)
            {
                return null;
            }

            var orderDtos = new List<VendorOrderDto>();

            foreach (var order in orders)
            {
                var customer = await _userRepository.GetUserByIdAsync(order.CustomerId);

                if (customer == null)
                {
                    continue;
                }

                var orderDto = new VendorOrderDto
                {
                    OrderId = order.Id.ToString(),
                    CustomerName = customer.Name,
                    OrderDate = order.OrderDate,
                    Items = new List<VendorOrderItemDto>()
                };

                foreach (var item in order.Items.Where(item => item.VendorId == vendorId))
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId);

                    if (product == null)
                    {
                        continue;
                    }

                    var orderItemDto = new VendorOrderItemDto
                    {
                        ProductId = item.ProductId.ToString(),
                        ProductName = product.ProductName,
                        Quantity = item.Quantity,
                        TotalPrice = item.Quantity * item.Price,
                        Status = item.Status
                    };

                    orderDto.Items.Add(orderItemDto);
                }

                if (orderDto.Items.Count > 0)
                {
                    orderDtos.Add(orderDto);
                }
            }

            return orderDtos;
        }

        public async Task<string> GetCancellationStatus(string orderId)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(orderId);
            if (existingOrder == null)
            {
                throw new Exception("Order not found.");
            }

            // Get the cancellation status
            string status = existingOrder.Cancellation?.Status ?? "No cancellation requested.";

            // Prepare the notification message
            string notificationMessage = status switch
            {
                "Approved" => $"Your cancellation request for Order ID {orderId} has been approved.",
                "Rejected" => $"Your cancellation request for Order ID {orderId} has been rejected.",
                _ => $"There is currently no cancellation request for Order ID {orderId}."
            };

            // Send the notification to the user
            var customer = await _userRepository.GetUserByIdAsync(existingOrder.CustomerId);
            if (customer != null)
            {
                await _notificationService.SendNotificationToUserAsync(customer.Id, notificationMessage);
            }

            // Return the cancellation status
            return status;
        }

    }
}