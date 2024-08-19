using RateLimiter.API.Model;

namespace RateLimiter.API.Mock;

public interface IExternalService
{
    Task<List<User>> GetDataAsync();
}
