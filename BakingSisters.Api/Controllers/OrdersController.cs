using BakingSisters.Api.Models;
using BakingSisters.Api.Models.Enum;
using BakingSisters.Api.Services.Orders;
using Microsoft.AspNetCore.Mvc;

namespace BakingSisters.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetCustomerOrders(int customerId)
    {
        var orders = await _orderService.GetCustomerOrdersAsync(customerId);
        return Ok(orders);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByStatus(OrderStatus status)
    {
        var orders = await _orderService.GetOrdersByStatusAsync(status);
        return Ok(orders);
    }

    [HttpGet("{id}/items")]
    public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems(int id)
    {
        var items = await _orderService.GetOrderItemsAsync(id);
        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(Order order)
    {
        var createdOrder = await _orderService.CreateAsync(order);
        return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, Order order)
    {
        if (id != order.Id)
            return BadRequest();

        await _orderService.UpdateAsync(order);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        await _orderService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<Order>> UpdateOrderStatus(int id, [FromBody] OrderStatus status)
    {
        var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, status);
        return Ok(updatedOrder);
    }

    [HttpPost("{id}/items")]
    public async Task<IActionResult> AddOrderItem(int id, OrderItem orderItem)
    {
        await _orderService.AddOrderItemAsync(id, orderItem);
        return NoContent();
    }

    [HttpDelete("{id}/items/{itemId}")]
    public async Task<IActionResult> RemoveOrderItem(int id, int itemId)
    {
        await _orderService.RemoveOrderItemAsync(id, itemId);
        return NoContent();
    }

    [HttpPatch("items/{itemId}/quantity")]
    public async Task<IActionResult> UpdateOrderItemQuantity(int itemId, [FromBody] int quantity)
    {
        await _orderService.UpdateOrderItemQuantityAsync(itemId, quantity);
        return NoContent();
    }

    [HttpGet("{id}/total")]
    public async Task<ActionResult<decimal>> CalculateOrderTotal(int id)
    {
        var total = await _orderService.CalculateOrderTotalAsync(id);
        return Ok(total);
    }
} 