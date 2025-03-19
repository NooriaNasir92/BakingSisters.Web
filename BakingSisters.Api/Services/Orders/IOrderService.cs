using BakingSisters.Api.Models;
using BakingSisters.Api.Models.Enum;

namespace BakingSisters.Api.Services.Orders;

public interface IOrderService : IBaseService<Order>
{
    Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
    Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status);
    Task<decimal> CalculateOrderTotalAsync(int orderId);
    Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId);
    Task AddOrderItemAsync(int orderId, OrderItem orderItem);
    Task RemoveOrderItemAsync(int orderId, int orderItemId);
    Task UpdateOrderItemQuantityAsync(int orderItemId, int quantity);
} 