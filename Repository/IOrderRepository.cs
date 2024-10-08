/*
 * File: IOrderRepository.cs
 * Project: Healthy Bites
 * Description: This file defines the IOrderRepository interface, which outlines the contract for the OrderRepository class.
 *              It contains method signatures for performing various data operations related to orders, including
 *              creation, retrieval, updating, and fetching orders based on customer or vendor filters. This interface
 *              provides a consistent way for the service layer to interact with the data layer.
 * 
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyBites._Models;

namespace HealthyBites.Repository
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task<(List<Order> orders, long totalOrders)> GetAllOrdersAsync(int pageNumber, int pageSize, string customerId = null);
        Task<(List<Order> orders, long totalOrders)> GetAllCancelReqOrdersAsync(int pageNumber, int pageSize, string customerId = null);
        Task<Order> GetOrderByIdAsync(string orderId);
        Task UpdateOrderAsync(Order order);
        Task<List<Order>> GetOrdersByVendorIdAsync(string vendorId);
    }
}
