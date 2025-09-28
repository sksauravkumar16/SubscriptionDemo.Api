using SubscriptionDemo.Api.Models;

namespace SubscriptionDemo.Api.Repositories;

public interface IAuthRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<int> CreateAsync(User user);
}
