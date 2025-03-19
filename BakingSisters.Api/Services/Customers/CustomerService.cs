using BakingSisters.Api.Data;
using BakingSisters.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BakingSisters.Api.Services.Customers;

public class CustomerService : BaseService<Customer>, ICustomerService
{
    public CustomerService(BakeryDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _dbSet
            .Include(c => c.Orders)
            .ToListAsync();
    }

    public override async Task<Customer?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<IEnumerable<Order>> GetCustomerOrderHistoryAsync(int customerId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Customizations)
                    .ThenInclude(c => c.CustomizationOption)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbSet.AnyAsync(c => c.Email == email);
    }

    public override async Task<Customer> CreateAsync(Customer customer)
    {
        if (await EmailExistsAsync(customer.Email))
            throw new InvalidOperationException($"A customer with email {customer.Email} already exists.");

        return await base.CreateAsync(customer);
    }
} 