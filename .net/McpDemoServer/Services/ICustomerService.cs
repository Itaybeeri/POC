using MpcDemoServer.Models;

namespace MpcDemoServer.Services;

public interface ICustomerService
{
    Task<Customer?> GetCustomerByIdAsync(string id);
    Task<IEnumerable<Customer>> ListCustomersAsync();
}