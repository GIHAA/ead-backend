/*
 * File: OrderDtos.cs
 * Project: TechFixBackend
 * Description: This file contains Data Transfer Objects (DTOs) related to order operations in the TechFixBackend system.
 *              DTOs are used to transfer data between the client and server layers. They are designed to keep only necessary 
 *              information and omit sensitive or internal fields, ensuring proper abstraction and clean data handling. 
 *              These DTOs are involved in creating orders, updating order statuses, handling cancellations, and retrieving order details.
 * 
 * Author: Kandambige S.T. it21181856
 * 
 * Classes:
 * - CreateOrderDto: Contains the data required to create a new order.
 * - OrderItemDto: Represents individual items in an order, including product, quantity, and price.
 * - OrderStatusUpdateDto: Used to update the status of an order.
 * - RequestCancelOrderDto: Contains the reason for requesting an order cancellation.
 * - CancellationResponseDto: Contains the response to a cancellation request, either "Approved" or "Rejected."
 * - GetOrderItemDto: Represents individual order items when retrieving order details, including product and vendor information.
 * - GetOrderDetailsDto: Provides full order details, including customer, items, and cancellation information.
 * - GetCancelOrderDetailsDto: Provides order details specific to canceled or cancellation-requested orders.
 * - VendorOrderDto: Represents orders assigned to a specific vendor, including items and customer details.
 * - VendorOrderItemDto: Represents individual items in a vendor order, including product and status details.
 * 
 * Notes:
 * - These DTOs are mapped from domain models to simplify data handling between client and server.
 * - Use CreateOrderDto for new order creation and GetOrderDetailsDto for displaying order details to users.
 * - Cancellation and status update DTOs facilitate smooth order lifecycle management.
 * 
 */



namespace TechFixBackend.Dtos
{
    public class CreateOrderDto
    {
       // public string CustomerId { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public string DeliveryAddress { get; set; }
    }

    public class OrderItemDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string Status { get; set; } = "Processing";
    }

   

    public class OrderStatusUpdateDto
    {
        public string Status { get; set; }
    }


    public class RequestCancelOrderDto
    {
        public string Reason { get; set; } // Optional cancellation reason provided by the customer
    }

    public class CancellationResponseDto 
    {
        public string Response { get; set;}
    }
   
    public class GetOrderItemDto
    {
        public string ProductId { get; set; }
        public ProductWithVendorDto Product { get; set; } // Use ProductWithVendorDto for product details
        public int Quantity { get; set; }
        public float Price { get; set; }
        public float TotalPrice { get; set; }
        public string Status { get; set; }
    }

    public class GetOrderDetailsDto
    {
        public string OrderId { get; set; }
        public User Customer { get; set; }
        public string DeliveryAddress { get; set; }
        public float TotalAmount { get; set; }
        public string Status { get; set; }
        public List<GetOrderItemDto> Items { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime? DispatchedDate { get; set; }

       
    }

    public class GetCancelOrderDetailsDto
    {
        public string OrderId { get; set; }
        public User Customer { get; set; }
        public string DeliveryAddress { get; set; }
        public float TotalAmount { get; set; }
        public string Status { get; set; }
        public List<GetOrderItemDto> Items { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime? DispatchedDate { get; set; }
        public CancellationDetailsDto Cancellation { get; set; }
    
    }

      public class VendorOrderDto
    {
        public string OrderId { get; set; } 
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; } 
        public List<VendorOrderItemDto> Items { get; set; } 
    }

  public class VendorOrderItemDto
    {
        public string ProductId { get; set; } 
        public string ProductName { get; set; } 
        public int Quantity { get; set; } 
        public float TotalPrice { get; set; } 
        public string Status { get; set; }
    }
}