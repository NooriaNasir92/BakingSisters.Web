using BakingSisters.Api.Data;
using BakingSisters.Api.Models;
using BakingSisters.Api.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace BakingSisters.Api.Services.Orders;

public class OrderService : BaseService<Order>, IOrderService
{
    public OrderService(BakeryDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Customizations)
                    .ThenInclude(c => c.CustomizationOption)
            .ToListAsync();
    }

    public override async Task<Order?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Customizations)
                    .ThenInclude(c => c.CustomizationOption)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Customizations)
                    .ThenInclude(c => c.CustomizationOption)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Where(o => o.Status == status)
            .OrderBy(o => o.RequiredDate)
            .ToListAsync();
    }

    public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        var order = await _dbSet.FindAsync(orderId);
        if (order == null)
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");

        order.Status = status;
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<decimal> CalculateOrderTotalAsync(int orderId)
    {
        var order = await _dbSet
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Customizations)
                    .ThenInclude(c => c.CustomizationOption)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");

        decimal total = 0;
        foreach (var item in order.OrderItems)
        {
            total += item.UnitPrice * item.Quantity;
            total += item.Customizations.Sum(c => c.CustomizationOption.AdditionalPrice);
        }

        order.TotalAmount = total;
        await _context.SaveChangesAsync();
        return total;
    }

    public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId)
    {
        var order = await _dbSet
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Customizations)
                    .ThenInclude(c => c.CustomizationOption)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        return order?.OrderItems ?? Enumerable.Empty<OrderItem>();
    }

    public async Task AddOrderItemAsync(int orderId, OrderItem orderItem)
    {
        var order = await _dbSet.FindAsync(orderId);
        if (order == null)
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");

        orderItem.OrderId = orderId;
        _context.OrderItems.Add(orderItem);
        await _context.SaveChangesAsync();
        await CalculateOrderTotalAsync(orderId);
    }

    public async Task RemoveOrderItemAsync(int orderId, int orderItemId)
    {
        var orderItem = await _context.OrderItems
            .FirstOrDefaultAsync(oi => oi.Id == orderItemId && oi.OrderId == orderId);

        if (orderItem != null)
        {
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            await CalculateOrderTotalAsync(orderId);
        }
    }

    public async Task UpdateOrderItemQuantityAsync(int orderItemId, int quantity)
    {
        var orderItem = await _context.OrderItems.FindAsync(orderItemId);
        if (orderItem == null)
            throw new KeyNotFoundException($"OrderItem with ID {orderItemId} not found.");

        orderItem.Quantity = quantity;
        await _context.SaveChangesAsync();
        await CalculateOrderTotalAsync(orderItem.OrderId);
    }
} 