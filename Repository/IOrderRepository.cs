/*
 * File: IOrderRepository.cs
 * Project: TechFixBackend
 * Description: This file defines the IOrderRepository interface, which outlines the contract for the OrderRepository class.
 *              It contains method signatures for performing various data operations related to orders, including
 *              creation, retrieval, updating, and fetching orders based on customer or vendor filters. This interface
 *              provides a consistent way for the service layer to interact with the data layer.
 * 
 * Authors: Kandambige S.T. it21181856 | Perera W.H.T.H. it21165498
 * 
 * Methods:
 * - CreateOrderAsync(Order): Inserts a new order into the database.
 * - GetAllOrdersAsync(int, int, string): Retrieves a paginated list of all orders, with optional customer filtering.
 * - GetAllCancelReqOrdersAsync(int, int, string): Retrieves a paginated list of all orders with cancellation requests, with optional customer filtering.
 * - GetOrderByIdAsync(string): Retrieves a specific order by its unique ID.
 * - UpdateOrderAsync(Order): Updates an existing order document in the database.
 * - GetOrdersByVendorIdAsync(string): Retrieves all orders associated with a specific vendor by filtering items within orders.
 * 
 * Notes:
 * - The repository pattern ensures clean separation of data access logic and allows for easy swapping of data sources.
 * - Pagination is included for both order and cancellation request retrieval methods to improve performance.
 * 
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;

namespace TechFixBackend.Repository
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
