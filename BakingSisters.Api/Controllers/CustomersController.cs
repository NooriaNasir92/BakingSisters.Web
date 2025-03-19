using BakingSisters.Api.Models;
using BakingSisters.Api.Services.Customers;
using Microsoft.AspNetCore.Mvc;

namespace BakingSisters.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        var customers = await _customerService.GetAllAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        if (customer == null)
            return NotFound();

        return Ok(customer);
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<Customer>> GetCustomerByEmail(string email)
    {
        var customer = await _customerService.GetByEmailAsync(email);
        if (customer == null)
            return NotFound();

        return Ok(customer);
    }

    [HttpGet("{id}/orders")]
    public async Task<ActionResult<IEnumerable<Order>>> GetCustomerOrderHistory(int id)
    {
        var orders = await _customerService.GetCustomerOrderHistoryAsync(id);
        return Ok(orders);
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
    {
        try
        {
            var createdCustomer = await _customerService.CreateAsync(customer);
            return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.Id }, createdCustomer);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
    {
        if (id != customer.Id)
            return BadRequest();

        await _customerService.UpdateAsync(customer);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        await _customerService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("email-exists/{email}")]
    public async Task<ActionResult<bool>> CheckEmailExists(string email)
    {
        var exists = await _customerService.EmailExistsAsync(email);
        return Ok(exists);
    }
} 