/*
 * File: IOrderService.cs
 * Project: TechFixBackend
 * Description: This file defines the IOrderService interface, which outlines the contract for the OrderService class.
 *              It contains method signatures for handling various order-related operations including creating orders, 
 *              retrieving orders (with pagination), managing order cancellations, and handling vendor-specific orders.
 * 
 * Authors: Kandambige S.T. it21181856 | Perera W.H.T.H. it21165498
 * 
 * Methods:
 * - CreateOrderAsync(CreateOrderDto, string): Creates a new order for the specified customer.
 * - GetAllOrdersAsync(int, int, string): Retrieves a paginated list of all orders for the customer (if specified).
 * - GetAllCancelReqOrdersAsync(int, int, string): Retrieves a paginated list of all cancellation request orders for the customer (if specified).
 * - CancelRequestOrderAsync(string, RequestCancelOrderDto): Submits a cancellation request for a specific order.
 * - UpdateOrderCancelAsync(string, CancellationResponseDto): Updates the status of a cancellation request.
 * - UpdateOrderStatusAsync(string, string): Updates the overall status of a specific order.
 * - UpdateOrderItemStatusAsync(string, string, string): Updates the status of a specific item within an order.
 * - GetOrdersByVendorIdAsync(string): Retrieves all orders associated with a specific vendor.
 * 
 * Notes:
 * - This interface is implemented by the OrderService class to ensure standardization and separation of concerns.
 * 
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend.Dtos;
using TechFixBackend._Models;

namespace TechFixBackend.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderDto createOrderDto , string customerId);
        Task<(List<GetOrderDetailsDto> orders, long totalOrders)> GetAllOrdersAsync(int pageNumber, int pageSize, string customerId = null);
        Task<(List<GetOrderDetailsDto> orders, long totalOrders)> GetOrdersByCustomerIdAsync(string customerId, int pageNumber, int pageSize);
        Task<(List<GetCancelledOrderDetailsDto> orders, long totalOrders)> GetAllCancelReqOrdersAsync(int pageNumber, int pageSize, string customerId = null);
        Task CancelRequestOrderAsync(string orderId, RequestCancelOrderDto cancelOrderDto);
        Task UpdateOrderCancelAsync(string orderId, CancellationResponseDto cancellationResponseDto);
        Task UpdateOrderStatusAsync(string orderId, string status);
        Task UpdateOrderItemStatusAsync(string orderId, string productId, string status);
        Task<List<VendorOrderDto>> GetOrdersByVendorIdAsync(string vendorId);
        Task<GetOrderDetailsDto> GetOrderByIdAsync(string orderId); 
    }
}