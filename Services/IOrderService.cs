/*
 * File: IOrderService.cs
 * Project: Healthy Bites
 * Description: This file defines the IOrderService interface, which outlines the contract for the OrderService class.
 *              It contains method signatures for handling various order-related operations including creating orders, 
 *              retrieving orders (with pagination), managing order cancellations, and handling vendor-specific orders.
 * 
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyBites.Dtos;
using HealthyBites._Models;

namespace HealthyBites.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderDto createOrderDto, string customerId);
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