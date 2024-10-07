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
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
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
        }




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

        // Updated: Validate ProductId when updating order items.
        public async Task UpdateOrderAsync(string orderId, OrderUpdateDto updateDto)
        {
            // var existingOrder = await GetOrderByIdAsync(orderId);
            // if (existingOrder == null) throw new Exception("Order not found.");

            // if (!string.IsNullOrEmpty(updateDto.DeliveryAddress))
            // {
            //     existingOrder.DeliveryAddress = updateDto.DeliveryAddress;
            // }

            // if (updateDto.Items != null && updateDto.Items.Any())
            // {
            //     foreach (var item in updateDto.Items)
            //     {
            //         // Validate if the product exists before updating the item
            //         var product = await _productRepository.GetProductByIdAsync(item.ProductId);
            //         if (product == null)
            //             throw new Exception($"Product with ID {item.ProductId} not found.");

            //         var existingItem = existingOrder.Items.FirstOrDefault(i => i.ProductId == item.ProductId);

            //         if (existingItem != null)
            //         {
            //             if (existingItem.Status == "Processing")
            //             {
            //                 existingItem.Quantity = item.Quantity;
            //             }
            //             else
            //             {
            //                 throw new InvalidOperationException($"Cannot update item '{existingItem.ProductId}' as it is not in 'Processing' status.");
            //             }
            //         }
            //         else
            //         {
            //             existingOrder.Items.Add(new OrderItem
            //             {
            //                 ProductId = item.ProductId,
            //                 Quantity = item.Quantity,
            //                 Price = item.Price
            //             });
            //         }
            //     }
            // }

            // existingOrder.TotalAmount = existingOrder.Items.Sum(i => i.TotalPrice);
            // await _orderRepository.UpdateOrderAsync(existingOrder);
        }

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
        }

        public async Task UpdateOrderCancelAsync(string orderId, CancellationResponseDto cancellationResponseDto)
        {
            // Fetch the existing order
            var existingOrder = await _orderRepository.GetOrderByIdAsync(orderId);
            if (existingOrder == null) throw new Exception("Order not found.");
            if (existingOrder.Cancellation == null) throw new Exception("No cancellation requested for this order");

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
            }  else {
                throw new Exception("Response is not valid");
            }

            // Save the updated order back to the database
            await _orderRepository.UpdateOrderAsync(existingOrder);
        }

        public async Task UpdateOrderStatusAsync(string orderId, string status)
        {
            // var existingOrder = await GetOrderByIdAsync(orderId);
            // if (existingOrder == null) throw new Exception("Order not found.");

            // existingOrder.Status = status;
            // if (status == "Shipped") existingOrder.DispatchedDate = DateTime.UtcNow;

            // await _orderRepository.UpdateOrderAsync(existingOrder);
        }

        public async Task UpdateOrderItemStatusAsync(string orderId, string productId, string status)
        {
            // var existingOrder = await GetOrderByIdAsync(orderId);
            // if (existingOrder == null) throw new Exception("Order not found.");

            // var orderItem = existingOrder.Items.FirstOrDefault(i => i.ProductId == productId);
            // if (orderItem == null) throw new Exception("Order item not found.");

            // orderItem.Status = status;
            // await _orderRepository.UpdateOrderAsync(existingOrder);
        }


        public async Task<List<VendorOrderDto>> GetOrdersByVendorIdAsync(string vendorId)
        {
            var orders = await _orderRepository.GetOrdersByVendorIdAsync(vendorId);
            if (orders == null || orders.Count == 0)
            {
                return null;
            }

            var orderDtos = orders.Select(order => new VendorOrderDto
            {
                OrderId = order.Id.ToString(),
                OrderDate = order.OrderDate,
                Items = order.Items
                            .Where(item => item.VendorId == vendorId)
                            .Select(item => new VendorOrderItemDto
                            {
                                ProductId = item.ProductId,
                                Quantity = item.Quantity,
                                TotalPrice = item.Quantity * item.Price
                            }).ToList()
            })
            .Where(orderDto => orderDto.Items.Count > 0)
            .ToList(); 
           
            return orderDtos;
        }


    }
}