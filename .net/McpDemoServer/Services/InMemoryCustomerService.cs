using MpcDemoServer.Models;

namespace MpcDemoServer.Services;

public class InMemoryCustomerService : ICustomerService
{
    private static readonly List<Customer> _data = new()
    {
        new Customer("1", "Alice", "alice@example.com"),
        new Customer("2", "Bob", "bob@example.com")
    };

    public Task<Customer?> GetCustomerByIdAsync(string id) =>
        Task.FromResult(_data.FirstOrDefault(c => c.Id == id));

    public Task<IEnumerable<Customer>> ListCustomersAsync() =>
        Task.FromResult<IEnumerable<Customer>>(_data);
}