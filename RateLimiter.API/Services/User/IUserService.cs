using RateLimiter.API.Model;

namespace RateLimiter.API.Services;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
}