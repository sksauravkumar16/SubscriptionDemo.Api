using SubscriptionDemo.Api.Models;

namespace SubscriptionDemo.Api.Repositories;

public interface ISubscriptionRepository
{
    Task<IEnumerable<CustomerSubscription>> GetAsync(string customerId, string? subscriptionName, DateTime? start, DateTime? end);
    Task<int> CreateAsync(CustomerSubscription item);
    Task<bool> UpdateCountAsync(string customerId, string subscriptionName, int delta);
    Task<bool> DeleteAsync(int id);
}
