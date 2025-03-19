using BakingSisters.Api.Models;

namespace BakingSisters.Api.Services.Customers;

public interface ICustomerService : IBaseService<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
    Task<IEnumerable<Order>> GetCustomerOrderHistoryAsync(int customerId);
    Task<bool> EmailExistsAsync(string email);
} 